using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.Model
{
    internal class Category : ObservableObject
    {
        public int idCategory { get; set; }

        public string _NameCategory;
        public string NameCategory
        {
            get => _NameCategory;
            set => SetProperty(ref _NameCategory, value);
        }

        public string _Description;
        public string Description
        {
            get => _Description;
            set => SetProperty(ref _Description, value);
        }
    }
}
