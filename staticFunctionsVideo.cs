using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace waRealTimeDB
{
    public class staticFunctionsVideo
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


    public partial class WebFormFindVideo : System.Web.UI.Page
    {

        private static string staticConnectionStringRT;     // init from Page_Load WebFormFindVideo.aspx.cs



        private static bool staticUpdateVideoIp(int Location_Code, string val)
        {
            bool isOk = true;
            int okCount = 0;
            int RT_StoreID = Location_Code;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE Store SET ip = @val where ID = @RT_StoreID";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@RT_StoreID", RT_StoreID);
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





        // copy from driver page, for NewsVideoName field

        private static bool staticInsertNewsVideo(int Location_Code, string val)
        {
            bool isOk = true;
            int okCount = 0;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT NewsVideo (id_store, name) VALUES (@id_store, @name)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@id_store", Location_Code);
            cmd.Parameters.AddWithValue("@name", val);

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



        private static bool staticUpdateNewsVideo(int Location_Code, string val)
        {
            bool isOk = true;
            int okCount = 0;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE NewsVideo SET name = @name where id_store = @id_store";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@id_store", Location_Code);
            cmd.Parameters.AddWithValue("@name", val);

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


        private static bool staticDeleteNewsVideo(int Location_Code)
        {
            bool isOk = true;
            int okCount = 0;

            SqlConnection sqlConnection = new SqlConnection(staticConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM NewsVideo where id_store = @id_store";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@id_store", Location_Code);

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