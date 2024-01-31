<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormEditor.aspx.cs" Inherits="waRealTimeDB.WebForm1" %>

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
    <link rel="stylesheet" href="dist/jquery-cle/jquery.cleditor.css" />
    <!-- Datepicker -->
    <link rel="stylesheet" href="dist/jquery-ui/jquery-ui.css">
    <link rel="stylesheet" href="dist/jquery-ui/jquery-ui-timepicker-addon.css">

    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src="dist/js/jquery.min.js"></script>
    <script src="dist/js/popper.min.1.12.9.js"></script>
    <script src="dist/js/bootstrap.min.js"></script>
	<script src="dist/js/js.cookie.js"></script>

    <script src="dist/myFunctions.js"></script>
    <script src="dist/find.js"></script>
    <script src="dist/find-jquery.js"></script>
    <script src="dist/jquery-ui/jquery-ui.js"></script>
    <script src="dist/jquery-ui/jquery-ui-timepicker-addon.js"></script>

    <!-- CLEditor -->
    <script src="dist/jquery-cle/jquery.cleditor.min.js"></script>

    <!-- ASPxHtmlEditor validation 
    <script type="text/javascript">
        function ValidationHandler(s, e) {
            if (e.html.length > 1000 ) {
                e.isValid = false;
                e.errorText = "HTML content's length exceeds 1000 characters.";
            }
        }
        function HtmlChangedHandler(s, e) {
            ContentLength.SetText(s.GetHtml().length);
        }
    </script>
    -->



	
</head>
<body>
    <div class="site-wrapper">

      <div class="site-wrapper-inner">
    <form id="form1" runat="server">

        <asp:SqlDataSource ID="SqlDataSourceStore" runat="server" ConnectionString="<%$ ConnectionStrings:RTStatConnectionString %>" SelectCommand="SELECT * FROM [Store]"></asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSourceNews" runat="server" ConnectionString="<%$ ConnectionStrings:RTStatConnectionString %>" SelectCommand="SELECT * FROM [News]"></asp:SqlDataSource>




        <div class="cover-container">

          <div class="masthead clearfix">
            <div class="inner">
              <h3 class="masthead-brand" style="color: #b1aaaa;">Dominos News editor</h3>

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
          <tr>
              <td>
                  <table class ="table" style="margin-bottom: 0.1rem;">
                      <tr>
                          <td style="width: 55px; vertical-align: middle; text-align: right;">
                              <asp:Label ID="Label7" runat="server" Text="Find: "></asp:Label>
                          </td>
                          <td>
                              <%--
                              <asp:TextBox ID="TextBoxFind" style="border-radius: 0.5rem; width: 92%; padding: .3rem .75rem;" class="form-control" placeholder="Find Name"  runat="server"></asp:TextBox>
                              --%>
                              <!--
                              <span id="errmsg"></span>
                              -->
                              <div class="input-group" style="width: 92%;">
                                  <div class="input-group-prepend">
                                  <button style="border-radius: 0.5rem 0.0rem 0.0rem 0.5rem; padding: .15rem .75rem; height: 2rem;" class="btn btn-outline-secondary dropdown-toggle selectFrBtn" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Все</button>
                                  <div class="dropdown-menu">
                                      <a class="dropdown-item selectFr" style="text-decoration: none;" href="#" data-param="" onclick="selectAll(event)">Все</a>
                                      <a class="dropdown-item selectFr" style="text-decoration: none;" href="#" data-param="True">Франчайзинг</a>
                                      <a class="dropdown-item selectFr" style="text-decoration: none;" href="#" data-param="False">Корпоративные</a>
                                  </div>
                                  </div>
                                  <input type="text" id="TextBoxFind" style="border-radius: 0.0rem 0.5rem 0.5rem 0.0rem; padding: .15rem .75rem; height: 2rem;" class="form-control" />
                              </div>
                          </td>

                      </tr>

                  </table>
                  
              </td>
              <td>

              </td>

          </tr>
		  <tr>
			<td style="width: 40%; vertical-align: top;">


            

            <asp:GridView class="table table-striped table-bordered table-find-filter" style="line-height: 1;" ID="GridViewStores" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSourceStore">
                <Columns>
                     
                    <%--
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:HyperLink id="HyperLinkID" 
                                           NavigateUrl='<%# Eval("ID","http://ml.dominospizza.mobi/default.aspx?StoreID={0}&PeriodType=1") %>'                            
                                           Text='<%#Eval("ID")%>'
                                           Target="_blank"
                                           runat="server" />                        
                        </ItemTemplate>
                    </asp:TemplateField>
                    --%>


                    <%-- id строки, используется в множественном выборе --%>
                    <asp:BoundField DataField="ID" HeaderText="ID-hidden" ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone" />


                    <asp:TemplateField HeaderText="Code">
                        <ItemTemplate>

                            <asp:Label ID="Label9" runat="server" Text='<%#Eval("Code")%>' ToolTip='<%#Eval("ID")%>'></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                    
                   
                    <%--
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" />
                    --%>
                    
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <%--
                    <asp:BoundField DataField="NewsName" HeaderText="News" SortExpression="NewsName" />
                    --%>

                    <asp:TemplateField ItemStyle-CssClass="strlimit" HeaderText="NewsName">
                        
                        <ItemTemplate>
                            <div class="" style="height: 1em;">
                            <asp:Label  ID="LabelNewsField" runat="server" Text='<%# "" + Eval("NewsName") + "&nbsp&nbsp&nbsp" %> '></asp:Label>
                            </div>

                        </ItemTemplate>
                        
                    </asp:TemplateField>


                    <asp:TemplateField>

                        <HeaderTemplate>
                            <%--
                            <asp:CheckBox ID="checkAll" runat="server" AutoPostBack="True" OnCheckedChanged="btnSelectAll_Click" />
                            --%>
                            <input type="checkbox" id="CheckBoxCheckAll">
                        </HeaderTemplate>

                        <ItemTemplate>
                            
                            <asp:CheckBox ID="CheckBoxStore" runat="server" OnCheckedChanged="CheckBoxStore_Click" AutoPostBack="True" ToolTip='<%# Eval("ID")+","+ Eval("ord")+","+ Eval("name")+","+ Eval("publishingTime") %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="isFr" HeaderText="isFr-hidden" ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone" />

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
<td style="vertical-align: top;padding: 20px;">

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
            
            <%-- <tr style="visibility: collapse; display: none;"> --%>
            <tr style="<% =styleOrd %>">
                <td style="width: 100px;">
                    <asp:Label ID="Label1" runat="server" Text="Label">Приоритет</asp:Label>
                </td>
                <td>
                    <asp:TextBox style="width: 30%;" class="form-control" placeholder="ord" ID="TextBoxOrd" runat="server"></asp:TextBox>
                </td>
            </tr>
			<tr>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Label">Опубликовать</asp:Label>
                </td>
                <td>
                    <div class="table1">
                        <%--
                        <dx:ASPxDateEdit ID="ASPxNewsDate" runat="server" EditFormat="DateTime" EditFormatString="dd.MM.yyyy HH:mm" Width="30%" NullText=" Дата публикации" ClientEnabled="False"></dx:ASPxDateEdit>
                        --%>
                        <asp:TextBox ID="TextBoxDate" style="width: 25%; height: 25px; padding: 0.45em 0.6em; pointer-events: none; font: 13px Tahoma, Geneva, sans-serif;" class="form-control open_datetimepicker" placeholder="Дата публикации"  runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Label">Текст новости</asp:Label>
                </td>
                <td>    
                    <div class="table1">

                        <textarea id="AreaNewsName" name="AreaNewsName" runat="server"></textarea>
                        <div class="areacolor" style="color: #b1aaaa; font: 12px Tahoma, Geneva, sans-serif; display: inline-block;">
                                The current length of HTML code representing the editor's content:&nbsp
                                <div class="areacolor" id="AreaLength" style="float: right;">0</div>
                        </div>

                      
                    <%--
                    <dx:ASPxHtmlEditor ID="ASPxHtmlEditorNewsName" runat="server" Height="250px" Width="100%">
                        <ClientSideEvents HtmlChanged="HtmlChangedHandler" Validation="ValidationHandler" />
                        <Toolbars>
                            <dx:HtmlEditorToolbar Name="StandardToolbar1" Visible="False">
                                <Items>
                                    <dx:ToolbarCutButton>
                                    </dx:ToolbarCutButton>
                                    <dx:ToolbarCopyButton>
                                    </dx:ToolbarCopyButton>
                                    <dx:ToolbarPasteButton>
                                    </dx:ToolbarPasteButton>
                                    <dx:ToolbarPasteFromWordButton>
                                    </dx:ToolbarPasteFromWordButton>
                                    <dx:ToolbarUndoButton BeginGroup="True">
                                    </dx:ToolbarUndoButton>
                                    <dx:ToolbarRedoButton>
                                    </dx:ToolbarRedoButton>
                                    <dx:ToolbarRemoveFormatButton BeginGroup="True">
                                    </dx:ToolbarRemoveFormatButton>
                                    <dx:ToolbarSuperscriptButton BeginGroup="True">
                                    </dx:ToolbarSuperscriptButton>
                                    <dx:ToolbarSubscriptButton>
                                    </dx:ToolbarSubscriptButton>
                                    <dx:ToolbarInsertOrderedListButton BeginGroup="True">
                                    </dx:ToolbarInsertOrderedListButton>
                                    <dx:ToolbarInsertUnorderedListButton>
                                    </dx:ToolbarInsertUnorderedListButton>
                                    <dx:ToolbarIndentButton BeginGroup="True">
                                    </dx:ToolbarIndentButton>
                                    <dx:ToolbarOutdentButton>
                                    </dx:ToolbarOutdentButton>
                                    <dx:ToolbarInsertLinkDialogButton BeginGroup="True" Visible="False">
                                    </dx:ToolbarInsertLinkDialogButton>
                                    <dx:ToolbarUnlinkButton Visible="False">
                                    </dx:ToolbarUnlinkButton>
                                    <dx:ToolbarInsertImageDialogButton Visible="False">
                                    </dx:ToolbarInsertImageDialogButton>
                                    <dx:ToolbarTableOperationsDropDownButton BeginGroup="True">
                                        <Items>
                                            <dx:ToolbarInsertTableDialogButton BeginGroup="True" Text="Insert Table..." ToolTip="Insert Table...">
                                            </dx:ToolbarInsertTableDialogButton>
                                            <dx:ToolbarTablePropertiesDialogButton BeginGroup="True">
                                            </dx:ToolbarTablePropertiesDialogButton>
                                            <dx:ToolbarTableRowPropertiesDialogButton>
                                            </dx:ToolbarTableRowPropertiesDialogButton>
                                            <dx:ToolbarTableColumnPropertiesDialogButton>
                                            </dx:ToolbarTableColumnPropertiesDialogButton>
                                            <dx:ToolbarTableCellPropertiesDialogButton>
                                            </dx:ToolbarTableCellPropertiesDialogButton>
                                            <dx:ToolbarInsertTableRowAboveButton BeginGroup="True">
                                            </dx:ToolbarInsertTableRowAboveButton>
                                            <dx:ToolbarInsertTableRowBelowButton>
                                            </dx:ToolbarInsertTableRowBelowButton>
                                            <dx:ToolbarInsertTableColumnToLeftButton>
                                            </dx:ToolbarInsertTableColumnToLeftButton>
                                            <dx:ToolbarInsertTableColumnToRightButton>
                                            </dx:ToolbarInsertTableColumnToRightButton>
                                            <dx:ToolbarSplitTableCellHorizontallyButton BeginGroup="True">
                                            </dx:ToolbarSplitTableCellHorizontallyButton>
                                            <dx:ToolbarSplitTableCellVerticallyButton>
                                            </dx:ToolbarSplitTableCellVerticallyButton>
                                            <dx:ToolbarMergeTableCellRightButton>
                                            </dx:ToolbarMergeTableCellRightButton>
                                            <dx:ToolbarMergeTableCellDownButton>
                                            </dx:ToolbarMergeTableCellDownButton>
                                            <dx:ToolbarDeleteTableButton BeginGroup="True">
                                            </dx:ToolbarDeleteTableButton>
                                            <dx:ToolbarDeleteTableRowButton>
                                            </dx:ToolbarDeleteTableRowButton>
                                            <dx:ToolbarDeleteTableColumnButton>
                                            </dx:ToolbarDeleteTableColumnButton>
                                        </Items>
                                    </dx:ToolbarTableOperationsDropDownButton>
                                    <dx:ToolbarFindAndReplaceDialogButton BeginGroup="True" Visible="False">
                                    </dx:ToolbarFindAndReplaceDialogButton>
                                    <dx:ToolbarFullscreenButton BeginGroup="True" Visible="False">
                                    </dx:ToolbarFullscreenButton>
                                </Items>
                            </dx:HtmlEditorToolbar>
                            <dx:HtmlEditorToolbar Name="StandardToolbar2">
                                <Items>
                                    <dx:ToolbarParagraphFormattingEdit Visible="False" Width="120px">
                                        <Items>
                                            <dx:ToolbarListEditItem Text="Normal" Value="p" />
                                            <dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
                                            <dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
                                            <dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
                                            <dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
                                            <dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
                                            <dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
                                            <dx:ToolbarListEditItem Text="Address" Value="address" />
                                            <dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
                                        </Items>
                                    </dx:ToolbarParagraphFormattingEdit>
                                    <dx:ToolbarFontNameEdit Width="100px">
                                        <Items>
                                            <dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
                                            <dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
                                            <dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
                                            <dx:ToolbarListEditItem Text="Arial" Value="Arial" />
                                            <dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
                                            <dx:ToolbarListEditItem Text="Courier" Value="Courier" />
                                            <dx:ToolbarListEditItem Text="Segoe UI" Value="Segoe UI" />
                                        </Items>
                                    </dx:ToolbarFontNameEdit>
                                    <dx:ToolbarFontSizeEdit Width="80px">
                                        <Items>
                                            <dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
                                            <dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
                                            <dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
                                            <dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
                                            <dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
                                            <dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
                                            <dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
                                        </Items>
                                    </dx:ToolbarFontSizeEdit>
                                    <dx:ToolbarBoldButton>
                                    </dx:ToolbarBoldButton>
                                    <dx:ToolbarItalicButton>
                                    </dx:ToolbarItalicButton>
                                    <dx:ToolbarUnderlineButton>
                                    </dx:ToolbarUnderlineButton>
                                    <dx:ToolbarStrikethroughButton>
                                    </dx:ToolbarStrikethroughButton>
                                    <dx:ToolbarJustifyLeftButton BeginGroup="True">
                                    </dx:ToolbarJustifyLeftButton>
                                    <dx:ToolbarJustifyCenterButton>
                                    </dx:ToolbarJustifyCenterButton>
                                    <dx:ToolbarJustifyRightButton>
                                    </dx:ToolbarJustifyRightButton>
                                    <dx:ToolbarBackColorButton BeginGroup="True" Visible="False">
                                    </dx:ToolbarBackColorButton>
                                    <dx:ToolbarFontColorButton>
                                    </dx:ToolbarFontColorButton>
                                </Items>
                            </dx:HtmlEditorToolbar>
                        </Toolbars>
                        <SettingsValidation>
                            <RequiredField IsRequired="True" />
                        </SettingsValidation>
                    </dx:ASPxHtmlEditor>
                    <div style="white-space: nowrap;">
                        <div style="color: #b1aaaa; font: 12px Tahoma, Geneva, sans-serif; display: inline-block;">
                            The current length of HTML code representing the editor's content: 
                        </div>
                        <dx:ASPxLabel style="color: #b1aaaa;" ID="lblContentLength" runat="server" ClientInstanceName="ContentLength" Text="0"></dx:ASPxLabel>
                    </div>
                    --%>

                    <%--
                    <asp:TextBox class="form-control" rows="5" ID="TextBoxNewsName" runat="server" TextMode="MultiLine"></asp:TextBox>
                    --%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Label">Действие</asp:Label> 
                </td>
                <td>
                    <%-- display: none; --%>
                    <div style="">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                        <%--<dx:ASPxButton ID="btnAdd" runat="server" Native = "True" Text="Add" OnClick="btnAdd_Click"></dx:ASPxButton>--%>
                    </div>

                    <%--<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />--%>
                    <asp:Button ID="btnSave" runat="server" Text="Опубликовать" OnClick="btnSave_Click" />
                    <%--<dx:ASPxButton ID="btnSave" runat="server" Native = "True" Text="Опубликовать" OnClick="btnSave_Click"></dx:ASPxButton>--%>

                    <div style="">
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                    </div>
                    
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
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Label">Картинка</asp:Label> 
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />

                    <asp:Button ID="btnLoadImage" runat="server" OnClick="btnLoadImage_Click" Text="Save Image" />
                    <asp:Button ID="btnDeleteImage" runat="server" OnClick="btnDeleteImage_Click" Text="Delete Image" />
                </td>
            </tr>


        </table>




        <br />
        <div style="color: #b1aaaa;margin-top: 0px;">preview:</div>


        <% if (previewUrl != "") {
                //<iframe class="box_scale_preview" src="< =previewUrl >" id="frame1" style="width: 1280px; height: 1024px; margin: -600px -675px 0px;" runat="server"></iframe>
                Response.Write("<iframe class=\"box_scale_preview\" src=\""+ previewUrl +"\" id=\"frame1\" style=\"width: 1280px; height: 1024px; margin: -625px -670px 0px;\" runat=\"server\"></iframe>");
           }
        %> 
        <br />



        <%-- стиль отключает наследование margin-bottom --%>
        <table class="table" style="margin-bottom: 5px; display: none;">
            <tr>                
                <td style="width: 40px;">
                
                </td>
                <td>
                    <%-- атрибут стиля меняется программно в модуле формы --%>
                    <asp:Image ID="Image1" style="max-height: 400px; max-width: 275px;" runat="server" />
                </td>
                <td style="border-left: 1px solid gainsboro; width: 50%;">
                    <div style="width: 289px; font: 11pt Tahoma, Geneva, sans-serif;">
                        <asp:Label ID="LabelPreview" runat="server" Text=""></asp:Label>
                    </div>
                    
                </td>

            </tr>
        </table>

        
        <div style="color: #b1aaaa;margin-top: 0px; margin-bottom: 5px;">news list:</div>


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
		  </tr>
		  </table>
		  

          </div>




          <div class="mastfoot">
            <div class="inner">
              <p>News template for <a href="https://dominospizza.ru">Dominos Pizza</a><br /><asp:Label ID="LabelVersion" runat="server" Text=""></asp:Label></p>
            </div>
          </div>




        </div>
    </form>
      </div>

    </div>




</body>
</html>
