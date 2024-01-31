<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormDriverEditor.aspx.cs" Inherits="waDriver.WebFormDriverEditor" %>

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



          <div class="masthead clearfix">
            <div class="inner">
              <h3 class="masthead-brand" style="color: #b1aaaa; position: absolute;">
                  <asp:Label ID="LabelAppName" runat="server" Text=""></asp:Label>
              </h3>
                <%--
              <div style="float: right; margin-right: 1.8em; margin-top: 0.2em;">
                  <div style="position: absolute;">
                      <button type="button" 
                          onclick='<%= "window.location.replace(\"" + RedirectPage + "\");" %>'
                          class="btn btn-sm btn-outline-secondary">Выход</button>
                  </div>
              </div>
                --%>

              <div class="menuline" style="float: right; margin-top: 0.4em;">
                <a class="menulink" href="#" onclick='redirectPost("WebFormEditor.aspx", {isLogin: "True"})'>News</a>&nbsp;
                <a class="menulink" href="#" onclick='redirectPost("WebFormStatusEditor.aspx", {isLogin: "True"})'>Status</a>&nbsp;
                <a class="menulink" href="#" onclick='redirectPost("WebFormDriverEditor.aspx", {isLogin: "True"})'>Safety</a>

                <a class="menuexit" href="#" onclick='<%= "window.location.replace(\"" + RedirectPage + "\");" %>'>Выход</a>
              </div>

            </div>
          </div>


          <div class="inner cover">



		  <table border="0">



          
          <!--
          <tr>
              <td>
                    <div class="row">
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">1</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">2</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">3</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">4</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">5</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">6</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">7</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">8</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">9</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">0</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">1</div>
                        <div class="col-1" style="border-width: 1px;border: solid 1px;">2</div>
                    </div>
              </td>
          </tr>

          <!--
          <tr>
              <td>
                    <div class="row">
                        <div class="col-1" style="align-self: center; text-align: center;">Find:</div>
                        <div class="col-2" style="padding: 0;"><input type="text" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Code"></div>
                        <div class="col-1" style="align-self: center; text-align: center;">Text:</div>
                        <div class="col-3" style="padding: 0;"><input type="text" style="width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name"></div>                        
                        <div class="col-1"><input type="submit" value="Save" style="align-self: center;"></div>
                        <div class="col-1" style="align-self: center; text-align: center;">Date:</div>
                        <div class="col-3" style="padding: 0;"><input type="text" onchange="dtChange()" style="border-radius: 0.5rem; width: 82%; padding: .3rem .75rem;" class="form-control open_datepicker" placeholder="Select date"></div>
                    </div>
              </td>
          </tr>
          -->

          <tr>
              <td>
                    <div class="row" style="margin-bottom: 0.5rem; margin-top: 0.7rem;">
                        <div class="col-1 center">Find:</div>
                        <div class="col-2" style="padding: 0;"><asp:TextBox ID="TextBoxFind2" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name" runat="server"></asp:TextBox></div>
                        <div class="col-1 center">Text:</div>
                        <div class="col-3" style="padding: 0;"><asp:TextBox ID="TextBoxNewsName" class="form-control" style="padding: .2rem .75rem;line-height: inherit;" placeholder="Text" runat="server"></asp:TextBox></div>
                        <div class="col-1 center"><asp:Button ID="ButtonSaveDriverText" runat="server" Text="Save" OnClick="ButtonSaveDriverText_Click" /></div>
                        <div class="col-1 center">Date:</div>
                        <div class="col-3" style="padding: 0;">
                            <asp:TextBox ID="TextBoxNewsDate" onchange="dtChange()" style="border-radius: 0.5rem; width: 82%; padding: .3rem .75rem;" class="form-control open_datepicker" placeholder="Select date" runat="server"></asp:TextBox>
                            <asp:Button ID="ButtonDateChange" style="display:none" runat="server" Text="DateSaveButton" OnClick="ButtonDateChange_Click" />
                        </div>
                    </div>
              </td>
          </tr>

          <%--
          <tr>
              <td>
                  <table class ="table" style="margin-bottom: 0.1rem; margin-top: 0.2rem;">
                      <tr>
                          <%--
                          <td style="width: 70px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label3" runat="server" Text="Code: "></asp:Label>
                          </td>
                          <td style="width: 17%;">
                              <asp:TextBox ID="TextBoxFindCode" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Code"  runat="server"></asp:TextBox>
                          </td>
                          --%><%--

                          <td style="width: 70px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label7" runat="server" Text="Find: "></asp:Label>
                          </td>
                          <td style="width: 20%;">
                              <asp:TextBox ID="TextBoxFind2_del" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name"  runat="server"></asp:TextBox>
                          </td>

                          <td style="width: 70px; vertical-align: middle; text-align: right;">
                              <div>Text: </div>
                          </td>
                          <td style="width: 0%;">
                              <asp:TextBox ID="TextBoxNewsName_del" class="form-control" style="padding: .2rem .75rem;line-height: inherit;" placeholder="Text" runat="server"></asp:TextBox>
                          </td>
                          <td style="width: 0%; vertical-align: middle;">
                              <asp:Button ID="ButtonSaveDriverText_del" runat="server" Text="Save" OnClick="ButtonSaveDriverText_Click" />
                          </td>

                          
                          <td style="width: 55px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label1" runat="server" Text="Date: "></asp:Label>
                          </td>
                          <td style="width: 0%;">
                              <asp:TextBox ID="TextBoxNewsDate_del" onchange="dtChange()" style="border-radius: 0.5rem; width: 90%; padding: .3rem .75rem;" class="form-control open_datepicker" placeholder="Select date" runat="server"></asp:TextBox>
                              <asp:Button ID="ButtonDateChange_del" style="display:none" runat="server" Text="DateSaveButton" OnClick="ButtonDateChange_Click" />
                          </td>

                      </tr>

                  </table>
                  
              </td>
          </tr>
          --%>


		  <tr>
			<td style="width: 40%; vertical-align: top;">


            

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

                    <%--
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                    --%>
                    
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <%--
                    <asp:BoundField DataField="NewsName" HeaderText="News" SortExpression="NewsName" />
                    --%>

                    <asp:TemplateField ItemStyle-CssClass="strlimit" HeaderText="Header">
                        
                        <%--
                        <ItemTemplate>
                            <div class="" style="height: 1em;">
                            <asp:Label  ID="LabelNewsField" runat="server" Text='<%# "" + Eval("NewsName") + "&nbsp&nbsp&nbsp" %> '></asp:Label>
                            </div>

                        </ItemTemplate>
                        --%>


                        <ItemTemplate>
                            <div class="" style="height: 1em;">                            

                            <asp:TextBox 
                                ID="TextBoxHeader"
                                onkeydown='<%# "if (event.keyCode === 13) {valChangeNewsDriver(event, " +Eval("ID")+ ",\""+TextBoxNewsDate.Text+"\")} else {};" %>'
                                Text='<%# Eval("NewsName") %>' style="padding: .0rem .2rem; margin: -0.15rem 0; border: none; background-color: inherit; color: black;" class="form-control" placeholder="" runat="server">
                            </asp:TextBox>
                            
                            </div>

                        </ItemTemplate>


                        
                    </asp:TemplateField>

                    <%--
                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Value" ItemStyle-Width="30%">
                        
                        <ItemTemplate>
                            <div class="" style="height: 1em;">                            

                            <%--'<%#
                            <asp:TextBox ID="TextBoxValue" onkeydown="if (event.keyCode === 13) { valChange(event, 'test-text') } else { };" onchange="" Text='<%# !String.IsNullOrEmpty(Eval("Val").ToString()) ? Convert.ToInt32(Eval("Val")) : Eval("Val")  %>' style="padding: .0rem .4rem; margin: -0.1rem 0; border: none;" class="form-control" placeholder="no value"  runat="server" ToolTip='<%#Eval("ID")%>'></asp:TextBox>
                            -%>
                            <asp:TextBox 
                                ID="TextBoxValue" 
                                onkeydown='<%# "if (event.keyCode === 13) {valChange(event, " +Eval("ID")+ ",\""+TextBoxNewsDate.Text+"\")} else {};" %>'                                 
                                onchange="" Text='<%# !String.IsNullOrEmpty(Eval("Val").ToString()) ? Convert.ToInt32(Eval("Val")) : Eval("Val")  %>' style="padding: .0rem .4rem; margin: -0.1rem 0; border: none;" class="form-control" placeholder="no value"  runat="server">
                            </asp:TextBox>
                            

                            </div>

                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    --%>

                    <asp:BoundField DataField="publishingTime" HeaderText="Date" ItemStyle-Width="30%" DataFormatString="{0:dd.MM.yyyy}" />

                    
                    <asp:TemplateField ShowHeader="True" Visible="True">

                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" AutoPostBack="True" OnCheckedChanged="btnSelectAll_Click" />
                        </HeaderTemplate>

                        <ItemTemplate>
                            
                            <asp:CheckBox ID="CheckBoxStore" runat="server" OnCheckedChanged="CheckBoxStore_Click" AutoPostBack="True" ToolTip='<%# Eval("ID")+"|"+ Eval("ord")+"|"+ Eval("name")+"|"+ Eval("publishingTime")+"|"+ Eval("newsName")+"|" +Eval("code") %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    
                    

                    <%-- пример
                    <asp:TemplateField>
                        <ItemTemplate>
                            
                            <asp:LinkButton ID="LinkView" runat="server" CommandArgument='<%# Eval("ID")+","+ Eval("Name") %>' OnClick="linkEdit_Click">LinkButton</asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>
                    --%>
   

                </Columns>
            </asp:GridView>
             
            
            </td>

		  </tr>
		  </table>
		  

          </div>




          <div class="mastfoot">
            <div class="inner">
              <p>Template for <a href="https://dominospizza.ru">Dominos Pizza</a><br /><asp:Label ID="LabelVersion" runat="server" Text=""></asp:Label></p>
            </div>
          </div>




        </div>

    </form>



    </div>
    </div>
</body>
</html>