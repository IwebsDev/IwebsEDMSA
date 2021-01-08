<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreviewPrintingDevis.aspx.cs" Inherits="Galatee.Silverlight.Web.PreviewPrintingDevis" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function MINIMIZE_onclick() {

            try {
                alert("minimize");
                var control = document.getElementById("MainPlg");
                control.content.Page.ReduceFrame();

            } catch (e) {
                alert("error on invoking silverlight method   : " + e.Message);
            }
        }

        function CLOSE_onclick() {

            try {
                alert("close");
                var control = document.getElementById("MainPlg");
                control.content.Page.ReduceFrame();

            } catch (e) {
                alert("error on invoking silverlight method   : " + e.Message);
            }
        }

        function CloseFrame() {

            try {
                var control = document.getElementById("hiddenframe");
                //document.getElementById("GalateePreview").;
                //var divFrameHtml = parent.document.getElementById("RecouvrementDIV_IFRAME");
                parent.document.getElementById("DevisDIV_IFRAME").style.display = "none";
                //var divFrameHtml = document.getElementById(control.value);
                //alert(divFrameHtml.innerHTML);
                //this.setAttribute("visibility", "hidden");
                //this.setAttribute("width", "0px");
                //this.setAttribute("height", "0px");
                //divFrameHtml.setAttribute("z-index", "0");
                

            } catch (e) {
                alert("error on invoking silverlight method   : " + e.description);
            }
        }

        function SetWaitCursor() {

            document.body.style.cursor = "wait";
        }
        function SetDefaultCursor() {

            document.body.style.cursor = "default";
        }
// ]]>
    </script>
    <style type="text/css">


    </style>
   
    <link href="../forms.css" rel="stylesheet" type="text/css" />
   
</head>
<body id="bodyFrame" style=" background-image:url(reprt.jpg); background-repeat:repeat-x; background-color:#F2F8FC" bgcolor="#CC0000 ; Width:100%; height:100%;">
    <form id="form1" runat="server" style="Width:100%; height:100%; border:none; ">
       <asp:ScriptManager runat="server" id="scriptManager">
    </asp:ScriptManager>
        <div >  
            <input id="Button1" type="button" value="Fermer" onclick="CloseFrame();" runat="server" onserverclick="Button1_ServerClick"/>
            <rsweb:ReportViewer ID="GalateePreview" runat="server" Width="100%" 
            height="100%" style="float:left">
            </rsweb:ReportViewer>
        </div>
        <input id="hiddenframe" type="hidden" runat="server"  name="hiddenName"/>
    </form>
</body>
</html> 
