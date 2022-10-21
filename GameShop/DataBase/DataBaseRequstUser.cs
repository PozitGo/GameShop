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

namespace GameShop.DataBase
{
    public class DataBaseRequstUser
    {
        public delegate ObservableCollection<UserAccount> ReadingDataUserInCollection(FindByValueUser readBy = FindByValueUser.None, object parametr = null);
        public static ObservableCollection<UserAccount> ReadingDataCheck(FindByValueUser readBy = FindByValueUser.None, object parametr = null)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string NameFieldByTable;
            ObservableCollection<UserAccount> Collection = new ObservableCollection<UserAccount>();

            switch (readBy)
            {
                case FindByValueUser.idUser:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.idUser);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = idUser", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Login:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Login);
                        command = new MySqlCommand($"SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Login", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.PhoneNumber:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.PhoneNumber);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = PhoneNumber", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Email:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Email);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Email", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Name:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Name);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Name", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Surname:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Surname);
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` WHERE " + NameFieldByTable + " = Surname", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.Status:
                    if (parametr != null)
                    {
                        NameFieldByTable = "@" + nameof(FindByValueUser.Status);
                        command = new MySqlCommand($"SELECT idUser, Login,PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user` " + NameFieldByTable + " = Status", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table, parametr, NameFieldByTable);
                    }
                    break;
                case FindByValueUser.None:
                    if (parametr == null)
                    {
                        command = new MySqlCommand("SELECT idUser, Login, PhoneNumber, Email, Name, Surname, Status, PathAvatar FROM `user`", db.IsConnection());
                        Collection = ReadOrdersByParametr(db, command, adapter, table);
                    }
                    break;
            }

            db.CloseConnection();
            return Collection;
        }

        public static ObservableCollection<UserAccount> ReadOrdersByParametr(DataBaseConnect db, MySqlCommand command, MySqlDataAdapter adapter, DataTable table, object parametr = null, string NameFieldByTable = null)
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
            db.CloseConnection();
            return Collection;
        }

    }
}
