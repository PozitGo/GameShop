using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    internal class CheckModel : ObservableObject
    {
        private int idCheck { get; set; }

        private DateTime _Data;

        public DateTime Data
        {
            get => _Data;
            set => SetProperty(ref _Data, value);
        }

        public double _Sum;

        public double Sum
        {
            get => _Sum;
            set => SetProperty(ref _Sum, value);
        }
    }
}
