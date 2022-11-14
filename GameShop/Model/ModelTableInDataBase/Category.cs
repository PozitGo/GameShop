using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace GameShop.Model
{
    public class Category : ObservableObject
    {
        public int idCategory { get; set; }

        public string NameCategory { get; set; }

        public string Description { get; set; }

        public Category()
        {
            
        }

        public Category(int idCategory, string NameCategory, string Descripction)
        {
            this.idCategory = idCategory; this.NameCategory = NameCategory; this.Description = Descripction;
        }
    }
}
