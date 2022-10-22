using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DataBase.DataBaseRequstInTable
{
    public class DataBaseRequstProduct
    {
        public delegate ObservableCollection<Product> ReadingDataProductInCollection(FindByValueProduct readBy = FindByValueProduct.None, object parametr = null);

        public delegate string FindNameByidProductDelegate(int idProduct);

        public delegate Task<bool> SaveNewItemProductByDBDelegate(Product product);
        public static ObservableCollection<Product> ReadingDataProduct(FindByValueProduct readBy = FindByValueProduct.None, object parametr = null)
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
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.idCategory:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.idCategory);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = idCategory", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Price:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Price);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Price", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Quantity:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Quantity);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Quantity", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Name:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Name);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Name", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.Manufacturer:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.Manufacturer);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Manufacturer", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.BasicDescription:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueProduct.BasicDescription);
                        command = new MySqlCommand("SELECT * FROM `product` WHERE " + NameFieldByTable + " = Description", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueProduct.None:
                    if (parametr == null)
                    {
                        command = new MySqlCommand("SELECT * FROM `product`", db.IsConnection());
                        Collection = ReadProductsByParametr(db, command, adapter, table);
                    }
                    break;
            }

            db.CloseConnection();
            return Collection;

        }

        private static ObservableCollection<Product> ReadProductsByParametr(DataBaseConnect db, MySqlCommand command, MySqlDataAdapter adapter, DataTable table, object parametr = null, string NameFieldByTable = null)
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
                    Collection[i].Price = double.Parse(readerBy["Price"].ToString());
                    Collection[i].Quantity = int.Parse(readerBy["Quantity"].ToString());
                    Collection[i].Name = readerBy["Name"].ToString();
                    Collection[i].Manufacturer = readerBy["Manufacturer"].ToString();
                    Collection[i].Rating = int.Parse(readerBy["Rating"].ToString());
                    Collection[i].BasicDescription = readerBy["BasicDescription"].ToString();
                }
            }

            command.Parameters.Clear();
            db.CloseConnection();
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

        public static string FindNameByidProduct(int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string Name = "";
            command = new MySqlCommand("SELECT Name FROM `product` WHERE @idProduct = idProduct", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Name = readerBy["Name"].ToString();
                }
            }

            return Name;
        }
    }
}
