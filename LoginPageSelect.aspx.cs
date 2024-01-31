using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waRealTimeDB
{
    public partial class LoginPageSelect : System.Web.UI.Page
    {

        protected string styleOrd = "visibility: collapse; display: none;";     // используется непосредственно в WebFormEditor.aspx
        private LogWriter log;

        protected string RedirectPage = "";                         // данная страница открывается при неверном пароле


        protected void Page_Load(object sender, EventArgs e)
        {
            
            // чтение строки подключения к БД из файла
            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"App_Data\Config.txt"));

            foreach (string line in lines)
            {
                // пропускаем пустые строки и комментарии
                if ((line.Length != 0) && (line.Substring(0, 2) != "//"))
                {

                    if (line.IndexOf("RedirectPage=") == 0)
                    {
                        RedirectPage = line.Substring(("RedirectPage=").Length, line.Length - ("RedirectPage=").Length);
                    }

                }
            }



            // обработка подключения при первом открытии

            log = LoginForm.log;

            if (log == null)        // иначе невозможно проверить user_name и отобразить кнопку
            {
                Response.Redirect(RedirectPage);
                Response.End();
            }

            if (log.user_name == "test1")
            {
                //styleBtn = "";    // используется на пару строк ниже
                styleOrd = "";      // используется в WebFormEditor.aspx
            }
        }

        protected void ButtonNews_Click(object sender, EventArgs e)
        {
            //Response.Redirect("WebFormEditor.aspx");
            NextPage("WebFormEditor.aspx", LoginForm.isLogin);
        }

        protected void ButtonTarget_Click(object sender, EventArgs e)
        {
            //NextPage(RedirectPage, LoginForm.isLogin);
            NextPage("WebFormStatusEditor.aspx", LoginForm.isLogin);
        }

        protected void ButtonDriver_Click(object sender, EventArgs e)
        {
            //NextPage(DriverPage, LoginForm.isLogin);
            NextPage("WebFormDriverEditor.aspx", LoginForm.isLogin);
        }

        protected void ButtonFind_Click(object sender, EventArgs e)
        {
            NextPage("WebFormFind.aspx", LoginForm.isLogin);
        }

        protected void ButtonFindVideo_Click(object sender, EventArgs e)
        {
            NextPage("WebFormFindVideo.aspx", LoginForm.isLogin);
        }

        protected void ButtonFindChange_Click(object sender, EventArgs e)
        {
            NextPage("WebFormFindChange.aspx", LoginForm.isLogin);
        }

        protected void ButtonFindDiscrepancy_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://pulsefps.dominospizza.mobi");
            Response.End();
        }


        private void NextPage(string url, bool isLogin)
        {

            // var url = "http://www.somepaymentprovider.com";
            // source https://forums.asp.net/t/1910473.aspx?POST+and+Response+Redirect+without+a+form

            Response.Clear();
            var sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form action='{0}' method='post'>", url);
            sb.AppendFormat("<input type='hidden' name='isLogin' value='{0}'>", isLogin);
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();


        }


    }
}