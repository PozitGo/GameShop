using GameShop.Enum;
using GameShop.Model;
using GameShop.Model.ModelTableInDataBase;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static GameShop.DataBase.DataBaseRequstInTable.DataBaseRequstProduct;
using static GameShop.DataBase.DataBaseRequstUser;

namespace GameShop.ViewModels
{
    public struct ConvertOrderAndOrderUPGRADE
    {
        public static ObservableCollection<ModelUPGRADEOrder> ConvertFromOrderCollectionInOrderUPGRADECollection(ObservableCollection<Order> OrderCollection)
        {
            ObservableCollection <ModelUPGRADEOrder> OrderUPGRADECollection = new ObservableCollection<ModelUPGRADEOrder>();
            if (OrderCollection != null)
            {
                for (int i = 0; i < OrderCollection.Count; i++)
                {
                    ModelUPGRADEOrder orderU = new ModelUPGRADEOrder();

                    OrderUPGRADECollection.Add(orderU);

                    OrderUPGRADECollection[i].idOrder = OrderCollection[i].idOrder;
                    OrderUPGRADECollection[i].idProduct = OrderCollection[i].idProduct;
                    OrderUPGRADECollection[i].NameProduct = FindValueByidProduct<string>(OrderCollection[i].idProduct, FindByValueProduct.Name);
                    OrderUPGRADECollection[i].idUser = OrderCollection[i].idUser;
                    OrderUPGRADECollection[i].LoginUser = FindValueByidUser<string>(OrderCollection[i].idUser, FindByValueUser.Login);
                    OrderUPGRADECollection[i].NameUser = FindValueByidUser<string>(OrderCollection[i].idUser, FindByValueUser.Name);
                    OrderUPGRADECollection[i].SurnameUser = FindValueByidUser<string>(OrderCollection[i].idUser, FindByValueUser.Surname);
                    OrderUPGRADECollection[i].NameSurnameUser = OrderUPGRADECollection[i].NameUser + " " + OrderUPGRADECollection[i].SurnameUser;
                    OrderUPGRADECollection[i].Quantity = OrderCollection[i].Quantity;
                    OrderUPGRADECollection[i].Price = OrderCollection[i].Price;
                    OrderUPGRADECollection[i].Discount = OrderCollection[i].Discount;
                    OrderUPGRADECollection[i].Status = OrderCollection[i].Status;
                    OrderUPGRADECollection[i].idCheck = OrderCollection[i].idCheck;
                }
            };

            return OrderUPGRADECollection;
        }

        public static ObservableCollection<Order> ConvertFromOrderUPGRADECollectionInOrderCollection(ObservableCollection<ModelUPGRADEOrder> OrderUPGRADECollection)
        {
            ObservableCollection<Order> OrderCollection = new ObservableCollection<Order>();

            if (OrderUPGRADECollection != null)
            {
                for (int i = 0; i < OrderUPGRADECollection.Count; i++)
                {
                    Order order = new Order();
                    OrderCollection.Add(order);
                    OrderCollection[i].idOrder = OrderUPGRADECollection[i].idOrder;
                    OrderCollection[i].idProduct = OrderUPGRADECollection[i].idProduct;
                    OrderCollection[i].idUser = OrderUPGRADECollection[i].idUser;
                    OrderCollection[i].Quantity = OrderUPGRADECollection[i].Quantity;
                    OrderCollection[i].Price = OrderUPGRADECollection[i].Price;
                    OrderCollection[i].Discount = OrderUPGRADECollection[i].Discount;
                    OrderCollection[i].Status = OrderUPGRADECollection[i].Status;
                    OrderCollection[i].idCheck = OrderUPGRADECollection[i].idCheck;
                }
            }

            return OrderCollection;
        }

        public static ModelUPGRADEOrder ConvertFromOrderInOrderUPGRADE(Order order, int idOrder = -1)
        {
            ModelUPGRADEOrder orderU = new ModelUPGRADEOrder();

            if (idOrder == -1)
            {
                if (order.idOrder != 0)
                    orderU.idOrder = order.idOrder;
                else
                    orderU.idOrder = 1;
            }
            else
            {
                orderU.idOrder = idOrder;
            }

            orderU.idProduct = order.idProduct;
            orderU.NameProduct = FindValueByidProduct<string>(orderU.idProduct, FindByValueProduct.Name);
            orderU.idUser = order.idUser;
            orderU.LoginUser = FindValueByidUser<string>(order.idUser, FindByValueUser.Login);
            orderU.NameUser = FindValueByidUser<string>(order.idUser, FindByValueUser.Name);
            orderU.SurnameUser = FindValueByidUser<string>(order.idUser, FindByValueUser.Surname);
            orderU.NameSurnameUser = orderU.NameUser + " " + orderU.SurnameUser;
            orderU.Quantity = order.Quantity;
            orderU.Price = order.Price;
            orderU.Discount = order.Discount;
            orderU.Status = order.Status;
            orderU.idCheck = order.idCheck;

            return orderU;
        }

        public static Order ConvertFromOrderUPGRADEInOrder(ModelUPGRADEOrder OrderU)
        {
            Order Order = new Order();

            Order.idOrder = OrderU.idOrder;
            Order.idProduct = OrderU.idProduct;
            Order.idUser = OrderU.idUser;
            Order.Quantity = OrderU.Quantity;
            Order.Price = OrderU.Price;
            Order.Discount = OrderU.Discount;
            Order.Status = OrderU.Status;
            Order.idCheck = OrderU.idCheck;

            return Order;
        }
    }

}
