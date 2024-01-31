using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace waRealTimeDB
{
    public partial class WebFormFindVideo : System.Web.UI.Page
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
        private LogWriter log;

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
            LabelAppName.Text = "Dominos video surveillance";
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
            
            log = LoginForm.log;

            if (log.user_name == "test1")
            {
                styleBtn = "";      // используется на пару строк ниже
                styleOrd = "";      // используется в WebFormEditor.aspx
            }


            // инициализация компонентов DataSource

            SqlDataSourceStore.ConnectionString = ConnectionStringRT;


            // заспрос возвращает список магазинов с одной новостью наибольшего приоритета
            mainDefaultSqlText =
                //"select case when ip110 is null then ip else ip110 end ip110, ip, Id, Code, name, newsName, ord, publishingTime from " +
                "select nvname, ip110, ip, Id, Code, name, newsName, ord, publishingTime from " +
                "(" +
                "select nv.name nvname, ip, STUFF(st.ip, PATINDEX('%.2', st.ip), 2, '.110') ip110, st.Id, st.Code, st.Name2 name, head1.Name newsName, head1.ord, head1.publishingTime from Store st " +

                "left outer join(select t1.* from NewsDriver t1 " +
                "where t1.PublishingTime is null or t1.publishingTime = ( " +
                "select " + highPriority + "(t2.publishingTime) from NewsDriver t2 " +
                "where t2.RT_StoreID = t1.RT_StoreID " +
                ") " +
                ") head1 on st.ID = head1.RT_StoreID " +
                "left outer join NewsVideo nv on nv.id_store = st.id " +

                "where st.Active = 1 " +
                ") st2 " +
                "order by Id" +
                "";


            if (mainNewSqlText == "")
                mainNewSqlText = mainDefaultSqlText;

            SqlDataSourceStore.SelectCommand = mainNewSqlText;

            // Обработка подключения

            if (!Page.IsPostBack)
            {
                //TextBoxNewsDate.Attributes.Add("placeholder", DateTime.Now.ToString("dd.MM.yyyy"));
                //TextBoxNewsDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
            }


            //btnAdd.Attributes.CssStyle.Value = styleBtn;
            //btnDelete.Attributes.CssStyle.Value = styleBtn;





            // сохраняем настройки флагов (обновление страницы происходит до обработки нажатия клавиш)
            //CheckBoxSave();

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









        [System.Web.Services.WebMethod]
        public static string valChangeVideoIp(string code, string val)
        {
            // copy from driver page

            string isOk = "ok";

            int Location_Code;

            try
            {
                Location_Code = Convert.ToInt32(code);
            }
            catch (Exception e)
            {
                isOk = "UpdateError: " + e.Message;
                return isOk;
            }


            // Далее работа только с корректными данными



            if (!staticUpdateVideoIp(Location_Code, val))
            {
                isOk = "Not processed record.";
            }


            return isOk;

        }







        [System.Web.Services.WebMethod]
        public static string valChangeNewsVideo(string code, string val)
        {
            // copy from driver page

            string isOk = "ok";
            int Location_Code;

            try
            {
                Location_Code = Convert.ToInt32(code);
            }
            catch (Exception e)
            {
                isOk = "UpdateError: " + e.Message;
                return isOk;
            }


            if (val == "")
            {
                if (!staticDeleteNewsVideo(Location_Code))
                {
                    isOk += "Not processed record. id = " + Location_Code.ToString() + "\n";
                }

                return "Data Delete is OK";
            }



            // Далее работа только с корректными данными


            if (staticUpdateNewsVideo(Location_Code, val))
            {
                isOk = "Data Update is OK";
            }
            else
            {
                if (!staticInsertNewsVideo(Location_Code, val))
                {
                    isOk = "Not processed record.";
                }
                else
                {
                    isOk = "Data Insert is OK";
                }
            }

            return isOk;

        }















    }
}