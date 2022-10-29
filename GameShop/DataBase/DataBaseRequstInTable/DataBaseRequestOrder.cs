using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GameShop.DataBase
{
    public struct DataBaseRequestOrder
    {
        public static ObservableCollection<Order> ReadingDataOrder<T>(FindByValueOrder readBy, T parametr)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();


            command.Parameters.Clear();
            string NameFieldByTable;
            ObservableCollection<Order> Collection = new ObservableCollection<Order>();

            switch (readBy)
            {
                case FindByValueOrder.idOrder:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idOrder);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idOrder", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.idProduct:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idProduct);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idProduct", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.idUser:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idUser);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idUser", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.Quantity:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.Quantity);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = Quantity", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.Price:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.Price);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = Price", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.Status:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.Status);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = Status", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.idCheck:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idCheck);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idCheck", db.IsConnection());
                        Collection = ReadOrdersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
            }

            command.Parameters.Clear();
            return Collection;
        }

        public static ObservableCollection<Order> ReadingDataOrder()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            ObservableCollection<Order> Collection = new ObservableCollection<Order>();

            command = new MySqlCommand("SELECT * FROM `order`", db.IsConnection());
            Collection = ReadOrdersByParametr(command, adapter, table);

            command.Parameters.Clear();
            return Collection;
        }

        private static ObservableCollection<Order> ReadOrdersByParametr<T>(MySqlCommand command, MySqlDataAdapter adapter, DataTable table, T parametr, string NameFieldByTable = null)
        {
            if (parametr != null)
            {
                try
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Double));
                    command.Parameters[NameFieldByTable].Value = parametr;
                }
                catch
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Int32));
                        command.Parameters[NameFieldByTable].Value = parametr;
                    }
                    catch
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.VarChar));
                        command.Parameters[NameFieldByTable].Value = parametr;
                    }
                }
            }

            adapter.SelectCommand = command;
            adapter.Fill(table);

            ObservableCollection<Order> Collection = new ObservableCollection<Order>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Order order = new Order();
                    Collection.Add(order);
                    Collection[i].idOrder = int.Parse(readerBy["idOrder"].ToString());
                    Collection[i].idProduct = int.Parse(readerBy["idProduct"].ToString());
                    Collection[i].idUser = int.Parse(readerBy["idUser"].ToString());
                    Collection[i].Quantity = int.Parse(readerBy["Quantity"].ToString());
                    Collection[i].Price = double.Parse(readerBy["Price"].ToString());
                    Collection[i].Discount = int.Parse(readerBy["Discount"].ToString());
                    Collection[i].Status = readerBy["Status"].ToString();
                    Collection[i].idCheck = int.Parse(readerBy["idCheck"].ToString());
                }
            }

            command.Parameters.Clear();
            return Collection;
        }

        private static ObservableCollection<Order> ReadOrdersByParametr(MySqlCommand command, MySqlDataAdapter adapter, DataTable table)
        {

            adapter.SelectCommand = command;
            adapter.Fill(table);

            ObservableCollection<Order> Collection = new ObservableCollection<Order>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Order order = new Order();
                    Collection.Add(order);
                    Collection[i].idOrder = int.Parse(readerBy["idOrder"].ToString());
                    Collection[i].idProduct = int.Parse(readerBy["idProduct"].ToString());
                    Collection[i].idUser = int.Parse(readerBy["idUser"].ToString());
                    Collection[i].Quantity = int.Parse(readerBy["Quantity"].ToString());
                    Collection[i].Price = double.Parse(readerBy["Price"].ToString());
                    Collection[i].Discount = int.Parse(readerBy["Discount"].ToString());
                    Collection[i].Status = readerBy["Status"].ToString();
                    Collection[i].idCheck = int.Parse(readerBy["idCheck"].ToString());
                }
            }

            command.Parameters.Clear();
            return Collection;
        }

        public static Task<bool> SaveNewItemOrderByDB(Order value)
        {
            return Task.Factory.StartNew(() =>
            {
                if (value != null)
                {
                    DataBaseConnect db = new DataBaseConnect();
                    MySqlCommand command = new MySqlCommand("INSERT INTO `Order` (idOrder, idProduct, idUser, Quantity, Price, Discount, Status, idCheck) VALUES (@idOrder, @idProduct, @idUser, @Quantity, @Price, @Discount, @Status, @idCheck)", db.IsConnection());

                    command.Parameters.Add("@idOrder", MySqlDbType.Int32).Value = value.idOrder;
                    command.Parameters.Add("@idProduct", MySqlDbType.Int32).Value = value.idProduct;
                    command.Parameters.Add("@idUser", MySqlDbType.Int32).Value = value.idUser;
                    command.Parameters.Add("@Quantity", MySqlDbType.Int32).Value = value.Quantity;
                    command.Parameters.Add("@Price", MySqlDbType.Double).Value = value.Price;
                    command.Parameters.Add("@Discount", MySqlDbType.Int32).Value = value.Discount;
                    command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = value.Status;
                    command.Parameters.Add("@idCheck", MySqlDbType.Int32).Value = value.idCheck;

                    if (command.ExecuteNonQuery() == 7)
                    {
                        command.Parameters.Clear();
                        return true;
                    }
                    else
                        command.Parameters.Clear();
                }

                return false;
            });


        }

        public static T FindValueByidOrder<T>(int idOrder, FindByValueOrder findBy)
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
                case FindByValueOrder.idCheck:
                    NameField = nameof(FindByValueOrder.idCheck);
                    command = new MySqlCommand($"SELECT {NameField} FROM `order` WHERE @idOrder = idOrder", db.IsConnection());
                    break;
                case FindByValueOrder.idUser:
                    NameField = nameof(FindByValueOrder.idUser);
                    command = new MySqlCommand($"SELECT {NameField} FROM `order` WHERE @idOrder = idOrder", db.IsConnection());
                    break;
                case FindByValueOrder.Status:
                    NameField = nameof(FindByValueOrder.Status);
                    command = new MySqlCommand($"SELECT {NameField} FROM `order` WHERE @idOrder = idOrder", db.IsConnection());
                    break;
                case FindByValueOrder.Quantity:
                    NameField = nameof(FindByValueOrder.Quantity);
                    command = new MySqlCommand($"SELECT {NameField} FROM `order` WHERE @idOrder = idOrder", db.IsConnection());
                    break;
                case FindByValueOrder.Price:
                    NameField = nameof(FindByValueOrder.Price);
                    command = new MySqlCommand($"SELECT {NameField} FROM `order` WHERE @idOrder = idOrder", db.IsConnection());
                    break;
            }


            command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
            command.Parameters["@idOrder"].Value = idOrder;

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

        public static void UpdateItemInTableOrder<T>(FindByValueOrder findBy, T newValue, int IdPrimaryKey)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"UPDATE `order` SET `{findBy.ToString()}` = @newValue WHERE `{nameof(FindByValueOrder.idOrder)}` = @idOrder", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
            command.Parameters["@idOrder"].Value = IdPrimaryKey;
            try
            {
                command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Double));
                command.Parameters["@newValue"].Value = newValue;
                command.ExecuteNonQuery();
            }
            catch
            {

                try
                {
                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
                    command.Parameters["@idOrder"].Value = IdPrimaryKey;

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Int32));
                    command.Parameters["@newValue"].Value = newValue;
                    command.ExecuteNonQuery();
                }
                catch
                {
                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
                    command.Parameters["@idOrder"].Value = IdPrimaryKey;

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.VarChar));
                    command.Parameters["@newValue"].Value = newValue;
                    command.ExecuteNonQuery();
                }
            }

            command.Parameters.Clear();
        }

        public static bool DeleteItemInTableOrder(int idOrder, int iCheckToOrder, int CountItemsToCheck)
        {
            DataBaseConnect db = new DataBaseConnect();
            
            if (CountItemsToCheck > 1)
            {
                MySqlCommand command = new MySqlCommand($"DELETE FROM `order` WHERE `{nameof(FindByValueOrder.idOrder)}` = @idOrder", db.IsConnection());

                command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
                command.Parameters["@idOrder"].Value = idOrder;

                command.ExecuteNonQuery();

                command.Parameters.Clear();
            }
            else if (CountItemsToCheck == 1)
            {
                MySqlCommand command = new MySqlCommand($"DELETE FROM `order` WHERE `idOrder` = @idOrder", db.IsConnection());

                command.Parameters.Add(new MySqlParameter("@idOrder", MySqlDbType.Int32));
                command.Parameters["@idOrder"].Value = idOrder;

                if (command.ExecuteNonQuery() != 0)
                {
                    command.Parameters.Clear();
                    command = new MySqlCommand($"DELETE FROM `check` WHERE `idCheck` = @idCheck", db.IsConnection());

                    command.Parameters.Add(new MySqlParameter("@idCheck", MySqlDbType.Int32));
                    command.Parameters["@idCheck"].Value = iCheckToOrder;

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    return false;
                }
            }
            
            return true;
        }

    }
}
