using System;
using System.Linq;
using System.Threading.Tasks;
using GameShop.Core.Model;
using GameShop.Core.Services;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GameShop.ViewModels
{
    public class ProductCatalogDetailViewModel : ObservableObject
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public ProductCatalogDetailViewModel()
        {
        }

        public async Task InitializeAsync(long orderID)
        {
            var data = await SampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }
}
