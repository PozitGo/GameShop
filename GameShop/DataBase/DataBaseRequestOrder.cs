using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GameShop.DataBase
{
    public class DataBaseRequestOrder : DataBaseConnect
    {

        public delegate ObservableCollection<Order> ReadingDataOrderInCollection(FindByValueOrder readBy = FindByValueOrder.None, object parametr = null);

        public delegate Task<bool> SaveNewItemOrderByDBDelegate(Order order);
        public static ObservableCollection<Order> ReadingDataOrder(FindByValueOrder readBy = FindByValueOrder.None, object parametr = null)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string NameFieldByTable;
            ObservableCollection<Order> Collection = new ObservableCollection<Order>();

            switch (readBy)
            {
                case FindByValueOrder.idOrder:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idOrder);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idOrder", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.idProduct:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idProduct);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idProduct", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.idUser:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idUser);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idUser", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.Quantity:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.Quantity);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = Quantity", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.Price:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.Price);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = Price", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.Status:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.Status);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = Status", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.idCheck:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueOrder.idCheck);
                        command = new MySqlCommand("SELECT * FROM `order` WHERE " + NameFieldByTable + " = idCheck", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueOrder.None:
                    if (parametr == null)
                    {
                        command = new MySqlCommand("SELECT * FROM `order`", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table);
                    }
                    break;
            }

            db.CloseConnection();
            return Collection;

        }

        private static ObservableCollection<Order> ReadOrdersByParametr(DataBaseConnect db, MySqlCommand command, MySqlDataAdapter adapter, DataTable table, object parametr = null, string NameFieldByTable = null)
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
                        command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Double));
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
                    Collection[i].Status = bool.Parse(readerBy["Status"].ToString());
                    Collection[i].idCheck = int.Parse(readerBy["idCheck"].ToString());
                }
            }

            command.Parameters.Clear();
            db.CloseConnection();
            return Collection;
        }

        public static Task<bool> SaveNewItemOrderByDB(Order value)
        {
            //потом добавить делегат в принемаемые параметры и инициализировать коллекцию вместе с записью
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
                    command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = value.Status.ToString();
                    command.Parameters.Add("@idCheck", MySqlDbType.Int32).Value = value.idCheck;

                    if (command.ExecuteNonQuery() == 7)
                        return true;
                }

                return false;
            });

        }

        public DataBaseRequestOrder()
        {

        }
    }
}
