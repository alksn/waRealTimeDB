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
//using Excel = Microsoft.Office.Interop.Excel;


namespace waDriver
{
    public partial class WebFormDriverEditor : System.Web.UI.Page
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
        protected string RedirectPage = "";                             // данная страница открывается при неверном пароле


        protected void Page_Load(object sender, EventArgs e)
        {
            LabelAppName.Text = "Dominos Safety editor";
            //LabelVersion.Text = "Application v.1.03";
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


            // заспрос возвращает список магазинов с одной новостью наибольшего приоритета
            mainDefaultSqlText =
                "select st.Id, st.Code, st.Name2 name, head1.Name newsName, head1.ord, head1.publishingTime from Store st " +

                "left outer join(select t1.* from NewsDriver t1 " +
                "where t1.PublishingTime is null or t1.publishingTime = ( " +
                "select " + highPriority + "(t2.publishingTime) from NewsDriver t2 " +
                "where t2.RT_StoreID = t1.RT_StoreID " +
                ") " +
                ") head1 on st.ID = head1.RT_StoreID " +

                "where st.Active = 1 " +
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





            // сохраняем настройки флагов (обновление страницы происходит до обработки нажатия клавиш)
            CheckBoxSave();

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
            //SqlDataSourceNews.SelectCommand = "Select * from News where RT_StoreID = " + id;
            //SqlDataSourceNews.SelectCommand = "";
            //GridViewNews.DataBind();
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


                string NewsName = commandArgs[4];
                string NewsTime = commandArgs[3];
                if (NewsTime != "")
                    NewsTime = DateTime.Parse(NewsTime).ToString("dd.MM.yyyy");


                TextBoxNewsName.Text = NewsName;
                //TextBoxNewsDate.Text = NewsTime; - не читается из aspx page

 
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
                mainNewSqlText =
                    "select st.Id, st.Code, st.Name2 name, head1.Name newsName, head1.ord, head1.publishingTime from Store st " +

                    "left outer join(select t1.* from NewsDriver t1 " +
                    "where t1.PublishingTime is null or t1.publishingTime = ( " +
                    "select " + highPriority + "(t2.publishingTime) from NewsDriver t2 " +
                    "where t2.RT_StoreID = t1.RT_StoreID " +
                    "and CAST(CONVERT(varchar(30), t2.publishingTime, 102) as datetime) <= CAST('" + updateDate.ToString("MM/dd/yyyy") + "' as datetime) " +
                    ") " +
                    ") head1 on st.ID = head1.RT_StoreID " +

                    "where st.Active = 1 " +
                    "order by st.Id";

                SqlDataSourceStore.SelectCommand = mainNewSqlText;
            }


            RefreshDataGrid();

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();
            
            //setPreviewUrl();
        }






        protected void ButtonSaveDriverText_Click(object sender, EventArgs e)
        {

            string isOk = "ok";
            string val = TextBoxNewsName.Text;

            DateTime updateDay = DateTime.MinValue;

            try
            {
                updateDay = Convert.ToDateTime(TextBoxNewsDate.Text);
            }
            catch (Exception ex)
            {
                isOk = "UpdateError: " + ex.Message;
                isOk += "\n" + "Ошибка: Не выбрана дата.";
            }

            if (isOk == "ok")
            for (int i = 0; i < GridViewStores.Rows.Count; i++)
            {

                if ((GridViewStores.Rows[i].FindControl("CheckBoxStore") as CheckBox).Checked == true)
                {

                    string strID = GridViewStores.Rows[i].Cells[0].Text;
                    int id = Convert.ToInt32(strID);

                    
                    if (val == "")
                    {
                        if (!staticDeleteNewsDriver(id, updateDay))
                        {
                            isOk += "Not processed record. id = " + id.ToString() + "\n";
                        }
                    }
                    else

                    if (staticUpdateNewsDriver(id, updateDay, val))
                    {
                        //isOk = "Data Update is OK";
                    }
                    else
                    {
                        if (!staticInsertNewsDriver(id, updateDay, val))
                        {
                            isOk += "Not processed record. id = " + id.ToString() + "\n";
                        }
                        else
                        {
                            //isOk = "Data Insert is OK";
                        }
                    }


                }
            }

            if (isOk == "ok")
            {
                //MessageBoxAlert.Show(Page, "All lines processed.");
                MessageBoxAlert.Show(Page, "Изменения внесены.");
            }
            else
            {
                MessageBoxAlert.Show(Page, isOk);
            }
            

            RefreshDataGrid();

            // восстанавливаем значение флагов. Событие обработки клавиш происходит после обновления страницы.
            CheckBoxRestore();

        }








        [System.Web.Services.WebMethod]
        public static string valChangeNewsDriver(string code, string updateDate, string val)
        {
            //return "Hello " + name + Environment.NewLine + "The Current Time is: "
            //    + DateTime.Now.ToString();


            string isOk = "ok";

            int Location_Code;
            DateTime updateDay;
            //int value;

            try
            {
                Location_Code = Convert.ToInt32(code);
                updateDay = Convert.ToDateTime(updateDate);
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
                if (!staticDeleteNewsDriver(Location_Code, updateDay))
                {
                    isOk += "Not processed record. id = " + Location_Code.ToString() + "\n";
                }

                return "Data Delete is OK";
            }

            // Далее работа только с корректными данными
            // UpdateDB(int Location_Code, DateTime updateDate, int val)
            
            // NewsDriver
            if (staticUpdateNewsDriver(Location_Code, updateDay, val))
            {
                isOk = "Data Update is OK";
            }
            else
            {
                if (!staticInsertNewsDriver(Location_Code, updateDay, val))
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









        // очистка области редактирования
        private void ClearFields()
        {

            TextBoxNewsName.Text = "";
            TextBoxNewsDate.Text = "";

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

            //setPreviewUrl();
        }


    }
}