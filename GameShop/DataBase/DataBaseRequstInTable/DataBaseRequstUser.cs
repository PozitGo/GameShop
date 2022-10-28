using GameShop.Enum;
using GameShop.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;

namespace GameShop.DataBase
{
    public struct DataBaseRequstUser
    {
        public static ObservableCollection<UserAccount> ReadingDataUser<T>(FindByValueUser readBy, T parametr)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            string NameFieldByTable;
            ObservableCollection<UserAccount> Collection = new ObservableCollection<UserAccount>();

            switch (readBy)
            {
                case FindByValueUser.idUser:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.idUser);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = idUser", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Login:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Login);
                        command = new MySqlCommand($"SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Login", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.PhoneNumber:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.PhoneNumber);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = PhoneNumber", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Email:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Email);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Email", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Name:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Name);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Name", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Surname:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Surname);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Surname", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Status:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Status);
                        command = new MySqlCommand($"SELECT idUser, Login,PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` " + NameFieldByTable + " = Status", db.IsConnection());
                        Collection = ReadUsersByParametr(command, adapter, table, parametr, NameFieldByTable);
                    }
                    break; 
            }

            command.Parameters.Clear();
            return Collection;
        }

        public static ObservableCollection<UserAccount> ReadingDataUser()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            command.Parameters.Clear();
            ObservableCollection<UserAccount> Collection = new ObservableCollection<UserAccount>();

            command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user`", db.IsConnection());
            Collection = ReadUsersByParametr(command, adapter, table);

            command.Parameters.Clear();
            return Collection;
        }

        public static ObservableCollection<UserAccount> ReadUsersByParametr<T>(MySqlCommand command, MySqlDataAdapter adapter, DataTable table, T parametr, string NameFieldByTable = null)
        {
            if (parametr != null)
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
                    command.Parameters.Add(new MySqlParameter(NameFieldByTable, MySqlDbType.Int32));
                    command.Parameters[NameFieldByTable].Value = parametr;
                }

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            ObservableCollection<UserAccount> Collection = new ObservableCollection<UserAccount>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    UserAccount user = new UserAccount();
                    Collection.Add(user);

                    Collection[i].idUser = int.Parse(readerBy["idUser"].ToString());
                    Collection[i].Login = readerBy["Login"].ToString();
                    Collection[i].PhoneNumber = readerBy["PhoneNumber"].ToString();
                    Collection[i].Email = readerBy["Email"].ToString();
                    Collection[i].Name = readerBy["Name"].ToString();
                    Collection[i].Surname = readerBy["Surname"].ToString();
                    Collection[i].PathAvatar = readerBy["PathAvatar"].ToString();
                    Collection[i].Status = (Status)int.Parse(readerBy["Status"].ToString());
                }
            }
            command.Parameters.Clear();
            return Collection;
        }

        public static ObservableCollection<UserAccount> ReadUsersByParametr(MySqlCommand command, MySqlDataAdapter adapter, DataTable table)
        {
            adapter.SelectCommand = command;
            adapter.Fill(table);
            
            ObservableCollection<UserAccount> Collection = new ObservableCollection<UserAccount>();
            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    UserAccount user = new UserAccount();
                    Collection.Add(user);

                    Collection[i].idUser = int.Parse(readerBy["idUser"].ToString());
                    Collection[i].Login = readerBy["Login"].ToString();
                    Collection[i].PhoneNumber = readerBy["PhoneNumber"].ToString();
                    Collection[i].Email = readerBy["Email"].ToString();
                    Collection[i].Name = readerBy["Name"].ToString();
                    Collection[i].Surname = readerBy["Surname"].ToString();
                    Collection[i].PathAvatar = readerBy["PathAvatar"].ToString();
                    Collection[i].Status = (Status)int.Parse(readerBy["Status"].ToString());
                }
            }
            command.Parameters.Clear();
            return Collection;
        }

        public static T FindValueByidUser<T>(int idUser, FindByValueUser findBy)
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
                case FindByValueUser.Login:
                    NameField = nameof(FindByValueUser.Login);
                    command = new MySqlCommand($"SELECT {NameField} FROM `user` WHERE @idUser = idUser", db.IsConnection());
                    break;
                case FindByValueUser.Name:
                    NameField = nameof(FindByValueUser.Name);
                    command = new MySqlCommand($"SELECT {NameField} FROM `user` WHERE @idUser = idUser", db.IsConnection());
                    break;
                case FindByValueUser.Surname:
                    NameField = nameof(FindByValueUser.Surname);
                    command = new MySqlCommand($"SELECT {NameField} FROM `user` WHERE @idUser = idUser", db.IsConnection());
                    break;
                case FindByValueUser.Email:
                    NameField = nameof(FindByValueUser.Email);
                    command = new MySqlCommand($"SELECT {NameField} FROM `user` WHERE @idUser = idUser", db.IsConnection());
                    break;
                case FindByValueUser.PhoneNumber:
                    NameField = nameof(FindByValueUser.PhoneNumber);
                    command = new MySqlCommand($"SELECT {NameField} FROM `user` WHERE @idUser = idUser", db.IsConnection());
                    break;
                case FindByValueUser.Status:
                    NameField = nameof(FindByValueUser.Status);
                    command = new MySqlCommand($"SELECT {NameField} FROM `user` WHERE @idUser = idUser", db.IsConnection());
                    break;
            }

            command.Parameters.Add(new MySqlParameter("@idUser", MySqlDbType.Int32));
            command.Parameters["@idUser"].Value = idUser;

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

        public static void UpdateItemInTableUser<T>(FindByValueUser findBy, T newValue, int IdPrimaryKey)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"UPDATE `check` SET `{findBy.ToString()}` = @newValue WHERE `{nameof(FindByValueUser.idUser)}` = @idUser", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idUser", MySqlDbType.Int32));
            command.Parameters["@idUser"].Value = IdPrimaryKey;

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

            command.ExecuteNonQuery();

            command.Parameters.Clear();
        }

    }
}
