using GameShop.DataBase;
using GameShop.Enum;
using GameShop.Model;
using GameShop.Model.ModelTableInDataBase;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
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

        public ObservableCollection<string> answerOrder = new ObservableCollection<string>();

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
            set
            {
                SetProperty(ref _IsCheckedCheckBoxUniteCheck, value);
            }
        }
        private string _DataGridTextColumnVisibilitySettingsidCheck;
        public string DataGridTextColumnVisibilitySettingsidCheck
        {
            get => _DataGridTextColumnVisibilitySettingsidCheck;
            set => SetProperty(ref _DataGridTextColumnVisibilitySettingsidCheck, value);
        }

        private string _ContentCheckBoxUniteCheck;
        public string ContentCheckBoxUniteCheck
        {
            get => _ContentCheckBoxUniteCheck;
            set => SetProperty(ref _ContentCheckBoxUniteCheck, value);
        }

        private bool IsUPDATE;
        private bool IsDELETE;
        private bool IsADD;

        public bool BeInCheck { get; set; }
        public bool BeInOrder { get; set; }

        private SelectionChangedEventArgs SelectItemsOrder;

        private SelectionChangedEventArgs SelectItemsCheck;
        public ICommand AppBarButtonAddOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonAddOrderClick);
        private async void AppBarButtonAddOrderClick(DataGrid data)
        {

            if (DataGridOrderSettings == null)
            {
                DataGridOrderSettings = data;
            }

            IsCheckedCheckBoxUniteCheck = false;
            DataGridOrderSettings.Columns[6].Visibility = Visibility.Visible;

            IsADD = true;
            IsUPDATE = false;
            InitializationCollectionAllId();

            DataGridOrderSettings.Columns[0].Visibility = Visibility.Visible;

            if (CheckCollection.Count == 0)
            {
                InitializationCollectionCheck();
            }

            Order order = new Order();

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Add(order);
            });

            ContentSaveButtonCommandBar = "Сохранить";
            VisibilitySaveButtonCommandBar = "Visible";
            VisibilityCheckBoxUniteCheck = "Visible";
            ContentCheckBoxUniteCheck = "Объеденить в новый чек";
        }

        public ICommand AppBarButtonEditOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonEditOrderClick);

        ObservableCollection<Order> OrderCollectionSettingsOldItems = new ObservableCollection<Order>();
        private async void AppBarButtonEditOrderClick(DataGrid data)
        {
            DataGridOrderSettings = data;
            IsUPDATE = true;
            IsADD = false;
            IsDELETE = false;

            IsCheckedCheckBoxUniteCheck = false;
            DataGridOrderSettings.Columns[6].Visibility = Visibility.Visible;

            DataGridOrderSettings.Columns[0].Visibility = Visibility.Collapsed;
            InitializationCollectionCheck();
            InitializationCollectionAllId();

            ContentSaveButtonCommandBar = "Изменить";
            VisibilitySaveButtonCommandBar = "Visible";
            VisibilityCheckBoxUniteCheck = "Visible";
            ContentCheckBoxUniteCheck = "Вынести в новый чек";

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Clear();
            });

            OrderCollectionSettingsOldItems.Clear();

            if (SelectItemsOrder != null)
            {
                try
                {
                    ModelUPGRADEOrder tempOrderAdd = SelectItemsOrder.AddedItems[0] as ModelUPGRADEOrder;

                    OrderCollectionSettings.Add(tempOrderAdd);

                    OrderCollectionSettingsOldItems.Add(ConvertOrderAndOrderUPGRADE.ConvertFromOrderUPGRADEInOrder(OrderUPGRADECollection.First(x => x.idOrder == tempOrderAdd.idOrder)));

                }
                catch (System.ArgumentOutOfRangeException)
                {

                }
            }
        }

        public ICommand AppBarButtonDeleteOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonDeleteClick);
        private async void AppBarButtonDeleteClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            if (DataGridOrderSettings == null)
                DataGridOrderSettings = data;

            IsCheckedCheckBoxUniteCheck = false;
            DataGridOrderSettings.Columns[6].Visibility = Visibility.Visible;
            DataGridOrderSettings.Columns[0].Visibility = Visibility.Visible;

            IsADD = false;
            IsUPDATE = false;
            IsDELETE = true;

            InitializationCollectionCheck();

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Clear();
            });

            if (CheckCollection.Count == 0)
                InitializationCollectionCheck();

            if (BeInOrder)
            {
                VisibilityCheckBoxUniteCheck = "Collapsed";
                ContentSaveButtonCommandBar = "Подтвердить удаление";
                VisibilitySaveButtonCommandBar = "Visible";
            }

            if (BeInCheck && SelectItemsCheck != null)
            {
                Check check = SelectItemsCheck.AddedItems[0] as Check;
                ContentDialog noWifiDialog = new ContentDialog()
                {
                    Title = "Удаление",
                    Content = "Подтвердите удаление всех заказов под выбранным чеком",
                    CloseButtonText = "Отмена",
                    PrimaryButtonText = "Подтвердить",
                    PrimaryButtonCommand = DeleteAllOrdersOnIdCheckCommand,
                    PrimaryButtonCommandParameter = check
                };

                await noWifiDialog.ShowAsync();
            }

            if (BeInOrder && SelectItemsOrder != null)
            {
                for (int i = 0; i < SelectItemsOrder.AddedItems.Count; i++)
                {
                    OrderCollectionSettings.Add(SelectItemsOrder.AddedItems[i] as ModelUPGRADEOrder);
                }
            }
        }

        public ICommand AppBarButtonAccept => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonAcceptClick);
        private void AppBarButtonAcceptClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            ModelUPGRADEOrder tempOrderAdd = SelectItemsOrder.AddedItems[0] as ModelUPGRADEOrder;

            int idOrder = tempOrderAdd.idOrder;
            tempOrderAdd.Status = "Одобрен";

            Task.Factory.StartNew(() => UpdateItemInTableOrder(FindByValueOrder.Status, tempOrderAdd.Status, idOrder));

            for (int i = 0; i < OrderUPGRADECollection.Count; i++)
            {
                if (idOrder == OrderUPGRADECollection[i].idOrder)
                {
                    OrderUPGRADECollection.Insert(i, tempOrderAdd);
                    OrderUPGRADECollection.RemoveAt(i + 1);
                }
            }
        }

        public ICommand AppBarButtonClearOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonClearClick);
        private void AppBarButtonClearClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            ModelUPGRADEOrder tempOrderClear = SelectItemsOrder.AddedItems[0] as ModelUPGRADEOrder;

            int idOrder = tempOrderClear.idOrder;
            tempOrderClear.Status = "Ожидает оплаты";

            Task.Factory.StartNew(() => UpdateItemInTableOrder(FindByValueOrder.Status, tempOrderClear.Status, idOrder));

            for (int i = 0; i < OrderUPGRADECollection.Count; i++)
            {
                if (idOrder == OrderUPGRADECollection[i].idOrder)
                {
                    OrderUPGRADECollection.Insert(i, tempOrderClear);
                    OrderUPGRADECollection.RemoveAt(i + 1);
                }
            }
        }

        public ICommand AppBarButtonRefresh => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonRefreshClick);
        private void AppBarButtonRefreshClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            InitializationCollectionAllId();
            InitializationCollectionCheck();
            InitializationCollectionOrderUPGRADE();
        }

        public ICommand DeleteAllOrdersOnIdCheckCommand => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<Check>(DeleteAllOrdersOnIdCheck);

        private async void DeleteAllOrdersOnIdCheck(Check check)
        {
            int[] idOrder = new int[1];

            Task task = Task.Factory.StartNew(() =>
            {
                idOrder = DataBaseRequstCheck.DeleteCheckAndAllOrders(check.idCheck);
            });

            task.Wait();
            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                for (int i = 0; i < CheckCollection.Count; i++)
                {
                    if (CheckCollection[i].idCheck == check.idCheck)
                    {
                        CheckCollection.RemoveAt(i);
                        break;
                    }
                }

                for (int j = 0; j < idOrder.Length; j++)
                {
                    if (idOrder[j] == -1)
                        break;

                    for (int i = 0; i < OrderUPGRADECollection.Count; i++)
                    {
                        if (OrderUPGRADECollection[i].idOrder == idOrder[j])
                        {
                            OrderUPGRADECollection.RemoveAt(i);
                            break;
                        }
                    }
                }
            });

        }

        public ICommand IsVisibleCheckBoxUniteCheckCommand;
        public void IsVisibleCheckBoxUniteCheck()
        {

            if (IsCheckedCheckBoxUniteCheck && IsADD || IsCheckedCheckBoxUniteCheck && IsUPDATE)
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
            VisibilitySaveButtonCommandBar = "Collapsed";
            VisibilityCheckBoxUniteCheck = "Collapsed";

            if (IsADD)
            {
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

                if (!IsCheckedCheckBoxUniteCheck && IsADD)
                    for (int i = 0; i < OrderCollectionSettings.Count; i++)
                    {
                        AddNewOrder(OrderCollectionSettings[i]);
                    }

                if (IsCheckedCheckBoxUniteCheck && IsADD)
                {

                    double Sum = OrderCollectionSettings.Sum(x => x.Price);

                    int UnionNewCheck = 0;

                    if (CheckCollection.Count != 0)
                    {
                        UnionNewCheck = CheckCollection.Max(x => x.idCheck) + 1;
                    }
                    else
                        UnionNewCheck = 1;

                    if (Sum != 0)
                    {

                        for (int i = 0; i < OrderCollectionSettings.Count; i++)
                        {
                            OrderCollectionSettings[i].idCheck = UnionNewCheck;
                        }

                        Check check = new Check();
                        check.idCheck = UnionNewCheck;
                        check.Sum = Sum;

                        Task task = Task.Factory.StartNew(() => SaveNewItemCheckByDB(UnionNewCheck, Sum));

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
                DataGridOrderSettings.Columns[6].Visibility = Visibility.Visible;
            }

            if (IsUPDATE)
            {
                UPDATEItemInCollectionOrder(OrderCollectionSettings, OrderCollectionSettingsOldItems);

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    OrderCollectionSettings.Clear();
                });

                OrderCollectionSettingsOldItems.Clear();
                IsUPDATE = false;
                DataGridOrderSettings.Columns[0].Visibility = Visibility.Visible;
                IsCheckedCheckBoxUniteCheck = false;
            }

            if (IsDELETE)
            {
                bool DeleteTo = false;
                Task task = Task.Factory.StartNew(() =>
                {
                    DeleteTo = DeleteItemInTableOrder(OrderCollectionSettings[0].idOrder, OrderCollectionSettings[0].idCheck, OrderUPGRADECollection.Where(x => x.idCheck == OrderCollectionSettings[0].idCheck).Count());
                });

                task.Wait();
                int idOrder = OrderCollectionSettings[0].idOrder;
                int idCheck = OrderCollectionSettings[0].idCheck;


                if (DeleteTo)
                {
                    for (int i = 0; i < OrderUPGRADECollection.Count; i++)
                    {
                        if (OrderUPGRADECollection[i].idOrder == idOrder)
                        {
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                OrderUPGRADECollection.RemoveAt(i);
                            });
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < OrderUPGRADECollection.Count; i++)
                    {
                        if (OrderUPGRADECollection[i].idOrder == idOrder)
                        {
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                OrderUPGRADECollection.RemoveAt(i);
                            });
                        }
                    }

                    for (int i = 0; i < CheckCollection.Count; i++)
                    {
                        if (CheckCollection[i].idCheck == idCheck)
                        {
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                CheckCollection.RemoveAt(i);
                            });
                        }
                    }
                }

                OrderCollectionSettings.Clear();

                IsDELETE = false;
                IsCheckedCheckBoxUniteCheck = false;
            }
        }

        public ICommand AppBarButtonClearSettingsTable => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonClearSettingsTableClick);
        private void AppBarButtonClearSettingsTableClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (OrderCollectionSettingsOldItems.Count != 0)
                {
                    OrderCollectionSettingsOldItems.Clear();
                }

                if (OrderCollectionSettings.Count != 0)
                {
                    OrderCollectionSettings.Clear();
                }
            });

            VisibilitySaveButtonCommandBar = "Collapsed";
            VisibilityCheckBoxUniteCheck = "Collapsed";
        }

        public ICommand RadionButtonTableOrderCommand => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(RadionButtonTableOrder);
        private void RadionButtonTableOrder(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            BeInOrder = true;
            BeInCheck = false;
            InitializationCollectionOrderUPGRADE();
            DataGridOrderUPGRADE.Visibility = Visibility.Visible;

            if (DataGridCheck != null)
                DataGridCheck.Visibility = Visibility.Collapsed;
        }

        public ICommand RadionButtonTableCheckCommand => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(RadionButtonTableCheck);
        private void RadionButtonTableCheck(DataGrid data)
        {
            DataGridCheck = data;
            BeInOrder = false;
            BeInCheck = true;
            InitializationCollectionCheck();
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
                    foreach (var item in ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(OrderCollection))
                    {
                        OrderUPGRADECollection.Add(item);
                    }
                }
            });

            DataGridOrderUPGRADE.SelectionChanged += DataGridOrderUPGRADE_SelectionChanged;

        }

        private void DataGridOrderUPGRADE_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.SelectItemsOrder = e;


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
            if (DataGridCheck != null)
                DataGridCheck.SelectionChanged += DataGridCheck_SelectionChanged;
        }

        private void DataGridCheck_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.SelectItemsCheck = e;

        private async void InitializationCollectionAllId()
        {

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AllidProductCollection.Clear();
                AllidUserCollection.Clear();
                AllidCheckCollection.Clear();
                answerOrder.Clear();

                foreach (var item in UniversalRequst.ReadingAllToIdFromTable("product", "idProduct"))
                {
                    AllidProductCollection.Add(item);
                }

                foreach (var item in UniversalRequst.ReadingAllToIdFromTable("user", "idUser"))
                {
                    AllidUserCollection.Add(item);
                }

                foreach (var item in UniversalRequst.ReadingAllToIdFromTable("check", "idCheck"))
                {
                    AllidCheckCollection.Add(item);
                }

                answerOrder.Add("Одобрить");
                answerOrder.Add("Ожидает оплаты");
            });

        }

        private async void AddNewOrder(Order item)
        {

            ModelUPGRADEOrder orderU = new ModelUPGRADEOrder();
            if (OrderUPGRADECollection.Count != 0)
                orderU = ConvertOrderAndOrderUPGRADE.ConvertFromOrderInOrderUPGRADE(item, OrderUPGRADECollection.Max(x => x.idOrder) + 1);
            else if (OrderCollectionSettings.Count != 0)
            {
                orderU = ConvertOrderAndOrderUPGRADE.ConvertFromOrderInOrderUPGRADE(item, OrderCollectionSettings.Max(x => x.idOrder) + 1);
            }

            item.idOrder = orderU.idOrder;

            if (!IsCheckedCheckBoxUniteCheck && IsADD)
            {
                var tempPrice = DataBaseRequstCheck.FindValueByidCheck<double>(orderU.idCheck, FindByValueCheck.Sum) + orderU.Price;
                UpdateItemInTableCheck(FindByValueCheck.Sum, tempPrice, orderU.idCheck);

                for (int i = 0; i < CheckCollection.Count; i++)
                {
                    if (CheckCollection[i].idCheck == orderU.idCheck)
                    {
                        Check tempCheck = new Check();
                        tempCheck.idCheck = CheckCollection[i].idCheck;
                        tempCheck.Sum = tempPrice;
                        tempCheck.Data = CheckCollection[i].Data;

                        CheckCollection.Insert(i, tempCheck);
                        CheckCollection.RemoveAt(i + 1);
                    }
                }
            }

            OrderUPGRADECollection.Add(orderU);

            await SaveNewItemOrderByDB(item);

        }

        private void UPDATEItemInCollectionOrder(ObservableCollection<Order> Collection, ObservableCollection<Order> CollectionOldItems)
        {
            bool isUpdateCollection = false;

            double newSumCheck = 0;
            Check newSumOldCheck = new Check();
            Check EditOldCheck = new Check();
            Check EditNewCheck = new Check();
            Check CheckUnion = new Check();

            Task task = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < Collection.Count; i++)
                {
                    if (Collection[i].idUser != CollectionOldItems[i].idUser)
                    {
                        isUpdateCollection = true;
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.idUser, Collection[i].idUser, Collection[i].idOrder);
                    }

                    if (Collection[i].Status != CollectionOldItems[i].Status)
                    {
                        isUpdateCollection = true;
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Status, Collection[i].Status, Collection[i].idOrder);
                    }

                    if (Collection[i].Quantity != CollectionOldItems[i].Quantity)
                    {
                        isUpdateCollection = true;
                        Collection[i].Price = FindValueByidProduct<double>(Collection[i].idProduct, FindByValueProduct.Price) * Collection[i].Quantity;

                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Quantity, Collection[i].Quantity, Collection[i].idOrder);
                    }

                    if (Collection[i].Discount != CollectionOldItems[i].Discount)
                    {
                        isUpdateCollection = true;
                        if (Collection[i].Discount != 0)
                        {
                            double PriceProduct = FindValueByidProduct<double>(Collection[i].idProduct, FindByValueProduct.Price) * Collection[i].Quantity;
                            double DiscountAmount = (PriceProduct * Collection[i].Discount) / 100;
                            Collection[i].Price = PriceProduct - DiscountAmount;
                        }

                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Discount, Collection[i].Discount, Collection[i].idOrder);
                    }

                    if (Collection[i].Price != CollectionOldItems[i].Price)
                    {
                        isUpdateCollection = true;
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Price, Collection[i].Price, Collection[i].idOrder);

                        if (!IsCheckedCheckBoxUniteCheck && Collection[i].idCheck == CollectionOldItems[i].idCheck)
                        {
                            newSumCheck = (DataBaseRequstCheck.FindValueByidCheck<double>(Collection[i].idCheck, FindByValueCheck.Sum) - CollectionOldItems[i].Price) + Collection[i].Price;
                            DataBaseRequstCheck.UpdateItemInTableCheck<double>(FindByValueCheck.Sum, newSumCheck, Collection[i].idCheck);
                        }

                    }

                    if (Collection[i].idCheck != CollectionOldItems[i].idCheck)
                    {
                        isUpdateCollection = true;

                        if (!IsCheckedCheckBoxUniteCheck && IsUPDATE)
                        {
                            var temp = MoveItemFromCheck(CollectionOldItems[i].idCheck, Collection[i].idCheck, CollectionOldItems[i]);
                            EditOldCheck = temp.newSumOldCheck;
                            EditNewCheck = temp.newSumNewCheck;
                            DataBaseRequestOrder.SaveNewItemOrderByDB(Collection[i]);
                        }
                    }

                    if (IsCheckedCheckBoxUniteCheck && IsUPDATE)
                    {
                        isUpdateCollection = true;
                        double Sum = Collection.Sum(x => x.Price);

                        int UnionNewidCheck = 0;

                        if (CheckCollection.Count != 0)
                        {
                            UnionNewidCheck = CheckCollection.Max(x => x.idCheck) + 1;
                        }

                        if (Sum != 0)
                        {

                            for (int j = 0; j < Collection.Count; j++)
                            {
                                Collection[j].idCheck = UnionNewidCheck;
                            }

                            CheckUnion.idCheck = UnionNewidCheck;
                            CheckUnion.Sum = Sum;

                            Task task2 = Task.Factory.StartNew(() => SaveNewItemCheckByDB(UnionNewidCheck, Sum));

                            task2.Wait();

                            if (IsCheckedCheckBoxUniteCheck && Collection[i].idCheck != CollectionOldItems[i].idCheck)
                            {

                                newSumOldCheck.Sum = (DataBaseRequstCheck.FindValueByidCheck<double>(CollectionOldItems[i].idCheck, FindByValueCheck.Sum) - CollectionOldItems[i].Price);
                                DataBaseRequstCheck.UpdateItemInTableCheck<double>(FindByValueCheck.Sum, newSumOldCheck.Sum, CollectionOldItems[i].idCheck);
                                newSumOldCheck.idCheck = CollectionOldItems[i].idCheck;
                                DataBaseRequestOrder.UpdateItemInTableOrder<int>(FindByValueOrder.idCheck, UnionNewidCheck, Collection[i].idOrder);
                            }

                            CheckUnion.Data = DataBaseRequstCheck.FindValueByidCheck<DateTime>(UnionNewidCheck, FindByValueCheck.Data);
                        }
                    }
                }
            });
            task.Wait();

            OrderCollectionSettingsOldItems.Clear();

            if (isUpdateCollection)
            {
                var item = Collection[0];
                for (int i = 0; i < OrderUPGRADECollection.Count; i++)
                {
                    if (OrderUPGRADECollection[i].idOrder == item.idOrder)
                    {
                        ModelUPGRADEOrder OrderU = ConvertOrderAndOrderUPGRADE.ConvertFromOrderInOrderUPGRADE(item);

                        Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OrderUPGRADECollection.Insert(i, OrderU);
                            OrderUPGRADECollection.RemoveAt(i + 1);
                        });

                        break;
                    }
                }

                if (newSumOldCheck != null)
                {
                    for (int j = 0; j < CheckCollection.Count; j++)
                    {
                        if (CheckCollection[j].idCheck == newSumOldCheck.idCheck)
                        {
                            newSumOldCheck.Data = CheckCollection[j].Data;

                            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                CheckCollection.Insert(j, newSumOldCheck);
                                CheckCollection.RemoveAt(j + 1);
                            });

                            break;
                        }
                    }
                }

                if (newSumCheck != 0)
                {
                    for (int j = 0; j < CheckCollection.Count; j++)
                    {
                        Check check = new Check();
                        if (CheckCollection[j].idCheck == item.idCheck)
                        {
                            check.idCheck = item.idCheck;
                            check.Sum = newSumCheck;
                            check.Data = CheckCollection[j].Data;

                            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                CheckCollection.Insert(j, check);
                                CheckCollection.RemoveAt(j + 1);
                            });

                            break;
                        }
                    }
                }

                if (EditOldCheck != default && EditOldCheck.Sum == -1)
                {
                    for (int j = 0; j < CheckCollection.Count; j++)
                    {
                        if (CheckCollection[j].idCheck == EditOldCheck.idCheck)
                        {
                            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                CheckCollection.RemoveAt(j);
                            });

                            break;
                        }
                    }
                }

                if (EditNewCheck != default && EditOldCheck != default)
                {
                    for (int j = 0; j < CheckCollection.Count; j++)
                    {
                        if (CheckCollection[j].idCheck == EditNewCheck.idCheck)
                        {

                            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                EditNewCheck.Data = CheckCollection[j].Data;
                                CheckCollection.Insert(j, EditNewCheck);
                                CheckCollection.RemoveAt(j + 1);
                            });

                            break;
                        }

                    }

                    if (EditOldCheck.Sum != -1 && EditOldCheck != null)
                        for (int j = 0; j < CheckCollection.Count; j++)
                        {
                            if (CheckCollection[j].idCheck == EditOldCheck.idCheck)
                            {

                                Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    CheckCollection.Insert(j, EditOldCheck);
                                    CheckCollection.RemoveAt(j + 1);
                                });

                                break;
                            }

                        }

                    if (CheckUnion != null && IsCheckedCheckBoxUniteCheck)
                    {
                        Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            CheckCollection.Add(CheckUnion);
                        });
                    }
                }
            }
        }
    }
}
