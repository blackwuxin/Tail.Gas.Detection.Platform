define(function (require, exports, module) {
	var $ = require('jquery');

	//获取字符串字节长度
	exports.getByteLength = function (val) {
		if (exports.isEmpty(val)) {
			return 0;
		}

		var len = 0;
		for (var i = 0; i < val.length; i++) {
			//全角 
			if (val[i].match(/[^\x00-\xff]/ig) != null)
				len += 2;
			else
				len += 1;
		}
		return len;
	}

	// 判断是否为空
	exports.isEmpty = function (s) {
		return $.isEmptyObject(s);
	};

	//判断是否正整数
	exports.isNumber = function (s) {
		var regex = new RegExp("^[0-9]+$");

		return s.search(regex) != -1;
	};

	// 中文
	exports.isChinese = function (s) {
		var p = /^[\u4e00-\u9fa5]+$/;
		return this.test(s, p);
	};

	// 英文
	exports.isEnglish = function (s) {
		var p = /^[A-Za-z]+$/;
		return this.test(s, p);
	};

	// 邮箱
	exports.isEmail = function (s) {
		var p = "^[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+@[-!#$%&\'*+\\/0-9=?A-Z^_`a-z{|}~]+\.[-!#$%&\'*+\\./0-9=?A-Z^_`a-z{|}~]+$";
		return this.test(s, p);
	};

	// 手机号码
	exports.isMobile = function (s) {
		return this.test(s, /^(13[0-9]|15[0|3|6|7|8|9]|18[2|8|9])\d{8}$/);
	};

	// 电话号码
	exports.isPhone = function (s) {
		return this.test(s, /^[0-9]{3,4}\-[0-9]{7,8}$/);
	};

	// 邮编
	exports.isPostCode = function (s) {
		return this.test(s, /^[1-9][0-9]{5}$/);
	};

	// isFax
	exports.isFax = function (s) {
		return this.test(s, /^((\+?[0-9]{2,4}\-[0-9]{3,4}\-)|([0-9]{3,4}\-))?([0-9]{7,8})(\-[0-9]+)?$/);
	};

	//是否为图片
	exports.isImage = function (s) {
		return this.test(s, /\.(gif|jpg|jpeg|png|GIF|JPG|PNG)$/);
	};

	exports.isURL = function (url) {
		var strRegex = '[a-zA-z]+://[^\s]*';
		var re = new RegExp(strRegex);
		if (re.test(url)) {
			return (true);
		} else {
			return (false);
		}
	}

	// 正则匹配
	exports.test = function (s, p) {
		s = s.nodeType == 1 ? s.value : s;
		return new RegExp(p).test(s);
	};

	exports.getValueFromAOData = function (aoData, key) {
	    if (!aoData) {
	        return;
	    }

	    for (var i = 0; i < aoData.length; i++) {
	        if (aoData[i].name === key) {
	            return aoData[i].value;
	        }
	    }

	    return '';
	};

    // 日期格式化
	exports.DateFormat = function (date, format) {
	    /* 
         * eg:format="yyyy-MM-dd hh:mm:ss"; 
         */
	    var o = {
	        "M+": date.getMonth() + 1, // month  
	        "d+": date.getDate(), // day  
	        "h+": date.getHours(), // hour  
	        "m+": date.getMinutes(), // minute  
	        "s+": date.getSeconds(), // second  
	        "q+": Math.floor((date.getMonth() + 3) / 3), // quarter  
	        "S": date.getMilliseconds()
	        // millisecond  
	    };

	    if (/(y+)/.test(format)) {
	        format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4
                            - RegExp.$1.length));
	    }

	    for (var k in o) {
	        if (new RegExp("(" + k + ")").test(format)) {
	            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                                ? o[k]
                                : ("00" + o[k]).substr(("" + o[k]).length));
	        }
	    }
	    return format;
	};
});