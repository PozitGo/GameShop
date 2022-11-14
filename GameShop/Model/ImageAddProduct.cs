using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace GameShop.Model
{
    public class ImageAddProduct : ObservableObject
    {
        public byte[] ByteImage { get; set; }

        public BitmapImage Image { get; set; }

        public ImageAddProduct()
        {
                
        }

        public ImageAddProduct(byte[] ByteImage, BitmapImage Image)
        {
            this.ByteImage = ByteImage; this.Image = Image;
        }
    }
}
