using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace GameShop.DataBase.DataBaseRequstInTable
{
    public struct DataBaseRequstCategory
    {
        public static ObservableCollection<Category> ReadingDataCategory<T>(FindByValueCategory readBy, T parametr)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            string NameFieldByTable;
            ObservableCollection<Category> Collection = new ObservableCollection<Category>();

            switch (readBy)
            {
                case FindByValueCategory.idCategory:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCategory.idCategory);
                        command = new MySqlCommand("SELECT * FROM `category` WHERE " + NameFieldByTable + " = idCategory", db.IsConnection());
                        Collection = ReadCategoryByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueCategory.NameCategory:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueCategory.NameCategory);
                        command = new MySqlCommand("SELECT * FROM `category` WHERE " + NameFieldByTable + " = NameCategory", db.IsConnection());
                        Collection = ReadCategoryByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
            }

            command.Parameters.Clear();
            return Collection;

        }

        public static ObservableCollection<Category> ReadingDataCategory()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            ObservableCollection<Category> Collection = new ObservableCollection<Category>();

            command = new MySqlCommand("SELECT * FROM `category`", db.IsConnection());
            Collection = ReadCategoryByParametr(command, adapter, table);

            command.Parameters.Clear();
            return Collection;

        }

        public static ObservableCollection<Category> ReadCategoryByParametr<T>(MySqlCommand command, MySqlDataAdapter adapter, DataTable table, T parametr, string NameFieldByTable = null)
        {
            if (parametr != null)
            {
                try
                {
                    command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.VarChar));
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

            ObservableCollection<Category> Collection = new ObservableCollection<Category>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Category category = new Category();
                    Collection.Add(category);

                    Collection[i].idCategory = int.Parse(readerBy["idCategory"].ToString());
                    Collection[i].NameCategory = readerBy["NameCategory"].ToString();
                    Collection[i].Description = readerBy["Description"].ToString();
                }
            }

            command.Parameters.Clear();
            return Collection;
        }

        public static ObservableCollection<Category> ReadCategoryByParametr(MySqlCommand command, MySqlDataAdapter adapter, DataTable table)
        {
            adapter.SelectCommand = command;
            adapter.Fill(table);

            ObservableCollection<Category> Collection = new ObservableCollection<Category>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    Category category = new Category();
                    Collection.Add(category);

                    Collection[i].idCategory = int.Parse(readerBy["idCategory"].ToString());
                    Collection[i].NameCategory = readerBy["NameCategory"].ToString();
                    Collection[i].Description = readerBy["Description"].ToString();
                }
            }

            command.Parameters.Clear();
            return Collection;
        }

        public static T FindValueByidCategory<T>(int idCategory, FindByValueCategory findBy)
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
                case FindByValueCategory.idCategory:
                    NameField = nameof(FindByValueCategory.idCategory);
                    command = new MySqlCommand($"SELECT `{NameField}` FROM `category` WHERE @idCategory = idCategory", db.IsConnection());
                    break;
                case FindByValueCategory.NameCategory:
                    NameField = nameof(FindByValueCategory.NameCategory);
                    command = new MySqlCommand($"SELECT `{NameField}` FROM `category` WHERE @idCategory = idCategory", db.IsConnection());
                    break;
            }

            command.Parameters.Add(new MySqlParameter("@idCategory", MySqlDbType.Int32));
            command.Parameters["@idCategory"].Value = idCategory;

            adapter.SelectCommand = command;
            adapter.FillAsync(table);

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

        public static void SaveNewItemCategoryByDB(Category category)
        {
            if (category != null)
            {
                DataBaseConnect db = new DataBaseConnect();
                MySqlCommand command = new MySqlCommand("INSERT INTO `category` (idCategory, NameCategory, Description) VALUES (@idCategory, @NameCategory, @Description)", db.IsConnection());
                command.Parameters.Add("@idCategory", MySqlDbType.Int32).Value = category.idCategory;
                command.Parameters.Add("@NameCategory", MySqlDbType.VarChar).Value = category.NameCategory;
                command.Parameters.Add("@Description", MySqlDbType.VarChar).Value = category.Description;

                command.ExecuteNonQuery();

                command.Parameters.Clear();

            }
        }
        public static void UpdateItemInTableCategory<T>(FindByValueCategory findBy, T newValue, int IdPrimaryKey)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"UPDATE `category` SET `{findBy.ToString()}` = @newValue WHERE `{nameof(FindByValueCategory.idCategory)}` = @idCategory", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idCategory", MySqlDbType.Int32));
            command.Parameters["@idCategory"].Value = IdPrimaryKey;

            command.Parameters.Add(new MySqlParameter("@newValue", MySqlDbType.VarChar));
            command.Parameters["@newValue"].Value = newValue;
            command.ExecuteNonQuery();

            command.Parameters.Clear();
        }

        public static int MaxIdCategory()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"SELECT MAX(`idCategory`) FROM `category`", db.IsConnection());

            return (int)command.ExecuteScalar();
        }

        public static void DeleteCategory(int idCategory)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"DELETE FROM `category` WHERE `idCategory` = @idCategory", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idCategory", MySqlDbType.Int32));
            command.Parameters["@idCategory"].Value = idCategory;

            command.ExecuteNonQuery();
            command.Parameters.Clear();
        }
    }
}
