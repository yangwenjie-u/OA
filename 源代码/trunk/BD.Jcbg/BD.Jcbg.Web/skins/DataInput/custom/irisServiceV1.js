var IrisReader = {
    createNew: function (ctrlId) {
        var irisReader = {
            ctrlid: ctrlId,     // 控件名称
            hasInit: false,      // 是否初始化
            code: false, 		// 函数操作返回代码
            msg: "", 			// 函数操作错误信息
            userid: "",          //用户id
            irisdata: ""         // 虹膜模板

        };
        // 输出控件
        irisReader.getControlString = function () {
            return "<object  classid=\"clsid:4753D079-DC16-4C37-B621-D3ED258A728A\" id=\"" + this.ctrlid + "\"  width=\"1\" height=\"1\" >"
	               + "<param name=\"_Version\" value=\"65536\"/>"
	               + "<param name=\"_ExtentX\" value=\"24262\"/>"
	               + "<param name=\"_ExtentY\" value=\"16219\"/>"
	               + "<param name=\"_StockProps\" value=\"0\"/>"
	               + "</object>";
        };

        // 输出事件
        irisReader.getEventString = function () {
            return "<script language=\"javascript\" type=\"text/javascript\" for=\"" + this.ctrlid + "\" event=\"BioIDEnrollBothIrisBEvent(irisleft,irisright,imgleft,imgright,g_irisresult)\">\r\n"
                + "if(g_irisresult == 1){\r\n"
			    + "\tg_irisreader.setIrisInfo(irisleft + irisright);\r\n"
        	    + "}\r\n"
        		+ "else if(g_irisresult = 0)\r\n"
		        + "{\r\n"
           		+ "\talert(\"超时失败\");\r\n"
		        + "}\r\n"
		        + "\telse if(g_irisresult = -1)\r\n"
        		+ "{\r\n"
    		    + "\talert(\"注册失败\");\r\n"
		        + "}\r\n"
        	    + "</script>\r\n";
        };
        // 初始化控件
        irisReader.init = function () {
            this.code = true;
            this.msg = "";
            try {
                if (this.hasInit)
                    return this.code;
                if (document.getElementById(this.ctrlid).BioIDCameraInit() != 1) {
                    this.code.msg = "虹膜注册设备初始化失败";
                    this.code = false;
                }
                else {
                    this.hasInit = true;
                    document.getElementById(this.ctrlid).BioIDCameraHMIVoiceOpen();
                    document.getElementById(this.ctrlid).SaveImageEnable = false;
                }
            } catch (e) {
                this.code = false;
                this.msg = e;
            }
            if (!this.code) {
                alert(this.msg);
            }

            return this.code;
        };
        // 反初始化
        irisReader.uninit = function () {
            this.code = true;
            this.msg = "";
            try {
                if (this.hasInit) {
                    document.getElementById(this.ctrlid).BioIDCameraClose();
                    this.hasInit = false;
                }
            } catch (e) {
                this.code = false;
                this.msg = e;
            }
            return this.code;
        };
        // 虹膜注册
        irisReader.enroll = function () {

            this.code = true;
            this.msg = "";
            try {
                if (!this.init()) {
                    this.code = false;
                    return this.code;
                }
                document.getElementById(this.ctrlid).BioIDEnrollBothIrisB();

            }
            catch (e) {
                this.code = false;
                this.msg = e;
            }
            return this.code;
        };
        // 注册后赋值
        irisReader.setIrisInfo = function (irisModule) {
            document.getElementById("I_M_RY_INFO.HM").value = irisModule;

        }
        return irisReader;
    }
};
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
            if (window.attachEvent) 
                document.getElementById(this.ctrlid).attachEvent("EnrollBothIrisStrEvent", irisobj.onRegisterEvent);
            else if (window.addEventListener) 
                window.addEventListener("EnrollBothIrisStrEvent", irisobj.onRegisterEvent, false);
            
        };
        irisobj.onRegisterEvent = function(sIrisLeft_Small, sIrisRight_Small, sIrisLeft_Big, sIrisRight_Big, sIrisLeft_I8, sIrisRight_I8, EnrollResult){
            
            if (EnrollResult == 1) {
                callback(sIrisRight_Big + sIrisLeft_Big);
            }
            else {
                alert("注册失败");
            }
        }
        // 初始化
        irisobj.init = function () {
            //if (this.hasInit)
            //	return this.hasInit;
            this.hasInit = false;

            try {
                var ctrlObj = document.getElementById(this.ctrlid);
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
            if(typeof(CallCSharpMethodByResult)!='undefined'){

            var data=CallCSharpMethodByResult("Iris","");data=eval('('+data+')');
            if(data.success){
            document.getElementById("I_M_RY_INFO.HM").value=data.sIrisLeftBig+data.sIrisRightBig;
            }
            else{
                var msg=data.msg;
                if(msg!=null){
                alert(msg);
                }
            }
            }
            else{
            if (!this.init())
                return;
            try {
                var ctrlObj = document.getElementById(this.ctrlid);
                ctrlObj.EnrollBothIris();
            }
            catch (e) {
                alert(e);
            }

        };
}
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
    SetCtrlValue("I_M_RY_INFO.HM",irisModel);
    //g_irisreader.uninit();
}