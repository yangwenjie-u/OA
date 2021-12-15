var Iris305 = {
    createNew: function (ctrlId, width, height, callback) {
        var irisobj = {
            ctrlid: ctrlId,     // 控件id
            hasInit: false,     // 是否已初始化
            width: width,		// 控件宽度
            height: height,		// 控件高度
            callback: callback	// 回调函数
        };
        // 控件js
        irisobj.getControlString = function () {
            return "<object  classid=\"clsid:BCC0CDFA-676A-43F2-B1D7-B4CD3FF72B6A\" id=\"" + this.ctrlid + "\"  style=\"width:" + this.width + "px;height:" + this.height + "px\" >" +
					"  <param name=\"_Version\" value=\"65536\"/>" +
					"  <param name=\"_ExtentX\" value=\"24262\"/>" +
					"  <param name=\"_ExtentY\" value=\"16219\"/>" +
					"  <param name=\"_StockProps\" value=\"0\"/>" +
					"</object>";
        };

        // 注册js

        irisobj.setRegisterEvent = function () {
            if (window.attachEvent) {
                document.getElementById(this.ctrlid).attachEvent("EnrollBothIrisStrEvent", irisobj.onRegisterEvent);
                document.getElementById(this.ctrlid).attachEvent("EnrollLeftIrisStrEvent", irisobj.onRegisterLeftEvent);
                document.getElementById(this.ctrlid).attachEvent("EnrollRightIrisStrEvent", irisobj.onRegisterRightEvent);
            }
            else if (window.addEventListener) {
                window.addEventListener("EnrollBothIrisStrEvent", irisobj.onRegisterEvent, false);
                window.addEventListener("EnrollLeftIrisStrEvent", irisobj.onRegisterLeftEvent, false);
                window.addEventListener("EnrollRightIrisStrEvent", irisobj.onRegisterRightEvent, false);
            }

        };
        irisobj.onRegisterEvent = function (sIrisLeft_Small, sIrisRight_Small, sIrisLeft_Big, sIrisRight_Big, sIrisLeft_I8, sIrisRight_I8, EnrollResult) {

            if (EnrollResult == 1) {
                callback(sIrisRight_Big + sIrisLeft_Big);
            }
            else {
                //$("#div_iris").dialog({ closed: true });
            }
        };
        irisobj.onRegisterLeftEvent = function (sIrisLeft_Small, sIrisLeft_Big, sIrisLeft_I8, sIrisRight_I8, EnrollResult) {

            if (EnrollResult == 1) {
                callback(sIrisLeft_Big);
            }
            else {
                //$("#div_iris").dialog({ closed: true });
            }
        };
        irisobj.onRegisterRightEvent = function (sIrisRight_Small, sIrisRight_Big, sIrisLeft_I8, sIrisRight_I8, EnrollResult) {

            if (EnrollResult == 1) {
                callback(sIrisRight_Big);
            }
            else {
                //$("#div_iris").dialog({ closed: true });
            }
        }
        // 初始化
        irisobj.init = function () {
            //if (this.hasInit)
            //	return this.hasInit;
            this.hasInit = false;

            try {
                var ctrlObj = document.getElementById(this.ctrlid);
                var iriscamera_params = "{\"camera_brightness\":\"8\",\"camera_contrast\":\"46\",\"camera_hue\":\"0\",\"camera_saturation\":\"56\",\"camera_sharpness\":\"5\",\"camera_gamma\":\"3\",\"camera_gain\":\"100\",\"camera_backlight\":\"950\",\"camera_exposure_auto\":\"0\",\"camera_exposure_manual_val\":\"0\"}";

                ctrlObj.SetCameraParams(0, iriscamera_params);

                ctrlObj.Burger = "{\"client_id\":\"gkzx\",\"capture_realtime_iris\":\"0\",\"with_big_iris\":\"1\",\"iris_with_bkcapture\":\"1\",\"iris_bkcapture_camera\":\"2\",\"capture_path\":\"d:\\\\sy305photoB\",\"bkcapture_path\":\"d:\\\\sy305photoA\"}";


                ctrlObj.EnableVoice = 1;
                ctrlObj.StringMode = 1;
                if (ctrlObj.InitIris != 1) {
                    alert("初始化虹膜设备失败");
                }
                else {
                    this.hasInit = true;

                }
            } catch (e) {
                alert(e);
            }
            return this.hasInit;
        };
        // 关闭
        irisobj.uninit = function () {
            try {
                var ctrlObj = document.getElementById(this.ctrlid);
                ctrlObj.CloseIris();
                ctrlObj.ClosePhotoCapture();
            } catch (e) {
            }
        };
        // 注册
        irisobj.enroll = function () {
            if (!this.init())
                return;
            try {
                /*
                $('#div_iris').dialog({
                    title: '虹膜注册',
                    width: 630,
                    height: 440,
                    closed: false,
                    cache: false,
                    modal: true
                });*/
                var ctrlObj = document.getElementById(this.ctrlid);
                ctrlObj.EnrollBothIris();
            }
            catch (e) {
                alert(e);
            }

        };
        irisobj.enrollLeft = function () {
            if (!this.init())
                return;
            try {
                var ctrlObj = document.getElementById(this.ctrlid);
                ctrlObj.EnrollLeftIris();
            }
            catch (e) {
                alert(e);
            }

        };
        irisobj.enrollRight = function () {
            if (!this.init())
                return;
            try {
                var ctrlObj = document.getElementById(this.ctrlid);
                ctrlObj.EnrollRightIris();
            }
            catch (e) {
                alert(e);
            }

        };
        irisobj.setEnrollQuality = function (id) {

            try {
                var ctrlObj = document.getElementById(this.ctrlid);
                var str = ctrlObj.GetEnrollIrisQuality();
                var arr = str.split(",");
                var arr1 = arr[0].split("-");
                var arr2 = arr[1].split("-");
                var q1 = arr1[32];
                var q2 = arr1[43];
                var q3 = arr2[32];
                var q4 = arr2[43];
                /*
                //if (q1*1<90 || q2*1<90 || q3*1<90 || q4*1<90)
                var tab = $('#tabfrm').tabs('getSelected');
                var tbId = tab.attr("id");
                //获取tab的iframe对象
                var tbIframe = $("#" + tbId + " iframe:first-child");
                var objfrm = tbIframe.get(0);
                var objdoc = objfrm.contentWindow.document;
                var texts = objdoc.getElementsByTagName("input");
                for (i = 0; i < texts.length; i++) {
                    if (texts[i].id.indexOf("." + id) > -1)
                        texts[i].value = q1 + ',' + q2 + ',' + q3 + ',' + q4;
                }*/
                SetCtrlValue(id, q1 + ',' + q2 + ',' + q3 + ',' + q4);
            }
            catch (e) {
                alert(e);
            }

        };
		irisobj.setEnrollQualityByStr = function (id, quantity) {

            try {
                var str = quantity;
                var arr = str.split(",");
                var arr1 = arr[0].split("-");
                var arr2 = arr[1].split("-");
                var q1 = arr1[32];
                var q2 = arr1[43];
                var q3 = arr2[32];
                var q4 = arr2[43];
                /*
                //if (q1*1<90 || q2*1<90 || q3*1<90 || q4*1<90)
                var tab = $('#tabfrm').tabs('getSelected');
                var tbId = tab.attr("id");
                //获取tab的iframe对象
                var tbIframe = $("#" + tbId + " iframe:first-child");
                var objfrm = tbIframe.get(0);
                var objdoc = objfrm.contentWindow.document;
                var texts = objdoc.getElementsByTagName("input");
                for (i = 0; i < texts.length; i++) {
                    if (texts[i].id.indexOf("." + id) > -1)
                        texts[i].value = q1 + ',' + q2 + ',' + q3 + ',' + q4;
                }*/
                SetCtrlValue(id, q1 + ',' + q2 + ',' + q3 + ',' + q4);
            }
            catch (e) {
                alert(e);
            }

        };
        return irisobj;
    }
};
/*
var g_irisreader = IrisReader.createNew("irisreader");
$(function () {
    $(document.body).append(g_irisreader.getControlString());
    //g_irisreader.init();
    $(window).bind('beforeunload', function () {
        g_irisreader.uninit();
    });
});
*/
var g_irisreader = Iris305.createNew("ctrl305", 0, 0, setIris);
$(function () {
    $('body').append(g_irisreader.getControlString());
    g_irisreader.setRegisterEvent();
    window.onunload = function () {
        //关闭虹膜
        //g_irisreader.uninit();
    }
});
function setIris(irisModel) {
    SetCtrlValue("I_M_RY.HM", irisModel);
    g_irisreader.setEnrollQuality('I_M_RY.HMZL');
    //g_irisreader.uninit();
}
function startEnroll() {
    var irisType = "0";
    layer.open({
        type: 1,
        title: '选择注册模式',
        shadeClose: true,
        shade: 0.8,
        area: ['400px', '200px'],
        content: "<br><br><center><select id='selIrisType'><option value='0'>双眼</option><option value='1'>左眼</option><option value='2'>右眼</option></select></center>",
        btn: ["开始注册", "取消"],
        success: function (layero, index) {
        },
        yes: function (index) {
            try {
                var irisType = $("#selIrisType").val();
                if (typeof (CallCSharpMethodByResult) != "undefined") {

                    if (irisType == "0") {
                        try {
                            var iris = CallCSharpMethodByResult('Iris', '1.0.0.16');
                            var jsonIris = eval('(' + iris + ')');
                            if (jsonIris.success) {
                                SetCtrlValue("I_M_RY.HM", jsonIris.sIrisLeftBig + jsonIris.sIrisRightBig);
								g_irisreader.setEnrollQualityByStr("I_M_RY.HMZL",jsonIris.EnrollIrisQuality);
                            }
                        } catch (err) {
                            alert(err);
                        }
                    }
                    else if (irisType == "1") {
                        var iris = CallCSharpMethodByResult('Iris', '1.0.0.16|LEFT');
                        var jsonIris = eval('(' + iris + ')');
                        if (jsonIris.success) {
                            SetCtrlValue("I_M_RY.HM", jsonIris.sIrisLeftBig);
							g_irisreader.setEnrollQualityByStr("I_M_RY.HMZL",jsonIris.EnrollIrisQuality);
                        }
                    }
                    else {
                        var iris = CallCSharpMethodByResult('Iris', '1.0.0.16|RIGHT');
                        var jsonIris = eval('(' + iris + ')');
                        if (jsonIris.success) {
                            SetCtrlValue("I_M_RY.HM", jsonIris.sIrisRightBig);
							g_irisreader.setEnrollQualityByStr("I_M_RY.HMZL",jsonIris.EnrollIrisQuality);
                        }
                    }

                } else {
                    if (irisType == "0")
                        g_irisreader.enroll();
                    else if (irisType == "1")
                        g_irisreader.enrollLeft();
                    else
                        g_irisreader.enrollRight();
                }
                layer.closeAll();
            } catch (err) {
                alert("err:" + err);
            }
        }
    });
}
