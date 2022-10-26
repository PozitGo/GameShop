using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DataBase
{
    public class DataBaseConnect
    {
        private MySqlConnection Connection;

        private string _PathConnection = "server=localhost;port=3366;username=root;password=root;database=gameshop";

        public string PathConnection
        {
            get => _PathConnection;
        }

        public DataBaseConnect()
        {
            Connection = new MySqlConnection(PathConnection);
            OpenConnection();
        }
        
        public MySqlConnection IsConnection()
        {
            return Connection;
        }

        public void OpenConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
            else
                Debug.WriteLine("База данных уже открыта");
        }

        public void CloseConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
            else
                Debug.WriteLine("База данных уже закрыта");
        }

        ~DataBaseConnect()
        {
            CloseConnection();
        }
    }
}
