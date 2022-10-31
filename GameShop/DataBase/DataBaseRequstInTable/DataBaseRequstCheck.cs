using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;

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
                        DateTime data = new DateTime();
                        data = DateTime.Parse(parametr.ToString());
                        NameFieldByTable = "@" + nameof(FindByValueCheck.Data);
                        command = new MySqlCommand($"SELECT * FROM `check` WHERE Data LIKE '{data.Year}-{data.Month}-{data.Day}%'", db.IsConnection());
                        Collection = ReadCheckByParametr(command, adapter, table);
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
                    command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Double));
                    command.Parameters[NameFieldByTable].Value = parametr;
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                }
                catch
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Int32));
                    command.Parameters[NameFieldByTable].Value = parametr;
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                }

            }

            ObservableCollection<Check> Collection = new ObservableCollection<Check>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Check check = new Check();
                    Collection.Add(check);

                    Collection[i].idCheck = int.Parse(readerBy["idCheck"].ToString());
                    Collection[i].Data = (DateTime)readerBy["Data"];
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
                    Collection[i].Data = (DateTime)readerBy["Data"];
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
                    NameField = nameof(FindByValueCheck.Sum);
                    command = new MySqlCommand($"SELECT `{NameField}` FROM `check` WHERE @idCheck = idCheck", db.IsConnection());
                    break;
                case FindByValueCheck.Data:
                    NameField = nameof(FindByValueCheck.Data);
                    command = new MySqlCommand($"SELECT `{NameField}` FROM `check` WHERE @idCheck = idCheck", db.IsConnection());
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

        public static void SaveNewItemCheckByDB(int idCheck, double value)
        {
            if (value != 0)
            {
                DataBaseConnect db = new DataBaseConnect();
                MySqlCommand command = new MySqlCommand("INSERT INTO `check` (Sum, idCheck) VALUES (@Sum, @idCheck)", db.IsConnection());
                command.Parameters.Add("@Sum", MySqlDbType.Double).Value = Math.Round(value, 2);
                command.Parameters.Add("@idCheck", MySqlDbType.Double).Value = idCheck;
                if (command.ExecuteNonQuery() == 1)
                {
                    command.Parameters.Clear();
                }
                else
                    command.Parameters.Clear();
            }
        }
        public static void UpdateItemInTableCheck<T>(FindByValueCheck findBy, T newValue, int IdPrimaryKey)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"UPDATE `check` SET `{findBy.ToString()}` = @newValue WHERE `{nameof(FindByValueCheck.idCheck)}` = @idCheck", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
            command.Parameters["@idCheck"].Value = IdPrimaryKey;

            try
            {
                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                command.Parameters["@idCheck"].Value = IdPrimaryKey;

                command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Double));
                command.Parameters["@newValue"].Value = newValue;
                command.ExecuteNonQuery();
            }
            catch
            {

                try
                {
                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                    command.Parameters["@idCheck"].Value = IdPrimaryKey;

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Int32));
                    command.Parameters["@newValue"].Value = newValue;
                    command.ExecuteNonQuery();
                }
                catch
                {
                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                    command.Parameters["@idCheck"].Value = IdPrimaryKey;

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.DateTime));
                    command.Parameters["@newValue"].Value = newValue;
                    command.ExecuteNonQuery();
                }
            }

            command.Parameters.Clear();
        }

        public static (Check newSumOldCheck, Check newSumNewCheck) MoveItemFromCheck(int idCheck, int newCheck, Order item)
        {
            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            command.Parameters.Clear();
            Check newSumOldCheck = new Check();
            Check newSumNewCheck = new Check();

            command = new MySqlCommand($"SELECT COUNT(*) FROM `order` WHERE `idCheck` = @idCheck", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
            command.Parameters["@idCheck"].Value = idCheck;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                Int64 count = (Int64)command.ExecuteScalar();
                if (count > 0 && count < 2)
                {
                    command.Parameters.Clear();

                    command = new MySqlCommand($"DELETE FROM `order` WHERE `idOrder` = @idOrder", db.IsConnection());

                    command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
                    command.Parameters["@idOrder"].Value = item.idOrder;

                    if (command.ExecuteNonQuery() != 0)
                    {
                        command.Parameters.Clear();
                        command = new MySqlCommand($"DELETE FROM `check` WHERE `idCheck` = @idCheck", db.IsConnection());

                        command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                        command.Parameters["@idCheck"].Value = idCheck;

                        command.ExecuteNonQuery();
                        newSumOldCheck.idCheck = idCheck;
                        newSumOldCheck.Sum = -1;
                    }
                }
                else
                {
                    if (item != null)
                    {
                        var newSumCheckOld = DataBaseRequstCheck.FindValueByidCheck<double>(idCheck, FindByValueCheck.Sum) - DataBaseRequestOrder.FindValueByidOrder<double>(item.idOrder, FindByValueOrder.Price);
                        DataBaseRequstCheck.UpdateItemInTableCheck<double>(FindByValueCheck.Sum, newSumCheckOld, idCheck);

                        newSumOldCheck.idCheck = idCheck;
                        newSumOldCheck.Sum = newSumCheckOld;
                    }
                }

                if (newCheck != 0 && item != null)
                {
                    newSumNewCheck.Sum = FindValueByidCheck<double>(newCheck, FindByValueCheck.Sum) + item.Price;
                    newSumNewCheck.idCheck = newCheck;
                    DataBaseRequstCheck.UpdateItemInTableCheck<double>(FindByValueCheck.Sum, newSumNewCheck.Sum, newCheck);
                }


            }

            command.Parameters.Clear();
            return (newSumOldCheck, newSumNewCheck);
        }

        public static int[] DeleteCheckAndAllOrders(int idCheck)
        {
            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();
            int[] IdNullOrders = new int[1] { -1};

            command = new MySqlCommand($"SELECT COUNT(*) FROM `order` WHERE `idCheck` = @idCheck", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
            command.Parameters["@idCheck"].Value = idCheck;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                Int64 count = (Int64)command.ExecuteScalar();
                int[] IdOrders = new int[count];
                command.Parameters.Clear();

                command = new MySqlCommand($"SELECT idOrder FROM `order` WHERE `idCheck` = @idCheck", db.IsConnection());

                command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                command.Parameters["@idCheck"].Value = idCheck;

                MySqlDataReader readerBy = command.ExecuteReader();

                for (int j = 0; readerBy.Read(); j++)
                {
                    IdOrders[j] = (int)readerBy["idOrder"];
                }

                readerBy.Close();
                command.Parameters.Clear();
                for (int i = 0; i < count; i++)
                {
                    command = new MySqlCommand($"DELETE FROM `order` WHERE `idOrder` = @idOrder", db.IsConnection());

                    command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
                    command.Parameters["@idOrder"].Value = IdOrders[i];

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }

                command = new MySqlCommand($"DELETE FROM `check` WHERE `idCheck` = @idCheck", db.IsConnection());

                command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                command.Parameters["@idCheck"].Value = idCheck;

                command.ExecuteNonQuery();
                command.Parameters.Clear();
                
                return IdOrders;
            }

            return IdNullOrders;
        }

        public static ObservableCollection<Check> ReadingSumCheckByMinValueAndMaxValue(double MinValue, double MaxValue)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            ObservableCollection<Check> Collection = new ObservableCollection<Check>();

            command = new MySqlCommand($"SELECT * FROM `check` WHERE `Sum` BETWEEN {MinValue} AND {MaxValue + 1}", db.IsConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Collection.Add(new Check
                    {
                        idCheck = int.Parse(readerBy["idCheck"].ToString()),
                        Sum = double.Parse(readerBy["Sum"].ToString()),
                        Data = (DateTime)readerBy["Data"],
                    });
                }
            }
            return Collection;
        }
    }
}




