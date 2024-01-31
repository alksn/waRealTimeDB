<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormStatusEditor.aspx.cs" Inherits="waStatusRT.WebFormStatusEditor" %>

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

        <%--<asp:SqlDataSource ID="SqlDataSourceNews" runat="server" ConnectionString="" SelectCommand="SELECT * FROM [News]"></asp:SqlDataSource>--%>





        <div class="cover-container">

          <div class="masthead clearfix">
            <div class="inner">
              <h3 class="masthead-brand" style="color: #b1aaaa; position: absolute;">Dominos Target editor</h3>

              <div class="menuline" style="float: right; margin-top: 0.4em;">
                <a class="menulink" href="#" onclick='redirectPost("WebFormEditor.aspx", {isLogin: "True"})'>News</a>&nbsp;
                <a class="menulink" href="#" onclick='redirectPost("WebFormStatusEditor.aspx", {isLogin: "True"})'>Status</a>&nbsp;
                <a class="menulink" href="#" onclick='redirectPost("WebFormDriverEditor.aspx", {isLogin: "True"})'>Safety</a>

                <a class="menuexit" href="#" onclick='<%= "window.location.replace(\"LoginForm.aspx\");" %>'>Выход</a>
              </div>

            </div>
          </div>


          <div class="inner cover">



		  <table border="0">


          <!--
          <tr>
              <td>
                    <div class="row">
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">1</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">2</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">3</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">4</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">5</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">6</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">7</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">8</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">9</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">0</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">1</div>
                        <div class="col-md-1" style="border-width: 1px;border: solid 1px;">2</div>
                    </div>
              </td>
          </tr>


          <!--
          <tr>
              <td>
                    <div class="row">
                        <div class="col-1">Header:</div>
                        <div class="col-5"><input type="text" class="form-control" style="padding: .2rem .75rem;line-height: inherit;" placeholder="Введите желаемый текст"></div>
                        <div class="col-1"><input type="submit" value="Submit"></div>
                        <div class="col-3"><input type="file" /></div>
                        <div class="col-1"><input type="submit" value="Сохранить"></div>
                        <div class="col-1"></div>
                    </div>
              </td>
          </tr>

          <tr>
              <td>
                    <div class="row">
                        <div class="col-1">Code:</div>
                        <div class="col-2"><input type="text" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Code"></div>
                        <div class="col-1">Find:</div>
                        <div class="col-3"><input type="text" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name"></div>
                        <div class="col-1">Date:</div>
                        <div class="col-3"><input type="text" onchange="dtChange()" style="border-radius: 0.5rem; width: 90%; padding: .3rem .75rem;" class="form-control open_datepicker" placeholder="Select date"></div>
                        <div class="col-1"></div>
                        <div class="col-1"></div>
                    </div>
              </td>
          </tr>
          -->

          <tr>
              <td>
                    <div class="row" style="margin-top: 0.7rem; margin-bottom: 0.5rem;">
                        <div class="col-1 center_right">Header:</div>
                        <div class="col-5" style="padding: 0;"><asp:TextBox ID="TextBoxNewsName" class="form-control" style="padding: .2rem .75rem; line-height: inherit;" placeholder="Введите желаемый текст" runat="server"></asp:TextBox></div>
                        <div class="col-1 center"><asp:Button ID="ButtonResetName" runat="server" Text="Reset" OnClick="ButtonReadConstant_Click" /></div>
                        <div class="col-3 center" style="overflow: hidden;"><asp:FileUpload ID="FileUpload1" accept='.csv, .xlsx' onchange='' runat="server" CssClass="" /></div>
                        <div class="col-2 center" style="padding: 0;">
                              <asp:Button ID="ButtonUpload" runat="server" Text="Сохранить" OnClick="ButtonUpload_Click" />
                              <button id="ButtonUploadJS" onmouseover="" onclick="uploadClick()" style="display:none">СохранитьJS</button>
                        </div>
                    </div>
              </td>
          </tr>

          <tr>
              <td>
                    <div class="row" style="margin-bottom: 0.5rem;">
                        <div class="col-1 center_right">Code:</div>
                        <div class="col-2" style="padding: 0;"><asp:TextBox ID="TextBoxFindCode" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Code" runat="server"></asp:TextBox></div>
                        <div class="col-1 center_right">Find:</div>
                        <div class="col-3" style="padding: 0;"><asp:TextBox ID="TextBoxFind1" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name" runat="server"></asp:TextBox></div>
                        <div class="col-1 center_right" style="padding-right: 2.2em;">Date:</div>
                        <div class="col-3" style="padding: 0;">
                            <asp:TextBox ID="TextBoxNewsDate" onchange="dtChange()" style="border-radius: 0.5rem; width: 116%; padding: .3rem .75rem;" class="form-control open_datepicker" placeholder="Select date"  runat="server"></asp:TextBox>
                            <asp:Button ID="ButtonDateChange" style="display:none" runat="server" Text="DateSaveButton" OnClick="ButtonDateChange_Click" />
                        </div>
                        <div class="col-1"></div>
                    </div>
              </td>
          </tr>


          <%--
          <tr>
              <td>
                  <table class ="table" style="margin-bottom: -0.8rem;">
                      <tr>

                          
                          <td style="width: 70px; vertical-align: middle; text-align: right;">
                              <div>Header: </div>
                          </td>
                          
                          <td style="width: 0%; padding-left: 0;">
                              <table style="width: 100%;">
                                  <tr>
                                      <td style="width: 100%;">
                                          <asp:TextBox ID="TextBoxNewsName_del" class="form-control" style="padding: .2rem .75rem;line-height: inherit;" placeholder="Введите желаемый текст" runat="server"></asp:TextBox>
                                      </td>
                                      <td style="width: 0%; vertical-align: middle;">
                                          <asp:Button ID="Button2_del" runat="server" Text="Reset" OnClick="ButtonReadConstant_Click" />
                                      </td>
                                  </tr>
                              </table>
                          </td>

                          <!--
                          <td style="width: 75px; vertical-align: middle; text-align: left;">
                              <div>Файл: </div>
                          </td>-->
                          <td class="strlimit" style="width: 26%; vertical-align: middle;">
                              <asp:FileUpload ID="FileUpload1_del" accept='.csv, .xlsx' onchange='' runat="server" CssClass="" />
                          </td>

                           <!--
                          <td style="width: 55px; vertical-align: middle; text-align: right;">
                              
                          </td>-->
                          <td style="width: 13%; vertical-align: middle;">
                              <asp:Button ID="ButtonUpload_del" runat="server" Text="Сохранить" OnClick="ButtonUpload_Click" />
                              <button id="ButtonUploadJS_del" onmouseover="" onclick="uploadClick()" style="display:none">СохранитьJS</button>
                          </td>

                      </tr>


                  </table>
                  
              </td>


          </tr>

          <!--
          <tr>
              <td>
                  <table class="table" style="margin-bottom: 0.0rem;">
                      <tr>
                          <td style="width: 75px;">
                              <div>Preview:</div>
                          </td>
                          <td>
                              <div>Здесь длинный текст превью, ваша цель 100000 выполнено 100%.</div>
                          </td>
                          <td style="width: 39%; color: blue;">
                              <div>Текст результата синего цвета</div>
                          </td>
                      </tr>
                  </table>
              </td>
          </tr>-->
          



          <tr>
              <td>
                  <table class ="table" style="margin-bottom: 0.1rem;">
                      <tr>
                          <td style="width: 70px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label3" runat="server" Text="Code: "></asp:Label>
                          </td>
                          <td style="width: 17%;">
                              <asp:TextBox ID="TextBoxFindCode_del" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Code"  runat="server"></asp:TextBox>
                          </td>

                          <td style="width: 55px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label7" runat="server" Text="Find: "></asp:Label>
                          </td>
                          <td style="width: 33%;">
                              <asp:TextBox ID="TextBoxFind_del" style="border-radius: 0.5rem; width: 100%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name"  runat="server"></asp:TextBox>
                          </td>
                          
                          <td style="width: 55px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label1" runat="server" Text="Date: "></asp:Label>
                          </td>
                          <td style="width: 0%;">
                              <asp:TextBox ID="TextBoxNewsDate_del" onchange="dtChange()" style="border-radius: 0.5rem; width: 90%; padding: .3rem .75rem;" class="form-control open_datepicker" placeholder="Select date"  runat="server"></asp:TextBox>
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

                    <asp:TemplateField ItemStyle-CssClass="strlimit" HeaderText="NewsHeader">
                        
                        <ItemTemplate>
                            <div class="" style="/*height: 1em;*/">
                            <asp:TextBox 
                                ID="TextBoxNewsField" 
                                onkeydown='<%# "if (event.keyCode === 13) {newsHeaderChange(event, " +Eval("ID")+ ",\""+TextBoxNewsDate.Text+"\")} else {};" %>'                                 
                                Text='<%# Eval("NewsName") %>' style="padding: .0rem .2rem; margin: -0.20rem 0; border: none; background-color: inherit; color: black; line-height: 1.4em;" class="form-control" placeholder=""  runat="server">
                            </asp:TextBox>
                            </div>
                        </ItemTemplate>
                        
                    </asp:TemplateField>


                    <asp:TemplateField ItemStyle-CssClass="" HeaderText="Value" ItemStyle-Width="30%">
                        
                        <ItemTemplate>
                            <div class="" style="height: 1em;">                            

                            <%--'<%#
                            <asp:TextBox ID="TextBoxValue" onkeydown="if (event.keyCode === 13) { valChange(event, 'test-text') } else { };" onchange="" Text='<%# !String.IsNullOrEmpty(Eval("Val").ToString()) ? Convert.ToInt32(Eval("Val")) : Eval("Val")  %>' style="padding: .0rem .4rem; margin: -0.1rem 0; border: none;" class="form-control" placeholder="no value"  runat="server" ToolTip='<%#Eval("ID")%>'></asp:TextBox>
                            --%>
                            <asp:TextBox 
                                ID="TextBoxValue" 
                                onkeydown='<%# "if (event.keyCode === 13) {valChange(event, " +Eval("ID")+ ",\""+TextBoxNewsDate.Text+"\")} else {};" %>'                                 
                                onchange="" Text='<%# !String.IsNullOrEmpty(Eval("Val").ToString()) ? Convert.ToInt32(Eval("Val")) : Eval("Val")  %>' style="padding: .0rem .4rem; margin: -0.1rem 0; border: none;" class="form-control" placeholder="no value"  runat="server">
                            </asp:TextBox>
                            

                            </div>

                        </ItemTemplate>
                        
                    </asp:TemplateField>

                    <asp:BoundField DataField="publishingTime" HeaderText="Date" ItemStyle-Width="30%" DataFormatString="{0:dd.MM.yyyy}" />

                    <%--
                    <asp:TemplateField ShowHeader="True" Visible="False">

                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" AutoPostBack="True" OnCheckedChanged="btnSelectAll_Click" />
                        </HeaderTemplate>

                        <ItemTemplate>
                            
                            <asp:CheckBox ID="CheckBoxStore" runat="server" OnCheckedChanged="CheckBoxStore_Click" AutoPostBack="True" ToolTip='<%# Eval("ID")+"|"+ Eval("ord")+"|"+ Eval("name")+"|"+ Eval("publishingTime")+"|"+ Eval("newsName")+"|" +Eval("code") %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    --%>
                    
                    

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

<%--
<td style="vertical-align: top;padding: 20px; display: none;">

    <div style="border-style: solid; border-width: 1px; padding: 10px; margin: -19px -10px -10px; min-height: 600px; border-color: gainsboro;">


        

        <div style="color: #b1aaaa;margin-top: -10px;">record selected:</div>
        <br />
        

        <table class="table">
            
            <tr>
                <td style="width: 10%;">
                    <asp:Label ID="Label4" runat="server" Text="Label">ID</asp:Label>
                </td>
                <td>
                    <asp:Label ID="LabelName" runat="server" Text=""></asp:Label>
                </td>
            </tr>


            < %--
			<tr>
                <td style="vertical-align: middle;">
                    <asp:Label ID="Label8" runat="server" Text="Label">Дата</asp:Label>
                </td>
                <td>                    
                    <asp:TextBox style="width: 30%; display: contents;" class="form-control open_datepicker" placeholder="" ID="TextBoxNewsDate" runat="server"></asp:TextBox>
                    <asp:Button ID="ButtonReadConstant" runat="server" Text="Обновить" OnClick="ButtonReadConstant_Click"  />
                </td>
            </tr>
            --% >

            <tr>
                <td style="vertical-align: middle;">
                    <asp:Label ID="Label2" runat="server" Text="Label">Шаблон</asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td style="width: 100%;">
                                <asp:TextBox style="" class="form-control" placeholder="Введите желаемый текст" ID="TextBoxNewsName1" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 0%; vertical-align: middle;">
                                <asp:Button ID="ButtonReadConstant" runat="server" Text="Reset" OnClick="ButtonReadConstant_Click" />
                            </td>
                        </tr>
                    </table>   
                </td>
            </tr>


            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="Label">Файл</asp:Label> 
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1_nouse_disabled" accept='.csv' onchange='' runat="server" />
                    <br /><br />
                    <asp:Button ID="ButtonUpload1" runat="server" Text="Сохранить изменения" OnClick="ButtonUpload_Click" style="display:none" />
                    <button id="ButtonUploadJS1" onmouseover="" onclick="uploadClick()">Сохранить изменения</button>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Label">Результат</asp:Label> 
                </td>
                <td>
                    <asp:Label ID="LabelResult" runat="server" Text=""></asp:Label> 
                </td>
            </tr>

        </table>




        <br />
        <div style="color: #b1aaaa;margin-top: 0px;">preview:</div>
        

        <table class="table" style="margin-bottom: 5px;">
            <tr>                
                <td style="width: 13%;">
                
                </td>
                <td>
                    <div style="font: 11pt Tahoma, Geneva, sans-serif;">
                        <asp:Label ID="LabelPreviewStatus" runat="server" Text=""></asp:Label>
                    </div>
                </td>
                <td>
                </td>
            </tr>
        </table>


        
        <% if (previewUrl != "") {
                //<iframe class="box_scale_preview" src="< =previewUrl >" id="frame1" style="width: 1280px; height: 1024px; margin: -600px -675px 0px;" runat="server"></iframe>
                //Response.Write("<iframe class=\"box_scale_preview\" src=\""+ previewUrl +"\" id=\"frame1\" style=\"width: 1280px; height: 1024px; margin: -600px -675px 0px;\" runat=\"server\"></iframe>");                
           }
        %>        


        


        
        

        <%-- стиль отключает наследование margin-bottom --% >
        <table class="table" style="<% =styleOrd %> margin-bottom: 5px;">
            <tr>                
                <td style="width: 40px;">
                
                </td>
                <td>
                    <%-- атрибут стиля меняется программно в модуле формы --% >
                    <asp:Image ID="Image1" style="max-height: 400px; max-width: 275px;" runat="server" />
                </td>
                <td style="border-left: 1px solid gainsboro; width: 50%;">
                    <div style="width: 289px; font: 11pt Tahoma, Geneva, sans-serif;">
                        <asp:Label ID="LabelPreview" runat="server" Text=""></asp:Label>
                    </div>
                    
                </td>

            </tr>
        </table>


        
        <div style="<% =styleOrd %> color: #b1aaaa;margin-top: 0px; margin-bottom: 5px;">news list:</div>


            <asp:GridView class="table table-bordered" style="line-height: 1.25;" ID="GridViewNews" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceNews">
                <Columns>
                    <asp:BoundField DataField="RT_StoreID" HeaderText="ID" SortExpression="RT_StoreID" />
                    <asp:BoundField DataField="Name" HeaderText="Текст" SortExpression="Name" HtmlEncode="false" />
                    <asp:BoundField DataField="PublishingTime" HeaderText="Дата публикации" SortExpression="PublishingTime" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                    <asp:BoundField DataField="ord" HeaderText="Приоритет" SortExpression="ord" />
                
                </Columns>
            </asp:GridView>



        </div>
</td>
--%>
		  </tr>
		  </table>
		  

          </div>




          <div class="mastfoot">
            <div class="inner">
              <p>Template for <a href="https://dominospizza.ru">Dominos Pizza</a><br /><asp:Label ID="LabelVersion" runat="server" Text=""></asp:Label></p>
            </div>
          </div>




        </div>



















        <%--
        <div>
        </div>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        &nbsp;
        <asp:Button ID="ButtonUpload" runat="server" Text="Upload File to Database" OnClick="ButtonUpload_Click" />
        --%>


    </form>




      </div>

    </div>




</body>
</html>