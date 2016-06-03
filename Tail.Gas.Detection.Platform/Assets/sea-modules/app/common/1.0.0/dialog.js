define(function (require, exports, module) {

	require("bootstrapDialog");

	exports.dialog = function (message, title) {
		BootstrapDialog.show({ title: '提示', message: message, type: BootstrapDialog.TYPE_DEFAULT, size: BootstrapDialog.SIZE_NORMAL });
	}
	exports.primary = function (message) {
	    BootstrapDialog.show({ title: '提示', message: message, type: BootstrapDialog.TYPE_PRIMARY, size: BootstrapDialog.SIZE_NORMAL });
	}
	exports.info = function (message) {
		BootstrapDialog.show({ title: '提示', message: message, type: BootstrapDialog.TYPE_INFO, size: BootstrapDialog.SIZE_NORMAL });
	}
	exports.warn = function (message, callback) {
	    BootstrapDialog.show({ title: '警告', message: message, type: BootstrapDialog.TYPE_WARNING, size: BootstrapDialog.SIZE_NORMAL, onhidden: callback });
	}
	exports.success = function (message, shownEventHandler, hiddenEventHandler) {
		BootstrapDialog.show({
			title: '提示', message: message, type: BootstrapDialog.TYPE_SUCCESS, size: BootstrapDialog.SIZE_NORMAL,
			onshown: shownEventHandler,
			onhidden: hiddenEventHandler
		});
	}
	exports.error = function (message, callback) {
	    BootstrapDialog.show({ title: '错误', message: message, type: BootstrapDialog.TYPE_DANGER, size: BootstrapDialog.SIZE_NORMAL, onhidden: callback });
	}
});