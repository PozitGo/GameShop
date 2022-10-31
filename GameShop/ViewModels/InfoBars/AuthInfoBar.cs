using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.ViewModels
{
    public class AuthInfoBar : ObservableObject
    {
        private bool _IsOpen;
        
        public bool IsOpen
        {
            get => _IsOpen;
            set => SetProperty(ref _IsOpen, value);
        }

        private InfoBarSeverity _Severity;

        public InfoBarSeverity Severity
        {
            get => _Severity;
            set => SetProperty(ref _Severity, value);
        }

        private string _Title;

        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        private string _Message;

        public string Message
        {
            get => _Message;
            set => SetProperty(ref _Message, value);
        }
        public void IsNull()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Не все поля заполенны.";
            Message = "Заполните оставшиеся поля и повторите попытку.";
        }

        public void LoginExists()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Пользователь с таким логином уже существует, либо вы ввели в поле пробел.";
            Message = "Придумайте другой логин, либо уберите пробел и повторите попытку.";
        }

        public void PasswordExists()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Вы ввели в поле 'Пароль' пробел.";
            Message = "Уберите пробел и повторите попытку.";
        }

        public void NameExists()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Вы ввели в поле 'Имя' пробел.";
            Message = "Уберите пробел и повторите попытку.";
        }

        public void SurnameExists()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Вы ввели в поле 'Фамилия' пробел.";
            Message = "Уберите пробел и повторите попытку.";
        }

        public void EmailExists()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Пользователь с такой почтой уже существует, либо вы ввели в поле пробел.";
            Message = "Введите другой почтовый ящик, либо уберите пробел и повторите попытку.";
        }

        public void PhoneNumberExists()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Пользователь с таким номером телефона уже существует, либо вы ввели в поле пробел.";
            Message = "Введите другой номер телефона, либо уберите пробел и повторите попытку.";
        }

        public void Successfully()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Success;
            Title = "Успешно.";
        }

        public void Wrong()
        {
            IsOpen = true;
            Severity = InfoBarSeverity.Error;
            Title = "Неверный логин или пароль, либо вы ввели в поле пробел";
            Message = "Проверьте правильность ввода данных и повторите попытку.";
        }
    }
}
