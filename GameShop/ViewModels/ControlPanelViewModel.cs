using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GameShop.Enum;
using GameShop.Model;
using GameShop.Model.ModelTableInDataBase;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static GameShop.DataBase.DataBaseRequestOrder;
using static GameShop.DataBase.DataBaseRequstCheck;
using static GameShop.DataBase.DataBaseRequstInTable.DataBaseRequstProduct;
using static GameShop.DataBase.DataBaseRequstUser;

namespace GameShop.ViewModels
{
    public class ControlPanelViewModel : ObservableObject
    {
        public ObservableCollection<OrderUPGRADE> OrderUPGRADECollection = new ObservableCollection<OrderUPGRADE>();

        public ObservableCollection<Order> OrderCollectionSettings = new ObservableCollection<Order>();

        public ObservableCollection<Check> CheckCollection = new ObservableCollection<Check>();

        public ObservableCollection<Check> CheckCollectionSettings = new ObservableCollection<Check>();

        
        public ReadingDataCheckInCollection ReadingDataCheckInCollectionHandeler { get; set; }

        public ReadingDataOrderInCollection ReadingDataOrderInCollectionHandeler { get; set; }

        public SaveNewItemOrderByDBDelegate SaveNewItemOrderByDBHandler { get; set; }
        
        public SaveNewItemOrderByDBDelegate SaveNewItemCheckByDBHandler { get; set; }

        public FindNameByidProductDelegate FindNameByidProductDelegateHandler { get; set; }

        public FindNameSurnameByidUserDelegate FindNameSurnameByidUserDelegateHandler { get; set; }

        public FindLoginByidUserDelegate FindLoginByidUserDelegateHandler { get; set; }


        private string _OrderCollectionIsVisibleDataGrid;
        public string OrderCollectionIsVisibleDataGrid
        {
            get => _OrderCollectionIsVisibleDataGrid;
            set => SetProperty(ref _OrderCollectionIsVisibleDataGrid, value);
        }
        
        private string _CheckCollectionIsVisibleDataGrid;
        public string CheckCollectionIsVisibleDataGrid
        {
            get => _CheckCollectionIsVisibleDataGrid;
            set => SetProperty(ref _CheckCollectionIsVisibleDataGrid, value);
        }

        private bool _IsCheckedOrderMainTable;

        public bool IsCheckedOrderMainTable
        {
            get => _IsCheckedOrderMainTable;
            set => SetProperty(ref _IsCheckedOrderMainTable, value); 
        }

        private bool _IsCheckedCheckMainTable;

        public bool IsCheckedCheckMainTable
        {
            get => _IsCheckedCheckMainTable;
            set => SetProperty(ref _IsCheckedCheckMainTable, value);
        }


        private string _HorizontalAligamentOrderCollectionDataGrid;
        public string HorizontalAligamentOrderCollectionDataGrid
        {
            get => _HorizontalAligamentOrderCollectionDataGrid;
            set => SetProperty(ref _HorizontalAligamentOrderCollectionDataGrid, value);
        }

        private string _HorizontalAligamentCheckCollectionDataGrid;
        public string HorizontalAligamentCheckCollectionDataGrid
        {
            get => _HorizontalAligamentCheckCollectionDataGrid;
            set => SetProperty(ref _HorizontalAligamentCheckCollectionDataGrid, value);
        }

        private string _VisibilitySaveButtonCommandBar;
        public string VisibilitySaveButtonCommandBar
        {
            get => _VisibilitySaveButtonCommandBar;
            set => SetProperty(ref _VisibilitySaveButtonCommandBar, value);
        }

        public ICommand ButtonMainTable;
        public ICommand AppBarButtonAddOrder;
        public ICommand AppBarButtonSaveButton;

        private void AppBarButtonAddOrderClick()
        {
            Order order = new Order();
            OrderCollectionSettings.Add(order);
            VisibilitySaveButtonCommandBar = "Visible";
        }
        
        private async void AppBarButtonSaveButtonClick()
        {
            VisibilitySaveButtonCommandBar = "Collapsed";

            for (int i = 0; i < OrderCollectionSettings.Count; i++)
            {
                AddNewOrder(OrderCollectionSettings[0]);
            }

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
               OrderCollectionSettings.Clear();
                
            });
        }

        private void ButtonMainTableClick()
        {
            if(IsCheckedCheckMainTable)
            {
                OrderCollectionIsVisibleDataGrid = "Collapsed";
                CheckCollectionIsVisibleDataGrid = "Visible";
            }
            else if(IsCheckedOrderMainTable)
            {
                CheckCollectionIsVisibleDataGrid = "Collapsed";
                OrderCollectionIsVisibleDataGrid = "Visible";
            }
        }

        public ControlPanelViewModel()
        {
            AppBarButtonSaveButton = new RelayCommand(AppBarButtonSaveButtonClick);
            AppBarButtonAddOrder = new RelayCommand(AppBarButtonAddOrderClick);
            ButtonMainTable = new RelayCommand(ButtonMainTableClick);
            VisibilitySaveButtonCommandBar = "Collapsed";
            HorizontalAligamentOrderCollectionDataGrid = "Left";
            HorizontalAligamentCheckCollectionDataGrid = "Left";
            OrderCollectionIsVisibleDataGrid = "Collapsed";
            CheckCollectionIsVisibleDataGrid = "Collapsed";
            Task.Factory.StartNew(() => InitializationCollection());
        }

        private Task InitializationCollection()
        {
            OrderUPGRADECollection.Clear();
            CheckCollection.Clear();

            ReadingDataOrderInCollectionHandeler = new ReadingDataOrderInCollection(ReadingDataOrder);
            ReadingDataCheckInCollectionHandeler = new ReadingDataCheckInCollection(ReadingDataCheck);
            FindNameByidProductDelegateHandler = new FindNameByidProductDelegate(FindNameByidProduct);
            FindNameSurnameByidUserDelegateHandler = new FindNameSurnameByidUserDelegate(FindNameSurnameByidUser);
            FindLoginByidUserDelegateHandler = new FindLoginByidUserDelegate(FindLoginByidUser);


            return Task.Factory.StartNew(() =>
            {
                ObservableCollection<Order> OrderCollection = new ObservableCollection<Order>();
                foreach (var item in ReadingDataOrderInCollectionHandeler())
                {
                    OrderCollection.Add(item);
                }
                
                Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < OrderCollection.Count; i++)
                    {
                        OrderUPGRADE orderU = new OrderUPGRADE();
                        OrderUPGRADECollection.Add(orderU);
                        OrderUPGRADECollection[i].idOrder = OrderCollection[i].idOrder;
                        OrderUPGRADECollection[i].idProduct = OrderCollection[i].idProduct;
                        OrderUPGRADECollection[i].NameProduct = FindNameByidProductDelegateHandler(OrderCollection[i].idOrder);
                        OrderUPGRADECollection[i].idUser = OrderCollection[i].idUser;
                        OrderUPGRADECollection[i].LoginUser = FindLoginByidUserDelegateHandler(OrderCollection[i].idUser);
                        var temp = FindNameSurnameByidUserDelegateHandler(OrderCollection[i].idUser);
                        OrderUPGRADECollection[i].NameUser = temp.Name;
                        OrderUPGRADECollection[i].SurnameUser = temp.Surname;
                        OrderUPGRADECollection[i].NameSurnameUser = temp.Name + " " + temp.Surname;
                        OrderUPGRADECollection[i].Quantity = OrderCollection[i].Quantity;
                        OrderUPGRADECollection[i].Price = OrderCollection[i].Price;
                        OrderUPGRADECollection[i].Discount = OrderCollection[i].Discount;
                        OrderUPGRADECollection[i].Status = OrderCollection[i].Status;
                        OrderUPGRADECollection[i].idCheck = OrderCollection[i].idCheck;
                    }
                });

                foreach (var item in ReadingDataCheckInCollectionHandeler())
                {
                    CheckCollection.Add(item);
                }
            });
        }
        private void AddNewOrder(Order item)
        {
            SaveNewItemOrderByDBHandler = new SaveNewItemOrderByDBDelegate(SaveNewItemOrderByDB);
            ReadingDataOrderInCollectionHandeler = new ReadingDataOrderInCollection(ReadingDataOrder);
            FindNameByidProductDelegateHandler = new FindNameByidProductDelegate(FindNameByidProduct);
            FindNameSurnameByidUserDelegateHandler = new FindNameSurnameByidUserDelegate(FindNameSurnameByidUser);
            FindLoginByidUserDelegateHandler = new FindLoginByidUserDelegate(FindLoginByidUser);

                OrderUPGRADE orderU = new OrderUPGRADE();

                orderU.idOrder = OrderUPGRADECollection.Max(x => x.idOrder) + 1;
                orderU.idProduct = item.idOrder;
                orderU.NameProduct = FindNameByidProductDelegateHandler(orderU.idOrder);
                orderU.idUser = item.idUser;
                orderU.LoginUser = FindLoginByidUserDelegateHandler(item.idUser);
                var temp = FindNameSurnameByidUserDelegateHandler(item.idUser);
                orderU.NameUser = temp.Name;
                orderU.SurnameUser = temp.Surname;
                orderU.NameSurnameUser = temp.Name + " " + temp.Surname;
                orderU.Quantity = item.Quantity;
                orderU.Price = item.Price;
                orderU.Discount = item.Discount;
                orderU.Status = item.Status;
                orderU.idCheck = item.idCheck;

                SaveNewItemOrderByDBHandler(item);;

                Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                OrderUPGRADECollection.Add(orderU);
                });

        }

        private Task<bool> DeleteItemInCollectionOrder(OrderUPGRADE item)
        {
            //Проверка на существование 
            Task.Factory.StartNew(() =>
            {
                
                OrderUPGRADECollection.Remove(item);
                //Вызов запроса на удаление из бд
            });

            return Task<bool>.Factory.StartNew(() => true);
        }

        private Task<bool> UPDATEItemInCollectionOrder(FindByValueOrder value, int idOrder)
        {
            //Какой элемент менять, и что в нём менять 
            Task.Factory.StartNew(() =>
            {
                var temp = ReadingDataOrderInCollectionHandeler(value, idOrder);
                if (temp.Count > 0 && OrderUPGRADECollection[idOrder] != null)
                {
                    //Изменение коллекции + вызов конкретики изменения + изменения в бд
                    //В запрос не включаем те данные которые не изменяются, показываем данные на данный момент и проверяем изменились они либо нет, из этого формируем запрос
                }
                else
                {
                    Debug.WriteLine("Error");
                }

            });

            return Task<bool>.Factory.StartNew(() => true);
        }
       
    }
}
