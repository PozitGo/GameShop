using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    internal class PhotoProductModel: ObservableObject
    {
        public int idPhoto { get; set; }
        public int idProduct { get; set; }

        private string _PhotoPath;

        public string PhotoPath
        {
            get { return _PhotoPath; }
            set { _PhotoPath = value; }
        }

    }
}
