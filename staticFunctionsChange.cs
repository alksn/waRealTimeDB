using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace waRealTimeDB
{
    public class staticFunctionsChange
    {
    }




    public partial class WebFormFindChange : System.Web.UI.Page
    {

        private static string staticConnectionStringRT;     // init from Page_Load WebFormFindVideo.aspx.cs


        public class isResult
        {
            public bool isOk;                               // boolean result instead error code 0
            public string str;                              // string result for some other functions
            public List<string> login;                      // string list result for this module
            public isResult()                               // constructor
            {
                isOk = true;
                str = "";
                login = new List<string>();
            }
        }

        private static isResult staticPasswordChange(string val)
        {

            isResult result = new isResult();


            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            //by name - cmd.CommandText = "select surname, login, sid from secure.Users where surname = @val and firstname = 'Ресторан'";
            /*
            cmd.CommandText =
                "select distinct name2, surname, firstname, login, s.sid ssid, u.sid usid from " +
                "dbo.store s, dbo.SecurePermission p, secure.Users u " +
                "where s.SID = p.SecureObjectId and u.SID = p.UserId " +
                "and s.sid = @val";
            */
            cmd.CommandText = "select top 1 login from secure.Users where surname = @val";

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;
            cmd.Parameters.AddWithValue("@val", val);
            
            try
            {
                sqlConnection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    result.login.Add(rdr["login"].ToString());
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                result.isOk = false;
                //Response.Write("Update error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return result;
        }





        // обновление пароля по логину
        private static isResult staticPasswordChangeNext(string val)
        {

            bool isOk = true;
            int okCount = 0;

            string str = "";

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"[dbo].[SecureSetSendUserPassword]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection;
            cmd.Parameters.AddWithValue("@UserLogin", val);
            SqlParameter returnParameter = cmd.Parameters.Add("@return_value", SqlDbType.Text);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            //sqlConnection.FireInfoMessageEventOnUserErrors = true;
            sqlConnection.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
            {
                str += "\n" + e.Message;
            };


            try
            {
                sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                isOk = false;
                //Response.Write("Update error. " + ex.Message + "\n");
                str = ex.Message;
            }
            finally
            {
                sqlConnection.Close();
            }

            isResult result = new isResult();
            result.str = str;
            result.isOk = isOk;
            return result;
        }


    }




}