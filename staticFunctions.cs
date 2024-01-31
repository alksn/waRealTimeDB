using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace waRealTimeDB
{


    public static class staticFunctions
    {

        // проверка ip адреса по количеству точек
        public static int WordsCount(string str, string sub)
        {
            int count = (str.Length - str.Replace(sub, "").Length) / sub.Length;
            return count;
        }

    }



}