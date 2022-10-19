using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    internal class OrderModel : ObservableObject
    {
        public int IdOrder { get; set; }

        public int idProduct { get; set; }

        public int IdUser { get; set; }

        public int Quantity { get; set; }

        private double _Price;

        public double Price
        {
            get => _Price;
            set => SetProperty(ref _Price, value);
        }

        public int Discount { get; set; }

        public int idCheck { get; set; }

        public OrderModel(int idOrder, int idProduct, int idUser, int Quantity, double Price, int Discount, int idCheck)
        {
            this.IdOrder = idOrder;
            this.idProduct = idProduct;
            this.IdUser = idUser;
            this.Quantity = Quantity;
            this.Price = Price;
            this.Discount = Discount;
            this.idCheck = idCheck;
        }
    }
}
