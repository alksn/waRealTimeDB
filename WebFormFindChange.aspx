<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormFindChange.aspx.cs" Inherits="waRealTimeDB.WebFormFindChange" %>

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
    <!-- Datepicker -->
    <link rel="stylesheet" href="dist/jquery-ui/jquery-ui.css">

    <script type="text/javascript" src="dist/js/jquery.min.js"></script>

    <script src="dist/myFunctions.js"></script>
    <script src="dist/find.js"></script>
    <script src="dist/find-jquery.js"></script>       
    <script src="dist/jquery-ui/jquery-ui.js"></script>

    <script>
    </script>

	
</head>
<body>
    <div class="site-wrapper">
    <div class="site-wrapper-inner">


    <form id="form1" runat="server" defaultbutton="ButtonSubmit">
        <asp:Button ID="ButtonSubmit" style="display: none;" runat="server" Text="Button" UseSubmitBehavior="False" OnClientClick="if (eventArgs.KeyCode == 13) return false;" />

        <asp:SqlDataSource ID="SqlDataSourceStore" runat="server" ConnectionString="" SelectCommand="SELECT * FROM [Store]"></asp:SqlDataSource>


        <div class="cover-container">



          <div class="masthead0 clearfix" style="margin: auto;text-align: center;">
            <div class="inner" style="padding-bottom: 0;">
              <h3 class="masthead-brand0" style="color: #b1aaaa; /*! position: absolute; */">
                  <asp:Label ID="LabelAppName" runat="server" Text=""></asp:Label>
              </h3>

              <div class="menuline0" style="/*! float: right; */ margin-top: 0.4em; margin-left: -1em; color: #ddd;">
                <a class="menulink displaynone" href="#" onclick='redirectPost("WebFormEditor.aspx", {isLogin: "False"})'>News</a>&nbsp;
                <a class="menulink displaynone" href="#" onclick='redirectPost("WebFormStatusEditor.aspx", {isLogin: "False"})'>Status</a>&nbsp;
                <a class="menulink displaynone" href="#" onclick='redirectPost("WebFormDriverEditor.aspx", {isLogin: "False"})'>Safety</a>
                <a class="menulink displaynone" href="#" onclick='<%= "redirectPost(\"WebFormDriverEditor.aspx\", {isLogin: \"" + false.ToString() + "\"})" %>'>Safety</a>

                Поиск ресторана по ID
                <a class="menuexit" href="#" onclick='<%= "window.location.replace(\"" + RedirectPage + "\");" %>'>Выход</a>
              </div>

            </div>
          </div>


          <div class="inner cover">



		  <table border="0" style="margin: auto;">

          <tr>
              <td>
                    <div class="row" style="margin-bottom: 0.5rem; margin-top: 0.7rem;">
                        <div class="col-2 center">Find:</div>
                        <div class="col-6" style="padding: 0;"><asp:TextBox ID="TextBoxFind2" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name" runat="server"></asp:TextBox></div>
                        <div class="col-4 center">
                            <input type="button" class="btn btn-outline-primary btn-sm" onclick="btnPasswordChange(this, event)" value="Сменить пароль">
                        </div>
                    </div>
              </td>
              <td></td>
          </tr>




		  <tr>
			<td style="width: 0%; vertical-align: top;">


            

            <asp:GridView class="table table-striped table-bordered table-find-filter" style="line-height: 1;" ID="GridViewStores" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSourceStore">
                <Columns>
                    
                    <%-- id строки, используется в множественном выборе --%>
                    <asp:BoundField DataField="ID" HeaderText="ID-hidden" ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone" />


                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:HyperLink id="HyperLinkID" 
                                           NavigateUrl='<%# Eval("ID","http://ml.dominospizza.mobi/default.aspx?StoreID={0}&PeriodType=1") %>'                            
                                           Text='<%#Eval("Code")%>'
                                           Target="_blank"
                                           runat="server" />                        
                        </ItemTemplate>
                    </asp:TemplateField>


                    
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />

                    <asp:BoundField DataField="surname" HeaderText="Surname" />
                    <asp:BoundField DataField="login" HeaderText="Login" />
                    <asp:BoundField DataField="email" HeaderText="Email" />


                    <asp:TemplateField ShowHeader="True" Visible="True">

                        <HeaderTemplate>
                            <input type="checkbox" id="CheckBoxCheckAll">
                        </HeaderTemplate>

                        <ItemTemplate>

                            <input type="checkbox" value='<%# Eval("surname") %>' title=''>

                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
            </asp:GridView>
             
            
            </td>

		  </tr>
		  </table>


          </div>




          <div class="mastfoot">
            <div class="inner" style="margin: auto;width: 300px;">
              <p>Template for <a href="https://dominospizza.ru">Dominos Pizza</a><br /><asp:Label ID="LabelVersion" runat="server" Text=""></asp:Label></p>
            </div>
          </div>




        </div>

    </form>



    </div>
    </div>
</body>
</html>
