(function () {
    var config = {
        alias: {
            "jquery": "jquery/jquery/1.10.1/jquery",
            "$": "jquery/jquery/1.10.1/jquery",
            "bootstrap": "extend/bootstrap/3.3.2/js/bootstrap",
            "bootstrapDialog": "extend/bootstrapdialog/3.3.2/bootstrap-dialog",
            "getUrlQueryString": "jquery/getUrlQueryString/1.0.0/getUrlQueryString",
            "position": "extend/position/1.0.0/position",
            "cookie": "arale/cookie/1.0.2/cookie",
            "json": "gallery/json/1.0.3/json",
            'moment': "extend/moment/2.8.3/moment",
            "template": "extend/template/3.0.0/template",
            'dataTables': "jquery/dataTables/1.10.4/jquery.dataTables",
            'dataTables.bootstrap': "jquery/dataTables/1.10.4/dataTables.bootstrap",
            'blockui': 'jquery/jquery.BlockUI/2.7.0/jquery.blockUI',
            "jquery.ui": "jquery/jquery.ui/1.11.0/jquery-ui",
            "jquery.ui.timepicker": "jquery/jquery.ui/1.11.0/jquery-ui-timepicker-addon",

            "commondata": "app/common/1.0.0/commondata",
            "constants": "app/common/1.0.0/constants",
            "utils": "app/common/1.0.0/utils",
            "dialog": "app/common/1.0.0/dialog",
            "carstatusinfo": "app/carstatusinfo/1.0.0/index",
            "carinfo": "app/carinfo/edit",
            "login": "app/login/index",
            "importcarinfo": "app/importcarinfo/index"
        },
        paths: {
            "sea": "sea-modules",
            "arale": "sea-modules/arale",
            "gallery": "sea-modules/gallery",
            "jquery": "sea-modules/jquery",
            "extend": "sea-modules/extend",
            "app": "sea-modules/app"
        },
        vars: {},
        map: [
            [/^(.*\.(?:css|js))(.*)$/i, "$1?v=201410100000000"]
        ],
        debug: false,
        charset: "utf-8"
    };
    if (typeof seajs === "object") {
        seajs.config(config);
    }
})();