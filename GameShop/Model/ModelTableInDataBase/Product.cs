using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GameShop.Model
{
    public class Product : ObservableObject
    {
        public int idProduct { get; set; }

        public int idCategory { get; set; }

        public float Price { get; set; }

        public string Name { get; set; }

        public string Manufacturer { get; set; }

        public string BasicDescription { get; set; }
    }
}
