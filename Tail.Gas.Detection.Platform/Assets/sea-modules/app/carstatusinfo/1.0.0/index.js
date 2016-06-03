define(function (require, exports, module) {

    var $ = require('jquery');
    var JSON = require('json');
    var Constants = require("constants");
    var commonData = require("commondata");
    var Utils = require("utils");
    require("bootstrap");
    require("dataTables");
    require("bootstrapDialog");
    require("dataTables.bootstrap");
    var Dialog = require('dialog');
    var interval;
    var timems = 5000;
    exports.init = function () {
        initSelectData();
        var table = initDataTable();
        $("#btn-search").click(function () {
            table.fnDraw();
        });

        interval = setInterval(function () {
            if ($("#pclist_next").attr("class") && $("#pclist_next").attr("class").indexOf("disabled") > -1) {
                table.fnPageChange(0);
            }
            else {
                table.fnPageChange('next');
            }
        }, timems);
        $("#autoscroll").change(function () {
            if ($("#autoscroll").prop('checked')) {
                interval = setInterval(function () {
                    if ($("#pclist_next").attr("class").indexOf("disabled") > -1) {
                        table.fnPageChange(0);
                    }
                    else {
                        table.fnPageChange('next');
                    }
                }, timems);
            } else {
                clearInterval(interval);
            }
        });

    }

    function initDataTable() {

        return $('#pclist').dataTable({
            'processing': true,
            'bStateSave': true,
            "bFilter": false,
            "bSort": false,
            "bServerSide": true,
            "sAjaxSource": window.BASE_PATH + '/carstatusinfo/query',
            "fnServerData": retrieveData,
            "columnDefs": [
                {
                    "render": function (data, type, row) {
                        if (row["SystemStatus"] == 0 ) {
                            return "正常";
                        } else {
                            return "异常";
                        }
                    },
                    "targets": 6
                },
                 {
                     "render": function (data, type, row) {
                         return "<a target='_blank' href='" + window.BASE_PATH + "/carinfo/detail?carno=" + row["CarNo"] + "' title='查看详情' class='message-details'>" + row["CarNo"] + "</a>";
                     },
                     "targets": 0
                 },{
                     "render": function (data, type, row) {
                         return row["Data_LastChangeTime"].replace("T", " ");
                     },
                     "targets": 7
                 }
                
            ],
            "aoColumns": [{
                "mData": "CarNo"
            }, {
                "mData": "Color"
            }, {
                "mData": "Category"
            }, {
                "mData": "Belong"
            },
             {
                 "mData": "TemperatureBefore"
             },
             {
                 "mData": "SensorNum"
             }, {
                 "mData": "SystemStatus"
             }, {
                 "mData": "Data_LastChangeTime"
             }],
            "language": Constants.DataTableLanguage
        });
    }
    function convertQueryResult(result) {
        var aaData = {};
        aaData['iDisplayLength'] = 10;

        if (result == null) {
            aaData["iTotalRecords"] = 0;
            aaData["iTotalDisplayRecords"] = 0;
            aaData["aaData"] = [];
            aaData["aaData"].push({});
        } else {
            aaData["iTotalRecords"] = result["total-records"];
            aaData["iTotalDisplayRecords"] = aaData["iTotalRecords"];
            aaData["aaData"] = [];
            for (var i = 0 ; i < result.messages.length; i++) {
                aaData["aaData"].push(result.messages[i]);
            }
        }

        return aaData;
    }
    function retrieveData(sSource, aoData, fnCallback) {
        var Category = $("#Category").val();
        var Belong = $("#Belong").val();
        var CarNo = $("#CarNo").val();
        
        var iDisplayStart = Utils.getValueFromAOData(aoData, "iDisplayStart");
        var iDisplayLength = Utils.getValueFromAOData(aoData, "iDisplayLength");

        $.ajax({
            type: "get",
            contentType: "application/json; charset=utf-8",
            url: sSource,
            dataType: "json",
            data: {
                PageUrl: location.pathname,
                CarNo:CarNo,
                Category: Category,
                Belong: Belong,
                istart: iDisplayStart,
                ilen: iDisplayLength
            },
            success: function (resp) {
                fnCallback(convertQueryResult(resp));
                if (resp.errors) {
                    for (var i = 0 ; i < resp.errors.length; i++) {
                        console.log(resp.errors[i]);
                    }
                }
            },
            error: function (resp) {
                fnCallback(convertQueryResult(null));
            }
        });
    }

    function initSelectData() {
        commonData.initSelectWithCarType("CarType", $("#Category"), '全部');
        commonData.initSelectWithBelong("Belong", $("#Belong"), '全部');
    }


});