using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GameShop.Model
{
    public class PhotoProduct : ObservableObject
    {
        public int idPhoto { get; set; }
        public int idProduct { get; set; }
        public string PhotoPath { get; set; }
    }
}
