using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Management;        // for user message

namespace waRealTimeDB
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }


        // нигде не используется.
        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var httpException = ex as HttpException ?? ex.InnerException as HttpException;
            if (httpException == null) return;

            if (((System.Web.HttpException)httpException.InnerException).WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
            {
                //handle the error
                Response.Write("Too big a file, dude"); //for example
            }
        }


    }




}