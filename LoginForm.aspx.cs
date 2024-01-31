using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waRealTimeDB
{
    public partial class LoginForm : System.Web.UI.Page
    {

        // статические поля используются из класса WebFormEditor
        public static bool isLogin = false;
        public static LogWriter log;


        protected void Page_Load(object sender, EventArgs e)
        {
            LabelInfo.Text = "";
            
            // передаем путь к AppLog
            if (log is null)
                log = new LogWriter(Server.MapPath(@"App_Data\AppLog.txt"));
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            //isLogin = true;
            //Response.Redirect("WebForm1.aspx");

            log.user_name = TextBoxLogin.Text;
            log.password = TextBoxPassword.Text;


            isLogin = false;

            string[] lines = System.IO.File.ReadAllLines(Server.MapPath(@"App_Data\Passwords.txt"));

            foreach (string line in lines)
            {
                // пропускаем комментарии
                if (line.Length != 0)
                if (line.Substring(0, 2) != "//")
                {
                    

                    string[] commandArgs = line.Split(new char[] { ',' });
                    string name = commandArgs[0];
                    string pass = commandArgs[1];

                    if ((TextBoxLogin.Text == name) && (TextBoxPassword.Text == pass))
                    {
                        isLogin = true;
                    }
                }
            }


            if (isLogin)
            {
                log.action_result = "Login successful.";
                log.isOk = isLogin;
                log.WriteLoginInfo();
                // Response.Redirect("WebFormEditor.aspx");
                Response.Redirect("LoginPageSelect.aspx");
            }
            else
            {
                log.action_result = "Login or password is incorrect.";
                log.isOk = isLogin;
                log.WriteLoginInfo();

                LabelInfo.Text = "Login or password is incorrect.";
            }



        }
    }
}