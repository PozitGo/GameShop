using GameShop.DataBase;
using GameShop.Enum;
using GameShop.Model;
using GameShop.Model.ModelTableInDataBase;
using GameShop.ViewModels.InfoBars;
using GameShop.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
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
using NavigationService = GameShop.Services.NavigationService;

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

        public ObservableCollection<string> NameProduct = new ObservableCollection<string>();

        public ObservableCollection<string> ResultNameProduct = new ObservableCollection<string>();

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

        private bool _IsOpenInfoBar;

        public bool IsOpenInfoBar
        {
            get => _IsOpenInfoBar;
            set
            {
                SetProperty(ref _IsOpenInfoBar, value);
            }
        }

        private InfoBarSeverity _SeverityInfoBar;

        public InfoBarSeverity SeverityInfoBar
        {
            get => _SeverityInfoBar;
            set => SetProperty(ref _SeverityInfoBar, value);
        }

        private string _TitleInfoBar;

        public string TitleInfoBar
        {
            get
            {
                return _TitleInfoBar;
            }
            set
            {
                SetProperty(ref _TitleInfoBar, value);
            }
        }

        private string _MessageInfoBar;

        public string MessageInfoBar
        {
            get => _MessageInfoBar;
            set
            {
                SetProperty(ref _MessageInfoBar, value);
            }
        }

        private string _TextAutoSuggestBoxFindNameProduct;
        public string TextAutoSuggestBoxFindNameProduct
        {
            get => _TextAutoSuggestBoxFindNameProduct;
            set
            {
                FindNameProductOnidProduct();

                SetProperty(ref _TextAutoSuggestBoxFindNameProduct, value);
            }
        }

        private Visibility _VisibilityStackPanelReport;
        public Visibility VisibilityStackPanelReport
        {
            get => _VisibilityStackPanelReport;
            set => SetProperty(ref _VisibilityStackPanelReport, value);
        }

        private string _TextBoxTextReport;

        public string TextBoxTextReport
        {
            get => _TextBoxTextReport;
            set => SetProperty(ref _TextBoxTextReport, value);
        }

        public void ShowInfoBar(InfoBar bar)
        {
            TitleInfoBar = bar.Title;
            MessageInfoBar = bar.Message;
            SeverityInfoBar = bar.Severity;
            IsOpenInfoBar = bar.IsOpen;
        }

        private bool IsUPDATE;
        private bool IsDELETE;
        private bool IsADD;

        public bool BeInCheck { get; set; }
        public bool BeInOrder { get; set; }

        private SelectionChangedEventArgs SelectItemsOrder;

        private SelectionChangedEventArgs SelectItemsCheck;
        public ICommand AppBarButtonAddOrder => new RelayCommand<DataGrid>(AppBarButtonAddOrderClick);
        private async void AppBarButtonAddOrderClick(DataGrid data)
        {
            DataGridOrderSettings = data;
            DataGridOrderSettings.Visibility = Visibility.Visible;
            if (DataGridOrderSettings == null)
            {
                DataGridOrderSettings = data;
            }
            if (!BeInCheck && !BeInOrder)
                ShowInfoBar(ControlPageInfoBar.Warning("У вас не выбрана ни одна таблица", "Выберите таблицу"));

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

        public ICommand AppBarButtonEditOrder => new RelayCommand<DataGrid>(AppBarButtonEditOrderClick);

        ObservableCollection<Order> OrderCollectionSettingsOldItems = new ObservableCollection<Order>();
        private async void AppBarButtonEditOrderClick(DataGrid data)
        {
            DataGridOrderSettings = data;

            IsUPDATE = true;
            IsADD = false;
            IsDELETE = false;

            if (!BeInCheck && !BeInOrder)
                ShowInfoBar(ControlPageInfoBar.Warning("У вас не выбрана ни одна таблица", "Выберите таблицу"));

            IsCheckedCheckBoxUniteCheck = false;
            DataGridOrderSettings.Columns[6].Visibility = Visibility.Visible;

            DataGridOrderSettings.Columns[0].Visibility = Visibility.Collapsed;
            InitializationCollectionCheck();
            InitializationCollectionAllId();

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Clear();
            });

            OrderCollectionSettingsOldItems.Clear();

            if (SelectItemsOrder != null)
            {
                DataGridOrderSettings.Visibility = Visibility.Visible;

                ContentSaveButtonCommandBar = "Изменить";
                VisibilitySaveButtonCommandBar = "Visible";
                VisibilityCheckBoxUniteCheck = "Visible";
                ContentCheckBoxUniteCheck = "Вынести в новый чек";

                try
                {
                    ModelUPGRADEOrder tempOrderAdd = null;

                    tempOrderAdd = SelectItemsOrder.AddedItems[0] as ModelUPGRADEOrder;

                    OrderCollectionSettings.Add(tempOrderAdd);

                    OrderCollectionSettingsOldItems.Add(ConvertOrderAndOrderUPGRADE.ConvertFromOrderUPGRADEInOrder(OrderUPGRADECollection.First(x => x.idOrder == tempOrderAdd.idOrder)));


                }
                catch (Exception ex)
                {
                    ShowInfoBar(ControlPageInfoBar.Warning("Ошибка", ex.Message));
                }
            }
            else
                ShowInfoBar(ControlPageInfoBar.Error("Заказ для изменения не выбран", "Выберете заказ и повторите попытку"));
        }

        public ICommand AppBarButtonDeleteOrder => new RelayCommand<DataGrid>(AppBarButtonDeleteClick);
        private async void AppBarButtonDeleteClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;


            if (DataGridOrderSettings == null)
                DataGridOrderSettings = data;
            if (!BeInCheck && !BeInOrder)
                ShowInfoBar(ControlPageInfoBar.Warning("У вас не выбрана ни одна таблица", "Выберите таблицу"));

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

            if (SelectItemsCheck != null || SelectItemsOrder != null)
            {
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
                else
                    DataGridOrderSettings.Visibility = Visibility.Visible;

                if (BeInOrder && SelectItemsOrder != null)
                {
                    for (int i = 0; i < SelectItemsOrder.AddedItems.Count; i++)
                    {
                        OrderCollectionSettings.Add(SelectItemsOrder.AddedItems[i] as ModelUPGRADEOrder);
                    }
                }
            }
            else
                ShowInfoBar(ControlPageInfoBar.Error("У вас не выбран заказ/чек для изменения", "Выберете заказ/чек и повторите попытку"));
        }

        public ICommand AppBarButtonAccept => new RelayCommand<DataGrid>(AppBarButtonAcceptClick);
        private void AppBarButtonAcceptClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            if (!BeInCheck && !BeInOrder)
                ShowInfoBar(ControlPageInfoBar.Error("У вас не выбрана ни одна таблица", "Выберите таблицу"));
            else
            {
                ModelUPGRADEOrder tempOrderAdd = null;
                if (SelectItemsOrder != null)
                    tempOrderAdd = SelectItemsOrder.AddedItems[0] as ModelUPGRADEOrder;
                else

                    ShowInfoBar(ControlPageInfoBar.Error("У вас не выбран заказ", "Выберите и повторите попытку"));

                if (tempOrderAdd != null)
                {
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
            }

        }

        public ICommand AppBarButtonClearOrder => new RelayCommand<DataGrid>(AppBarButtonClearClick);
        private void AppBarButtonClearClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            ModelUPGRADEOrder tempOrderClear = null;
            if (SelectItemsOrder != null)
                tempOrderClear = SelectItemsOrder.AddedItems[0] as ModelUPGRADEOrder;
            else
                ShowInfoBar(ControlPageInfoBar.Error("У вас не выбран заказ", "Выберите и повторите попытку"));

            if (tempOrderClear != null)
            {
                int idOrder = tempOrderClear.idOrder;
                tempOrderClear.Status = "Ожидает оплаты";

                if (!BeInCheck && !BeInOrder)
                    ShowInfoBar(ControlPageInfoBar.Error("У вас не выбрана ни одна таблица", "Выберите таблицу"));
                else
                {
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
            }
        }

        public ICommand AppBarButtonRefresh => new RelayCommand<DataGrid>(AppBarButtonRefreshClick);
        private void AppBarButtonRefreshClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            if (!BeInCheck && !BeInOrder)
                ShowInfoBar(ControlPageInfoBar.Warning("У вас не выбрана ни одна таблица", "Выберите таблицу"));

            InitializationCollectionAllId();
            InitializationCollectionCheck();
            InitializationCollectionOrderUPGRADE();
        }

        public ICommand DeleteAllOrdersOnIdCheckCommand => new RelayCommand<Check>(DeleteAllOrdersOnIdCheck);

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
            DataGridOrderSettings.Visibility = Visibility.Collapsed;

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

        public ICommand AppBarButtonClearSettingsTable => new RelayCommand<DataGrid>(AppBarButtonClearSettingsTableClick);
        private void AppBarButtonClearSettingsTableClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;

            if (!BeInCheck && !BeInOrder)
                ShowInfoBar(ControlPageInfoBar.Warning("У вас не выбрана ни одна таблица", "Выберите таблицу"));
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

            DataGridOrderSettings.Visibility = Visibility.Collapsed;

            VisibilitySaveButtonCommandBar = "Collapsed";
            VisibilityCheckBoxUniteCheck = "Collapsed";
        }

        public ICommand RadionButtonTableOrderCommand => new RelayCommand<DataGrid>(RadionButtonTableOrder);
        private async void RadionButtonTableOrder(DataGrid data)
        {
            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                DataGridOrderUPGRADE = data;
                BeInOrder = true;
                BeInCheck = false;
                DataGridOrderUPGRADE.Visibility = Visibility.Visible;

                if (DataGridCheck != null)
                    DataGridCheck.Visibility = Visibility.Collapsed;
            });

            InitializationCollectionOrderUPGRADE();

            VisibilityStackPanelReport = Visibility.Collapsed;
        }

        public ICommand RadionButtonTableCheckCommand => new RelayCommand<DataGrid>(RadionButtonTableCheck);
        private void RadionButtonTableCheck(DataGrid data)
        {
            DataGridCheck = data;
            BeInOrder = false;
            BeInCheck = true;
            InitializationCollectionCheck();
            DataGridCheck.Visibility = Visibility.Visible;

            if (DataGridOrderUPGRADE != null)
                DataGridOrderUPGRADE.Visibility = Visibility.Collapsed;

            VisibilityStackPanelReport = Visibility.Collapsed;
        }

        public ControlPanelViewModel()
        {
            IsVisibleCheckBoxUniteCheckCommand = new RelayCommand(IsVisibleCheckBoxUniteCheck);
            AppBarButtonSaveButton = new RelayCommand(AppBarButtonSaveButtonClick);
            DataGridTextColumnVisibilitySettingsidCheck = "Visible";
            VisibilityCheckBoxUniteCheck = "Collapsed";
            VisibilitySaveButtonCommandBar = "Collapsed";
            IsOpenInfoBar = false;
            VisibilityStackPanelReport = Visibility.Collapsed;

            Task.Factory.StartNew(() => InitializationNameProduct());
        }

        private async void InitializationCollectionOrderUPGRADE()
        {
            await Task.Factory.StartNew(() =>
            {
                if (DataGridOrderUPGRADE != null)
                    DataGridOrderUPGRADE.UnloadingRowDetails += DataGridOrderUPGRADE_UnloadingRowDetails;
                else
                    ShowInfoBar(ControlPageInfoBar.Warning("Коллекция заказов пустая", "Выберете таблицу заказы и повторите попытку"));
            });

            if (OrderUPGRADECollection.Count == 0)
            {
                ObservableCollection<Order> OrderCollection = new ObservableCollection<Order>();
                await Task.Factory.StartNew(() =>
                    {
                        foreach (var item in ReadingDataOrder())
                        {
                            OrderCollection.Add(item);
                        }
                    });

                foreach (var item in ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(OrderCollection))
                {
                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        OrderUPGRADECollection.Add(item);
                    });
                }
            }

            if (DataGridOrderUPGRADE != null)
                DataGridOrderUPGRADE.SelectionChanged += DataGridOrderUPGRADE_SelectionChanged;
            else
                ShowInfoBar(ControlPageInfoBar.Warning("Коллекция заказов пустая", "Выберете таблицу заказы и повторите попытку"));
        }

        private void DataGridOrderUPGRADE_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.SelectItemsOrder = e;


        private void DataGridOrderUPGRADE_UnloadingRowDetails(object sender, DataGridRowDetailsEventArgs e) => e.Row.DetailsVisibility = Visibility.Collapsed;

        private async void InitializationCollectionCheck()
        {
            if (CheckCollection.Count == 0)

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    foreach (var item in ReadingDataCheck())
                    {
                        CheckCollection.Add(item);
                    }

                });

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

                answerOrder.Add("Одобрен");
                answerOrder.Add("Ожидает оплаты");
            });

        }

        private void InitializationNameProduct()
        {
            foreach (var item in UniversalRequst.ReadingAllToIdFromTableString("product", "Name"))
            {
                NameProduct.Add(item);
            }
        }

        private async void FindNameProductOnidProduct()
        {
            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ResultNameProduct.Clear();
                int idProduct = 0;

                int.TryParse(TextAutoSuggestBoxFindNameProduct, out idProduct);

                if (idProduct != 0 && idProduct <= NameProduct.Count)
                {
                    ResultNameProduct.Add(NameProduct[idProduct - 1]);
                }
                else
                {
                    ResultNameProduct.Add("Нет результатов");
                }
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

        public ICommand SortActiveOrderCollection => new RelayCommand<DataGrid>(SortActiveOrderCollectionClick);
        private async void SortActiveOrderCollectionClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            InitializationCollectionOrderUPGRADE();
            DataGridOrderUPGRADE.Visibility = Visibility.Visible;

            if (DataGridCheck != null)
                DataGridCheck.Visibility = Visibility.Collapsed;
            else
                ShowInfoBar(ControlPageInfoBar.Error("Коллекция чеков пустая", "Выберете таблицу чек и повторите попытку"));

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var SortCollection = ReadingDataOrder(FindByValueOrder.Status, "Ожидает оплаты");

                OrderUPGRADECollection.Clear();

                foreach (var item in ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(SortCollection))
                {
                    OrderUPGRADECollection.Add(item);
                }
            });
        }

        public ICommand SortByidCheck => new RelayCommand<DataGrid>(SortByidCheckClick);
        private async void SortByidCheckClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            int idCheck = 0;
            bool isClosed = false;

            InitializationCollectionOrderUPGRADE();
            TextBox textBox = new TextBox();
            textBox.Visibility = Visibility.Visible;
            textBox.Header = "Введите номер чека";

            ContentDialog SortIdCheck = new ContentDialog()
            {
                Title = "Фильтр по номеру чека",
                Content = "Введите номер чека и нажмите кнопку найти",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Найти",

                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() =>
                {
                    int.TryParse(textBox.Text, out idCheck);
                }),
            };

            SortIdCheck.Content = textBox;
            await SortIdCheck.ShowAsync();

            DataGridOrderUPGRADE.Visibility = Visibility.Visible;
            if (DataGridCheck != null)
                DataGridCheck.Visibility = Visibility.Collapsed;
            else
                ShowInfoBar(ControlPageInfoBar.Warning("Коллекция чеков пустая", "Выберете таблицу чек и повторите попытку"));
            DataGridOrderUPGRADE.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Collapsed;
            if (!isClosed && idCheck != 0)
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var SortCollection = ReadingDataOrder(FindByValueOrder.idCheck, idCheck);

                    OrderUPGRADECollection.Clear();

                    foreach (var item in ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(SortCollection))
                    {
                        OrderUPGRADECollection.Add(item);
                    }
                });
            }
        }

        public ICommand SortByRangeSumOrder => new RelayCommand<DataGrid>(SortByRangeSumOrderClick);
        private async void SortByRangeSumOrderClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            double MinSum = -1;
            double MaxSum = 0;
            InitializationCollectionOrderUPGRADE();
            bool isClosed = false;

            StackPanel stackPanel = new StackPanel();

            CheckBox check = new CheckBox();
            check.Content = "До макс существующего";
            check.IsChecked = false;

            TextBox textBoxMax = new TextBox();
            textBoxMax.Visibility = Visibility.Visible;
            textBoxMax.Header = "До";

            TextBox textBoxMin = new TextBox();
            textBoxMin.Header = "От";
            textBoxMin.Visibility = Visibility.Visible;

            stackPanel.Children.Add(textBoxMin);
            stackPanel.Children.Add(textBoxMax);
            stackPanel.Children.Add(check);

            stackPanel.Spacing = 10;
            stackPanel.Orientation = Orientation.Horizontal;

            check.Checked += (s, e) =>
            {
                if (check.IsChecked == true) textBoxMax.IsEnabled = false;
            };

            ContentDialog SortIdCheck = new ContentDialog()
            {
                Title = "Фильтр по диапазону суммы",
                Content = "Введите минимальную и максимальную сумму",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Найти",
                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() =>
                {
                    if (textBoxMax.IsEnabled == true)
                        double.TryParse(textBoxMax.Text, out MaxSum);
                    else
                    {
                        MaxSum = OrderUPGRADECollection.Max(x => x.Price);
                    }
                    double.TryParse(textBoxMin.Text, out MinSum);
                }),
            };

            SortIdCheck.Content = stackPanel;
            await SortIdCheck.ShowAsync();

            stackPanel.Visibility = Visibility.Collapsed;

            if (MaxSum > 0 && MinSum != -1 && !isClosed)
            {
                DataGridOrderUPGRADE.Visibility = Visibility.Visible;
                if (DataGridCheck != null)
                    DataGridCheck.Visibility = Visibility.Collapsed;
                else
                    ShowInfoBar(ControlPageInfoBar.Error("Коллекция чеков пустая", "Выберете таблицу чеков и повторите попытку"));
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var SortCollection = ReadingOrderPriceByMinValueAndMaxValue(MinSum, MaxSum);

                    OrderUPGRADECollection.Clear();

                    foreach (var item in ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(SortCollection))
                    {
                        OrderUPGRADECollection.Add(item);
                    }

                });
            }
        }

        public ICommand SortByRangeSumCheck => new RelayCommand<DataGrid>(SortByRangeSumCheckClick);
        private async void SortByRangeSumCheckClick(DataGrid data)
        {
            DataGridCheck = data;
            InitializationCollectionCheck();
            double MinSum = -1;
            double MaxSum = 0;
            bool isClosed = false;

            StackPanel stackPanel = new StackPanel();

            CheckBox check = new CheckBox();
            check.Content = "До макс существующего";
            check.IsChecked = false;

            TextBox textBoxMax = new TextBox();
            textBoxMax.Visibility = Visibility.Visible;
            textBoxMax.Header = "До";

            TextBox textBoxMin = new TextBox();
            textBoxMin.Header = "От";
            textBoxMin.Visibility = Visibility.Visible;

            stackPanel.Children.Add(textBoxMin);
            stackPanel.Children.Add(textBoxMax);
            stackPanel.Children.Add(check);

            stackPanel.Spacing = 10;
            stackPanel.Orientation = Orientation.Horizontal;

            check.Checked += (s, e) =>
            {
                if (check.IsChecked == true) textBoxMax.IsEnabled = false;
            };

            ContentDialog SortIdCheck = new ContentDialog()
            {
                Title = "Фильтр по диапазону суммы",
                Content = "Введите минимальную и максимальную сумму",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Найти",
                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() =>
                {
                    if (textBoxMax.IsEnabled == true)
                        double.TryParse(textBoxMax.Text, out MaxSum);
                    else
                    {
                        MaxSum = CheckCollection.Max(x => x.Sum);
                    }

                    double.TryParse(textBoxMin.Text, out MinSum);
                }),
            };

            SortIdCheck.Content = stackPanel;
            await SortIdCheck.ShowAsync();

            stackPanel.Visibility = Visibility.Collapsed;

            if (MaxSum > 0 && MinSum != -1 && !isClosed)
            {
                DataGridCheck.Visibility = Visibility.Visible;
                if (DataGridOrderUPGRADE != null)
                    DataGridOrderUPGRADE.Visibility = Visibility.Collapsed;
                else
                    ShowInfoBar(ControlPageInfoBar.Error("Коллекция заказов пустая", "Выберете таблицу заказов и повторите попытку"));
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var SortCollection = ReadingSumCheckByMinValueAndMaxValue(MinSum, MaxSum);

                    CheckCollection.Clear();

                    foreach (var item in SortCollection)
                    {
                        CheckCollection.Add(item);
                    }

                });
            }
        }

        public ICommand SortByDataCheck => new RelayCommand<DataGrid>(SortByDataCheckClick);
        private async void SortByDataCheckClick(DataGrid data)
        {
            DataGridCheck = data;
            bool isClosed = false;
            DateTime tempdata = new DateTime();
            DatePicker date = new DatePicker();
            date.Header = "Выберете дату";
            date.Visibility = Visibility.Visible;

            ContentDialog SortData = new ContentDialog()
            {
                Title = "Поиск чека по дате",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Найти",

                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() =>
                {
                    tempdata = date.Date.DateTime;
                }),
            };

            SortData.Content = date;
            await SortData.ShowAsync();

            if (!isClosed && tempdata != default)
            {
                DataGridCheck.Visibility = Visibility.Visible;
                if (DataGridOrderUPGRADE != null)
                    DataGridOrderUPGRADE.Visibility = Visibility.Collapsed;
                else
                    ShowInfoBar(ControlPageInfoBar.Error("Ошибка", "Не выбрана таблица заказов"));

                var SortCollection = ReadingDataCheck(FindByValueCheck.Data, tempdata);

                CheckCollection.Clear();

                foreach (var item in SortCollection)
                {
                    CheckCollection.Add(item);
                }

            }
        }

        public ICommand SortOrdersByLoginUser => new RelayCommand<DataGrid>(SortOrdersByLoginUserClick);
        private async void SortOrdersByLoginUserClick(DataGrid data)
        {
            DataGridOrderUPGRADE = data;
            string Login = null;
            bool isClosed = false;
            TextBox textBox = new TextBox();
            textBox.Header = "Введите логин";
            textBox.Visibility = Visibility.Visible;

            ContentDialog SortLogin = new ContentDialog()
            {
                Title = "Поиск заказов по логину",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Найти",

                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() =>
                {
                    Login = textBox.Text;
                }),
            };

            SortLogin.Content = textBox;
            await SortLogin.ShowAsync();

            if (!isClosed && Login != null)
            {
                DataGridOrderUPGRADE.Visibility = Visibility.Visible;
                if (DataGridCheck != null)
                    DataGridCheck.Visibility = Visibility.Collapsed;

                ObservableCollection<UserAccount> idUser = new ObservableCollection<UserAccount>();
                try
                {
                    idUser = DataBaseRequstUser.ReadingDataUser(FindByValueUser.Login, Login);
                }
                catch
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Логин не найден", "Введите другой логин и повторите попытку"));
                }

                bool LoginCorrect = true;
                ObservableCollection<Order> SortCollection = new ObservableCollection<Order>();

                try
                {
                    SortCollection = ReadingDataOrder(FindByValueOrder.idUser, idUser[0].idUser);
                }
                catch
                {
                    LoginCorrect = false;
                    ShowInfoBar(ControlPageInfoBar.Error("Логин не найден", "Введите другой логин и повторите попытку"));
                }

                if (LoginCorrect)
                {
                    OrderUPGRADECollection.Clear();

                    foreach (var item in ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(SortCollection))
                    {
                        OrderUPGRADECollection.Add(item);
                    }
                }
            }
        }
        public ICommand SortCheckByIdOrder => new RelayCommand<DataGrid>(SortCheckByIdOrderClick);
        private async void SortCheckByIdOrderClick(DataGrid data)
        {
            DataGridCheck = data;
            bool isClosed = false;
            int idOrder = 0;

            TextBox textBoxidOrder = new TextBox();
            textBoxidOrder.Header = "Введите номер заказа";
            textBoxidOrder.Visibility = Visibility.Visible;

            ContentDialog SortCheckByIdOrder = new ContentDialog()
            {
                Title = "Поиск по номеру заказа",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Найти",

                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() =>
                {
                    int.TryParse(textBoxidOrder.Text, out idOrder);
                }),
            };

            SortCheckByIdOrder.Content = textBoxidOrder;
            await SortCheckByIdOrder.ShowAsync();


            if (!isClosed && idOrder != 0)
            {
                DataGridCheck.Visibility = Visibility.Visible;
                if (DataGridOrderUPGRADE != null)
                    DataGridOrderUPGRADE.Visibility = Visibility.Collapsed;
                else
                    ShowInfoBar(ControlPageInfoBar.Error("Ошибка", "Не выбрана таблица заказов"));

                int idCheck = FindValueByidOrder<int>(idOrder, FindByValueOrder.idCheck);

                var SortCollection = ReadingDataCheck(FindByValueCheck.idCheck, idCheck);

                CheckCollection.Clear();

                foreach (var item in SortCollection)
                {
                    CheckCollection.Add(item);
                }
            }
        }

        public ICommand ClearFilters => new RelayCommand(ClearFiltersClick);

        private void ClearFiltersClick()
        {
            OrderUPGRADECollection.Clear();
            CheckCollection.Clear();

            InitializationCollectionOrderUPGRADE();

            InitializationCollectionCheck();
        }

        public ICommand ReportOrder => new RelayCommand(ReportOrderClick);

        private async void ReportOrderClick()
        {

            Check tempReportCheck = null;
            ModelUPGRADEOrder tempReportOrder = null;

            Check checkReport = null;
            ObservableCollection<ModelUPGRADEOrder> tempCollectionOrders = null;


            if (BeInCheck && DataGridCheck.SelectedItem != null)
            {
                tempReportCheck = (Check)DataGridCheck.SelectedItem;
            }
            else if (BeInOrder && DataGridOrderUPGRADE.SelectedItem != null)
            {
                tempReportOrder = (ModelUPGRADEOrder)DataGridOrderUPGRADE.SelectedItem;
            }

            if (tempReportCheck != null)
            {
                checkReport = tempReportCheck;
                var tempOrder = ReadingDataOrder(FindByValueOrder.idCheck, checkReport.idCheck);
                tempCollectionOrders = ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(tempOrder);
            }
            else if (tempReportOrder != null)
            {

                var tempCheck = ReadingDataCheck(FindByValueCheck.idCheck, tempReportOrder.idCheck);
                checkReport = tempCheck[0];

                var tempOrder = ReadingDataOrder(FindByValueOrder.idCheck, tempReportOrder.idCheck);
                tempCollectionOrders = ConvertOrderAndOrderUPGRADE.ConvertFromOrderCollectionInOrderUPGRADECollection(tempOrder);
            }

            if (tempReportCheck != null || tempReportOrder != null)
            {
                string idCheck = "Чек # " + checkReport.idCheck.ToString();
                string NameCompany = $"❖GameShop\t{idCheck}\t\t\t\t\t\t\t\t{checkReport.Data}\n";
                string trait = "--------------------------------------------------------------------------------------------------------\n";
                string Orders = "";

                foreach (var item in tempCollectionOrders)
                {
                    Orders = Orders + $"# {item.idOrder}  Назв.: {item.NameProduct}\tЛогин:{item.LoginUser}  Кол-во:{item.Quantity}  Сумма:{item.Price}\n";
                }

                string Check = $"\nИтого: {checkReport.Sum}";

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    VisibilityStackPanelReport = Visibility.Visible;

                    if (DataGridOrderUPGRADE != null)
                        DataGridOrderUPGRADE.Visibility = Visibility.Collapsed;

                    if (DataGridCheck != null)
                        DataGridCheck.Visibility = Visibility.Collapsed;

                    TextBoxTextReport = NameCompany + trait + Orders + trait + Check;
                });
            }
            else
                ShowInfoBar(ControlPageInfoBar.Error("Cоздать отчёт невозможно", "Выберете поле в таблице и повторите попытку"));
        }

        public ICommand CloseReport => new RelayCommand(CloseReportClick);

        private void CloseReportClick()
        {
            if (BeInCheck)
            {
                if (DataGridCheck != null)
                    DataGridCheck.Visibility = Visibility.Visible;
            }
            else
            {
                if (DataGridOrderUPGRADE != null)
                    DataGridOrderUPGRADE.Visibility = Visibility.Visible;
            }

            VisibilityStackPanelReport = Visibility.Collapsed;
        }

        public ICommand NavigateToStaffPage => new RelayCommand(NavigateToStaffPageClick);

        private void NavigateToStaffPageClick() => NavigationService.Navigate(typeof(ControlPanelStaffPage));

        public ICommand NavigateToProductPage => new RelayCommand(NavigateToProductPageClick);

        private void NavigateToProductPageClick() => NavigationService.Navigate(typeof(ProductControlPanelPage));
    }
}
