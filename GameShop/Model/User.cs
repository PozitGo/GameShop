using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    public class User
    {
        private string _UserLogin;

        public string UserLogin
        {
            get { return _UserLogin; }
            set { _UserLogin = value; }
        }

        private string _UserPhoneNumber;

        public string UserPhoneNumber
        {
            get { return _UserPhoneNumber; }
            set { _UserPhoneNumber = value; }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _UserSurname;

        public string UserSurname
        {
            get { return _UserSurname; }
            set { _UserSurname = value; }
        }

        private string _UserEmail;

        public string UserEmail
        {
            get { return _UserEmail; }
            set { _UserEmail = value; }
        }





    }
}
