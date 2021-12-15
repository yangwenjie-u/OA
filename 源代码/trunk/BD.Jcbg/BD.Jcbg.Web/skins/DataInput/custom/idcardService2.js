
var str;
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
            yxq: "",             // 有效期
            zp: ""				// 照片
        };
        // 输出控件
        cardIdr.getControlString = function () {
            return "<object classid=\"clsid:5EB842AE-5C49-4FD8-8CE9-77D4AF9FD4FF\" id=\"" + cardIdr.ctrlid + "\" width=\"1\" height=\"1\" codebase=\"/cab/idr.cab\"></object><object classid='clsid:4B3CB088-9A00-4D24-87AA-F65C58531039' id='SynCardOcx1' codeBase='/cab/SynCardOcx.CAB#version=1,0,0,1' width='1' height='1' ></object>";
        };
        // 读取身份证信息
        cardIdr.read = function () {
            try {
                if (str > 0) {
                    SynCardOcx1.SetReadType(0);
                    var str1 = SynCardOcx1.ReadCardMsg();
                    if (str1 == 0) {
                        cardIdr.code = true;
                        cardIdr.realname = $.trim(SynCardOcx1.NameA);
                        cardIdr.mz = $.trim(SynCardOcx1.Nation);
                        cardIdr.xb = $.trim(SynCardOcx1.Sex);
                        cardIdr.csny = SynCardOcx1.Born;
                        cardIdr.sfzhm = $.trim(SynCardOcx1.CardNo);
                        cardIdr.dz = $.trim(SynCardOcx1.Address);
                        cardIdr.fzjg = $.trim(SynCardOcx1.Police);
                        cardIdr.yxq = $.trim(SynCardOcx1.UserLifeB + "-" + SynCardOcx1.UserLifeE);
                        cardIdr.sfzkh = "";
                        cardIdr.zp = "";
                    }
                }
                else {

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
                }
            } catch (e) {
                cardIdr.code = false;
                cardIdr.msg = e;
            }
            return cardIdr.code;

        };
        // 设置控件值
        cardIdr.setCardInfo = function (sjbmc) {
            try {
                var sfzhm = "";
                var RYXM = "";
                var zp = "";
                var XB = "";
                var mz = "";
                var csny = "";
                var dz = "";
                var fzjg = "";
                var yxq = "";
                var zp = "";
                if (typeof (CallCSharpMethodByResult) != 'undefined') {

                    var datasfz = CallCSharpMethodByResult("IdCard", "1|yyyy-MM-dd|75"); datasfz = eval('(' + datasfz + ')');
                    if (datasfz != "undefined" && datasfz.success) {
                        if (document.getElementById("I_M_WGRY.SFZHM")!=null && document.getElementById("I_M_WGRY.SFZHM").value == "") {
                            sfzhm = datasfz.IDCardNo;                          
                        }
                        else {
                            alert("读取失败");
                        }
                    }
                    else {
                        alert(data1.msg);

                    }
                }
                else {
                    if (!this.read()) {
                        alert(this.msg);
                        return;
                    }
                    if (sjbmc == "I_M_WGRY") {
                         sfzhm = this.sfzhm;
                         RYXM = this.realname;
                         zp = this.zp;
                         XB = this.xb;
                         mz = this.mz;
                         csny = this.csny;
                         dz = this.dz;
                         fzjg = this.fzjg;
                         yxq = this.yxq;                       
                    }
                }
                if (sfzhm != "")
                {
                    $.ajax({
                        type: "POST",
                        url: "/zj_info/getryinfo?sfzhm=" + encodeURIComponent(sfzhm),
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            // var msg="";	
                            if (data.code == "1") //人员库中无该人员
                            {
                                alert("人员库中无该人员，请在人员库中录入");
                                var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
                                var tablename = encodeURIComponent("I_M_RY_INFO"); 			// 表名
                                var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
                                var title = encodeURIComponent("人员信息"); 	// 标题
                                var buttons = encodeURIComponent("提交|TJ| "); // 按钮
                                var js = encodeURIComponent("userService.js,idcardService.js,irisService.js,bankset.js");

                                var rdm = Math.random();
                                var ryurl = "/datainput/Index?zdzdtable=" + zdzdtable +
                                    "&t1_tablename=" + tablename +
                                    "&t1_pri=" + tablerecid +
                                    "&t1_title=" + title +
                                    "&button=" + buttons +
                                    "&js=" + js +
                                    "&rownum=2" +
                                    "&preproc=InputCheckImryinfo|$i_m_ry_info.SFZHM" +
                                    "&LX=N" +
                                    "&_=" + rdm;

                                parent.layer.open({
                                    type: 2,
                                    title: '录入人员信息',
                                    shadeClose: false,
                                    shade: 0.8,
                                    area: ['95%', '95%'],
                                    content: ryurl,
                                    end: function () {

                                    }
                                });
                            }
                            else {
                                if (data.ryxm != "") {
                                    if (document.getElementById(sjbmc + ".RYBH") != null)
                                        document.getElementById(sjbmc + ".RYBH").value = data.rybh;
                                    if (document.getElementById(sjbmc + ".SFZHM") != null)
                                        document.getElementById(sjbmc + ".SFZHM").value = data.sfzhm;
                                    if (document.getElementById(sjbmc + ".RYXM") != null)
                                        document.getElementById(sjbmc + ".RYXM").value = data.ryxm;
                                    if (document.getElementById(sjbmc + ".XB") != null)
                                        document.getElementById(sjbmc + ".XB").value = data.xb;
                                    if (document.getElementById(sjbmc + ".CSRQ") != null)
                                        document.getElementById(sjbmc + ".CSRQ").value = data.csrq;
                                    if (document.getElementById(sjbmc + ".SJHM") != null)
                                        document.getElementById(sjbmc + ".SJHM").value = data.sjhm;
                                    if (document.getElementById(sjbmc + ".ZP") != null)
                                        document.getElementById(sjbmc + ".ZP").value = data.zp;
                                    if (document.getElementById(sjbmc + ".HM") != null)
                                        document.getElementById(sjbmc + ".HM").value = data.hm;
                                    if (document.getElementById(sjbmc + ".HMZL") != null)
                                        document.getElementById(sjbmc + ".HMZL").value = data.hmzl;
                                    if (document.getElementById(sjbmc + ".YHKYH") != null)
                                        document.getElementById(sjbmc + ".YHKYH").value = data.yhkyh;
                                    if (document.getElementById(sjbmc + ".YHKH") != null)
                                        document.getElementById(sjbmc + ".YHKH").value = data.yhkh;
                                    if (document.getElementById(sjbmc + ".SFZDZ") != null)
                                        document.getElementById(sjbmc + ".SFZDZ").value = data.sfzdz;
                                    if (document.getElementById(sjbmc + ".QFJG") != null)
                                        document.getElementById(sjbmc + ".QFJG").value = data.qfjg;
                                    if (document.getElementById(sjbmc + ".SFZYXQ") != null)
                                        document.getElementById(sjbmc + ".SFZYXQ").value = data.sfzyxq;
                                }
                            }

                        },
                        complete: function (XMLHttpRequest, textStatus) {
                        },
                        beforeSend: function (XMLHttpRequest) {
                        }
                    });
                }
               

            } catch (e) {
                alert(e);
            }
        };
        cardIdr.checksfzh = function () {
            try {
                if (!this.read()) {
                    alert(this.msg);
                    return;
                }
                var sfz = document.getElementById("I_M_WGRY.SFZHM").value;

                var rdsfz = this.sfzhm;
                if (sfz == rdsfz)
                    alert("身份证匹配");
                else
                    alert("身份证不匹配:" + rdsfz);
            } catch (e) {
                alert(e);
            }
        };
        return cardIdr;
    }
};
var g_cardidr = CardIdr.createNew("cardreader");
$(function () {
    if (typeof (CallCSharpMethodByResult) != 'undefined')
    { }
    else {
        $(document.body).append(g_cardidr.getControlString());
        str = SynCardOcx1.FindReader();
        if (str > 0) {
            //str = SynCardOcx1.GetSAMID();
            SynCardOcx1.SetPhotoPath(0, "");
            SynCardOcx1.SetPhotoType(1);
            SynCardOcx1.SetPhotoName(2);
            SynCardOcx1.SetNationType(1);
            SynCardOcx1.SetBornType(3);
            SynCardOcx1.SetSexType(1);
            SynCardOcx1.SetUserLifeBType(3);
        }
    }
});


