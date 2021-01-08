<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Galatee.Silverlight.Web.index" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>iWebs EDM</title>
   <style type="text/css">
    html, body {
	    height: 100%;
	    overflow:hidden;
    }
    body {
	    padding: 0;
	    margin: 0;
         background-color:#606060;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
          background-color:#606060;
    }
    </style>
    <script type="text/javascript" src="jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">

       function CreatePluginModule(name) {

            try {
                Silverlight.createObjectEx({
                    source: "ClientBin/Galatee.ModuleLoader.xap",
                    parentElement: document.getElementById(name),
                    id: name + "plugin",
                    properties: {
                        width: "100%",
                        height: "100%",
                        background: "white"//,
                        //top: "50px"
                    },
                    events: {}
                });
            } catch (e) {
                alert("Erreur creation plugin");

            }
        }

        function CreatePluginMainFrames(name) {

            try {
                Silverlight.createObjectEx({
                    source: "ClientBin/Galatee.Silverlight.xap",
                    parentElement: document.getElementById(name),
                    id: name + "plugin",
                    properties: {
                        width: "100%",
                        height: "100%",
                        background: "white",
                        position: "absolute"//,
                        //top: "0px"
                    },
                    events: {}
                });
            } catch (e) {
                alert("Erreur creation plugin");

            }
        }

        function CreatePluginMainFrame(name, silverlight) {

            try {
                var parentId = "Content";
                Silverlight.createObjectEx({
                    source: "ClientBin/Galatee.Silverlight.xap",
                    parentElement: document.getElementById(name),
                    id: silverlight + "pgl",
                    properties: {
                        width: "100%",
                        height: "100%",
                        background: "white"
                    },
                    events: {}
                });
            } catch (e) {
                alert("Erreur creation plugin");

            }
        }

        function CreatePluginMainFrame(name) {

            try {
                Silverlight.createObjectEx({
                    source: "ClientBin/Galatee.Silverlight.xap",
                    parentElement: document.getElementById(name),
                    id: name + "pgl",
                    properties: {
                        width: "100%",
                        height: "100%",
                        background: "white"
                    },
                    events: {}
                });
            } catch (e) {
                alert("Erreur creation plugin");

            }
        }
        function invokeSilverlight() {

            try {
                var control = document.getElementById("MainPlg");
                control.content.Page.ReduceFrame();

            } catch (e) {
                alert("error on invoking silverlight method   : " + e.Message);
            }

        }

        function TEST() {

            try {
                alert(" on test iframe");
            } catch (e) {
                alert("error on invoking silverlight method   : " + e.Message);
            }

        }

        $(window).bind('beforeunload', function () {

            try {
                var control = document.getElementById("MainPlg");
                control.content.Page.InvokeOnCloseNavigator();

            } catch (e) {
                alert("Erreur fermeture plugin");

            }

        });


    </script>
    <script type="text/javascript">

       
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
                alert("SMS ERROR : " + errMsg);
            }

            throw new Error(errMsg);
        }

        function CloseFrame() {

            try {
                var control = document.getElementById("hiddenframe");
                //var divFrameHtml = document.getElementById("RecouvrementDIV_IFRAME");
                document.getElementById("RecouvrementDIV_IFRAME").style.display = "none";
                //var divFrameHtml = document.getElementById(control.value);
                //alert(divFrameHtml.innerHTML);
                //divFrameHtml.setAttribute("visibility", "hidden");
                //divFrameHtml.setAttribute("width", "0px");
                //divFrameHtml.setAttribute("height", "0px");
                //divFrameHtml.setAttribute("z-index", "0");


            } catch (e) {
                alert("error on invoking silverlight method   : " + e.description);
            }
        }

    </script>
</head>
<body>
    <div style=" background:center; height:55px; width:100%"> <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/logogala.png" Width="200px" /> 
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input type = "button" value = " Déconnexion " onclick = "window.location.reload()" style="position:fixed ;  height:auto;width:auto"  /></div>  
    
    
    
        
   <form id="form1" runat="server" style="height:100%;overflow:hidden">
    
  
        <table style="width: 100%; height: 100%">
<tr>
<td id="ModuleDivTD" style="width:0%;height:0%">
<div id="ModuleDiv" style="width:100%;height:100%;background-color:#4E4E4E;margin-top:1%;">
       
</div>
</td>
<td id="MainPlgTD" style="width:100%;overflow:hidden">
<div id="black" style="position:absolute;top:50px;width:100%;height:95%;background-color:#606060;overflow:hidden;">
     <div id="div4" style="width:100%;height:96%; background-color:#EFEFF2;min-height:400px;position:absolute;top:10px; z-index:-10; left: 0px;">
            <object style="" id="MainPlg" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/Galatee.Silverlight.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="#606060" />
		  <param name="minRuntimeVersion" value="4.0.50401.0" />
		  <param name="autoUpgrade" value="true" />
          <%--<param name="initParams" value="<%= string.Format("HostName={0},HostIP={1}", HostName, UserIp) %>" />--%> 
          <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object><iframe id="IFRAME" style="visibility:hidden;height:0px;width:0px;border:0px" name="I1">
        </iframe>
      </div>
</div>
</td>
</tr>
</table>
    </form>

    <script type = "text/javascript">
        window.onload = function () {
            document.onkeydown = function (e) {
                return (e.which || e.keyCode) != 116;
            };
        }

        window.onbeforeunload = function () {
            return "Le raffraichissement peut provoquer la perte de certaines données";
        }

</script>

</body>
</html>

