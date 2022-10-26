using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace GameShop.DataBase
{
    public struct DataBaseRequstCheck
    {
        public static ObservableCollection<Check> ReadingDataCheck<T>(FindByValueCheck readBy, T parametr)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            string NameFieldByTable;
            ObservableCollection<Check> Collection = new ObservableCollection<Check>();

            switch (readBy)
            {
                case FindByValueCheck.idCheck:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCheck.idCheck);
                        command = new MySqlCommand("SELECT * FROM `check` WHERE " + NameFieldByTable + " = idCheck", db.IsConnection());
                        Collection = ReadCheckByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueCheck.Sum:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCheck.Sum);
                        command = new MySqlCommand("SELECT * FROM `check` WHERE " + NameFieldByTable + " = Sum", db.IsConnection());
                        Collection = ReadCheckByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueCheck.Data:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCheck.Data);
                        command = new MySqlCommand("SELECT * FROM `check` WHERE " + NameFieldByTable + " = Data", db.IsConnection());
                        Collection = ReadCheckByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
            }

            command.Parameters.Clear();
            return Collection;

        }

        public static ObservableCollection<Check> ReadingDataCheck()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            ObservableCollection<Check> Collection = new ObservableCollection<Check>();

            command = new MySqlCommand("SELECT * FROM `check`", db.IsConnection());
            Collection = ReadCheckByParametr(command, adapter, table);

            command.Parameters.Clear();
            return Collection;

        }

        public static ObservableCollection<Check> ReadCheckByParametr<T>(MySqlCommand command, MySqlDataAdapter adapter, DataTable table, T parametr, string NameFieldByTable = null)
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
            return Collection;
        }

        public static ObservableCollection<Check> ReadCheckByParametr(MySqlCommand command, MySqlDataAdapter adapter, DataTable table)
        {
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
            return Collection;
        }

        public static T FindValueByidCheck<T>(int idCheck, FindByValueCheck findBy)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();

            string NameField = "";
            T Data = default;
            switch (findBy)
            {
                case FindByValueCheck.Sum:
                    NameField = nameof(FindByValueProduct.idCategory);
                    command = new MySqlCommand($"SELECT {NameField} FROM `check` WHERE @idCheck = idCheck", db.IsConnection());
                    break;
                case FindByValueCheck.Data:
                    NameField = nameof(FindByValueProduct.Price);
                    command = new MySqlCommand($"SELECT {NameField} FROM `check` WHERE @idCheck = idCheck", db.IsConnection());
                    break;
            }


            command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
            command.Parameters["@idCheck"].Value = idCheck;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Data = (T)readerBy[NameField];
                }
            }

            command.Parameters.Clear();
            return Data;
        }

        public static void UpdateItemInTableCheck<T>(FindByValueCheck findBy, T newValue, int IdPrimaryKey)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
            command.Parameters["@idCheck"].Value = IdPrimaryKey;

            try
            {
                command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Int32));
                command.Parameters["@newValue"].Value = newValue;
            }
            catch
            {

                try
                {
                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Double));
                    command.Parameters["@newValue"].Value = newValue;
                }
                catch
                {

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.VarChar));
                    command.Parameters["@newValue"].Value = newValue;
                }
            }

            command = new MySqlCommand($"UPDATE `check` SET `{nameof(findBy)}` = @newValue WHERE `{nameof(FindByValueCheck.idCheck)}` = @idCheck", db.IsConnection());

            command.ExecuteNonQuery();

            command.Parameters.Clear();
        }

    }
}




