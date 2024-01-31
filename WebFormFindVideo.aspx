<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormFindVideo.aspx.cs" Inherits="waRealTimeDB.WebFormFindVideo" %>

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

        <asp:SqlDataSource ID="SqlDataSourceStore" runat="server" ConnectionString="" SelectCommand="SELECT 110 ip110, * FROM [Store]"></asp:SqlDataSource>


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
                    <div class="row" style="margin-bottom: 0.5rem; margin-top: 0.7rem; <% =styleOrd %>">
                        <div class="col form-check text-center">
                            <input class="form-check-input" type="checkbox" value="" onchange="checkVideoEdit(event)" style="margin-top: .4em;" id="checkboxVideo">
                            <label class="form-check-label" for="checkboxVideo">Включить редактирование</label>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 0.5rem; margin-top: 0.7rem;">
                        <div class="col-3 center">Find:</div>
                        <div class="col-8" style="padding: 0;"><asp:TextBox ID="TextBoxFindVideo" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name" runat="server"></asp:TextBox></div>
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

                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-Width="40%" />

                    <asp:TemplateField ItemStyle-CssClass="ip110link_del" HeaderStyle-CssClass="ip110link_del" HeaderText="IP">
                        <ItemTemplate>
                            <asp:HyperLink id="HyperLinkIP110" 
                                           class="tdnone"
                                           NavigateUrl='<%# (waRealTimeDB.staticFunctions.WordsCount(Eval("ip").ToString(), ".") != 3) ? "" : ((Eval("ip110") == DBNull.Value) ? Eval("ip") : Eval("ip110", "http://{0}")) %>'                            
                                           Text='<%# (Eval("ip110") == DBNull.Value) ? Eval("ip") : String.Format("{0}", Eval("ip110")) %>'
                                           Target="_blank"
                                           runat="server" />                        
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--
                    <asp:TemplateField ItemStyle-CssClass="ip110edit stredit"  HeaderStyle-CssClass="ip110edit" HeaderText="IP" >
                        <ItemTemplate>
                            <div class="" style="height: 1.3em;">
                            <asp:TextBox 
                                ID="TextBoxHeader"
                                onkeydown='<%# "if (event.keyCode === 13) {valChangeVideoIp(event, " +Eval("ID")+ ")} else {};" %>'
                                Text='<%# Eval("ip") %>' style="padding: .0rem .2rem; margin: -0.15rem 0; border: none; background-color: inherit; color: black;" class="form-control" placeholder="" runat="server">
                            </asp:TextBox>
                            
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    --%>



                    <asp:BoundField DataField="nvname" HeaderText="Info" ItemStyle-CssClass="ip110link" HeaderStyle-CssClass="ip110link" />

                    <asp:TemplateField ItemStyle-CssClass="ip110edit stredit"  HeaderStyle-CssClass="ip110edit" HeaderText="Info" >
                        <ItemTemplate>
                            <div class="" style="height: 1.3em;">
                            <asp:TextBox 
                                ID="TextBoxHeader3"
                                onkeydown='<%# "if (event.keyCode === 13) {valChangeNewsVideo(event, " +Eval("ID")+ ")} else {};" %>'
                                Text='<%# Eval("nvname") %>' style="padding: .0rem .2rem; margin: -0.15rem 0; border: none; background-color: inherit; color: black;" class="form-control" placeholder="" runat="server">
                            </asp:TextBox>
                            
                            </div>
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