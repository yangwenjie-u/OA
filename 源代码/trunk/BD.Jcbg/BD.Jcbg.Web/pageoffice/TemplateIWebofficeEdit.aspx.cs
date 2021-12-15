using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BD.WorkFlow.Common;
using BD.WorkFlow.DataModal.Entities;
using BD.WorkFlow.DataModal.VitrualEntities;
using BD.WorkFlow.IBll;
using Spring.Context;
using Spring.Context.Support;
using PageOffice;

namespace BD.WorkFlow.Web.pageoffice
{
    public partial class TemplateIWebofficeEdit : System.Web.UI.Page
    {
        int TemplateId
        {
            get { return DataFormat.GetSafeInt(ViewState["TemplateId"]); }
            set { ViewState["TemplateId"] = value; }
        }
        string DocType
        {
            get { return DataFormat.GetSafeString(ViewState["DocType"]); }
            set { ViewState["DocType"] = value; }
        }
        #region 服务
        private IWorkFlowService _workFlowService = null;
        private IWorkFlowService WorkFlowService
        {
            get
            {
                if (_workFlowService == null)
                {
                    IApplicationContext webApplicationContext = ContextRegistry.GetContext();
                    _workFlowService = webApplicationContext.GetObject("WorkFlowService") as IWorkFlowService;
                }
                return _workFlowService;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                TemplateId = DataFormat.GetSafeInt(Request["id"]);
                DocType = DataFormat.GetSafeString(Request["type"]); 
            }
            
        }

        protected void PageOfficeCtrl1_Load(object sender, EventArgs e)
        {
            try
            {
                // 设置PageOffice组件服务页面
                PageOfficeCtrl1.ServerPage = Request.ApplicationPath + "pageoffice/server.aspx";
                // 设置保存文件页面
                PageOfficeCtrl1.SaveFilePage = "/WorkFlow/SaveTemplateCotentWord?templateid="+TemplateId;
                // 打开文档
                //PageOfficeCtrl1.Caption = "";
                PageOfficeCtrl1.Theme = PageOffice.ThemeType.Office2010;

                PageOfficeCtrl1.OfficeToolbars = true;
                PageOfficeCtrl1.CustomToolbar = true;

                string url = "";
                if (TemplateId > 0)
                {
                    StFormTemplate template = WorkFlowService.GetFormTemplate(TemplateId);
                    if (template != null)
                        url = "/workflow/TemplateIWebOffice?OPTION=LOADFILE&RECORDID=" + template.Templateid;
                }
                //if (url == "")
                {
                    if (DocType.Equals(".doc", StringComparison.OrdinalIgnoreCase))
                        url = "/workflowtemplates/default.doc";
                    else if (DocType.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                        url = "/workflowtemplates/default.xls";

                }

                PageOfficeCtrl1.WebOpen(url, OpenModeType.docNormalEdit, WorkFlowUser.RealName);
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }
        }
    }
}