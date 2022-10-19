using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameShop.Views
{
    public sealed partial class СontrolPanelPage : Page
    {
        public СontrolPanelViewModel ViewModel { get; } = new СontrolPanelViewModel();

        public СontrolPanelPage()
        {
            InitializeComponent();
        }
    }
}
