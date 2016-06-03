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



	exports.init = function () {
	    //导入Json模板
	    $("#btn_import").click(function () {

	        $('#uploadfileinput').val('');
	        $('#upload').modal();
	    });

	    $('#upfileiframe').bind('load', function () {
	        try {
	            var str = upfileiframe.document.body.innerText;
	            if (str && str.length > 0) {
	                var resp = $.parseJSON(str);
	                if (resp.result == 0) {
	                     Dialog.primary("<p>导入成功</p>");
	                } else {
	                    Dialog.error("导入失败，请检查文件是否正确！");
	                }
	            }
	        } catch (e) {
	            Dialog.error("导入失败，请检查文件是否正确！");
	        }
	    });
	    $("#btn-uploadfile").click(function () {
	        var filename = $('#uploadfileinput')[0].value;
	        if (!filename || filename.length == 0) {
	            Dialog.warn('请选择文件');
	        } else {

	            $('#uploadform').submit();
	        }
	    });
		
	}
	
});