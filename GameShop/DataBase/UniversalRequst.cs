using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace GameShop.DataBase
{
    public struct UniversalRequst
    {
        public static ObservableCollection<int> ReadingAllToIdFromTable(string NameTable, string NameId)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            ObservableCollection<int> Collection = new ObservableCollection<int>();

            command = new MySqlCommand($"SELECT {NameId} FROM `{NameTable}`", db.IsConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Collection.Add(int.Parse(readerBy[NameId].ToString()));
                }
            }

            return Collection;
        }
    }
}
