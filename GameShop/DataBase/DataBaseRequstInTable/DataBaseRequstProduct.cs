using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GameShop.DataBase.DataBaseRequstInTable
{
    public struct DataBaseRequstProduct
    {
        public static ObservableCollection<Product> ReadingDataProduct<T>(FindByValueProduct readBy, T parametr)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string NameFieldByTable;
            ObservableCollection<Product> Collection = new ObservableCollection<Product>();

            switch (readBy)
            {
                case FindByValueProduct.idProduct:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.idProduct);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = idProduct", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.idCategory:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.idCategory);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = idCategory", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Price:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Price);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Price", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Quantity:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Quantity);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Quantity", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Name:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Name);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Name", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Manufacturer:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Manufacturer);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Manufacturer", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.BasicDescription:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.BasicDescription);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Description", db.IsConnection());
                        Collection = ReadProductsByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
            }

            command.Parameters.Clear();
            return Collection;

        }

        public static ObservableCollection<Product> ReadingDataProduct()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            ObservableCollection<Product> Collection = new ObservableCollection<Product>();

            command = new MySqlCommand("SELECT * FROM `product`", db.IsConnection());
            Collection = ReadProductsByParametr(command, adapter, table);


            command.Parameters.Clear();
            return Collection;

        }

        private static ObservableCollection<Product> ReadProductsByParametr<T>(MySqlCommand command, MySqlDataAdapter adapter, DataTable table, T parametr, string NameFieldByTable = null)
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

            ObservableCollection<Product> Collection = new ObservableCollection<Product>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();

                for (int i = 0; readerBy.Read(); i++)
                {
                    Product order = new Product();
                    Collection.Add(order);

                    Collection[i].idProduct = int.Parse(readerBy["idProduct"].ToString());
                    Collection[i].idCategory = int.Parse(readerBy["idCategory"].ToString());
                    Collection[i].Price = float.Parse(readerBy["Price"].ToString());
                    Collection[i].Quantity = int.Parse(readerBy["Quantity"].ToString());
                    Collection[i].Name = readerBy["Name"].ToString();
                    Collection[i].Manufacturer = readerBy["Manufacturer"].ToString();
                    Collection[i].Rating = int.Parse(readerBy["Rating"].ToString());
                    Collection[i].BasicDescription = readerBy["BasicDescription"].ToString();
                }
            }

            command.Parameters.Clear(); ;
            return Collection;
        }

        private static ObservableCollection<Product> ReadProductsByParametr(MySqlCommand command, MySqlDataAdapter adapter, DataTable table)
        {

            adapter.SelectCommand = command;
            adapter.Fill(table);

            ObservableCollection<Product> Collection = new ObservableCollection<Product>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();

                for (int i = 0; readerBy.Read(); i++)
                {
                    Product order = new Product();
                    Collection.Add(order);

                    Collection[i].idProduct = int.Parse(readerBy["idProduct"].ToString());
                    Collection[i].idCategory = int.Parse(readerBy["idCategory"].ToString());
                    Collection[i].Price = float.Parse(readerBy["Price"].ToString());
                    Collection[i].Quantity = int.Parse(readerBy["Quantity"].ToString());
                    Collection[i].Name = readerBy["Name"].ToString();
                    Collection[i].Manufacturer = readerBy["Manufacturer"].ToString();
                    Collection[i].Rating = int.Parse(readerBy["Rating"].ToString());
                    Collection[i].BasicDescription = readerBy["BasicDescription"].ToString();
                }
            }

            command.Parameters.Clear(); ;
            return Collection;
        }

        public static Task<bool> SaveNewItemProductByDB(Product value)
        {
            //потом добавить делегат в принемаемые параметры и инициализировать коллекцию вместе с записью
            return Task.Factory.StartNew(() =>
            {
                if (value != null)
                {
                    DataBaseConnect db = new DataBaseConnect();
                    MySqlCommand command = new MySqlCommand("INSERT INTO `product` (idProduct, idCategory, Price, Quantity, Name, Manufacturer, Rating, BasicDescription) VALUES (@idProduct, @idCategory, @Price, @Quantity, @Name, @Manufacturer, @Rating, @BasicDescription)", db.IsConnection());
                    command.Parameters.Add("@idProduct", MySqlDbType.Int32).Value = value.idProduct;
                    command.Parameters.Add("@idCategory", MySqlDbType.Int32).Value = value.idCategory;
                    command.Parameters.Add("@Price", MySqlDbType.Int32).Value = value.Price;
                    command.Parameters.Add("@Quantity", MySqlDbType.Int32).Value = value.Quantity;
                    command.Parameters.Add("@Name", MySqlDbType.Double).Value = value.Name;
                    command.Parameters.Add("@Manufacturer", MySqlDbType.Int32).Value = value.Manufacturer;
                    command.Parameters.Add("@Rating", MySqlDbType.VarChar).Value = value.Rating;
                    command.Parameters.Add("@BasicDescription", MySqlDbType.Int32).Value = value.BasicDescription;

                    if (command.ExecuteNonQuery() == 7)
                        return true;
                }
                return false;
            });
        }

        public static T FindValueByidProduct<T>(int idProduct, FindByValueProduct findBy)
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
                case FindByValueProduct.idCategory:
                    NameField = nameof(FindByValueProduct.idCategory);
                    command = new MySqlCommand($"SELECT {NameField} FROM `product` WHERE @idProduct = idProduct", db.IsConnection());
                    break;
                case FindByValueProduct.Price:
                    NameField = nameof(FindByValueProduct.Price);
                    command = new MySqlCommand($"SELECT {NameField} FROM `product` WHERE @idProduct = idProduct", db.IsConnection());
                    break;
                case FindByValueProduct.Quantity:
                    NameField = nameof(FindByValueProduct.Quantity);
                    command = new MySqlCommand($"SELECT {NameField} FROM `product` WHERE @idProduct = idProduct", db.IsConnection());
                    break;
                case FindByValueProduct.Name:
                    NameField = nameof(FindByValueProduct.Name);
                    command = new MySqlCommand($"SELECT {NameField} FROM `product` WHERE @idProduct = idProduct", db.IsConnection());
                    break;
                case FindByValueProduct.Manufacturer:
                    NameField = nameof(FindByValueProduct.Manufacturer);
                    command = new MySqlCommand($"SELECT {NameField} FROM `product` WHERE @idProduct = idProduct", db.IsConnection());
                    break;
                case FindByValueProduct.BasicDescription:
                    NameField = nameof(FindByValueProduct.BasicDescription);
                    command = new MySqlCommand($"SELECT {NameField} FROM `product` WHERE @idProduct = idProduct", db.IsConnection());
                    break;
            }


            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

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

        public static void UpdateItemInTableProduct<T>(FindByValueProduct findBy, T newValue, int IdPrimaryKey)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();


            command = new MySqlCommand($"UPDATE `product` SET `{findBy.ToString()}` = @newValue WHERE `{nameof(FindByValueProduct.idProduct)}` = @idProduct", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = IdPrimaryKey;

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

                    command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
                    command.Parameters["@idProduct"].Value = IdPrimaryKey;

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.Int32));
                    command.Parameters["@newValue"].Value = newValue;
                    command.ExecuteNonQuery();
                }
                catch
                {
                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
                    command.Parameters["@idProduct"].Value = IdPrimaryKey;

                    command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.VarChar));
                    command.Parameters["@newValue"].Value = newValue;
                    command.ExecuteNonQuery();
                }
            }

            command.Parameters.Clear();
        }
    }
}
