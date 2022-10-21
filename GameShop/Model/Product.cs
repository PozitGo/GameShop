using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    public class Product : ObservableObject
    {
        public int idProduct { get; set; }

        public int idCategory { get; set; }

        public double Price { get; set; }

        public double Quantity { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public float Rating { get; set; }

        public string BasicDescription { get; set; }
    }
}
