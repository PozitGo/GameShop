using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameShop.Views
{
    public sealed partial class ProductControlPanelPage : Page
    {
        public ProductControlPanelViewModel ViewModel { get; } = new ProductControlPanelViewModel();

        public ProductControlPanelPage()
        {
            InitializeComponent();
        }
    }
}
