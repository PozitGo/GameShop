using GameShop.Enum;
using GameShop.Model.ModelTableInDataBase;
using GameShop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DataBase.DataBaseRequstInTable;
using Microsoft.UI.Xaml.Controls;

namespace GameShop.Convert
{
    public struct ConvertProductAndProductUPGRADE
    {
        public static ObservableCollection<ModelUPGRADEProduct> ConvertFromProductCollectionInProductUPGRADECollection(ObservableCollection<Product> Product)
        {
            ObservableCollection<ModelUPGRADEProduct> ProductUPGRADE = new ObservableCollection<ModelUPGRADEProduct>();
            if (Product != null)
            {
                for (int i = 0; i < Product.Count; i++)
                {
                    ModelUPGRADEProduct orderU = new ModelUPGRADEProduct();
                    
                    ProductUPGRADE.Add(orderU);

                    ProductUPGRADE[i].idProduct = Product[i].idProduct;
                    ProductUPGRADE[i].idCategory = Product[i].idCategory;
                    ProductUPGRADE[i].NameCategory = DataBaseRequstCategory.FindValueByidCategory<string>(Product[i].idCategory, FindByValueCategory.NameCategory);
                    ProductUPGRADE[i].Name = Product[i].Name;
                    ProductUPGRADE[i].Price = Product[i].Price;
                    ProductUPGRADE[i].Manufacturer = Product[i].Manufacturer;
                    ProductUPGRADE[i].BasicDescription = Product[i].BasicDescription;
                    ProductUPGRADE[i].PhotoProduct = ImageConverter.CreateImageSources(DataBasePhotoRequst.ReadPhoto(Product[i].idProduct));
                }
            };

            return ProductUPGRADE;
        }

        public static ObservableCollection<Product> ConvertFromProductUPGRADECollectionInProductCollection(ObservableCollection<ModelUPGRADEProduct> ProductUPGRADE)
        {
            ObservableCollection<Product> Product = new ObservableCollection<Product>();

            if (ProductUPGRADE != null)
            {
                for (int i = 0; i < ProductUPGRADE.Count; i++)
                {
                    Product product = new Product();

                    Product.Add(product);

                    Product[i].idProduct = ProductUPGRADE[i].idProduct;
                    Product[i].idCategory = ProductUPGRADE[i].idCategory;
                    Product[i].Name = ProductUPGRADE[i].Name;
                    Product[i].Price = ProductUPGRADE[i].Price;
                    Product[i].Manufacturer = ProductUPGRADE[i].Manufacturer;
                    Product[i].BasicDescription = ProductUPGRADE[i].BasicDescription;
                }
            }

            return Product;
        }

        public static ModelUPGRADEProduct ConvertFromProductInProductUPGRADE(Product product, int idProduct = -1)
        {
            ModelUPGRADEProduct ProductU = new ModelUPGRADEProduct();

            if (idProduct == -1)
            {
                if (product.idProduct != 0)
                    ProductU.idProduct = product.idProduct;
                else
                    ProductU.idProduct = 1;
            }
            else
            {
                ProductU.idProduct = idProduct;
            }

            ProductU.idProduct = product.idProduct;
            ProductU.idCategory = product.idCategory;
            ProductU.NameCategory = DataBaseRequstCategory.FindValueByidCategory<string>(product.idCategory, FindByValueCategory.NameCategory);
            ProductU.Name = product.Name;
            ProductU.Price = product.Price;
            ProductU.Manufacturer = product.Manufacturer;
            ProductU.BasicDescription = product.BasicDescription;
            ProductU.PhotoProduct = ImageConverter.CreateImageSources(DataBasePhotoRequst.ReadPhoto(product.idProduct));

            return ProductU;
        }

        public static Product ConvertFromProductUPGRADEInProduct(ModelUPGRADEProduct ProductU)
        {
            Product product = new Product();


            product.idProduct = ProductU.idProduct;
            product.idCategory = ProductU.idCategory;
            product.Name = ProductU.Name;
            product.Price = ProductU.Price;
            product.Manufacturer = ProductU.Manufacturer;
            product.BasicDescription = ProductU.BasicDescription;

            return product;
        }
    }
}
