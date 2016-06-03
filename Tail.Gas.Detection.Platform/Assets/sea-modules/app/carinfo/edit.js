define(function (require, exports, module) {

    var $ = require('jquery');
    var JSON = require('json');
    var CommonData = require("commondata");
    var Constants = require("constants");
    var template = require('template');
    var query = require("getUrlQueryString");
    var Utils = require("utils");

    require("bootstrap");
    var Dialog = require('dialog');
    var oldMessage = {};
    var oldMessageChannel = {};

    require('blockui');

    init();


    function init() {
        var query = $.getUrlQueryString(window.location.href);
        var carno = decodeURIComponent(query.carno);
        loadCarInfo(carno);
    }

    function loadCarInfo(carno) {

        $.blockUI({ message: "<img src='" + window.BASE_PATH + "/Assets/images/loading.gif' />" });
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/carinfo/get',
            dataType: "json",
            data: {
                carno: carno
            },
            success: function (resp) {
                $("#No").val(resp.NO);
                $("#Color").val(resp.Color);
                $("#Category").val(resp.Category);
                $("#Belong").val(resp.Belong);
                $("#OriginalEmissionValues").val(resp.OriginalEmissionValues);
                $("#ProductModel").val(resp.ProductModel);
                $("#ModifiedCompany").val(resp.ModifiedCompany);
                $("#UserInfo").val(resp.UserInfo);
                $("#IndividualCompany").val(resp.IndividualCompany);
                $("#Telphone").val(resp.Telphone);
                $("#ModifiedTime").val(resp.ModifiedTime.replace("T", " "));
                
            },
            error: function (resp) {
                Dialog.error("消息加载出现错误，请重试！");
            },
            complete: function () {
                $.unblockUI();
            }
        });
    }
    function saveMessageTemplate(Message, type) {
        //=============================微信========================================
        if ($("#message-channel-wechat").is(':checked') && (type == 'wechat' || type == 'all')) {
            var wechatTemplate = GetWeChatContent();
            var loginCheck = $("#rdo-login-check-yes").prop("checked");
            var redirectUrlValue = "";
            var fixedURL = $("#rdo-fixed-check-yes").prop("checked"),
                dynamiclink = $("#rdo-fixed-check-no").prop("checked");
            if (fixedURL) {
                redirectUrlValue = $("#wechat-fixed-redUrl").val();
            } else if (dynamiclink) {
                redirectUrlValue = "dynamiclink";
            } else {
                redirectUrlValue = "";
            }
            Message.Wechat = JSON.stringify({
                WechatTemplate: JSON.stringify(wechatTemplate),
                Url: redirectUrlValue,
                LoginCheck: loginCheck
            });
        }
        //=============================手Q========================================
        if ($("#message-channel-shouq").is(':checked') && (type == 'shouq' || type == 'all')) {
            var shouqTemplate = GetShouqContent();
            var shouqTemplateName = $("#shouq-template-select").find("option:selected").text();
            var shouqloginCheck = $("#rdo-shouq-login-check-yes").prop("checked");
            var shouqredirectUrl = "";
            var shouqfixedURL = $("#rdo-shouq-fixed-check-yes").prop("checked");
            if (shouqfixedURL) {
                shouqredirectUrl = $("#shouq-fixed-redUrl").val();
            }
            Message.Shouq = JSON.stringify({
                ShouqTemplate: JSON.stringify(shouqTemplate),
                RedirectUrl: shouqredirectUrl,
                LoginCheck: shouqloginCheck
            });
        }
        //=============================小米========================================
        if ($("#message-channel-mi").is(':checked') && (type == 'xiaomi' || type == 'all')) {
            //模板编号
            var miTemplateNumber = $("#mi-template-select").find("option:selected").attr('templatenumber');
            //详细内容
            var miTemplate = $("#mi-template-content").text();
            //模板名称
            var miTemplateName = $("#mi-template-select").find("option:selected").text();
            //跳转链接
            var miredirectUrl = $("#mi-chk-url").prop("checked");
            //跳转链接登录态验证
            var miloginCheck = $("#rdo-mi-login-check-yes").prop("checked");

            Message.Xiaomi = JSON.stringify({
                MessageType: 7,
                MiTemplateNumber: miTemplateNumber,
                MiTemplateName: miTemplateName,
                MiTemplate: miTemplate,
                RedirectUrl: miredirectUrl,
                LoginCheck: miloginCheck
            });
        }
        //===========================App推送=======================================
        if ($("#message-channel-app").is(':checked') && (type == 'app' || type == 'all')) {
            //推送类型
            var appPushType = $("#app-push-type-select").val();
            var messageChannel = $("#hdn-message-channel").val();
            //推送标题
            var appTitle = $("#app-title").val();
            //推送内容
            var appContent = $("#app-content").val();
            //扩展ext
            var appExt = $("#app-ext").val();
            //ios扩展
            var appIosExt = $("#app-iosext").val();
            //展现样式
            var appShowStyleType = $("#app-show-style-select").val();
            var appShowStyle = $("#app-show-style-json").val();
            var appUrlType = '';
            if ($("#app-radio-none").prop("checked")) {
                appUrlType = 1;
            } else if ($("#app-radio-letterdetails").prop("checked")) {
                appUrlType = 2;
            } else if ($("#app-radio-custom").prop("checked")) {
                appUrlType = 3;
            }
            //内容跳转URL
            var appUrl = $("#app-url").val();
            //静默推送
            var appContentAvailable = false;
            //版本号
            var appVersionCode = '';
            var hasAppServerVersionCode = '';

            if (appPushType == Constants.PushType.PushMessageToClientsRequestType) {
                appContentAvailable = $("#app-chk-toclient-content-available").is(':checked');
                appVersionCode = $("#app-chk-toclient-version").is(':checked');
                hasAppServerVersionCode = $("#app-chk-toclient-server-version").is(':checked');
            } else if (appPushType == Constants.PushType.PushMessageToUsersRequestType) {
                appContentAvailable = $("#app-chk-touser-content-available").is(':checked');
                appVersionCode = $("#app-chk-touser-version").is(':checked');
                hasAppServerVersionCode = $("#app-chk-touser-server-version").is(':checked');
            } else if (appPushType == Constants.PushType.PushMessageToClientsExRequestType) {
                appContentAvailable = $("#app-chk-toclientex-content-available").is(':checked');
                appVersionCode = $("#app-chk-toclientex-version").is(':checked');
                hasAppServerVersionCode = $("#app-chk-toclientex-server-version").is(':checked');
            }
            Message.App = JSON.stringify({
                MessageType: 3,
                PushType: appPushType,
                Title: appTitle,
                Content: appContent,
                AppExt: appExt,
                AppIosExt: appIosExt,
                ShowStyleType: appShowStyleType,
                ShowStyle: appShowStyle,
                UrlType: appUrlType,
                Url: appUrl,
                ClientAttribute: {
                    ContentAvailable: appContentAvailable,
                    CustomVersionCode: appVersionCode,
                    HasCustomServerVersionCode: hasAppServerVersionCode,
                },
                UserAttribute: {
                    ContentAvailable: appContentAvailable,
                    CustomVersionCode: appVersionCode,
                    HasCustomServerVersionCode: hasAppServerVersionCode,
                },
                ClientEXAttribute: {
                    ContentAvailable: appContentAvailable,
                    CustomVersionCode: appVersionCode,
                    HasCustomServerVersionCode: hasAppServerVersionCode,
                }
            });
        }
        //============================短信======================================
        if ($("#message-channel-sms").is(':checked') && (type == 'sms' || type == 'all')) {
            //短信类型
            var smsType = $("#sms-type-select").val();
            //短信内容
            var smsContent = $("#sms-content").val();
            var smsBusinessType = $("#sms-business-type").val();
            var smsModuleType = $("#sms-module-type").val();
            var smsSendCode = $("#sms-send-code").val();
            var smsSourceID = $("#sms-source-id").val();
            var smsUID = $("#sms-chk-uid").is(':checked');
            var smsEID = $("#sms-chk-eid").is(':checked');
            var smsOrderID = $("#sms-chk-orderid").is(':checked');
            var smsSendTime = $("#sms-chk-sendtime").is(':checked');
            var smsExpiredTime = $("#sms-chk-expiredtime").is(':checked');
            Message.Sms = JSON.stringify({
                MessageType: 2,
                SmsType: smsType,
                SmsContent: smsContent,
                SmsAttribute: {
                    BusinessType: smsBusinessType,
                    ModuleType: smsModuleType,
                    SendCode: smsSendCode,
                    SourceID: smsSourceID,
                    UID: smsUID,
                    EID: smsEID,
                    OrderID: smsOrderID,
                    SendTime: smsSendTime,
                    ExpiredTime: smsExpiredTime
                }
            });
        }
        //===============================站内信=============================
        if ($("#message-channel-letter").is(':checked') && (type == 'letter' || type == 'all')) {
            //发送平台
            var sendApp = $("#letter-chk-platform-mainapp").is(':checked');
            var sendH5 = $('#letter-chk-platform-mainapp-h5').is(':checked');
            var sendOnline = $("#letter-chk-platform-online").is(':checked');
            //标题
            var letterTitle = $("#letter-title").val();
            window.editorLetterOnline.sync();
            var letterOnlineContent = $("#letter-online-content").val();
            window.editorLetterApp.sync();
            var letterAppContent = $("#letter-mainapp-content").val();
            window.editorLetterAppH5.sync();
            var letterH5Content = $('#letter-mainapp-h5-content').val();
            var letterAppIconType = $("#letter-app-icon").val();
            var letterAppIconUrl = $("#letter-app-icon-url").val();
            var letterRedirectUrl = $("#letter-redirect-url").val();
            var letterH5RedirectUrl = $("#letter-h5-redirect-url").val();
            //消息类型
            var letterMessageType = $("#letter-message-type").val();
            //消息子类型
            var letterMessageSubtype = $("#letter-message-subtype").val();
            //发送人
            var letterSendUser = $("#letter-sender-name").val();
            //批次号
            var letterBatchCode = $("#letter-chk-batchcode").is(':checked');
            //到期时间
            var letterExpiredTime = $("#letter-chk-expiredtime").is(':checked');
            //服务ID
            var letterServiceID = $("#letter-service-id").val();
            Message.Letter = JSON.stringify({
                MessageType: 6,
                SendApp: sendApp,
                SendH5: sendH5,
                SendOnline: sendOnline,
                Title: letterTitle,
                OnlineContent: letterOnlineContent,
                AppContent: letterAppContent,
                H5Content: letterH5Content,
                AppIconUrl: letterAppIconUrl,
                RedirectUrl: letterRedirectUrl,
                H5RedirectUrl: letterH5RedirectUrl,
                MsgType: letterMessageType,
                MsgSubType: letterMessageSubtype,
                LetterAttribute: {
                    SendUser: letterSendUser,
                    BatchCode: letterBatchCode,
                    ExpiredTime: letterExpiredTime,
                    MsgServiceID: letterServiceID
                }
            });
        }

    }
    function loadWechatTemlpate(templateNumber, fnCallback) {
        $.ajax({
            type: "get",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/wechattemplate/GetByNumber',
            dataType: "json",
            data: {
                TemplateNumber: templateNumber
            },
            success: function (resp) {
                if (fnCallback) {
                    fnCallback(resp);
                }
            }
        });
    }

    function loadMiTemplate(templateNumber, fnCallback) {
        $.ajax({
            type: "get",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/mitemplate/GetByNumber',
            dataType: "json",
            data: {
                TemplateNumber: templateNumber
            },
            success: function (resp) {
                if (fnCallback) {
                    fnCallback(resp);
                }
            }
        });
    }

    function loadWechatTemplates(businessName, callback) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/message/loadwechattemplates',
            dataType: "json",
            data: {
                businessName: businessName
            },
            success: function (resp) {
                if (resp.result == 0) {
                    var html = template('tpl-wechat-template-item', resp);

                    $("#wechat-template-select").html(html);
                } else {
                    if (resp.resultMessage && resp.resultMessage != '') {
                        Dialog.error(resp.resultMessage);
                    } else {
                        Dialog.error("微信模板加载失败，请重试！");
                    }
                }
                if (callback) {
                    callback();
                }
            },
            error: function (resp) {

            }
        });
    }

    function loadMiTemplates(businessName, callback) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/message/loadmittemplates',
            dataType: "json",
            data: {
                businessName: businessName
            },
            success: function (resp) {
                if (resp.result == 0) {
                    var html = template('tpl-mi-template-item', resp);

                    $("#mi-template-select").html(html);
                } else {
                    if (resp.resultMessage && resp.resultMessage != '') {
                        Dialog.error(resp.resultMessage);
                    } else {
                        Dialog.error("小米模板加载失败，请重试！");
                    }
                }
                if (callback) {
                    callback();
                }
            },
            error: function (resp) {

            }
        });
    }


    function loadShouqTemlpate(templateNumber, fnCallback) {
        $.ajax({
            type: "get",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/shouqtemplate/GetByNumber',
            dataType: "json",
            data: {
                TemplateNumber: templateNumber
            },
            success: function (resp) {
                if (fnCallback) {
                    fnCallback(resp);
                }
            }
        });
    }

    function loadShouqTemplates(businessName, callback) {
        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: window.BASE_PATH + '/message/loadshouqttemplates',
            dataType: "json",
            data: {
                businessName: businessName
            },
            success: function (resp) {
                if (resp.result == 0) {
                    var html = template('tpl-shouq-template-item', resp);

                    $("#shouq-template-select").html(html);
                } else {
                    if (resp.resultMessage && resp.resultMessage != '') {
                        Dialog.error(resp.resultMessage);
                    } else {
                        Dialog.error("手Q模板加载失败，请重试！");
                    }
                }
                if (callback) {
                    callback();
                }
            },
            error: function (resp) {

            }
        });
    }

    function setBusinessType(businessType, ddlBusinessType) {
        var options = ddlBusinessType.find("option");
        for (var j = 0; j < options.length; j++) {
            if ($(options[j]).text() == businessType) {
                ddlBusinessType.val($(options[j]).val());
                break;
            }
        }
    }

    function setTemplate(templateNumber, ddlTemplate) {
        var options = ddlTemplate.find("option");
        for (var j = 0; j < options.length; j++) {
            if ($(options[j]).attr("templatenumber") == templateNumber) {
                ddlTemplate.val($(options[j]).val());
                ddlTemplate.change();
                break;
            }
        }
    }
    //重复代码 开始
    function hideChannelPriorityPanel() {
        $("#panel-channel-priority").addClass("hide");
    }

    function showChannelPriorityPanel() {
        $("#panel-channel-priority").removeClass("hide");
    }

    function processChannelPriorityPanelDisplay() {
        var channelPriority = $("#channel-priority").val();

        if ($("#message-channel-app").prop("checked") && $("#message-channel-letter").prop("checked")) {
            $(".channel-rule-container-1").show();
            $(".channel-rule-container-2 label").text('');
        } else {
            $(".channel-rule-container-1").hide();
            $(".channel-rule-container-2 label").text('发送规则：');

            $("#app-after-letter").prop("checked", false);
        }
        if ($("#message-channel-app").prop("checked") &&
            $("#message-channel-letter").prop("checked") &&
            $("#app-after-letter").prop("checked") &&
            !$("#message-channel-sms").prop("checked") &&
            !$("#message-channel-wechat").prop("checked") &&
            !$("#message-channel-mi").prop("checked") &&
            !$("#message-channel-shouq").prop("checked")) {

            $(".channel-rule-container-2").hide();
            $("#panel-channel-priority").hide();
        } else {
            $(".channel-rule-container-2").show();
            $("#panel-channel-priority").show();

            if (window.location.href.indexOf("message/details") == -1) {
                $("#app-after-letter").removeAttr("disabled");
                $("#message-channel-letter").removeAttr("disabled");
            }

            if (channelPriority === Constants.ChannelPriority.CUSTOM) {//高优先级通道发送失败，发送低优先级通道
                var cnt = $("input.message-channel[type=checkbox]:checked").length;

                //APP推送+站内信合并显示
                if ($("#app-after-letter").prop("checked")) {
                    cnt = cnt - 1;
                }
                for (var i = 1; i <= cnt; i++) {
                    $("#panel-channel-priority-" + i).removeClass("hide");
                }
                for (var i = Constants.CHANNEL_COUNT; i > cnt; i--) {
                    $("#panel-channel-priority-" + i).addClass("hide");
                }

                //APP推送+站内信合并显示
                if ($("#app-after-letter").prop("checked")) {
                    $("input[type='checkbox'][id^='channel-priority-appafterletter-']").parent().removeClass("hide");
                    $("input[type='checkbox'][id^='channel-priority-letter-']").parent().addClass("hide");
                    $("input[type='checkbox'][id^='channel-priority-letter-']").prop("checked", false);
                    $("input[type='checkbox'][id^='channel-priority-app-']").parent().addClass("hide");
                    $("input[type='checkbox'][id^='channel-priority-app-']").prop("checked", false);
                } else {
                    $("input[type='checkbox'][id^='channel-priority-appafterletter-']").parent().addClass("hide");
                    if ($("#message-channel-letter").prop("checked")) {
                        $("input[type='checkbox'][id^='channel-priority-letter-']").parent().removeClass("hide");
                    }
                    if ($("#message-channel-app").prop("checked")) {
                        $("input[type='checkbox'][id^='channel-priority-app-']").parent().removeClass("hide");
                    }
                }

                $("input.message-channel[type=checkbox]").each(function (index, element) {
                    var id = element.id;
                    if ($(element).is(':checked')) {
                        if (id === 'message-channel-wechat') {
                            $("input[type='checkbox'][id^='channel-priority-wechat-']").parent().show();
                        } else if (id === 'message-channel-mi') {
                            $("input[type='checkbox'][id^='channel-priority-mi-']").parent().show();
                        } else if (id === 'message-channel-app') {
                            $("input[type='checkbox'][id^='channel-priority-app-']").parent().show();
                        } else if (id === 'message-channel-sms') {
                            $("input[type='checkbox'][id^='channel-priority-sms-']").parent().show();
                        } else if (id === 'message-channel-letter') {
                            $("input[type='checkbox'][id^='channel-priority-letter-']").parent().show();
                        } else if (id === 'message-channel-shouq') {
                            $("input[type='checkbox'][id^='channel-priority-shouq-']").parent().show();
                        }
                    } else {
                        if (id === 'message-channel-wechat') {
                            $("input[type='checkbox'][id^='channel-priority-wechat-']").parent().hide().prop("checked", false);
                        } else if (id === 'message-channel-mi') {
                            $("input[type='checkbox'][id^='channel-priority-mi-']").parent().hide().prop("checked", false);
                        } else if (id === 'message-channel-app') {
                            $("input[type='checkbox'][id^='channel-priority-app-']").parent().hide().prop("checked", false);
                        } else if (id === 'message-channel-sms') {
                            $("input[type='checkbox'][id^='channel-priority-sms-']").parent().hide().prop("checked", false);
                        } else if (id === 'message-channel-letter') {
                            $("input[type='checkbox'][id^='channel-priority-letter-']").parent().hide().prop("checked", false);
                        } else if (id === 'message-channel-shouq') {
                            $("input[type='checkbox'][id^='channel-priority-shouq-']").parent().hide().prop("checked", false);
                        }
                    }
                });
            } else if (channelPriority === Constants.ChannelPriority.RESEND) {//站内信未读补充发送其他通道
                $("#app-after-letter").attr("disabled", true);
                $("#message-channel-letter").attr("disabled", true);
                $("#message-channel-letter").prop("checked", true);

                if ($("#message-channel-app").prop("checked") && $("#message-channel-letter").prop("checked")) {
                    $("#app-after-letter").prop("checked", true);
                }
            }
        }
    }
    //重复代码 结束

    function processDetailsTabDisplay() {
        var hasResetTab = false;
        $("input.message-channel[type=checkbox]").each(function (index, element) {
            var messageChannel = $(this).val();
            if ($(this).is(':checked')) {
                $("#tab-item-" + messageChannel).removeClass("hide");
                $("#tab-" + messageChannel).removeClass("hide");

                if (!hasResetTab) {
                    $("#tab-item-" + messageChannel + " a").tab('show');
                    hasResetTab = true;
                }
            } else {
                $("#tab-item-" + messageChannel).addClass("hide");
                $("#tab-" + messageChannel).addClass("hide");
            }
        });
    }

    
});
