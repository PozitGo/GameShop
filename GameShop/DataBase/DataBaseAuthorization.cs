using GameShop.Enum;
using GameShop.Model;
using GameShop.ViewModels;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Windows.UI.Xaml.Controls;
using Windows.Web.AtomPub;
using static GameShop.DataBase.DataBaseRequestOrder;
using static GameShop.DataBase.DataBaseRequstUser;

namespace GameShop.DataBase
{
    public struct DataBaseAuthorization 
    {
        public static UserAccount CurrentUser;

        public delegate UserAccount ReturnCurrentUserDelegate();

        private UserAccount ReturnCurrentUser()
        {
            return CurrentUser;
        }
        public static void RegUser(string LoginReg, string PasswordReg, string PhoneNumberReg, string NameReg, string SurnameReg, string EmailReg, AuthInfoBar AuthBar)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand("INSERT INTO `user` (Login, Password, Name, Surname, Email, PhoneNumber) VALUES (@login, @password, @name, @surname, @email, @phonenumber)", db.IsConnection());
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = LoginReg;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = PasswordReg;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = NameReg;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = SurnameReg;
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = EmailReg;
            command.Parameters.Add("@phonenumber", MySqlDbType.VarChar).Value = PhoneNumberReg;

            if (command.ExecuteNonQuery() > 0)
            {
                var temp = DataBaseRequstUser.ReadingDataUser(FindByValueUser.Login, LoginReg);
                CurrentUser = temp[0];
                temp.Clear();
                command.Parameters.Clear();
            }
            else
            {
                AuthBar.Wrong();
                command.Parameters.Clear();
            }
        }

        public static bool LogUser(string LoginLog, string PasswordLog, AuthInfoBar AuthBar = null)
        {
            if ((LoginLog != null || LoginLog != "") && (PasswordLog != null || PasswordLog != ""))
            {
                DataBaseConnect db = new DataBaseConnect();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand("SELECT Login, Password FROM `user` WHERE @log = Login AND @pass = Password", db.IsConnection());
                command.Parameters.Add("@log", MySqlDbType.VarChar).Value = LoginLog;
                command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = PasswordLog;
                adapter.SelectCommand = command;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    ObservableCollection<UserAccount> tempCollection = ReadingDataUser(FindByValueUser.Login, LoginLog);
                    CurrentUser = tempCollection[0];
                    
                    command.Parameters.Clear();
                    return true;
                } 
                else
                {
                    if(AuthBar != null)
                    AuthBar.Wrong();
                    command.Parameters.Clear();
                }
            }
            else
            {
                if (AuthBar != null)
                    AuthBar.IsNull();
            }
            return false;
        }



        public static bool CheckRegData(string LoginReg, string PasswordReg, string PhoneNumberReg, string NameReg, string SurnameReg, string EmailReg, AuthInfoBar AuthBar)
        {

            if ((LoginReg == null || LoginReg == "") && (PasswordReg == null || PasswordReg == "") && (PhoneNumberReg == null || PhoneNumberReg == "")
                && (NameReg == null || NameReg == "") && (SurnameReg == null || SurnameReg == "") && (EmailReg == null || EmailReg == ""))
            {
                AuthBar.IsNull();
                return false;
            }
            else if (DataBaseAuthorization.LoginCheck(LoginReg))
            {
                AuthBar.LoginExists();
                return false;
            }
            else if (DataBaseAuthorization.PhoneNumberCheck(PhoneNumberReg))
            {
                AuthBar.PhoneNumberExists();
                return false;
            }
            else if (DataBaseAuthorization.EmailCheck(EmailReg))
            {
                AuthBar.EmailExists();
                return false;
            }
            else if (CheckSpace(SurnameReg))
            {
                AuthBar.SurnameExists();
                return false;
            }
            else if (CheckSpace(NameReg))
            {
                AuthBar.NameExists();
                return false;
            }
            else if (CheckSpace(PasswordReg))
            {
                AuthBar.PasswordExists();
                return false;
            }
            else 
            {
                return true;
            }
        }
        

        public static bool CheckSpace(string value)
        {
            if (value == null)
                return true;

            bool CheckSpace = false;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == ' ')
                {
                    CheckSpace = true;
                    break;
                }
            }
            return CheckSpace;
        }

        public static bool LoginCheck(string LoginReg)
        {
            bool LoginCheck = false;
            if (CheckSpace(LoginReg))
                return true;

            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT Login FROM `user` WHERE @log = Login", db.IsConnection());
            command.Parameters.Add("@log", MySqlDbType.VarChar).Value = LoginReg;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if(table.Rows.Count > 0)
            {
                return LoginCheck = true;
            }

            return LoginCheck;
        }
        
        public static bool PhoneNumberCheck(string PhoneNumberReg)
        {
            bool PhoneNumberCheck = false;
            if (CheckSpace(PhoneNumberReg))
                return true;

            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT PhoneNumber FROM `user` WHERE @PhoneNumber = PhoneNumber", db.IsConnection());
            command.Parameters.Add("@PhoneNumber", MySqlDbType.VarChar).Value = PhoneNumberReg;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if(table.Rows.Count > 0)
            {
                return PhoneNumberCheck = true;
            }

            return PhoneNumberCheck;
        }

        public static bool EmailCheck(string EmailReg)
        {
            bool EmailCheck = false;
            if (CheckSpace(EmailReg))
                return true;

            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT Email FROM `user` WHERE @Email = Email", db.IsConnection());
            command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = EmailReg;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if(table.Rows.Count > 0)
            {
                return EmailCheck = true;
            }
            
            return EmailCheck;
        }
    }


}
