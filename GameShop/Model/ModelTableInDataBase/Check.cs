using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    public class Check : ObservableObject
    {
        public int idCheck { get; set; }
        public string Data { get; set; }
        public double Sum { get; set; }
    }
}
