using System.Collections.Generic;

namespace GameShop.Model
{
    public class ModelUPGRADEProduct : Product
    {
        public string NameCategory { get; set; }

        public List<PhotoProduct> PhotoProducts { get; set; }

        public ModelUPGRADEProduct()
        {

        }

        public ModelUPGRADEProduct(ModelUPGRADEProduct product)
        {
            this.idProduct = product.idProduct; this.idCategory = product.idCategory; this.Price = product.Price; this.Name = product.Name; this.Manufacturer = product.Manufacturer;
            this.BasicDescription = product.BasicDescription; this.NameCategory = product.NameCategory; this.PhotoProducts = product.PhotoProducts;
        }

    }
}
