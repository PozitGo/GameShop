using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameShop.Views
{
    public sealed partial class AccountsPage : Page
    {
        public AccountsViewModel ViewModel { get; } = new AccountsViewModel();

        public AccountsPage()
        {
            InitializeComponent();
        }
    }
}
