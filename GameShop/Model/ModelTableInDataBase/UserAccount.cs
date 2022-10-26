using GameShop.Enum;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GameShop.Model
{
    public class UserAccount : ObservableObject
    {
        public int idUser { get; set; }

        private string _Login;

        public string Login
        {
            get => _Login;
            set => SetProperty(ref _Login, value);
        }

        private string _PhoneNumber;

        public string PhoneNumber
        {
            get => _PhoneNumber;
            set => SetProperty(ref _PhoneNumber, value);
        }

        private string _Name;

        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private string _Surname;

        public string Surname
        {
            get => _Surname;
            set => SetProperty(ref _Surname, value);
        }

        private string _Email;

        public string Email
        {
            get => _Email;
            set => SetProperty(ref _Email, value);
        }

        public string PathAvatar { get; set; }

        public Status Status { get; set; }

    }

}
