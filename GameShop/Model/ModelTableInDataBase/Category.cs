using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GameShop.Model
{
    public class Category : ObservableObject
    {
        public int idCategory { get; set; }

        public string NameCategory { get; set; }

        public string Description { get; set; }
    }
}
