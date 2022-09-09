using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameShop.Views
{
    public sealed partial class LoginPPage : Page
    {
        public LoginPViewModel ViewModel { get; } = new LoginPViewModel();

        public LoginPPage()
        {
            InitializeComponent();
        }
    }
}
