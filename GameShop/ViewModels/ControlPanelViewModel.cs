using GameShop.DataBase;
using GameShop.Enum;
using GameShop.Model;
using GameShop.Model.ModelTableInDataBase;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using NPOI.XWPF.UserModel;
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

        public DataGrid DataGridOrderUPGRADE;
        public DataGrid DataGridCheck;
        public DataGrid DataGridOrderSettings;
        public DataGrid DataGridCheckSettings;


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
        private string _VisibilityTextBoxidOrderEditAndDelete;
        public string VisibilityTextBoxidOrderEditAndDelete
        {
            get => _VisibilityTextBoxidOrderEditAndDelete;
            set => SetProperty(ref _VisibilityTextBoxidOrderEditAndDelete, value);
        }
        private string _TextBoxTextidOrderEditAndDelete;
        public string TextBoxTextidOrderEditAndDelete
        {
            get => _TextBoxTextidOrderEditAndDelete;
            set => SetProperty(ref _TextBoxTextidOrderEditAndDelete, value);
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
        private bool _IsOpenTeachingTipEdit;
        public bool IsOpenTeachingTipEdit
        {
            get => _IsOpenTeachingTipEdit; 
            set => SetProperty(ref _IsOpenTeachingTipEdit, value); 
        }


        private bool IsUPDATE;
        private bool IsDELETE;
        private bool IsADD;

        private bool isOpenTableCheckSettings;


        public ICommand AppBarButtonAddOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonAddOrderClick);
        private async void AppBarButtonAddOrderClick(DataGrid data)
        {
            IsADD = true;


            if (DataGridOrderSettings == null)
            {
                DataGridOrderSettings = data;
            }

            DataGridOrderSettings.Visibility = Visibility.Visible;
            Order order = new Order();

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Add(order);
            });

            InitializationCollectionAllId();
            ContentSaveButtonCommandBar = "Сохранить";
            VisibilitySaveButtonCommandBar = "Visible";
            VisibilityCheckBoxUniteCheck = "Visible";
            VisibilityTextBoxidOrderEditAndDelete = "Collapsed";
        }

        public ICommand AppBarButtonEditOrder => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(AppBarButtonEditOrderClick);
        private void AppBarButtonEditOrderClick(DataGrid data)
        {
            DataGridOrderSettings = data;
            IsUPDATE = true;
            IsOpenTeachingTipEdit = true;

            InitializationCollectionAllId();
            VisibilityTextBoxidOrderEditAndDelete = "Visible";
            ContentSaveButtonCommandBar = "Изменить";
            VisibilitySaveButtonCommandBar = "Visible";
            VisibilityCheckBoxUniteCheck = "Collapsed";
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
            if(IsADD)
            {
                VisibilityCheckBoxUniteCheck = "Collapsed";
                VisibilitySaveButtonCommandBar = "Collapsed";
                DataGridTextColumnVisibilitySettingsidCheck = "Visible";
                DataGridOrderSettings.Visibility = Visibility.Collapsed;


                for (int i = 0; i < OrderCollectionSettings.Count; i++)
                {
                    OrderCollectionSettings[i].idCheck = OrderUPGRADECollection.Max(x => x.idCheck) + 1;
                }

                for (int i = 0; i < OrderCollectionSettings.Count; i++)
                {
                    if (OrderCollectionSettings[i].idProduct != 0 && OrderCollectionSettings[i].Quantity != 0)
                    {
                        if (OrderCollectionSettings[i].Price == 0)
                        {
                            OrderCollectionSettings[i].Price = FindValueByidProduct<float>(OrderCollectionSettings[i].idProduct, FindByValueProduct.Price) * OrderCollectionSettings[i].Quantity;

                            await Task.Factory.StartNew(() => AddNewOrder(OrderCollectionSettings[i]));
                        }
                    }

                }

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    OrderCollectionSettings.Clear();
                });
                
                IsADD = false;
            }

            if(IsUPDATE)
            {
                ObservableCollection<Order> OrderCollectionSettingsOldItems = new ObservableCollection<Order>();

                if (TextBoxTextidOrderEditAndDelete != default(String))
                {

                    foreach (var item in ReadingDataOrder(FindByValueOrder.idOrder, int.Parse(TextBoxTextidOrderEditAndDelete)))
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            OrderCollectionSettings.Add(item);
                        });
                    }

                    OrderCollectionSettingsOldItems = OrderCollectionSettings;
                    UPDATEItemInCollectionOrder(OrderCollectionSettings, OrderCollectionSettingsOldItems);
                }
                IsUPDATE = false;
            }
        }

        public ICommand OpenTableCheckSettingsCommand => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(OpenTableCheckSettings);
        private void OpenTableCheckSettings(DataGrid data)
        {
            DataGridCheckSettings = data;
            isOpenTableCheckSettings = true;
            DataGridOrderSettings.Visibility = Visibility.Collapsed;
            DataGridCheckSettings.Visibility = Visibility.Visible;
        }

        public ICommand OpenTableOrderSettingsCommand => new RelayCommand(OpenTableOrderSettings);
        private void OpenTableOrderSettings()
        {
            isOpenTableCheckSettings = false;
            DataGridOrderSettings.Visibility = Visibility.Visible;

            if(DataGridCheckSettings != null)
            DataGridCheckSettings.Visibility = Visibility.Collapsed;
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
            IsOpenTeachingTipEdit = false;
            VisibilityTextBoxidOrderEditAndDelete = "Collapsed";
            DataGridTextColumnVisibilitySettingsidCheck = "Visible";
            VisibilityCheckBoxUniteCheck = "Collapsed";
            VisibilitySaveButtonCommandBar = "Collapsed";
        }

        private void InitializationCollectionOrderUPGRADE()
        {
            DataGridOrderUPGRADE.UnloadingRowDetails += DataGridOrderUPGRADE_UnloadingRowDetails;
            //DataGridOrderUPGRADE.LoadingRowDetails += DataGridOrderUPGRADE_LoadingRowDetails;
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
        }

        private void DataGridOrderUPGRADE_LoadingRowDetails(object sender, DataGridRowDetailsEventArgs e) => this.e = e;

        private DataGridRowDetailsEventArgs e;

        private void DataGridOrderUPGRADE_UnloadingRowDetails(object sender, DataGridRowDetailsEventArgs e) => e.Row.DetailsVisibility = Visibility.Collapsed;

        public ICommand CloseDetailVisible => new RelayCommand(CloseDetailVisibleClick);
        private void CloseDetailVisibleClick()
        {
            if (e.Row.DetailsVisibility == Visibility.Visible)
                e.Row.DetailsVisibility = Visibility.Collapsed;
        }
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


        private void AddNewOrder(Order item)
        {

            ModelUPGRADEOrder orderU = new ModelUPGRADEOrder();
            orderU = ConverOrderAndOrderUPGRADE.ConvertFromOrderInOrderUPGRADE(item, OrderUPGRADECollection.Max(x => x.idOrder) + 1);
            SaveNewItemOrderByDB(item);

            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderUPGRADECollection.Add(orderU);
            });
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
                    else if (Collection[i].Quantity != OrderCollectionSettingsOldItems[i].Quantity)
                    {
                        if(Collection[i].Price == OrderCollectionSettingsOldItems[i].Price)
                        Collection[i].Price = FindValueByidProduct<float>(Collection[i].idProduct, FindByValueProduct.Price) * Collection[i].Quantity;
                        
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Quantity, Collection[i].Quantity, Collection[i].idOrder);
                    }
                    else if (Collection[i].Price != OrderCollectionSettingsOldItems[i].Price)
                    {
                        DataBaseRequestOrder.UpdateItemInTableOrder(FindByValueOrder.Price, Collection[i].Price, Collection[i].idOrder);
                    }
                }
            });

            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrderCollectionSettings.Clear();
            });
        }

    }
}
