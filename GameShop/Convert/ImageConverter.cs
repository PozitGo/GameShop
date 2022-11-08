using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace GameShop.Convert
{
    public static class ImageConverter
    {
        public static BitmapImage GetBitmapAsync(byte[] data)
        {
            if (data is null)
            {
                return null;
            }

            var bitmapImage = new BitmapImage();
            
            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream))
                {
                    writer.WriteBytes(data);
                    writer.StoreAsync().GetResults();
                    writer.FlushAsync().GetResults();
                    writer.DetachStream();
                }

                stream.Seek(0);
                bitmapImage.SetSource(stream);
            }

            return bitmapImage;
        }

        public static List<ImageSource> CreateImageSources(List<byte[]> imagesData)
        {
            List<ImageSource> imageSources = new List<ImageSource>();
            if (imagesData != null)
            {
                foreach (var item in imagesData)
                {
                    imageSources.Add(GetImageSourceAsync(item));
                }
            }

            return imageSources;
        }

        private static ImageSource GetImageSourceAsync(byte[] data)
        {
            return GetBitmapAsync(data);
        }
    }
}
