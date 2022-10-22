using System;
using System.Diagnostics;
using System.Windows.Input;
using GameShop.Activation;
using GameShop.DataBase;
using GameShop.Helpers;
using GameShop.Services;
using GameShop.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameShop.ViewModels
{
    public class LoginPViewModel : ObservableObject
    {
        private AuthInfoBar _AuthBar;

        public AuthInfoBar AuthBar
        {
            get => _AuthBar;
            set => SetProperty(ref _AuthBar, value);
        }

        private bool _AccountIsSave;

        public bool AccountIsSave
        {
            get => _AccountIsSave; 
            set => SetProperty(ref _AccountIsSave, value);
        }
        public LoginPViewModel()
        {
            RegButton = new RelayCommand(RegButtonClick);
            LogButton = new RelayCommand(LogButtonClick);
            AuthBar = new AuthInfoBar();
        }

        private void RegButtonClick()
        {
            if (DataBaseAuthorization.CheckRegData(LoginReg, PasswordReg, PhoneNumberReg, NameReg, SurnameReg, EmailReg, AuthBar))
            {
                DataBaseAuthorization.RegUser(LoginReg, PasswordReg, PhoneNumberReg, NameReg, SurnameReg, EmailReg, AuthBar);
                Window.Current.Content = new ShellPage();
            }
        }
        
        private void LogButtonClick()
        {
            if (DataBaseAuthorization.CheckSpace(LoginLog) != true)
            {
                if (DataBaseAuthorization.CheckSpace(PasswordLog) != true)
                {
                    if (DataBaseAuthorization.LogUser(LoginLog, PasswordLog, AuthBar))
                    {
                        if (AccountIsSave)
                        {
                            UserSessionService.SaveUserSession(LoginLog, PasswordLog);
                        }
                        
                        Window.Current.Content = new ShellPage();
                    }
                }
                else
                {
                    if(AuthBar != null)
                    AuthBar.Wrong();
                }

            }
            else
            {
                if (AuthBar != null)
                    AuthBar.Wrong();
            }
            //await ApplicationData.Current.LocalSettings.SaveAsync("Login", LoginLog);
            //string data = await ApplicationData.Current.LocalSettings.ReadAsync<string>("Login");
            //MyData data = new MyData() { Data1 = 2, Data2 = LoginLog, Data3 = PasswordLog};
            //await ApplicationData.Current.LocalSettings.SaveAsync("Data1", data);
            //var datadata = await ApplicationData.Current.LocalSettings.ReadAsync<MyData>("Data1");
            //Debug.WriteLine(datadata.Data1);
            //Debug.WriteLine(datadata.Data2);
            //Debug.WriteLine(datadata.Data3);
        }

        class MyData
        {
            public int Data1 { get; set; }
            public string Data2 { get; set; }
            public string Data3 { get; set; }
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
