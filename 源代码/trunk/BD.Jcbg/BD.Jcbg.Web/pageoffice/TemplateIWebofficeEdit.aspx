<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateIWebofficeEdit.aspx.cs" Inherits="BD.WorkFlow.Web.pageoffice.TemplateIWebofficeEdit" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228"
    Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script language="javascript" type="text/javascript">
        function Save() {
            document.getElementById("PageOfficeCtrl1").WebSave();
            return document.getElementById("PageOfficeCtrl1").CustomSaveResult;
        }
    </script>
</head>
<body style="margin:0px 0px 0px 0px">
    <form id="form1" runat="server">
    <div style=" width:auto; height:700px;">
        <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server" CustomToolbar="False" 
            onload="PageOfficeCtrl1_Load">
        </po:PageOfficeCtrl>
    </div>
    </form>
</body>
</html>
