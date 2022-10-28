using GameShop.DataBase;
using GameShop.Enum;
using GameShop.Model;
using GameShop.Model.ModelTableInDataBase;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static GameShop.DataBase.DataBaseRequestOrder;
using static GameShop.DataBase.DataBaseRequstCheck;
using static GameShop.DataBase.DataBaseRequstInTable.DataBaseRequstProduct;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace GameShop.ViewModels
{
    public class ControlPanelViewModel : ObservableObject
    {
        public ObservableCollection<ModelUPGRADEOrder> OrderUPGRADECollection = new ObservableCollection<ModelUPGRADEOrder>();

        public ObservableCollection<Order> OrderCollectionSettings = new ObservableCollection<Order>();

        public ObservableCollection<Check> CheckCollection = new ObservableCollection<Check>();

        public ObservableCollection<Check> CheckCollectionSettings = new ObservableCollection<Check>();

        public ObservableCollection<int> AllidCheckCollection = new ObservableCollection<int>();

        public ObservableCollection<int> AllidProductCollection = new ObservableCollection<int>();

        public ObservableCollection<int> AllidUserCollection = new ObservableCollection<int>();

        public DataGrid DataGridOrderUPGRADE;
        public DataGrid DataGridCheck;
        public DataGrid DataGridOrderSettings;


        private string _VisibilitySaveButtonCommandBar;
        public string VisibilitySaveButtonCommandBar
        {
            get => _VisibilitySaveButtonCommandBar;
            set => SetProperty(ref _VisibilitySaveButtonCommandBar, value);
        }
        private string _VisibilityCheckBoxUniteCheck;
        private string _ContentSaveButtonCommandBar;
        public string ContentSaveButtonCommandBar
        {
            get => _ContentSaveButtonCommandBar;
            set => SetProperty(ref _ContentSaveButtonCommandBar, value);
        }
        public string VisibilityCheckBoxUniteCheck
        {
            get => _VisibilityCheckBoxUniteCheck;
            set => SetProperty(ref _VisibilityCheckBoxUniteCheck, value);
        }

        private bool _IsCheckedCheckBoxUniteCheck;
        public bool IsCheckedCheckBoxUniteCheck
        {
            get => _IsCheckedCheckBoxUniteCheck;
            set => SetProperty(ref _IsCheckedCheckBoxUniteCheck, value);
        }
        private string _DataGridTextColumnVisibilitySettingsidCheck;
        public string DataGridTextColumnVisibilitySettingsidCheck
        {
            get => _DataGridTextColumnVisibilitySettingsidCheck;
            set => SetProperty(ref _DataGridTextColumnVisibilitySettingsidCheck, value);
        }

        private bool IsUPDATE;
        private bool IsDELETE;
        private bool IsADD;

        private SelectionChangedEventArgs SelectItems;
        public ICommand AppBarButtonAddOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonAddOrderClick);
        private async void AppBarButtonAddOrderClick(DataGrid data)
        {
            IsADD = true;
            if (CheckCollection.Count == 0)
            {
                InitializationCollectionCheck();
            }

            if (DataGridOrderSettings == null)
            {
                DataGridOrderSettings = data;
            }

            Order order = new Order();

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Add(order);
            });

            InitializationCollectionAllId();
            ContentSaveButtonCommandBar = "Сохранить";
            VisibilitySaveButtonCommandBar = "Visible";
            VisibilityCheckBoxUniteCheck = "Visible";
        }

        public ICommand AppBarButtonEditOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonEditOrderClick);
        private async void AppBarButtonEditOrderClick(DataGrid data)
        {
            DataGridOrderSettings = data;
            IsUPDATE = true;

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Clear();
            });

            if (SelectItems != null)
            {
                ModelUPGRADEOrder tempOrder = SelectItems.AddedItems[0] as ModelUPGRADEOrder;

                OrderCollectionSettings.Add(tempOrder);

                InitializationCollectionAllId();
                ContentSaveButtonCommandBar = "Изменить";
                VisibilitySaveButtonCommandBar = "Visible";
                VisibilityCheckBoxUniteCheck = "Collapsed";
            }
        }

        public ICommand IsVisibleCheckBoxUniteCheckCommand;
        public void IsVisibleCheckBoxUniteCheck()
        {

            if (IsCheckedCheckBoxUniteCheck)
            {
                DataGridOrderSettings.Columns[6].Visibility = Visibility.Collapsed;
            }
            else
            {
                DataGridOrderSettings.Columns[6].Visibility = Visibility.Visible;
            }
        }

        public ICommand AppBarButtonSaveButton;
        private async void AppBarButtonSaveButtonClick()
        {
            if (IsADD && OrderUPGRADECollection.Count != 0)
            {
                VisibilityCheckBoxUniteCheck = "Collapsed";
                VisibilitySaveButtonCommandBar = "Collapsed";
                DataGridTextColumnVisibilitySettingsidCheck = "Visible";


                for (int i = 0; i < OrderCollectionSettings.Count; i++)
                {
                    if (OrderCollectionSettings[i].idProduct != 0 && OrderCollectionSettings[i].Quantity != 0)
                    {
                        if (OrderCollectionSettings[i].Price == 0)
                        {
                            OrderCollectionSettings[i].Price = FindValueByidProduct<double>(OrderCollectionSettings[i].idProduct, FindByValueProduct.Price) * OrderCollectionSettings[i].Quantity;

                            if (OrderCollectionSettings[i].Discount != 0)
                            {
                                double DiscountAmount = (OrderCollectionSettings[i].Price * OrderCollectionSettings[i].Discount) / 100;
                                OrderCollectionSettings[i].Price -= DiscountAmount;
                            }
                        }
                    }
                }

                if (!IsCheckedCheckBoxUniteCheck)
                    for (int i = 0; i < OrderCollectionSettings.Count; i++)
                    {
                        AddNewOrder(OrderCollectionSettings[i]);
                    }

                if (IsCheckedCheckBoxUniteCheck)
                {

                    double Sum = OrderCollectionSettings.Sum(x => x.Price);

                    int UnionNewCheck = 0;

                    if (CheckCollection.Count != 0)
                    {
                        UnionNewCheck = CheckCollection.Max(x => x.idCheck) + 1;
                    }

                    if (Sum != 0)
                    {

                        for (int i = 0; i < OrderCollectionSettings.Count; i++)
                        {
                            OrderCollectionSettings[i].idCheck = UnionNewCheck;
                        }

                        Check check = new Check();
                        check.idCheck = UnionNewCheck;
                        check.Sum = Sum;

                        Task task = Task.Factory.StartNew(() => SaveNewItemCheckByDB(Sum));

                        task.Wait();
                        check.Data = DataBaseRequstCheck.FindValueByidCheck<DateTime>(UnionNewCheck, FindByValueCheck.Data);

                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            CheckCollection.Add(check);
                        });

                    }

                    for (int i = 0; i < OrderCollectionSettings.Count; i++)
                    {
                        AddNewOrder(OrderCollectionSettings[i]);
                    }
                }

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    OrderCollectionSettings.Clear();
                });

                IsADD = false;

            }

            if (IsUPDATE && OrderUPGRADECollection.Count != 0)
            {
                ObservableCollection<Order> OrderCollectionSettingsOldItems = new ObservableCollection<Order>();

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OrderCollectionSettings.Clear();
                        });

                OrderCollectionSettingsOldItems = OrderCollectionSettings;
                await Task.Factory.StartNew(() => UPDATEItemInCollectionOrder(OrderCollectionSettings, OrderCollectionSettingsOldItems));
                IsUPDATE = false;
            }
        }

        public ICommand RadionButtonTableOrderCommand => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(RadionButtonTableOrder);
        private void RadionButtonTableOrder(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            InitializationCollectionOrderUPGRADE();
            DataGridOrderUPGRADE.Visibility = Visibility.Visible;

            if (DataGridCheck != null)
                DataGridCheck.Visibility = Visibility.Collapsed;
        }

        public ICommand RadionButtonTableCheckCommand => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(RadionButtonTableCheck);
        private void RadionButtonTableCheck(DataGrid data)
        {
            InitializationCollectionCheck();
            DataGridCheck = data;
            DataGridCheck.Visibility = Visibility.Visible;

            if (DataGridOrderUPGRADE != null)
                DataGridOrderUPGRADE.Visibility = Visibility.Collapsed;
        }

        public ControlPanelViewModel()
        {
            IsVisibleCheckBoxUniteCheckCommand = new RelayCommand(IsVisibleCheckBoxUniteCheck);
            AppBarButtonSaveButton = new RelayCommand(AppBarButtonSaveButtonClick);
            DataGridTextColumnVisibilitySettingsidCheck = "Visible";
            VisibilityCheckBoxUniteCheck = "Collapsed";
            VisibilitySaveButtonCommandBar = "Collapsed";
        }

        private void InitializationCollectionOrderUPGRADE()
        {
            DataGridOrderUPGRADE.UnloadingRowDetails += DataGridOrderUPGRADE_UnloadingRowDetails;
            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ObservableCollection<Order> OrderCollection = new ObservableCollection<Order>();
                if (OrderUPGRADECollection.Count == 0)
                {

                    Task task = Task.Factory.StartNew(() =>
                    {
                        foreach (var item in ReadingDataOrder())
                        {
                            OrderCollection.Add(item);
                        }
                    });

                    task.Wait();
                    foreach (var item in ConverOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(OrderCollection))
                    {
                        OrderUPGRADECollection.Add(item);
                    }
                }
            });

            DataGridOrderUPGRADE.SelectionChanged += DataGridOrderUPGRADE_SelectionChanged;

        }

        private void DataGridOrderUPGRADE_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.SelectItems = e;


        private void DataGridOrderUPGRADE_UnloadingRowDetails(object sender, DataGridRowDetailsEventArgs e) => e.Row.DetailsVisibility = Visibility.Collapsed;

        private void InitializationCollectionCheck()
        {
            if (CheckCollection.Count == 0)
                foreach (var item in ReadingDataCheck())
                {
                    Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        CheckCollection.Add(item);
                    });
                }
        }

        private void InitializationCollectionAllId()
        {
            Task.Factory.StartNew(() =>
            {
                if (AllidProductCollection.Count == 0)
                {
                    foreach (var item in UniversalRequst.ReadingAllToIdFromTable("product", "idProduct"))
                    {
                        AllidProductCollection.Add(item);
                    }
                }

                if (AllidUserCollection.Count == 0)
                {
                    foreach (var item in UniversalRequst.ReadingAllToIdFromTable("user", "idUser"))
                    {
                        AllidUserCollection.Add(item);
                    }
                }

                if (AllidCheckCollection.Count == 0)
                {
                    foreach (var item in UniversalRequst.ReadingAllToIdFromTable("check", "idCheck"))
                    {
                        AllidCheckCollection.Add(item);
                    }
                }
            });
        }


        private async void AddNewOrder(Order item)
        {

            ModelUPGRADEOrder orderU = new ModelUPGRADEOrder();
            orderU = ConverOrderAndOrderUPGRADE.ConvertFromOrderInOrderUPGRADE(item, OrderUPGRADECollection.Max(x => x.idOrder) + 1);

            if (!IsCheckedCheckBoxUniteCheck)
            {
                var tempPrice = DataBaseRequstCheck.FindValueByidCheck<double>(orderU.idCheck, FindByValueCheck.Sum) + orderU.Price;
                UpdateItemInTableCheck(FindByValueCheck.Sum, tempPrice, orderU.idCheck);

                foreach (var itemCollection in CheckCollection)
                {
                    if(itemCollection.idCheck == orderU.idCheck)
                    {
                        itemCollection.Sum = tempPrice;
                        break;
                    }
                }
            }

            OrderUPGRADECollection.Add(orderU);

            await SaveNewItemOrderByDB(item);

        }

        private Task<bool> DeleteItemInCollectionOrder(ModelUPGRADEOrder item)
        {
            //Проверка на существование 
            Task.Factory.StartNew(() =>
            {

                OrderUPGRADECollection.Remove(item);
                //Вызов запроса на удаление из бд
            });

            return Task<bool>.Factory.StartNew(() => true);
        }

        private async void UPDATEItemInCollectionOrder(ObservableCollection<Order> Collection, ObservableCollection<Order> OrderCollectionSettingsOldItems)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    if (Collection[i].idCheck != OrderCollectionSettingsOldItems[i].idCheck)
                    {
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.idCheck, Collection[i].idCheck, Collection[i].idOrder);
                    }
                    else if (Collection[i].idUser != OrderCollectionSettingsOldItems[i].idUser)
                    {
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.idUser, Collection[i].idUser, Collection[i].idOrder);
                    }
                    else if (Collection[i].Status != OrderCollectionSettingsOldItems[i].Status)
                    {
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Status, Collection[i].Status, Collection[i].idOrder);
                    }
                    else if (Collection[i].Price != OrderCollectionSettingsOldItems[i].Price)
                    {
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Price, Collection[i].Price, Collection[i].idOrder);
                    }
                    else if (Collection[i].Quantity != OrderCollectionSettingsOldItems[i].Quantity)
                    {
                            Collection[i].Price = FindValueByidProduct<float>(Collection[i].idProduct, FindByValueProduct.Price) * Collection[i].Quantity;

                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Quantity, Collection[i].Quantity, Collection[i].idOrder);
                    }
                    else if (Collection[i].Discount != OrderCollectionSettingsOldItems[i].Discount)
                    {
                        if (Collection[i].Discount != 0)
                        {
                            double DiscountAmount = (Collection[i].Price * Collection[i].Discount) / 100;
                            Collection[i].Price -= DiscountAmount;
                        }

                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Status, Collection[i].Status, Collection[i].idOrder);
                    }
                }
            });
        }

    }
}
