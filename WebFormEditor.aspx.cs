using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Drawing;                                                   // преобразование изображения в png при сохраении
using System.Drawing.Imaging;

namespace waRealTimeDB
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        private string ConnectionStringRT;
        //public static string ConnectionStringRT = @"Data Source=MAIN\SQLEXPRESS;Initial Catalog=RTStat;Persist Security Info=True;User ID=sa;Password=1234";
        //public static string ConnectionStringRT = @"Data Source=RT-SRV02;Initial Catalog=RTStat;Persist Security Info=True;User ID=sa;Password=5s5fs51x";


        //private string pictureFolder = @"c:\images\";                 // путь к папке с картинками. Завершающий обратный слеш обязателен.
        //private List<string> pictureFolderList = new List<string>() { @"c:\images\" };
        private List<string> pictureFolderList = new List<string>() {};

        //private string pictureExt = ".png";                           // формат файла картинок. НИ включая 1.36
        private bool pictureDefaultShow = true;
        private string pictureDefault = @"c:\images\default.png";       // картинка по умолчанию. Если нет картинки для новости
        private bool logToFile = true;                                  // сохранять весь Лог в файл и в базу. Иначе только в базу, в файл - при отсутствии соединения и ошибках
        private bool pictureFixedWidth = false;                         // фиксировать картинку по ширине
        private string highPriority = "max";                            // наивысший приоритет max или min
        private int NewsColomnTextLength = 40;                          // ограничение по длине для колонки News
        private bool EmptyDateToNull = true;


        private string styleBtn = "display: none;";                             // стиль кнопок btnAdd, btnDelete
        protected string styleOrd = "visibility: collapse; display: none;";     // используется непосредственно в WebFormEditor.aspx
        protected string previewUrl = "";

        private bool[] isChecked;
        string RedirectPage = "";                                       // данная страница открывается при неверном пароле

        private LogWriter log;




        protected void Page_Load(object sender, EventArgs e)
        {
            //LabelVersion.Text = "Application v.1.44b";
            LabelVersion.Text = waRealTimeDB.staticConstants.AppVersion;

            // чтение строки подключения к БД из файла
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"App_Data\Config.txt"));

            foreach (string line in lines)
            {
                // пропускаем пустые строки и комментарии
                if ((line.Length != 0) && (line.Substring(0, 2) != "//"))
                {
                    // ConnectionStringRT = line;

                    if (line.IndexOf("ConnectionString=") == 0)
                    {
                        ConnectionStringRT = line.Substring(("ConnectionString=").Length, line.Length - ("ConnectionString=").Length);                        
                    }

                    if (line.IndexOf("pictureFolder=") == 0)
                    {
                        string pictureFolder = line.Substring(("pictureFolder=").Length, line.Length - ("pictureFolder=").Length);
                        pictureFolder = pictureFolder.ToLower();
                        if (pictureFolder.Length > 0)
                            if (pictureFolderList.IndexOf(pictureFolder) == -1)
                                pictureFolderList.Add(pictureFolder);
                    }

                    /*
                    if (line.IndexOf("pictureExt=") == 0)
                    {
                        pictureExt = line.Substring(("pictureExt=").Length, line.Length - ("pictureExt=").Length);
                    }
                    */

                    if (line.IndexOf("pictureDefaultShow=") == 0)
                    {
                        try
                        {
                            pictureDefaultShow = Convert.ToBoolean(line.Substring(("pictureDefaultShow=").Length, line.Length - ("pictureDefaultShow=").Length));
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Config.txt error pictureDefaultShow=" + ex.Message + "\n");
                        }
                    }


                    if (line.IndexOf("pictureDefault=") == 0)
                    {
                        pictureDefault = line.Substring(("pictureDefault=").Length, line.Length - ("pictureDefault=").Length);
                    }


                    if (line.IndexOf("logToFile=") == 0)
                    {
                        try
                        {
                            logToFile = Convert.ToBoolean(line.Substring(("logToFile=").Length, line.Length - ("logToFile=").Length));
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Config.txt error logToFile=" + ex.Message + "\n");
                        }
                    }


                    if (line.IndexOf("pictureFixedWidth=") == 0)
                    {
                        try
                        {
                            pictureFixedWidth = Convert.ToBoolean(line.Substring(("pictureFixedWidth=").Length, line.Length - ("pictureFixedWidth=").Length));
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Config.txt error pictureFixedWidth=" + ex.Message + "\n");
                        }
                    }


                    if (line.IndexOf("highPriority=") == 0)
                    {
                        highPriority = line.Substring(("highPriority=").Length, line.Length - ("highPriority=").Length);

                        if (! ((highPriority == "max") || (highPriority == "min")) )
                        {
                            Response.Write("Config.txt error highPriority=" + highPriority + "\n");
                            highPriority = "max";                            
                        }
                    }


                    if (line.IndexOf("NewsColomnTextLength=") == 0)
                    {
                        try
                        {
                            NewsColomnTextLength = Convert.ToInt32(line.Substring(("NewsColomnTextLength=").Length, line.Length - ("NewsColomnTextLength=").Length));
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Config.txt error NewsColomnTextLength=" + ex.Message + "\n");
                        }
                    }


                    if (line.IndexOf("EmptyDateToNull=") == 0)
                    {
                        try
                        {
                            EmptyDateToNull = Convert.ToBoolean(line.Substring(("EmptyDateToNull=").Length, line.Length - ("EmptyDateToNull=").Length));
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Config.txt error EmptyDateToNull=" + ex.Message + "\n");
                        }
                    }


                    if (line.IndexOf("RedirectPage=") == 0)
                    {
                        RedirectPage = line.Substring(("RedirectPage=").Length, line.Length - ("RedirectPage=").Length);
                    }

                }
            }



            // инициализация компонентов DataSource

            SqlDataSourceStore.ConnectionString = ConnectionStringRT;
            SqlDataSourceNews.ConnectionString = ConnectionStringRT;

            SqlDataSourceNews.SelectCommand = "";


            /*
            SqlDataSourceStore.SelectCommand =
                "select top 15 st.Id id, st.Name2 name, SUBSTRING(news.Name,1,40) newsName, news.ord ord from RTStat.dbo.Store st " +
                "left outer join RTStat.dbo.News news on st.ID = news.RT_StoreID " +
                "where st.Active = 1";
            */

            /* то же самое ниже
            // заспрос возвращает список магазинов с одной новостью наибольшего приоритета
            SqlDataSourceStore.SelectCommand =
                "select top 1500 st.Id id, st.Name2 name, SUBSTRING(news.Name,1,40) newsName, news.ord ord from RTStat.dbo.Store st " +
                "left outer join(select top 100000 t1.* from RTStat.dbo.News t1 " +
                "group by t1.RT_StoreID, t1.Location_Code, t1.Name, t1.ord " +
                "having t1.ord = (" +
                "select max(t2.ord) from RTStat.dbo.News t2 " +
                "group by t2.RT_StoreID " +
                "having t2.RT_StoreID = t1.RT_StoreID) " +
                "order by t1.RT_StoreID) news on st.ID = news.RT_StoreID " +
                "where st.Active = 1";
            */


            // заспрос возвращает список магазинов с одной новостью наибольшего приоритета
            SqlDataSourceStore.SelectCommand =
                "select st.Id id, code, st.Name2 name, SUBSTRING(news.Name,1," + NewsColomnTextLength.ToString() + ") newsName, news.ord ord, news.publishingTime, st.isFr from Store st " +
                "left outer join(select t1.* from News t1 " +
                "where t1.ord = (" +
                "select " + highPriority + "(t2.ord) from News t2 " +
                "where t2.RT_StoreID = t1.RT_StoreID) " +
                ") news on st.ID = news.RT_StoreID " +
                "where st.Active = 1" +
                "order by st.Id";




            // обработка подключения при первом открытии

            if ((RedirectPage != "") && !Page.IsPostBack)
            {
                bool isLogin = Convert.ToBoolean(Request.Form["isLogin"]);
                if (!isLogin)
                {
                    Response.Redirect(RedirectPage);
                    Response.End();
                }
            }

            log = LoginForm.log;

            // если вход с паролем отключен RedirectPage = ""
            if (log is null)
                log = new LogWriter(Server.MapPath(@"App_Data\AppLog.txt"));                 

            log.ConnectionStringRT = ConnectionStringRT;
            log.logToFile = logToFile;

            if (pictureFixedWidth == true)
            {
                Image1.Attributes.CssStyle.Value = "width: 275px;";
            }                    
            else
            {
                // указано в шаблоне - "max-height: 300px; max-width: 400px;
            }

            if (log.user_name == "test1")
            {
                styleBtn = "";      // используется на пару строк ниже
                styleOrd = "";      // используется в WebFormEditor.aspx
            }


            /*if ((btnAdd is DevExpress.Web.ASPxButton) && styleBtn != "")
            {
                btnAdd.Visible = false;     // т.к. для DX не работает строка ниже
            }*/
   
            btnAdd.Attributes.CssStyle.Value = styleBtn;
            btnDelete.Attributes.CssStyle.Value = styleBtn;

            LabelResult.Text = "";



            // сохраняем настройки флагов (обновление страницы происходит до обработки нажатия клавиш)
            CheckBoxSave();
            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            setPreviewUrl();
        }




        // отображение сообщения в строке состояния
        private void ShowMessage(string msg, int status)
        {

            LabelResult.Text = msg;

            LabelResult.ForeColor = System.Drawing.Color.Black;

            if (status == 0)    // ok
                LabelResult.ForeColor = System.Drawing.Color.DarkGreen;

            if (status == 1)    // err
                LabelResult.ForeColor = System.Drawing.Color.Red;
                //LabelResult.ForeColor = System.Drawing.Color.Black;

            if (status == 2)    // info
                LabelResult.ForeColor = System.Drawing.Color.DarkSlateBlue;


            log.action_result = msg;
            log.action_name = "ShowMessage";
            if (status == 1)
                log.isOk = false;
            log.WriteInfo();

        }



        protected void CheckBoxStore_Click(object sender, EventArgs e)
        {
            // выводим информацию только при установке галочки
            if ((sender as CheckBox).Checked)
            { 

                string[] commandArgs = (sender as CheckBox).ToolTip.Split(new char[] { ',' });
                string id = commandArgs[0];
                string ord = commandArgs[1];
                string name = commandArgs[2];
                string publishingTime = commandArgs[3];


                SqlDataSourceNews.SelectCommand = "Select * from News where RT_StoreID = " + id;


                string NewsName = "";
                string NewsTime = "";

                ReadNew(id, ord, ref NewsName, out NewsTime);


                TextBoxOrd.Text = ord;
                //ASPxNewsDate.Value = NewsTime;
                //TextBoxDate.Text = 
                TextBoxDate.Text = (NewsTime == "") ? NewsTime : Convert.ToDateTime(NewsTime).ToString("dd.MM.yyyy HH:mm");     // no zero or unblink
                //TextBoxNewsName.Text = NewsName;                
                //ASPxHtmlEditorNewsName.Html = NewsName;
                AreaNewsName.InnerText = NewsName;


                LabelPreview.Text = NewsName;

                LabelName.Text = id + ". " + name;


                ImageShow(Convert.ToInt32(id));

            }
            else
            {
                // снятие флага. Очистка области редактирования

                int checkedCount = 0;

                // посчитаем количество выбранных строк
                for (int i = 0; i < GridViewStores.Rows.Count; i++)
                {
                    if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked)
                    {
                        checkedCount++;
                    }
                }

                // если не осталось выделенных строк, очищаем область редактирования
                if (checkedCount == 0)
                {
                    ClearFields();
                }

            }

        }


        // запоминаем выбранные флаги. Только для процедур добавления, сохранения, удаления
        private void CheckBoxSave()
        {
            isChecked = new bool[GridViewStores.Rows.Count];

            // запоминаем выбранные строки в массив
            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {
                isChecked[i] = false;

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked)
                {
                    isChecked[i] = true;
                }
            }
        }


        // восстанавливаем выбранные флаги. Только для процедур добавления, сохранения, удаления
        private void CheckBoxRestore()
        {
            // восстанавливаем флаги из глобального массива
            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {
                (GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked = isChecked[i];
            }
        }


        // обновление дочерней таблицы со списком новостей для одного Магазина
        private void RefreshDataGrid(int id = -1)
        {
            GridViewStores.DataBind();  // обновление данных в основной таблице

            // id - идентификатор Магазина, для которого будут отображены новости
            SqlDataSourceNews.SelectCommand = "Select * from News where RT_StoreID = " + id;
            GridViewNews.DataBind();

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            // требуется только после обновления таблицы. В остальных случаях флаги хранятся в viewstate
            // CheckBoxRestore();
            // здесь нельзя восстанавливать флаги, т.к. не работает check all, используется отдельно в ClearFields()
        }


        private void setPreviewUrl()
        {
            string str = LabelName.Text;

            // в случае если флаги не установлены, но выполнено какое-либо действие
            if (str != "")
            {
                string id = str.Substring(0, str.IndexOf("."));
                previewUrl = "http://ml.dominospizza.mobi/default.aspx?StoreID=" + id + "&PeriodType=1";
            }
        }






        private void ImageShow(int id = -1)
        {
            /*
            string fileNameStr = id.ToString() + pictureExt;

            Image1.Visible = false;
            File.Delete(Server.MapPath("images\\" + fileNameStr));
            if (File.Exists(pictureFolder + fileNameStr))
            {
                File.Copy(pictureFolder + fileNameStr, Server.MapPath("images\\" + fileNameStr));
                Image1.Visible = true;
            }
            Image1.ImageUrl = "images\\" + fileNameStr;
            */
            /*
            try
            {

            

                string fileNameStr = id.ToString() + pictureExt;

                Image1.Visible = false;
                File.Delete(Server.MapPath("images\\" + fileNameStr));                          // удаляем картинку
                File.Delete(Server.MapPath("images\\" + Path.GetFileName(pictureDefault)));     // удаляем картинку по умолчанию
                if (File.Exists(pictureFolder + fileNameStr))
                {
                    File.Copy(pictureFolder + fileNameStr, Server.MapPath("images\\" + fileNameStr));
                    Image1.Visible = true;
                    Image1.ImageUrl = "images\\" + fileNameStr + "?r=" + DateTime.Now.Ticks.ToString();
                }
                else
                {
                    // если картинка по умолчанию существует и установлена опция "показывать картинку по умолчанию"
                    if ((File.Exists(pictureDefault)) && pictureDefaultShow)
                    {
                        File.Copy(pictureDefault, Server.MapPath("images\\" + Path.GetFileName(pictureDefault)));
                        Image1.Visible = true;
                        Image1.ImageUrl = "images\\" + Path.GetFileName(pictureDefault) + "?r=" + DateTime.Now.Ticks.ToString();
                    }

                }

            }
            catch (Exception ex)
            {
                // например, "отказ в доступе"
                ShowMessage(ex.Message, 1);
            }
            */
        }



        // чтение новости.
        private bool ReadNew(string ID, string ord, ref string NewsName, out string NewsTime)
        {
            // ограничение при выборе строк с пустой новостью
            if (ord == "")
                ord = "0";

            NewsTime = "";

            bool isOk = false;


            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();


            cmd.CommandText = "Select top 1 * from News where RT_StoreID = " + ID + " and ord = " + ord;

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;


            sqlConnection.Open();

            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {

                //ord = (int)reader["ord"];
                NewsName = reader["name"].ToString();
                NewsTime = reader["publishingTime"].ToString();

                isOk = true;

            }


            reader.Close();



            sqlConnection.Close();





            return isOk;

        }


        // поиск приоритета по id. Используется только для обновления при удалении новости
        private int FindOrd(int ID, bool max = true)
        {

            int ord = 0;
            

            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            if (max)
                cmd.CommandText = "Select max(ord) ord from News where RT_StoreID = " + ID;
            else
                cmd.CommandText = "Select min(ord) ord from News where RT_StoreID = " + ID;

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                if (!Convert.IsDBNull(reader["ord"]))
                {
                    ord = (int)reader["ord"];
                }                    
            }

            reader.Close();
            sqlConnection.Close();
     
            return ord;
        }






        private void FileAdd(int StoreID, ref string errStr)
        {

            errStr = "";

            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                // string fn = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                // string SaveLocation = Server.MapPath("Images") + "\\" + fn;

                // string SaveLocation = pictureFolder + StoreID.ToString() + pictureExt;




                //string filePath = @"C:\inetpub\makeline2\img\" + StoreID.ToString() + ".png";
                //string SaveLocation = Server.MapPath("http://ml.dominospizza.mobi/img") + StoreID.ToString() + ".png";
                //Response.Write(SaveLocation);


                string pictureExt = Path.GetExtension(FileUpload1.PostedFile.FileName);

                try
                {
                    foreach (string str in pictureFolderList)  // many folders with one picture
                    {
                        //string SaveLocation = str + StoreID.ToString() + pictureExt;
                        //FileUpload1.PostedFile.SaveAs(SaveLocation);

                        string SaveLocation = str + StoreID.ToString();
                        if (pictureExt == ".png")
                            FileUpload1.PostedFile.SaveAs(SaveLocation + pictureExt);
                        else
                        {
                            Bitmap bitmap = (Bitmap)Bitmap.FromStream(FileUpload1.PostedFile.InputStream);
                            bitmap.Save(SaveLocation + ".png", ImageFormat.Png);
                        }
                    }
                    //Response.Write("The file has been uploaded.");
                    //ShowMessage("The file has been uploaded.", 0);
                }
                catch (Exception ex)
                {
                    //Response.Write("Error: " + ex.Message);
                    errStr = "Image add error: " + ex.Message;
                }
            }
            else
            {
                //Response.Write("Please select a file to upload.");
                errStr = "Please select a file to upload.";

            }
        }


        private int FileDelete(int StoreID, ref string errStr)
        {
            int deletedCount = 0;
            errStr = "";

            try
            {
                foreach (string str in pictureFolderList)  // many folders with one picture
                {
                    string[] files = Directory.GetFiles(str, StoreID + ".*");
                    foreach (string fileName in files)
                    {
                        File.Delete(fileName);
                    }
                }
                deletedCount++;
            }
            catch (Exception ex)
            {
                errStr = "File delete err: " + ex.Message;
            }

            return deletedCount;    // 0 или 1
        }








        protected void btnAdd_Click(object sender, EventArgs e)
        {

            int okCount = 0;
            int errCount = 0;
            int allCount = 0;

            string errStr = "";
            int id = -1;        // вынесено из цикла только для процедуры RefreshDataGrid(id);

            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked == true)
                {

                    string strID = GridViewStores.Rows[i].Cells[0].Text;
                    //string strID = (GridViewStores.Rows[i].Cells[0].Controls[1] as HyperLink).Text;
                    id = Convert.ToInt32(strID);

                    
                    SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "INSERT news (RT_StoreID, Location_Code, Name, ord, publishingTime) VALUES (@RT_StoreID, 0, @Name, @ord, @pTime)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection;

                    cmd.Parameters.AddWithValue("@RT_StoreID", id);
                    //cmd.Parameters.AddWithValue("@Name", TextBoxNewsName.Text);
                    //cmd.Parameters.AddWithValue("@Name", ASPxHtmlEditorNewsName.Html);
                    cmd.Parameters.AddWithValue("@Name", AreaNewsName.InnerText);                  
                    cmd.Parameters.AddWithValue("@ord", TextBoxOrd.Text);
                    if (TextBoxDate.Text == "")
                        if (EmptyDateToNull)
                            cmd.Parameters.AddWithValue("@pTime", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@pTime", DateTime.Now);
                    else
                        cmd.Parameters.AddWithValue("@pTime", Convert.ToDateTime(TextBoxDate.Text));

                    try
                    {
                        ReadNew(id.ToString(), TextBoxOrd.Text, ref log.text_old, out string NewTimeTemp);

                        sqlConnection.Open();
                        cmd.ExecuteNonQuery();

                        okCount++;
                    }
                    catch (Exception ex)
                    {
                        errCount++;
                        errStr = "Add err = " + ex.Message;
                        //Response.Write("Add err = " + ex.Message);                        

                        log.isOk = false;
                        log.action_result = ex.Message;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }

                    log.action_name = "btnAdd_Click";
                    log.store_id = id;
                    //log.text_new = TextBoxNewsName.Text;
                    //log.text_new = ASPxHtmlEditorNewsName.Html;
                    log.text_new = AreaNewsName.InnerText;
                    try { log.ord_new = Convert.ToInt32(TextBoxOrd.Text); } catch { log.ord_new = 0; };     // более верно - принудительно заполнить поле
                    // log.ord_old = log.ord_new;
                    log.WriteInfo();

                    allCount++;
                }

            } // for


            // отчет о выполненном действии
            if (allCount > 0)
            { 
                if (errCount > 0)
                {
                    //Response.Write("Err in add procedure");
                    ShowMessage("Error in " + errCount + " of " + allCount + " records. " + errStr, 1);
                }
                else
                {
                    //Response.Write("Add ok");
                    ShowMessage("Add " + okCount + " of " + allCount + " records. Add ok", 0);
                }
            }
            else
            {
                ShowMessage("Add. No record selected", 2);
            }


            if (okCount > 0)
                //LabelPreview.Text = TextBoxNewsName.Text;
                //LabelPreview.Text = ASPxHtmlEditorNewsName.Html;
                LabelPreview.Text = AreaNewsName.InnerText;

            if (!EmptyDateToNull && TextBoxDate.Text == "")
                TextBoxDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            RefreshDataGrid(id);

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();
        }





        protected void btnSave_Click(object sender, EventArgs e)
        {

            int okCount = 0;
            int errCount = 0;
            int allCount = 0;            

            string errStr = "";
            int id = -1;        // вынесено из цикла только для процедуры RefreshDataGrid(id);

            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked == true)
                {

                    string strID = GridViewStores.Rows[i].Cells[0].Text;
                    //string strID = (GridViewStores.Rows[i].Cells[0].Controls[1] as HyperLink).Text;
                    id = Convert.ToInt32(strID);


                    SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "UPDATE news SET Name = @Name, publishingTime = @pTime where RT_StoreID = @RT_StoreID and ord = @ord";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection;

                    cmd.Parameters.AddWithValue("@RT_StoreID", id);
                    //cmd.Parameters.AddWithValue("@Name", TextBoxNewsName.Text);
                    //cmd.Parameters.AddWithValue("@Name", ASPxHtmlEditorNewsName.Html);
                    cmd.Parameters.AddWithValue("@Name", AreaNewsName.InnerText);
                    cmd.Parameters.AddWithValue("@ord", TextBoxOrd.Text);
                    if (TextBoxDate.Text == "")
                        if (EmptyDateToNull)
                            cmd.Parameters.AddWithValue("@pTime", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@pTime", DateTime.Now);
                    else
                        cmd.Parameters.AddWithValue("@pTime", Convert.ToDateTime(TextBoxDate.Text));

                    try
                    {
                        ReadNew(id.ToString(), TextBoxOrd.Text, ref log.text_old, out string NewTimeTemp);

                        sqlConnection.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                            okCount++;
                        else
                            log.isOk = false;

                    }
                    catch (Exception ex)
                    {
                        errCount++;
                        errStr = "Update err = " + ex.Message;
                        //Response.Write("Update err = " + ex.Message);

                        log.isOk = false;
                        log.action_result = ex.Message;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }

                    log.action_name = "btnSave_Click";
                    log.store_id = id;
                    //log.text_new = TextBoxNewsName.Text;
                    //log.text_new = ASPxHtmlEditorNewsName.Html;
                    log.text_new = AreaNewsName.InnerText;
                    try { log.ord_new = Convert.ToInt32(TextBoxOrd.Text); } catch { log.ord_new = 0; };     // более верно - принудительно заполнить поле
                    log.ord_old = log.ord_new;
                    log.WriteInfo();

                    allCount++;
                }

            } // for


            // отчет о выполненном действии
            if (allCount > 0)
            {
                if (errCount > 0)
                {
                    //Response.Write("Err in update procedure");
                    ShowMessage("Error in " + errCount + " of " + allCount + " records. " + errStr, 1);
                }
                else
                {
                    //Response.Write("Update ok");
                    ShowMessage("Updated " + okCount + " of " + allCount + " records. Update ok", 0);
                }
            }
            else
            {
                ShowMessage("Save. No record selected", 2);
            }


            if (okCount > 0)
                //LabelPreview.Text = TextBoxNewsName.Text;
                //LabelPreview.Text = ASPxHtmlEditorNewsName.Html;
                LabelPreview.Text = AreaNewsName.InnerText;

            if (!EmptyDateToNull && TextBoxDate.Text == "")
                TextBoxDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            RefreshDataGrid(id);

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();
        }







        protected void btnDelete_Click(object sender, EventArgs e)
        {

            int okCount = 0;
            int errCount = 0;
            int allCount = 0;

            string errStr = "";
            int id = -1;        // вынесено из цикла только для процедуры RefreshDataGrid(id);

            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked == true)
                {

                    string strID = GridViewStores.Rows[i].Cells[0].Text;
                    //string strID = (GridViewStores.Rows[i].Cells[0].Controls[1] as HyperLink).Text;
                    id = Convert.ToInt32(strID);


                    SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "DELETE FROM news WHERE RT_StoreID = @RT_StoreID and ord = @ord";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection;

                    cmd.Parameters.AddWithValue("@RT_StoreID", id);
                    cmd.Parameters.AddWithValue("@ord", TextBoxOrd.Text);

                    try
                    {
                        ReadNew(id.ToString(), TextBoxOrd.Text, ref log.text_old, out string NewTimeTemp);

                        sqlConnection.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                            okCount++;
                        else
                            log.isOk = false;

                    }
                    catch (Exception ex)
                    {
                        errCount++;
                        errStr = "Delete err = " + ex.Message;
                        //Response.Write("Delete err = " + ex.Message);

                        log.isOk = false;
                        log.action_result = ex.Message;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }

                    log.action_name = "btnDelete_Click";
                    log.store_id = id;
                    try { log.ord_old = Convert.ToInt32(TextBoxOrd.Text); } catch { log.ord_old = 0; };     // более верно - принудительно заполнить поле
                    log.WriteInfo();

                    allCount++;
                }

            } // for


            // отчет о выполненном действии
            if (allCount > 0)
            {
                if (errCount > 0)
                {
                    //Response.Write("Err in delete procedure");
                    ShowMessage("Error in " + errCount + " of " + allCount + " records. " + errStr, 1);
                }
                else
                {
                    if (okCount == 0)
                    {
                        ShowMessage("Deleted " + okCount + " records", 0);
                    }
                    else
                    {
                        //Response.Write("Delete ok");
                        ShowMessage("Deleted " + okCount + " of " + allCount + " records. Delete ok", 0);
                    }
                }
            }
            else
            {
                ShowMessage("Delete. No record selected", 2);
            }



            // очистка и обновление новости, оригинал в CheckBoxStore_Click
            string idStr = id.ToString();
            string ordStr = FindOrd(id).ToString();
            if (ordStr == "0")
                ordStr = "";

            string NewsName = "";
            string NewsTime = "";

            ReadNew(idStr, ordStr, ref NewsName, out NewsTime);

            TextBoxOrd.Text = ordStr;
            //ASPxNewsDate.Value = NewsTime;
            TextBoxDate.Text = (NewsTime == "") ? NewsTime : Convert.ToDateTime(NewsTime).ToString("dd.MM.yyyy HH:mm");     // no zero or unblink
            //TextBoxNewsName.Text = NewsName;
            //ASPxHtmlEditorNewsName.Html = NewsName;
            AreaNewsName.InnerText = NewsName;
            LabelPreview.Text = NewsName;


            RefreshDataGrid(id);

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();
        }



        protected void btnLoadImage_Click(object sender, EventArgs e)
        {

            int okCount = 0;
            int errCount = 0;
            int allCount = 0;

            string errStr = "";
            string strID = "-1";    // вынесено из цикла только для процедуры ImageShow(Convert.ToInt32(strID));

            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked == true)
                {

                    strID = GridViewStores.Rows[i].Cells[0].Text;
                    //strID = (GridViewStores.Rows[i].Cells[0].Controls[1] as HyperLink).Text;
                    int id = Convert.ToInt32(strID);                    


                    FileAdd(id, ref errStr);

                    if (errStr == "")
                    {
                        okCount++;
                        log.action_result = "The file has been uploaded.";
                    }
                    else
                    {
                        //Response.Write("Load image err = " + ex.Message);
                        errCount++;

                        log.isOk = false;
                        log.action_result = errStr;
                    }

                    log.action_name = "btnLoadImage_Click";
                    log.store_id = id;
                    log.WriteInfo();

                    allCount++;
                }

            } // for


            // отчет о выполненном действии
            if (allCount > 0)
            {
                if (errCount > 0)
                {
                    //Response.Write("Err in load image procedure");
                    ShowMessage("Error in " + errCount + " of " + allCount + " records. " + errStr, 1);
                }
                else
                {
                    //Response.Write("Load image ok");
                    ShowMessage("Saved " + okCount + " of " + allCount + " images. Save image ok", 0);
                    ImageShow(Convert.ToInt32(strID));
                }
            }
            else
            {
                ShowMessage("Save image. No record selected", 2);
            }


        }





        protected void btnDeleteImage_Click(object sender, EventArgs e)
        {

            int okCount = 0;
            int errCount = 0;
            int allCount = 0;

            string errStr = "";
            string strID = "-1";    // вынесено из цикла только для процедуры ImageShow(Convert.ToInt32(strID));            

            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked == true)
                {

                    strID = GridViewStores.Rows[i].Cells[0].Text;
                    //strID = (GridViewStores.Rows[i].Cells[0].Controls[1] as HyperLink).Text;
                    int id = Convert.ToInt32(strID);


                    if (FileDelete(id, ref errStr) > 0)
                        okCount++;

                    if (errStr == "")
                    {
                        log.action_result = "";
                    }
                    else
                    {
                        //Response.Write("Delete err = " + ex.Message);
                        errCount++;

                        log.isOk = false;
                        log.action_result = errStr;
                    }

                    log.action_name = "btnDeleteImage_Click";
                    log.store_id = id;
                    log.WriteInfo();

                    allCount++;
                }

            } // for


            // отчет о выполненном действии
            if (allCount > 0)
            {
                if (errCount > 0)
                {
                    //Response.Write("Err in delete image procedure");
                    ShowMessage("Error in " + errCount + " of " + allCount + " records. " + errStr, 1);
                }
                else
                {
                    if (okCount == 0)
                    {
                        ShowMessage("Deleted " + okCount + " images", 0);
                    }
                    else
                    { 
                        //Response.Write("Delete image ok");
                        ShowMessage("Deleted " + okCount + " of " + allCount + " images. Delete image ok", 0);
                        ImageShow(Convert.ToInt32(strID));
                    }
                }
            }
            else
            {
                ShowMessage("Delete image. No record selected", 2);
            }


        }



        // очистка области редактирования
        private void ClearFields()
        {

            RefreshDataGrid();              // при отсутствии параметра очищается дочерняя таблица
            
            Image1.Visible = false;         // согласно процедуре ImageShow();

            //TextBoxNewsName.Text = "";
            //ASPxHtmlEditorNewsName.Html = "";
            AreaNewsName.InnerText = "";
            TextBoxOrd.Text = "";
            //ASPxNewsDate.Value = "";
            TextBoxDate.Text = "";

            LabelName.Text = "";
            LabelResult.Text = "";
            LabelPreview.Text = "";
        }



        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            int checkedCount = 0;

            // посчитаем количество выбранных строк
            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {
                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked)
                {
                    checkedCount++;
                }
            }

            // если выбраны все строки
            if (checkedCount == GridViewStores.Rows.Count)
            {
                for (int i = 0; i < GridViewStores.Rows.Count; i++)
                {
                    (GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked = false;
                }

                (sender as CheckBox).Checked = false;
                ClearFields();
            }

            // если выбраны некоторые строки
            if (checkedCount < GridViewStores.Rows.Count)
            {
                for (int i = 0; i < GridViewStores.Rows.Count; i++)
                {
                    (GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked = true;
                }

                (sender as CheckBox).Checked = true;
            }
        }






        /* пример
        protected void linkEdit_Click(object sender, EventArgs e)
        {

            string[] commandArgs = (sender as LinkButton).CommandArgument.ToString().Split(new char[] { ',' });
            string ID = commandArgs[0];
            string Name = commandArgs[1];


            TextBoxStore.Text = ID;

        }
        

        protected void Button1_Click(object sender, EventArgs e)
        {

            TextBoxStore.Text = "btn" + GridViewStores.Rows.Count.ToString();


            (GridViewStores.Rows[2].FindControl("CheckBoxStore") as CheckBox).Checked = true;


            SqlDataSourceNews.SelectCommand = "Select * from News where RT_StoreID = 21114";

        }
        */



    }
    }