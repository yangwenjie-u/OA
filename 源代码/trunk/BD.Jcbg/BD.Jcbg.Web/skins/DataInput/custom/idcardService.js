var CardIdr = {
	createNew: function (ctrlId) {
		var cardIdr = {
			ctrlid: ctrlId, // 控件名称
			code: false, 		// 读取成功标识
			msg: "", 			// 错误信息
			realname: "", 	// 姓名
			mz: "", 			// 民族
			xb: "", 			// 性别
			csny: "", 			// 出生年月
			sfzhm: "", 			// 身份证号码
			dz: "", 			// 地址
			sfzkh: "", 			// 身份证卡号
			fzjg: "",            // 发证机关
            yxq:"",             // 有效期
			zp: ""				// 照片
		};
		// 输出控件
		cardIdr.getControlString = function () {
			return "<object classid=\"clsid:5EB842AE-5C49-4FD8-8CE9-77D4AF9FD4FF\" id=\"" + cardIdr.ctrlid + "\" width=\"1\" height=\"1\" codebase=\"/cab/idr.cab\"></object>";
		};
		// 读取身份证信息
		cardIdr.read = function () {
			if (typeof (CallCSharpMethodByResult) != "undefined") {
				var ret = CallCSharpMethodByResult("IDCard","");
				var jsonstr = eval('(' + ret + ')');
				if (jsonstr.success) {
					cardIdr.realname = $.trim(jsonstr.Name);
					cardIdr.mz = $.trim(jsonstr.Nation);
					cardIdr.xb = $.trim(jsonstr.Sex);
					var reg = /\./g;
					cardIdr.csny = jsonstr.Born.replace(reg,"-");
					cardIdr.sfzhm = $.trim(jsonstr.IDCardNo);
					cardIdr.dz = $.trim(jsonstr.Address);
					cardIdr.fzjg = $.trim(jsonstr.GrantDept);
					cardIdr.yxq = $.trim(jsonstr.UserLifeBegin+"-"+jsonstr.UserLifeEnd);
					var sfzkh = $.trim(jsonstr.SN);
					var versfzhk = "";
					for (var i = sfzkh.length - 2; i >= 0; i -= 2) {
						versfzhk += sfzkh.substr(i);
						sfzkh = sfzkh.substr(0, sfzkh.length - 2);
					}
					cardIdr.sfzkh = versfzhk.toLowerCase();
					cardIdr.zp = jsonstr.Photo;
				}else{
					cardIdr.msg = jsonstr.msg;
					cardIdr.code = false;
				}
			}
			else{
				try {
					var idcontrol = document.getElementById(cardIdr.ctrlid);
					var result = idcontrol.ReadCard("1001", "");
					if (result == 1) {
						cardIdr.code = true;
						cardIdr.realname = $.trim(idcontrol.GetName());
						cardIdr.mz = $.trim(idcontrol.GetFolk());
						cardIdr.xb = $.trim(idcontrol.GetSex());
						cardIdr.csny = idcontrol.GetBirthYear() + "-" + idcontrol.GetBirthMonth() + "-" + idcontrol.GetBirthDay();
						cardIdr.sfzhm = $.trim(idcontrol.GetCode());
						cardIdr.dz = $.trim(idcontrol.GetAddress());
						cardIdr.fzjg = $.trim(idcontrol.GetAgency());
						cardIdr.yxq = $.trim(idcontrol.GetValid());
						var sfzkh = $.trim(idcontrol.GetIDCardSN());
						var versfzhk = "";
						for (var i = sfzkh.length - 2; i >= 0; i -= 2) {
							versfzhk += sfzkh.substr(i);
							sfzkh = sfzkh.substr(0, sfzkh.length - 2);
						}
						cardIdr.sfzkh = versfzhk.toLowerCase();
						cardIdr.zp = idcontrol.GetJPGPhotobuf();

					} else {
						cardIdr.code = false;
						if (result == -1)
							cardIdr.msg = "端口初始化失败！";
						else if (result == -2)
							cardIdr.msg = "请重新将卡片放到读卡器上！";
						else if (result == -3)
							cardIdr.msg = "读取数据失败！";
						else if (result == -4)
							cardIdr.msg = "生成照片文件失败，请检查设定路径和磁盘空间！";
						else
							cardIdr.msg = "未知错误，错误代码" + result;

					}
				} catch (e) {
					cardIdr.code = false;
					cardIdr.msg = e;
				}
			}
			return cardIdr.code;

		};
	    // 设置控件值
		cardIdr.setCardInfo = function () {
		    try {
				if (typeof (CallCSharpMethodByResult) != "undefined") 
				{
					var ret = CallCSharpMethodByResult("IDCard","");
					var jsonstr = eval('(' + ret + ')');
					if (jsonstr.success) {
						document.getElementById("I_M_RY.SFZHM").value = jsonstr.IDCardNo;
						document.getElementById("I_M_RY.RYXM").value = jsonstr.Name;
						document.getElementById("I_M_RY.XB").value = jsonstr.Sex;
						document.getElementById("I_M_RY.MZ").value = jsonstr.Nation;
						var reg = /\./g;
						document.getElementById("I_M_RY.CSRQ").value = jsonstr.Born.replace(reg,"-");
						document.getElementById("I_M_RY.SFZDZ").value = jsonstr.Address;
						document.getElementById("I_M_RY.QFJG").value = jsonstr.GrantDept;
						document.getElementById("I_M_RY.SFZYXQ").value = jsonstr.UserLifeBegin+"-"+jsonstr.UserLifeEnd;
						document.getElementById("I_M_RY.ZP").value = jsonstr.Photo;
						if (document.getElementById("I_M_RY.ZPimg") != null)
						    document.getElementById("I_M_RY.ZPimg").value = "data:image/png;base64," + jsonstr.Photo;
					}else{
						cardIdr.msg = jsonstr.msg;
						cardIdr.code = false;
						alert(cardIdr.msg);
					}
				}else{
					if (!this.read()) {
						alert(this.msg);
						return;
					}

					document.getElementById("I_M_RY.SFZHM").value = this.sfzhm;
					document.getElementById("I_M_RY.RYXM").value = this.realname;
					document.getElementById("I_M_RY.XB").value = this.xb;
					document.getElementById("I_M_RY.MZ").value = this.mz;
					document.getElementById("I_M_RY.CSRQ").value = this.csny;
					document.getElementById("I_M_RY.SFZDZ").value = this.dz;
					document.getElementById("I_M_RY.QFJG").value = this.fzjg;
					document.getElementById("I_M_RY.SFZYXQ").value = this.yxq;
					document.getElementById("I_M_RY.ZP").value = this.zp;
					if (document.getElementById("I_M_RY.ZPimg") != null)
					    document.getElementById("I_M_RY.ZPimg").value = "data:image/png;base64,"+this.zp;
				}
		    } catch (e) {
		        alert(e);
		    }
		};
		return cardIdr;
	}
};
var g_cardidr = CardIdr.createNew("cardreader");
$(function () {
    $(document.body).append(g_cardidr.getControlString());
});


