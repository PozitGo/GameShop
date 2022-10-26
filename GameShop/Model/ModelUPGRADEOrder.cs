using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model.ModelTableInDataBase
{
    public class ModelUPGRADEOrder : Order
    {
        public string NameProduct { get; set; }
        public string LoginUser { get; set; }
        public string NameUser { get; set; }
        public string SurnameUser { get; set; }
        public string NameSurnameUser { get; set; }
    }
}
