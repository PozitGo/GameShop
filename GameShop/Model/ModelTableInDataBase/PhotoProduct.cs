using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace GameShop.Model
{
    public class PhotoProduct : ObservableObject
    {
        public int idPhoto { get; set; }
        public int idProduct { get; set; }

        private ObservableCollection<BitmapImage> _BitPhotoProducts;

        public ObservableCollection<BitmapImage> BitPhotoProducts
        {
            get => _BitPhotoProducts; 
            set => SetProperty(ref _BitPhotoProducts, value); 
        }


        public PhotoProduct()
        {
            
        }

        public PhotoProduct(int idPhoto, int idProduct, BitmapImage BitPhotoProducts)
        {
            this.idPhoto = idPhoto; this.idProduct = idProduct; this.BitPhotoProducts.Add(BitPhotoProducts);
        }
    }
}
