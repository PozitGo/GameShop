using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace GameShop.Views
{
    public sealed partial class ProductCatalogPage : Page
    {
        public ProductCatalogViewModel ViewModel { get; } = new ProductCatalogViewModel();

        public ProductCatalogPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.LoadDataAsync();
        }
    }
}
