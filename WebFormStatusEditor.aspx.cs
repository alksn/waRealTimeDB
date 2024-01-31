using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

//using System.Runtime.InteropServices;
//using Excel = Microsoft.Office.Interop.Excel; было подключено вручную - browse... dll file

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;



namespace waStatusRT
{
    public partial class WebFormStatusEditor : System.Web.UI.Page
    {

        private string ConnectionStringRT;

        private string pictureFolder = @"C:\images\";                   // путь к папке с картинками. Завершающий обратный слеш обязателен.
        private string pictureExt = ".png";                             // формат файла картинок
        private bool pictureDefaultShow = true;
        private string pictureDefault = @"C:\images\default.png";       // картинка по умолчанию. Если нет картинки для новости
        private bool logToFile = true;                                  // сохранять весь Лог в файл и в базу. Иначе только в базу, в файл - при отсутствии соединения и ошибках
        private bool pictureFixedWidth = false;                         // фиксировать картинку по ширине
        private string highPriority = "max";                            // наивысший приоритет max или min
        private int NewsColomnTextLength = 4000;                        // ограничение по длине для колонки News
        private bool EmptyDateToNull = true;
        protected string DateTimePlaceholder = DateTime.Now.ToString();

        private string styleBtn = "display: none;";                             // стиль кнопок btnAdd, btnDelete
        protected string styleOrd = "visibility: collapse; display: none;";     // используется непосредственно в WebFormEditor.aspx
        protected string previewUrl = "";

                                                                        // статическая переменная сохраняет значение после обновления страницы
        //private static string mainNewSqlText1 = "";                   // текст запроса SqlDataSourceStore текущий. Меняется при выборе даты
        private string mainDefaultSqlText = "";                         // текст запроса SqlDataSourceStore по умолчанию. Используется при выборе даты

        private string mainNewSqlText                                   // эквивалент статической переменной. private static и ViewState работают одинаково
        {
            get
            {
                object obj = ViewState["mainNewSqlText"];
                if (obj != null)
                {
                    return (string)obj;
                }
                else
                {
                    ViewState["mainNewSqlText"] = "";
                    return "";
                }
            }

            set
            {
                ViewState["mainNewSqlText"] = value;
            }
        }



        private bool[] isChecked;                                       // массив для восстановления флагов
        string RedirectPage = "";                                       // данная страница открывается при неверном пароле


        protected void Page_Load(object sender, EventArgs e)
        {
            //LabelVersion.Text = "Application v.1.14";
            LabelVersion.Text = waRealTimeDB.staticConstants.AppVersion;

            // чтение строки подключения к БД из файла
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"App_Data\Config.txt"));

            foreach (string line in lines)
            {
                // пропускаем пустые строки и комментарии
                if ((line.Length != 0) && (line.Substring(0, 2) != "//"))
                {



                    if (line.IndexOf("ConnectionString=") == 0)
                    {
                        ConnectionStringRT = line.Substring(("ConnectionString=").Length, line.Length - ("ConnectionString=").Length);
                        staticConnectionStringRT = ConnectionStringRT;
                    }


                    if (line.IndexOf("RedirectPage=") == 0)
                    {
                        RedirectPage = line.Substring(("RedirectPage=").Length, line.Length - ("RedirectPage=").Length);
                    }

                }
            }


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


            // инициализация компонентов DataSource

            SqlDataSourceStore.ConnectionString = ConnectionStringRT;
            //SqlDataSourceNews.ConnectionString = ConnectionStringRT;

            //SqlDataSourceNews.SelectCommand = "";


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
            mainDefaultSqlText =
                "select st.Id, st.Code, st.Name2 name, head.Name newsName, head.ord, head1.date publishingTime, head1.val from Store st " +

                // шапка новостей из NewsHeader
                /*
                "left outer join(select t1.* from NewsHeader t1 " +
                "where t1.PublishingTime is null or t1.PublishingTime = (" +
                "select " + highPriority + "(t2.PublishingTime) from NewsHeader t2 " +
                "where t2.RT_StoreID = t1.RT_StoreID) " +
                ") head on st.ID = head.RT_StoreID " +
                */
                "left outer join(select * from NewsHeader " +
                ") head on st.ID = head.RT_StoreID " +

                // выбираем последние даты из NewsScore
                "left outer join(select t1.* from NewsScore t1 " +
                "where t1.date is null or t1.date = (" +
                "select " + highPriority + "(t2.date) from NewsScore t2 " +
                "where t2.RT_StoreID = t1.RT_StoreID) " +
                ") head1 on st.ID = head1.RT_StoreID " +

                "where st.Active = 1" +
                "order by st.Id";


            if (mainNewSqlText == "")
                mainNewSqlText = mainDefaultSqlText;
            
            SqlDataSourceStore.SelectCommand = mainNewSqlText;

            // Обработка подключения

            if (!Page.IsPostBack)
            {
                TextBoxNewsDate.Attributes.Add("placeholder", DateTime.Now.ToString("dd.MM.yyyy"));
                TextBoxNewsDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }

            //btnAdd.Attributes.CssStyle.Value = styleBtn;
            //btnDelete.Attributes.CssStyle.Value = styleBtn;

            //LabelResult.Text = "";



            // сохраняем настройки флагов (обновление страницы происходит до обработки нажатия клавиш)
            //CheckBoxSave();

        }

        /*
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
            SqlDataSourceNews.SelectCommand = "";
            GridViewNews.DataBind();
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

            setPreviewText();
        }


        private void setPreviewText()
        {

            string str = LabelName.Text;
            string result = "";

            // в случае если флаги не установлены, но выполнено какое-либо действие
            if (str != "")
            {
                int id = Convert.ToInt32(str.Substring(0, str.IndexOf(".")));

                string strDate = TextBoxNewsDate.Text;
                DateTime updateDate = DateTime.MinValue;

                try
                {
                    if (strDate != "")
                        updateDate = Convert.ToDateTime(strDate);
                }
                catch { }

                if (updateDate != DateTime.MinValue)
                { 
                    SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "dbo.spLeaderBoardScore";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;
                    SqlDataReader reader;


                    cmd.Parameters.AddWithValue("@StoreID", id);
                    cmd.Parameters.AddWithValue("@PeriodType", 0);
                    cmd.Parameters.AddWithValue("@Date", updateDate);

                    try
                    {
                        sqlConnection.Open();
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            if (!Convert.IsDBNull(reader["Name"]))
                            {
                                result = (string)reader["Name"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }    
                }


    
            }

            LabelPreviewStatus.Text = result;

        }




        protected void CheckBoxStore_Click(object sender, EventArgs e)
        {
            // выводим информацию только при установке галочки
            if ((sender as CheckBox).Checked)
            {

                string[] commandArgs = (sender as CheckBox).ToolTip.Split(new char[] { '|' });
                string id = commandArgs[0];
                string ord = commandArgs[1];
                string name = commandArgs[2];
                //string publishingTime = commandArgs[3];


                SqlDataSourceNews.SelectCommand = "Select * from News where RT_StoreID = " + id;
                SqlDataSourceNews.SelectCommand = "";


                string NewsName = commandArgs[4];
                string NewsTime = commandArgs[3];
                if (NewsTime != "")
                    NewsTime = DateTime.Parse(NewsTime).ToString("dd.MM.yyyy");

                //ReadNew(id, ord, ref NewsName, out NewsTime);


                //TextBoxOrd.Text = ord;
                //ASPxNewsDate.Value = NewsTime;
                TextBoxNewsName.Text = NewsName;
                TextBoxNewsDate.Text = NewsTime;
                //ASPxHtmlEditorNewsName.Html = NewsName;
                LabelPreview.Text = NewsName;
                LabelPreviewStatus.Text = "";

                LabelName.Text = id + ". " + name;


                //ImageShow(Convert.ToInt32(id));
 
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

            setPreviewUrl();
        }
        */


        private string ReadConstant(string ConstantName)
        {
            string result = "";

            SqlConnection sqlConnection1 = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "select value from Constants where Name = '" + ConstantName + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            try
            {
                sqlConnection1.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!Convert.IsDBNull(reader["value"]))
                    {
                        result = (string)reader["value"];
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Ошибка подключения к базе данных. Проверьте настройки.\r\n\r\n" + ex.Message);
                Response.Write("Select error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection1.Close();
            }

            return result;
        }


        protected void ButtonReadConstant_Click(object sender, EventArgs e)
        {
            TextBoxNewsName.Text = ReadConstant("NewsHeader");

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            // CheckBoxRestore(); - восстановление требуется только после RefreshDataGrid
            //setPreviewUrl();
        }



        protected void ButtonDateChange_Click(object sender, EventArgs e)
        {
            
            string strDate = TextBoxNewsDate.Text;
            DateTime updateDate = DateTime.MinValue;

            try
            {
                if (strDate != "")
                    updateDate = Convert.ToDateTime(strDate);
                else
                {
                    mainNewSqlText = "";                                        // текст запроса сформируется при загрузке страницы
                    SqlDataSourceStore.SelectCommand = mainDefaultSqlText;      // принудительно, т.к. обработчик PageLoad выполняется до событий компонентов RefreshDataGrid() из конца данной процедуры
                }
            }
            catch {}



            if (updateDate != DateTime.MinValue)
            {
                // CAST, CONVERT удаление времени из даты согласно короткому формату 102 = yyyy.mm.dd
                // внимание - не использовать при более чем одной шапке на один store!!!
                mainNewSqlText =
                    "select st.Id, st.Code, st.Name2 name, head.Name newsName, head.ord, head1.date publishingTime, ISNULL(head1.val, null) val from Store st " +
                    "left outer join(select * from NewsHeader " +
                    ") head on st.ID = head.RT_StoreID " +

                    // выбираем последние даты из NewsScore
                    "left outer join(select t1.* from NewsScore t1 " +
                    "where t1.date is null or CAST(CONVERT(varchar(30), t1.date, 102) as datetime) = " +
                    "CAST('" + updateDate.ToString("MM/dd/yyyy") + "' as datetime)" +
                    ") head1 on st.ID = head1.RT_StoreID " +

                    "where st.Active = 1" +
                    "order by st.Id";


                SqlDataSourceStore.SelectCommand = mainNewSqlText;
            }

            /*
            RefreshDataGrid();

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();
            
            setPreviewUrl();
            */
        }





















        private string FileAdd()
        {
            string SaveFileName = "";
            string errStr = "";

            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                string fn = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                string SaveLocation = Server.MapPath("App_Data") + "\\" + fn;
                SaveFileName = fn;

                try
                {
                    FileUpload1.PostedFile.SaveAs(SaveLocation);
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

            return SaveFileName;
        }









        private void ToDB(int Location_Code, DateTime updateDate, int val)
        {
            if (updateDate == DateTime.MinValue)
                return;

            // NewsScore
            if (UpdateDB(Location_Code, updateDate, val))
            {

            }
            else
            {
                if (!InsertDB(Location_Code, updateDate, val))
                {
                    Response.Write("Not processed NewsScore record. ");
                    //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                    Response.Write("Location_Code=" + Location_Code.ToString() + ". val=" + val.ToString() + ". updateDate=" + updateDate.ToString("dd.MM.yyyy") + "<br>");
                }
            }

            // NewsHeader
            if (UpdateNewsHeader(Location_Code, updateDate))
            {

            }
            else
            {
                if (!InsertNewsHeader(Location_Code, updateDate))
                {
                    Response.Write("Not processed NewsHeader record. ");
                    //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                    Response.Write("Location_Code=" + Location_Code.ToString() + ". val=" + val.ToString() + ". updateDate=" + updateDate.ToString("dd.MM.yyyy") + "<br>");
                }
            }
        }


        private void ExcelFileReadToDB(string FileName)
        {
            FileName = Server.MapPath(@"App_Data\" + FileName);

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(FileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                DateTime updateDate = DateTime.MinValue;
                int Location_Code;
                int val;

                foreach (Row r in sheetData.Elements<Row>())
                {
                    // пропуск строки с пустой ячейкой в столбце "А"
                    if (r.Elements<Cell>().ElementAt(0).CellReference.Value.IndexOf("A") == -1)
                        continue;

                    // получаем дату из первой строки и переходим к следующей
                    if (updateDate == DateTime.MinValue)
                    {
                        string str = r.Elements<Cell>().ElementAt(2).CellValue.Text;
                        updateDate = DateTime.FromOADate(int.Parse(str));
                        continue;
                    }

                    Location_Code = Convert.ToInt32(r.Elements<Cell>().ElementAt(1).CellValue.Text);
                    val = Convert.ToInt32(Math.Round(double.Parse(r.Elements<Cell>().ElementAt(2).CellValue.Text, System.Globalization.CultureInfo.InvariantCulture), 0));

                    ToDB(Location_Code, updateDate, val);
                }
            }
        }






        /*
        private void ExcelFileReadToDB1(string FileName)
        {

            Excel.Application excelApp;
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;

            excelApp = new Excel.Application();
            workbook = excelApp.Workbooks.Open(Server.MapPath(@"App_Data\" + FileName));
            worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);


            string str;
            int row = 1;


            DateTime updateDate = DateTime.MinValue;
            int Location_Code;
            int val;


            range = worksheet.UsedRange;


            while ((range.Cells[row, 1] as Excel.Range).Text != "")
            {
                if (row == 1)
                {
                    updateDate = Convert.ToDateTime((range.Cells[row, 3] as Excel.Range).Text);
                    //Response.Write(str + "<br />");
                    row++;
                }

                //str = (range.Cells[row, 2] as Excel.Range).Value2.ToString() + "|";
                //str += (range.Cells[row, 3] as Excel.Range).Value2.ToString();
                //Response.Write(str + "<br />");

                Location_Code = Convert.ToInt32((range.Cells[row, 2] as Excel.Range).Value2);
                val = Convert.ToInt32(Math.Round((range.Cells[row, 3] as Excel.Range).Value2, 0));


                // NewsScore
                if (UpdateDB(Location_Code, updateDate, val))
                {

                }
                else
                {
                    if (!InsertDB(Location_Code, updateDate, val))
                    {
                        Response.Write("Not processed NewsScore record. ");
                        //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                        Response.Write("Location_Code=" + Location_Code.ToString() + ". val=" + val.ToString() + ". updateDate=" + updateDate.ToString("dd.MM.yyyy") + "<br>");
                    }
                }

                // NewsHeader
                if (UpdateNewsHeader(Location_Code, updateDate))
                {

                }
                else
                {
                    if (!InsertNewsHeader(Location_Code, updateDate))
                    {
                        Response.Write("Not processed NewsHeader record. ");
                        //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                        Response.Write("Location_Code=" + Location_Code.ToString() + ". val=" + val.ToString() + ". updateDate=" + updateDate.ToString("dd.MM.yyyy") + "<br>");
                    }
                }


                row++;
            }

            workbook.Close(false);
            excelApp.Quit();

            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);

            
            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
            {
                // proc.Kill();
            }
            

            //Response.Write("All lines processed.<br>");
            ShowMessage("All lines processed.", 2);
            
        }
        */



        private void FileReadToDB(string FileName)
        {

            // характеристики для первой строки
            DateTime[] d = new DateTime[2];
            int k = 1;



            // чтение строки подключения к БД из файла
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"App_Data\" + FileName));

            foreach (string line in lines)
            {
                // пропускаем пустые строки и комментарии
                if ((line.Length != 0) && (line.Substring(0, 2) != "//"))
                {
                    string substr = line.Replace(" ", "");
                    int[] m = new int[2];
                    
                    int i = 0;
                    substr = substr.Substring(substr.IndexOf(";") + 1, substr.Length - substr.IndexOf(";") - 1);

                    // в первой строке выделяем только дату
                    if (k == 1)
                    {
                        // пропускаем [0] ячейку, т.к. там символ "#"
                        substr = substr.Substring(substr.IndexOf(";") + 1, substr.Length - substr.IndexOf(";") - 1);
                        i++;

                        while (substr.IndexOf(";") >= 0)
                        {
                            d[i] = Convert.ToDateTime(substr.Substring(0, substr.IndexOf(";")));
                            i++;

                            substr = substr.Substring(substr.IndexOf(";") + 1, substr.Length - substr.IndexOf(";") - 1);
                        }
                        d[i] = Convert.ToDateTime(substr);

                        k++;
                        continue;
                    }


                    while (substr.IndexOf(";") >= 0)
                    {
                        m[i] = Convert.ToInt32(substr.Substring(0, substr.IndexOf(";")));
                        i++;

                        substr = substr.Substring(substr.IndexOf(";") + 1, substr.Length - substr.IndexOf(";") - 1);
                    }
                    m[i] = Convert.ToInt32(substr);

                    ToDB(m[0], d[1], m[1]);

                    /*
                    // NewsScore
                    if (UpdateDB(m[0], d[1], m[1]))
                    {
                        
                    }
                    else
                    {
                        if (!InsertDB(m[0], d[1], m[1]))
                        {
                            Response.Write("Not processed NewsScore record. ");
                            //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                            Response.Write(line + "<br>");
                        }
                    }

                    // NewsHeader
                    if (UpdateNewsHeader(m[0], d[1]))
                    {

                    }
                    else
                    {
                        if (!InsertNewsHeader(m[0], d[1]))
                        {
                            Response.Write("Not processed NewsHeader record. ");
                            //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                            Response.Write(line + "<br>");
                        }
                    }
                    */
                }
            }


            //Response.Write("All lines processed.<br>");
            //ShowMessage("All lines processed.", 2);            

        }



        private int ReadID(int Location_Code)
        {
            int result = 0;

            SqlConnection sqlConnection1 = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "select id from Store where code = " + Location_Code.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            try
            {
                sqlConnection1.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!Convert.IsDBNull(reader["id"]))
                    {
                        result = (int)reader["id"];
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write("Ошибка подключения к базе данных. Проверьте настройки.\r\n\r\n" + ex.Message);
                Response.Write("Select error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection1.Close();
            }

            return result;
        }



        private bool InsertDB(int Location_Code, DateTime updateDate, int val)
        {
            bool isOk = true;
            int okCount = 0;

            int RT_StoreID = ReadID(Location_Code);
            if (RT_StoreID == 0)
                return false;

            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT NewsScore (RT_StoreID, date, ord, val) VALUES (@RT_StoreID, @date, @ord, @val)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
            cmd.Parameters.AddWithValue("@date", updateDate.Date);
            cmd.Parameters.AddWithValue("@ord", 1);
            cmd.Parameters.AddWithValue("@val", val);

            try
            {
                sqlConnection.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    okCount++;
                else
                    isOk = false;
            }
            catch (Exception ex)
            {
                isOk = false;
                Response.Write("Insert error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }



        private bool UpdateDB(int Location_Code, DateTime updateDate, int val)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = ReadID(Location_Code);
            if (RT_StoreID == 0)
                return false;

            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE NewsScore SET val = @val where RT_StoreID = @RT_StoreID and date = @date";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
            cmd.Parameters.AddWithValue("@date", updateDate.Date);
            cmd.Parameters.AddWithValue("@val", val);

            try
            {
                sqlConnection.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    okCount++;
                else
                    isOk = false;
            }
            catch (Exception ex)
            {
                isOk = false;
                Response.Write("Update error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }



        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            string SaveFileName = FileAdd();

            if (SaveFileName != "")
            {
                if (Path.GetExtension(SaveFileName) == ".csv")
                    FileReadToDB(SaveFileName);
                else
                    ExcelFileReadToDB(SaveFileName);

                MessageBoxAlert.Show(Page, "All lines processed.");
            }
            else
            {
                //Response.Write("FileName = \"\". File copy to server Error.<br>");
                //ShowMessage("File copy to server Error.", 2);
                MessageBoxAlert.Show(Page, "File not selected.");
            }

            /*
            RefreshDataGrid();
            
            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();

            setPreviewUrl();
            */
        }










        



        private bool InsertNewsHeader(int Location_Code, DateTime updateDate, string val = "")
        {            
            val = TextBoxNewsName.Text;
            if (val == "")
                return true;

            bool isOk = true;
            int okCount = 0;

            int RT_StoreID = ReadID(Location_Code);
            if (RT_StoreID == 0)
                return false;


            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "INSERT NewsHeader (RT_StoreID, PublishingTime, ord, Name) VALUES (@RT_StoreID, @date, @ord, @val)";
            cmd.CommandText = "INSERT NewsHeader (RT_StoreID, ord, Name) VALUES (@RT_StoreID, @ord, @val)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
            //cmd.Parameters.AddWithValue("@date", updateDate.Date);
            cmd.Parameters.AddWithValue("@ord", 10);
            cmd.Parameters.AddWithValue("@val", val);

            try
            {
                sqlConnection.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    okCount++;
                else
                    isOk = false;
            }
            catch (Exception ex)
            {
                isOk = false;
                Response.Write("Insert NewsHeader error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }







        private bool UpdateNewsHeader(int Location_Code, DateTime updateDate, string val = "")
        {
            val = TextBoxNewsName.Text;
            if (val == "")
                return true;

            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = ReadID(Location_Code);
            if (RT_StoreID == 0)
                return false;

            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "UPDATE NewsHeader SET Name = @val, PublishingTime = @date where RT_StoreID = @RT_StoreID and ord = 10";
            cmd.CommandText = "UPDATE NewsHeader SET Name = @val where RT_StoreID = @RT_StoreID and ord = 10";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
            //cmd.Parameters.AddWithValue("@date", updateDate.Date);
            cmd.Parameters.AddWithValue("@val", val);

            try
            {
                sqlConnection.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    okCount++;
                else
                    isOk = false;
            }
            catch (Exception ex)
            {
                isOk = false;
                Response.Write("Update NewsHeader error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }




        [System.Web.Services.WebMethod]
        public static string valChange(string code, string updateDate, string val)
        {
            //return "Hello " + name + Environment.NewLine + "The Current Time is: "
            //    + DateTime.Now.ToString();


            string isOk = "ok";

            int Location_Code;
            DateTime updateDay;
            int value = 0;

            try
            {
                Location_Code = Convert.ToInt32(code);
                updateDay = Convert.ToDateTime(updateDate);
                if (val != "")
                    value = Convert.ToInt32(val);


                //isOk = "code=" + code + ",updateDate=" + updateDate + ",val=" + val;
            }
            catch (Exception e)
            {
                isOk = "UpdateError: " + e.Message;
                return isOk;
            }


            if (val == "")
            {
                if (!staticDeleteNewsScore(Location_Code, updateDay))
                {
                    isOk += "Not processed record. id = " + Location_Code.ToString() + "\n";
                }

                return "Data Delete is OK";
            }

            // Далее работа только с корректными данными
            // UpdateDB(int Location_Code, DateTime updateDate, int val)

            // NewsScore
            if (staticUpdateDB(Location_Code, updateDay, value))
            {
                isOk = "Data Update is OK";
            }
            else
            {
                if (!staticInsertDB(Location_Code, updateDay, value))
                {
                    //Response.Write("Not processed record. ");
                    //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                    isOk = "Not processed record.";
                }
                else
                {
                    isOk = "Data Insert is OK";
                }
            }
            
            return isOk;

        }


        [System.Web.Services.WebMethod]
        public static string newsHeaderChange(string code, string updateDate, string val)
        {
            //return "Hello " + name + Environment.NewLine + "The Current Time is: "
            //    + DateTime.Now.ToString();


            string isOk = "ok";

            int Location_Code;
            DateTime updateDay = DateTime.MinValue;  // любая дата, не используется нигде далее
            //int value;

            try
            {
                Location_Code = Convert.ToInt32(code);
                //updateDay = Convert.ToDateTime(updateDate);
                //value = Convert.ToInt32(val);


                //isOk = "code=" + code + ",updateDate=" + updateDate + ",val=" + val;
            }
            catch (Exception e)
            {
                isOk = "UpdateError: " + e.Message;
                return isOk;
            }


            if (val == "")
            {
                if (!staticDeleteNewsHeader(Location_Code))
                {
                    isOk += "Not processed record. id = " + Location_Code.ToString() + "\n";
                }

                return "Data Delete is OK";
            }

            // Далее работа только с корректными данными
            // UpdateDB(int Location_Code, DateTime updateDate, int val)

            // NewsScore
            if (staticUpdateNewsHeader(Location_Code, updateDay, val))
            {
                isOk = "Data Update is OK";
            }
            else
            {
                if (!staticInsertNewsHeader(Location_Code, updateDay, val))
                {
                    //Response.Write("Not processed record. ");
                    //Response.Write(line + ". 1=" + m[0].ToString() + " 2=" + m[1].ToString() + " 3=" + d[1].ToString() + "<br>");
                    isOk = "Not processed record.";
                }
                else
                {
                    isOk = "Data Insert is OK";
                }
            }

            return isOk;

        }
























        /*

        // очистка области редактирования
        private void ClearFields()
        {

            //RefreshDataGrid();              // при отсутствии параметра очищается дочерняя таблица

            Image1.Visible = false;         // согласно процедуре ImageShow();

            TextBoxNewsName.Text = "";
            TextBoxNewsDate.Text = "";
            //ASPxHtmlEditorNewsName.Html = "";
            //TextBoxOrd.Text = "";
            //ASPxNewsDate.Value = "";

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

            setPreviewUrl();
        }
        */

    }
}