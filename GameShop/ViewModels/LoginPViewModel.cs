using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GameShop.ViewModels
{
    public class LoginPViewModel : ObservableObject
    {
        
        public LoginPViewModel()
        {
            RegButton = new RelayCommand(RegButtonClick);
            LogButton = new RelayCommand(LogButtonClick);
        }
        
        private void RegButtonClick()
        {
            
        }

        private void LogButtonClick()
        {

        }

        public ICommand RegButton { get; }
        public ICommand LogButton { get; }

        private string _LoginReg;
        public string LoginReg
        {
            get => _LoginReg; 
            set => SetProperty(ref _LoginReg, value); 
        }

        private string _PasswordReg;
        public string PasswordReg
        {
            get => _PasswordReg;
            set => SetProperty(ref _PasswordReg, value);
        }

        private string _LoginLog;
        public string LoginLog
        {
            get => _LoginLog;
            set => SetProperty(ref _LoginLog, value);
        }

        private string _PasswordLog;
        public string PasswordLog
        {
            get => _PasswordLog;
            set => SetProperty(ref _PasswordLog, value);
        }

        private string _PhoneNumberReg;
        public string PhoneNumberReg
        {
            get => _PhoneNumberReg;
            set => SetProperty(ref _PhoneNumberReg, value);
        }

        private string _NameReg;
        public string NameReg
        {
            get => _NameReg;
            set => SetProperty(ref _NameReg, value);
        }

        private string _SurnameReg;
        public string SurnameReg
        {
            get => _SurnameReg;
            set => SetProperty(ref _SurnameReg, value);
        }

        private string _EmailReg;
        public string EmailReg
        {
            get => _EmailReg;
            set => SetProperty(ref _EmailReg, value);
        }

    }
}
