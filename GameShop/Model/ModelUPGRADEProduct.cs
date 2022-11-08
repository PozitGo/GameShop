using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace GameShop.Model
{
    public class ModelUPGRADEProduct : Product
    {
        public string NameCategory { get; set; }

        public List<ImageSource> PhotoProduct { get; set; }
    }
}
