using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace GameShop.Model
{
    public class PhotoProduct : ObservableObject
    {
        public int idPhoto { get; set; }
        public int idProduct { get; set; }
        public List<BitmapImage> BitPhotoProducts { get; set; }

        public PhotoProduct()
        {
            
        }

        public PhotoProduct(int idPhoto, int idProduct, BitmapImage BitPhotoProducts)
        {
            this.idPhoto = idPhoto; this.idProduct = idProduct; this.BitPhotoProducts.Add(BitPhotoProducts);
        }
    }
}
