using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    internal class ProductModel : ObservableObject
    {
        public int idProduct { get; set; }

        public int idCategory { get; set; }
        
        public double _Price;
        public double Price
        {
            get => _Price;
            set => SetProperty(ref _Price, value);
        }

        public int Quantity { get; set; }

        public string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        public string _Manufacturer;
        public string Manufacturer
        {
            get => _Manufacturer;
            set => SetProperty(ref _Manufacturer, value);
        }

        public double Rating { get; set; }

        public string _BasicDescription;
        public string BasicDescription
        {
            get => _BasicDescription;
            set => SetProperty(ref _BasicDescription, value);
        }
    }
}
