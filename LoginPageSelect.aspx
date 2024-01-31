<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPageSelect.aspx.cs" Inherits="waRealTimeDB.LoginPageSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="Cache-Control" content="no-cache" />
    <title>Dominos News editor</title>

	
    <!-- Bootstrap core CSS -->
    <link href="dist/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="dist/cover.css" rel="stylesheet">

    <!-- <script src="dist/myFunc.js"></script> -->

</head>
<body>
    <form id="form1" runat="server" defaultbutton="ButtonNews">
        <div>
            <div class="outerdiv-menu"><div class ="middlediv">


            <table border="0" style="margin: 0% auto; padding: 0.55em; text-align: center;">

                <tr>
                    <td style="padding-bottom: 1.25em;">
                        <h4 class="masthead-brand" style="color: #b1aaaa;">Dominos Editor select page</h4>
                    </td>
                </tr>



                <tr>
                    <td style="padding-top: 0.5em;">
                        <asp:Button class="btn btn-success btn-menu-width" ID="ButtonNews" runat="server" Text="News Editor" OnClick="ButtonNews_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu">
                        <asp:Button class="btn btn-primary btn-menu-width" ID="ButtonTarget" runat="server" Text="Target Editor" OnClick="ButtonTarget_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu">
                        <asp:Button class="btn btn-warning btn-menu-width" ID="ButtonDriver" runat="server" Text="Safety Editor" OnClick="ButtonDriver_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu">
                        <hr class="mt-0 mb-0" style="width: 80%;">
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu">
                        <asp:Button class="btn btn-success btn-menu-width" ID="ButtonFind" runat="server" Text="Find restaurant" OnClick="ButtonFind_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu">
                        <asp:Button class="btn btn-primary btn-menu-width" ID="ButtonFindVideo" runat="server" Text="Video viewer" OnClick="ButtonFindVideo_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu" style="<% =styleOrd %>">
                        <asp:Button class="btn btn-danger-my btn-menu-width" ID="ButtonFindChange" runat="server" Text="Change RT" OnClick="ButtonFindChange_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="btnmenu">
                        <asp:Button class="btn btn-warning btn-menu-width" ID="ButtonFindDiscrepancy" runat="server" Text="Find discrepancy" OnClick="ButtonFindDiscrepancy_Click" />
                    </td>
                </tr>


                <tr>
                    <td style="padding-top: 1.25em;text-align: center;">
                        <div style="color: #b1aaaa;">Dominos Pizza Editor application <%=DateTime.Now.ToString("yyyy")%></div>
                    </td>
                </tr>
                

            </table>



            </div></div>
        </div>
    </form>
</body>
</html>
