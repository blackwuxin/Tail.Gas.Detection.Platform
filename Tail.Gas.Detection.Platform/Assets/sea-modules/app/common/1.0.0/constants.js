define(function (require, exports, module) {
    exports.ChannelPriority = {
        ALL: "1",//同时发送
        CUSTOM: "2",//自定义规则
        RESEND: "3"//站内信未读补发短信&微信
    };
    exports.MessageChannel = {
        WECHAT: "W",
        SMS: "S",
        LETTER: "L",
        APP: "A",
        XIAOMI: "M",
        SHOUQ: "Q"
    };
    exports.PushType = {
        PushMessageToClientsRequestType: "PushMessageToClientsRequestType",
        PushMessageToUsersRequestType: "PushMessageToUsersRequestType",
        PushMessageToClientsExRequestType: "PushMessageToClientsExRequestType"
    };
    exports.LetterMessageType = {
        SERVICE: "SERVICE",//服务消息
        CAMPAIGN: "CAMPAIGN",//活动消息
        CLUB: "CLUB",//社区消息
        OTHER: "OTHER"//其他消息
    };
    exports.LetterChannelType = {
        MOBILE: "MOBILE",
        H5: "H5",
        APP: "APP",
        ONLINE: "ONLINE"
    };
    exports.Operate = {
        UNKONW: "UNKONW",
        ADD: "ADD",
        EDIT: "EDIT",
        VIEW: "VIEW"
    };
    exports.AckCodeType = {
        Success: 0,
        Failure: 1,
        Warning: 2,
        PartialFailure: 3
    }
    exports.CHANNEL_COUNT = 6;
    exports.AND = "&";
    exports.OR = "|";
    exports.XOR = "^";
    exports.WAIT = "?";

    exports.Message = {
        MSG_UNMINIMUM_PRIORITY: "由于站内信到达率为100%，建议将站内信做为最低优先级"
    };
    exports.DataTableLanguage = {
        "processing": "<img src='" + window.BASE_PATH + "/Assets/images/loading.gif' />",
        "lengthMenu": "每页显示 _MENU_ 条记录",
        "zeroRecords": "没有检索到数据",
        "info": "<span class='mrw'>记录数：<b style='color:#000;'>_TOTAL_</b></span><span class='mrw'>当前显示：<b style='color:#000;'>_START_ - _END_</b></span><span class='mrw'>总页数：<b style='color:#000;'>_PAGES_</b></span><span class='mrw'>当前页：<b style='color:red;'>_PAGE_</b></span>",
        "infoEmpty": "共 0 条记录",
        "infoFiltered": "(从 _MAX_ 条数据中检索))",
        "paginate": {
            "next": "下一页",
            "previous": "上一页",
            "last": "尾页",
            "first": "首页"
        }
    };

    //短信示例最大长度
    exports.SMS_CONTENT_MAXLENGTH = 250;
    exports.ENGLISH_SMS_CONTENT_MAXLENGTH = 320;
});