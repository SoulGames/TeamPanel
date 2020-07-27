using Dapper.Contrib.Extensions;
using Library.Types;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library.Managers
{
    public class Entries
    {
        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        public static List<Entry> list = new List<Entry>();

        public static void Load(MySqlConnection MySQL)
        {
            foreach (Entry CurrentEntry in MySQL.GetAll<Entry>())
            {
                dic.Add(CurrentEntry.NAME, CurrentEntry.VALUE);
                list.Add(CurrentEntry);
            }
        }
    }
}
