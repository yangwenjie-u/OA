$.extend($.fn.validatebox.defaults.rules, {
	minLength: {
		validator: function (value, param) {
			return value.length >= param[0];
		},
		message: '输入字符数不能少于{0}个'
	},
	maxLength: {
		validator: function (value, param) {
			return value.length <= param[0];
		},
		message: '输入字符数不能多于{0}个'
	},
	//验证汉字
	CHS: {
		validator: function (value) {
			return /^[\u0391-\uFFE5]+$/.test(value);
		},
		message: '只能输入汉字'
	},
	//用户账号验证(只能包括 _ 数字 字母)
	account: {
		validator: function (value, param) {
			if (value.length < param[0] || value.length > param[1]) {
				$.fn.validatebox.defaults.rules.account.message = '用户名长度必须在' + param[0] + '至' + param[1] + '范围';
				return false;
			} else if (!/^[\w]+$/.test(value)) {
				$.fn.validatebox.defaults.rules.account.message = '用户名只能数字、字母、下划线组成.';
				return false;
			} else {
				return true;
			}
		}
	}
});