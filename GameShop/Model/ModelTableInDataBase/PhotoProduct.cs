using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    public class PhotoProduct : ObservableObject
    {
        public int idPhoto { get; set; }
        public int idProduct { get; set; }
        public string PhotoPath { get; set; }
    }
}
