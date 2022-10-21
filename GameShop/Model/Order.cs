using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    public class Order : ObservableObject
    {
        public int idOrder { get; set; }
        public int idProduct { get; set; }
        public int idUser { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public bool Status { get; set; }
        public int idCheck { get; set; }
    }
}
