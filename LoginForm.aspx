<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginForm.aspx.cs" Inherits="waRealTimeDB.LoginForm" %>

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
    <form id="form1" runat="server">
        <div>
            <div class="outerdiv"><div class ="middlediv">
            

            <table border="0" style="margin: 0% auto; padding: 0.55em">

                <tr>
                    <td style="padding-bottom: 1.25em;" colspan="2">
                        <h4 class="masthead-brand" style="color: #b1aaaa;">Dominos News editor login page</h4>
                    </td>
                </tr>


                <tr>
                    <td style="width: 100px; padding: 0.75em;">
                        <asp:Label ID="Label1" runat="server" Text="Login: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox class="form-control" type="text" placeholder="Name" ID="TextBoxLogin" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 0.75em;">
                        <asp:Label ID="Label2" runat="server" Text="Password: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox class="form-control" type="password" placeholder="Password" ID="TextBoxPassword" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="padding-top: 0.95em;">
                        <asp:Button class="btn btn-primary btnlogin" ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="padding-top: 0.95em;" colspan="2">
                        <asp:Label ID="LabelInfo" style="color: red;" runat="server" Text=""></asp:Label>
                        <div>Вход без пароля:</div>
                        <div><a class="login" href="#" onclick='<%= "window.location.replace(\"WebFormFind.aspx\");" %>'>Поиск ресторана по ID</a></div>
                    </td>
                </tr>



            </table>




            
            </div></div>
        </div>
    </form>
</body>
</html>
