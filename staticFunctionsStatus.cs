using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace waStatusRT
{
    public class staticFunctions
    {
    }

    public static class MessageBoxAlert
    {
        public static void Show(this System.Web.UI.Page Page, String Message)
        {
            Page.ClientScript.RegisterStartupScript(
               Page.GetType(),
               "MessageBox",
               "<script language='javascript'>alert('" + Message + "');</script>"
            );
        }
    }


    public partial class WebFormStatusEditor : System.Web.UI.Page
    {

        private static string staticConnectionStringRT;
 

        private static bool staticInsertDB(int Location_Code, DateTime updateDate, int val)
        {
            bool isOk = true;
            int okCount = 0;

            int RT_StoreID = Location_Code;


            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
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
                //Response.Write("Insert error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }



        private static bool staticUpdateDB(int Location_Code, DateTime updateDate, int val)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = Location_Code;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
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
                //Response.Write("Update error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }



        private static bool staticDeleteNewsScore(int Location_Code, DateTime updateDate)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = Location_Code;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            //DELETE FROM news WHERE RT_StoreID = @RT_StoreID and ord = @ord
            cmd.CommandText = "DELETE FROM NewsScore where RT_StoreID = @RT_StoreID and date = @date";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
            cmd.Parameters.AddWithValue("@date", updateDate.Date);

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
                //Response.Write("Delete error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }






        // *** NewsHeader ***


        private static bool staticInsertNewsHeader(int Location_Code, DateTime updateDate, string val)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = Location_Code;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
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
                //Response.Write("Insert error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }



        private static bool staticUpdateNewsHeader(int Location_Code, DateTime updateDate, string val)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = Location_Code;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
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
                //Response.Write("Update error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }


        private static bool staticDeleteNewsHeader(int Location_Code)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = Location_Code;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            //DELETE FROM news WHERE RT_StoreID = @RT_StoreID and ord = @ord
            cmd.CommandText = "DELETE FROM NewsHeader where RT_StoreID = @RT_StoreID";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
            //cmd.Parameters.AddWithValue("@date", updateDate.Date);

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
                //Response.Write("Delete error. " + ex.Message + "\n");
            }
            finally
            {
                sqlConnection.Close();
            }

            return isOk;
        }






    }

}