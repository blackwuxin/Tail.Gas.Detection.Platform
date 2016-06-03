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

	var code; //在全局定义验证码 

	exports.init = function () {
	
		createCode();
	}
	//产生验证码
	function createCode() {
		code = "";
		var codeLength = 4;//验证码的长度
		var checkCode = $("#code");
		var random = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
		'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');//随机数
		for (var i = 0; i < codeLength; i++) {//循环操作
			var index = Math.floor(Math.random() * 36);//取得随机数的索引（0~35）
			code += random[index];//根据索引取得随机数加到code上
		}
		checkCode.val(code);//把code值赋给验证码
	};
	$("#code").click(function () {
		createCode();
	});
	$("#login").click(function () {
		var inputCode = $("#validateCode").val(); //取得输入的验证码并转化为大写      
		if (!inputCode||inputCode=="") { //若输入的验证码长度为0
			Dialog.error("请输入验证码！"); //则弹出请输入验证码
		}
		else if (inputCode.toUpperCase() != code) { //若输入的验证码与产生的验证码不一致时
			Dialog.error("验证码输入错误！"); //则弹出验证码输入错误
			createCode();//刷新验证码
			$("#validateCode").val("");

		}
		else { //输入正确时
			localStorage.setItem("user", "company");
			
			var username = $("#username").val();
			var password = $("#password").val();
			var usertype = $("#usertype").val();

			$.ajax({
				type: "GET",
				contentType: "application/json; charset=utf-8",
				url: window.BASE_PATH + '/home/login',
				dataType: "json",
				data: {
					username: username,
					password: password,
					usertype: usertype
				},
				success: function (resp) {
					if (resp.result ==0) {
						window.location.href =window.BASE_PATH+"/carstatusinfo/index";
					} else {
						Dialog.error("登陆失败，请重试！");
					}
					if (callback) {
						callback();
					}
				},
				error: function (resp) {
					Dialog.error("登陆失败，请重试！");
				}
			});
		}
	});
});