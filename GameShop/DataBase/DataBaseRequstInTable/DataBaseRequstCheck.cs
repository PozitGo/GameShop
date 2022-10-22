using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GameShop.DataBase
{
    public class DataBaseRequstCheck
    {
        public delegate ObservableCollection<Check> ReadingDataCheckInCollection(FindByValueCheck readBy = FindByValueCheck.None, object parametr = null);

        public delegate Task<bool> SaveNewItemCheckByDBDelegate(Check check);
        public static ObservableCollection<Check> ReadingDataCheck(FindByValueCheck readBy = FindByValueCheck.None, object parametr = null)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string NameFieldByTable;
            ObservableCollection<Check> Collection = new ObservableCollection<Check>();

            switch (readBy)
            {
                case FindByValueCheck.idCheck:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCheck.idCheck);
                        command = new MySqlCommand("SELECT * FROM `check` WHERE " + NameFieldByTable + " = idCheck", db.IsConnection());
                        Collection = ReadCheckByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueCheck.Sum:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCheck.Sum);
                        command = new MySqlCommand("SELECT * FROM `check` WHERE " + NameFieldByTable + " = Sum", db.IsConnection());
                        Collection = ReadCheckByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueCheck.Data:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCheck.Data);
                        command = new MySqlCommand("SELECT * FROM `check` WHERE " + NameFieldByTable + " = Data", db.IsConnection());
                        Collection = ReadCheckByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueCheck.None:
                    if (parametr == null)
                    {
                        command = new MySqlCommand("SELECT * FROM `check`", db.IsConnection());
                        Collection = ReadCheckByParametr(db, command, adapter, table);
                    }
                    break;
            }

            db.CloseConnection();
            return Collection;

        }

        public static ObservableCollection<Check> ReadCheckByParametr(DataBaseConnect db, MySqlCommand command, MySqlDataAdapter adapter, DataTable table, object parametr = null, string NameFieldByTable = null)
        {
            if (parametr != null)
            {
                try
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Int32));
                    command.Parameters[NameFieldByTable].Value = parametr;
                }
                catch
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.VarChar));
                        command.Parameters[NameFieldByTable].Value = parametr;
                    }
                    catch
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Double));
                        command.Parameters[NameFieldByTable].Value = parametr;
                    }
                }

            }
            adapter.SelectCommand = command;
            adapter.Fill(table);

            ObservableCollection<Check> Collection = new ObservableCollection<Check>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Check check = new Check();
                    Collection.Add(check);

                    Collection[i].idCheck = int.Parse(readerBy["idCheck"].ToString());
                    Collection[i].Data = readerBy["Data"].ToString();
                    Collection[i].Sum = double.Parse(readerBy["Sum"].ToString());
                }
            }

            command.Parameters.Clear();
            db.CloseConnection();
            return Collection;
        }

    }
}
