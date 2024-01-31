using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace waRealTimeDB
{


    public class LogWriter
    {
        private string LogFileName;                 // путь к файлу журнала. Задается в конструкторе класса.

        public string user_name;
        public string password;

        public int store_id;
        private string store_name;
        public string text_old;
        public string text_new;
        public int ord_old;
        public int ord_new;

        public string action_name;                  // название выполняемого действия
        public string action_result;                // строка с комментарием о результате
        public bool isOk = true;                    // флаг ошибки (успешно ли)
        public string ConnectionStringRT = "";      // строка подключения к бд.
        public bool logToFile = true;               // сохранять весь Лог в файл и в базу. Иначе только в базу, в файл - при отсутствии соединения и ошибках


        private DateTime action_time;

        //public string pageUrl;                    // не используется. Request.Url.ToString()


        private void ClearInfo()
        {
            store_id = -1;
            store_name = "";
            text_old = "";
            text_new = "";
            ord_old = -1;
            ord_new = -1;

            action_name = "";
            action_result = "";

            isOk = true;                            // считаем операция успешна по умолчанию
        }



        // запись информации при подключении. Здесь записывается пароль.
        public void WriteLoginInfo()
        {
            System.IO.File.AppendAllText(LogFileName,
                                        "user_name=" + user_name +
                                        "|password=" + password +
                                        "|action_time=" + DateTime.Now.ToString() +
                                        "|action_result=" + action_result +
                                        "|action_ok=" + isOk.ToString() + "\n"
                                        );
            ClearInfo();
        }


        // запись информации в файл и в базу
        public void WriteInfo()
        {
            action_time = DateTime.Now;
            store_name = FindStoreNameById(store_id);

            // запись в файл
            if (logToFile == true)
                WriteToFile();

            // запись в базу

            string errStr = "";

            // this.ConnectionStringRT заполняется после открытия WebFormEditor
            // события по ветке "иначе" не должны происходить никогда,
            // т.к. процедура WriteInfo вызывается только после подключения к базе
            if (this.ConnectionStringRT != "")
            {
                errStr = WriteToDataBase(ConnectionStringRT);

                if (errStr != "")
                {
                    if (logToFile == false)
                        WriteToFile();

                    System.IO.File.AppendAllText(LogFileName, "LogWriter DB write error: " + errStr + "\n");
                };
            }
            else
            {
                if (logToFile == false)
                    WriteToFile();

                System.IO.File.AppendAllText(LogFileName, "LogWriter (this.ConnectionStringRT == \"\") waring.\n");
            }

            

            ClearInfo();
        }


        private string WriteToFile()
        {
            string errStr = "";

            try
            {
                System.IO.File.AppendAllText(LogFileName,
                                            "user_name=" + user_name +
                                            "|store_id=" + store_id +
                                            "|store_name=" + store_name +
                                            "|text_old=" + text_old +
                                            "|text_new=" + text_new +
                                            "|ord_old=" + ord_old +
                                            "|ord_new=" + ord_new +
                                            "|action_name=" + action_name +
                                            "|action_time=" + action_time.ToString() +
                                            "|action_result=" + action_result +
                                            "|action_ok=" + isOk.ToString() + "\n"
                                            );
            }
            catch (Exception ex)
            {
                errStr = ex.Message;
            }

            return errStr;
        }



        private string WriteToDataBase(string ConnectionStringRT)
        {
            string errStr = "";

            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT newslog (" +
                "user_name, store_id, store_name, text_old, text_new, ord_old, ord_new, " +
                "action_time, action_name, action_result, action_ok) " +
                "VALUES (" +
                "@user_name, @store_id, @store_name, @text_old, @text_new, @ord_old, @ord_new, " +
                "@action_time, @action_name, @action_result, @action_ok)";



            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            cmd.Parameters.AddWithValue("@user_name", user_name);
            cmd.Parameters.AddWithValue("@store_id", store_id);
            cmd.Parameters.AddWithValue("@store_name", store_name);
            cmd.Parameters.AddWithValue("@text_old", text_old);
            cmd.Parameters.AddWithValue("@text_new", text_new);
            cmd.Parameters.AddWithValue("@ord_old", ord_old);
            cmd.Parameters.AddWithValue("@ord_new", ord_new);
            cmd.Parameters.AddWithValue("@action_time", action_time);
            cmd.Parameters.AddWithValue("@action_name", action_name);
            cmd.Parameters.AddWithValue("@action_result", action_result);
            cmd.Parameters.AddWithValue("@action_ok", isOk);

            try
            {
                sqlConnection.Open();
                cmd.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {                
                errStr = ex.Message;
            }
            finally
            {
                sqlConnection.Close();
            }

            return errStr;
        }




        // поиск названия магазина по id
        private string FindStoreNameById(int id)
        {
            string name = "";

            SqlConnection sqlConnection = new SqlConnection(ConnectionStringRT);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Select name2 from Store where id = " + id;

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!Convert.IsDBNull(reader["name2"]))
                    {
                        name = (string)reader["name2"];
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                name = "LogWriter name2 read error.";
            }
            finally
            {                
                sqlConnection.Close();
            }

            return name;
        }







        // конструктор
        public LogWriter(string LogFilePath) {
            LogFileName = LogFilePath;
        }


    }
}