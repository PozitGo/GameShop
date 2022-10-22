using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    public class Category : ObservableObject
    {
        public int idCategory { get; set; }

        public string NameCategory { get; set; }

        public string Description { get; set; }
    }
}
