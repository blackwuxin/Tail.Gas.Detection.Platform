define(function (require, exports, module) {
	var $ = require('jquery');
	var JSON = require('json');

	exports.getCommonData = getCommonData;
	exports.initSelectWithCommonData = initSelectWithCommonData;
	exports.initCheckBoxWithCommonData = initCheckBoxWithCommonData;
	exports.setCheckBoxWithCommonData = setCheckBoxWithCommonData;
	exports.initSelectWithCarType = initSelectWithCarType;
	exports.initSelectWithBelong = initSelectWithBelong;

	function getCommonData(type, successHandler, errorHandler) {
		$.ajax({
			type: "POST",
			url: window.BASE_PATH + "/commondata/getcommondata",
			data: JSON.stringify({
				type: type
			}),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (res) {
				if (res.result === 0) {//success
					if (successHandler) {
						successHandler(res.data);
					}
				} else {//error
					if (errorHandler) {
						errorHandler();
					}

					console.log(res);
				}
			},
			error: function (err) {
				if (errorHandler) {
					errorHandler();
				}

				console.log(err);
			}
		});
	}

	function initSelectWithCommonData(type, $selectObj, firstItem, callback) {
		getCommonData(type, function (data) {
			var options = '';
			if (firstItem) {
				options += "<option value=''>" + firstItem + "</option>";
			}
			for (var i = 0; i < data.length; i++) {
				options += "<option value='" + data[i].PropertyValue + "'>" + data[i].PropertyName + "</option>";
			}

			$selectObj.html(options);

			if (callback) {
				callback();
			}
		});
	}

	function initCheckBoxWithCommonData(type, $selectObj, callback) {
	    getCommonData(type, function (data) {
	        try {
	            var options = '';
	            for (var i = 0; i < data.length; i++) {
	                options += "<label class=\"checkbox-inline\">";
	                options += "<input type=\"checkbox\" id='" + data[i].PropertyType + data[i].PropertyValue + "' value='" + data[i].PropertyValue + "'> " + data[i].PropertyName;
	                options += "</label>";
	            }

	            $selectObj.html(options);
	        } catch (e)
	        {
	            console.log(e);
	        }
	        if (callback) {
	            callback();
	        }
	    });
	}

	function setCheckBoxWithCommonData(valueStr, typeflag, $selectObj, callback) {
	    try {
	        if (valueStr && valueStr.length > 0) {
	            var valueAry = valueStr.split(",");
	            if (valueAry && valueAry.length > 0) {
	                for (var i = 0; i < valueAry.length; i++) {
	                    if (valueAry[i]) {
	                        $("input#" + typeflag + valueAry[i], $selectObj).prop("checked", true);
	                    }
	                }
	            }
	        }
	    } catch (e)
	    {
	        console.log(e);
	    }
	    if (callback) {
	        callback();
	    }
	}

	function initSelectWithBelong(type, $selectObj, firstItem, callback) {
	    getCommonData(type, function (data) {
	        var options = '';
	        if (firstItem) {
	            options += "<option value=''>" + firstItem + "</option>";
	        }
	        for (var i = 0; i < data.length; i++) {
	            options += "<option value='" + data[i].Belong + "'>" + data[i].Belong + "</option>";
	        }

	        $selectObj.html(options);

	        if (callback) {
	            callback();
	        }
	    });
	}
	function initSelectWithCarType(type, $selectObj, firstItem, callback) {
	    getCommonData(type, function (data) {
	        var options = '';
	        if (firstItem) {
	            options += "<option value=''>" + firstItem + "</option>";
	        }
	        for (var i = 0; i < data.length; i++) {
	            options += "<option value='" + data[i].Name + "'>" + data[i].Name + "</option>";
	        }

	        $selectObj.html(options);

	        if (callback) {
	            callback();
	        }
	    });
	}

});