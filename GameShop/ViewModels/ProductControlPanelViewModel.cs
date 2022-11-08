using GalaSoft.MvvmLight.Command;
using GameShop.Convert;
using GameShop.DataBase.DataBaseRequstInTable;
using GameShop.Enum;
using GameShop.Model;
using GameShop.ViewModels.InfoBars;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace GameShop.ViewModels
{
    public class ProductControlPanelViewModel : ObservableObject
    {
        public ObservableCollection<ModelUPGRADEProduct> ProductCollection = new ObservableCollection<ModelUPGRADEProduct>();

        public ObservableCollection<Category> CategoryCollection = new ObservableCollection<Category>();

        DataGrid DataGridCollectionProduct = new DataGrid();

        DataGrid DataGridCollectionCategory = new DataGrid();

        bool EditProduct;
        bool EditCategory;
        bool IsAddMode;

        bool IsVisibleProduct;

        public void ShowInfoBar(InfoBar bar)
        {
            TitleInfoBar = bar.Title;
            MessageInfoBar = bar.Message;
            SeverityInfoBar = bar.Severity;
            InfoBarIsOpen = bar.IsOpen;
        }

        private bool _InfoBarIsOpen;
        public bool InfoBarIsOpen
        {
            get => _InfoBarIsOpen;
            set => SetProperty(ref _InfoBarIsOpen, value);
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
            get => _TitleInfoBar;
            set => SetProperty(ref _TitleInfoBar, value);
        }
        private string _MessageInfoBar;
        public string MessageInfoBar
        {
            get => _MessageInfoBar;
            set => SetProperty(ref _MessageInfoBar, value);
        }

        private Visibility _VisibilityEditsMode;
        public Visibility VisibilityEditsMode
        {
            get => _VisibilityEditsMode;
            set => SetProperty(ref _VisibilityEditsMode, value);
        }
        private Visibility _VisibilityNoEditMode;
        public Visibility VisibilityNoEditMode
        {
            get => _VisibilityNoEditMode;
            set => SetProperty(ref _VisibilityNoEditMode, value);
        }

        private Visibility _VisibilityExitMode;
        public Visibility VisibilityExitMode
        {
            get => _VisibilityExitMode;
            set => SetProperty(ref _VisibilityExitMode, value);
        }

        private Visibility _VisibilityAddMode;
        public Visibility VisibilityAddMode
        {
            get => _VisibilityAddMode;
            set => SetProperty(ref _VisibilityAddMode, value);
        }

        private Visibility _PanelAddProduct;
        public Visibility PanelAddProduct
        {
            get => _PanelAddProduct;
            set => SetProperty(ref _PanelAddProduct, value);
        }
        public ProductControlPanelViewModel()
        {
            VisibilityEditsMode = Visibility.Collapsed;
            VisibilityAddMode = Visibility.Collapsed;
            VisibilityExitMode = Visibility.Collapsed;
            VisibilityNoEditMode = Visibility.Visible;
            PanelAddProduct = Visibility.Collapsed;

            InfoBarIsOpen = false;
        }

        private async void InitializationProductCollection()
        {
            ProductCollection.Clear();
            IsVisibleProduct = true;
            ObservableCollection<Product> product = new ObservableCollection<Product>();

            await Task.Factory.StartNew(() =>
            {
                foreach (var item in DataBaseRequstProduct.ReadingDataProduct())
                {
                    product.Add(item);
                }
            });

            foreach (var item in ConvertProductAndProductUPGRADE.ConvertFromProductCollectionInProductUPGRADECollection(product))
            {
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ProductCollection.Add(item);
                });
            }
        }
        private void InitializationCategoryCollection()
        {
            CategoryCollection.Clear();
            IsVisibleProduct = false;
            foreach (var item in DataBaseRequstCategory.ReadingDataCategory())
            {
                Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    CategoryCollection.Add(item);
                });
            }
        }
        public ICommand EditMode => new RelayCommand<DataGrid>(EditModeClick);
        private void EditModeClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;


            if (DataGridCollectionProduct.SelectedItem != null || DataGridCollectionCategory.SelectedItem != null)
            {
                ModelUPGRADEProduct tempProduct = (ModelUPGRADEProduct)DataGridCollectionProduct.SelectedItem;
                Category tempCategory = (Category)DataGridCollectionCategory.SelectedItem;

                if (DataGridCollectionProduct.SelectedItem != null)
                {
                    if (DataGridCollectionProduct.Columns.Count != 0)
                        DataGridCollectionProduct.Columns[0].Visibility = Visibility.Collapsed;
                    if (DataGridCollectionCategory.Columns.Count != 0)
                        DataGridCollectionCategory.Columns[0].Visibility = Visibility.Collapsed;

                    Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ProductCollection.Clear();
                        ProductCollection.Add(tempProduct);
                        DataGridCollectionProduct.Visibility = Visibility.Visible;
                        DataGridCollectionCategory.Visibility = Visibility.Collapsed;
                    });

                    EditProduct = true;
                    EditCategory = false;
                }
                else
                {
                    if (DataGridCollectionProduct.Columns.Count != 0)
                        DataGridCollectionProduct.Columns[0].Visibility = Visibility.Collapsed;
                    if (DataGridCollectionCategory.Columns.Count != 0)
                        DataGridCollectionCategory.Columns[0].Visibility = Visibility.Collapsed;

                    Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        CategoryCollection.Clear();
                        CategoryCollection.Add(tempCategory);
                        DataGridCollectionProduct.Visibility = Visibility.Collapsed;
                        DataGridCollectionCategory.Visibility = Visibility.Visible;
                    });

                    EditProduct = false;
                    EditCategory = true;
                }

                VisibilityEditsMode = Visibility.Visible;
                VisibilityNoEditMode = Visibility.Collapsed;
                VisibilityExitMode = Visibility.Visible;
                VisibilityAddMode = Visibility.Collapsed;

                ShowInfoBar(ControlPageInfoBar.Information("Режим редактирования включен", ""));
            }
            else
            {
                NoEditModeClick(obj);
                ShowInfoBar(ControlPageInfoBar.Error("Не выбран объект таблицы", ""));
            }
        }

        public ICommand AddMode => new RelayCommand<DataGrid>(AddModeClick);
        private void AddModeClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            IsAddMode = true;
            EditProduct = false;
            EditCategory = false;

            if (DataGridCollectionProduct != null)
                DataGridCollectionProduct.Visibility = Visibility.Collapsed;

            if (DataGridCollectionCategory != null)
                DataGridCollectionCategory.Visibility = Visibility.Collapsed;


            ShowInfoBar(ControlPageInfoBar.Information("Режим добавления включен", ""));

            VisibilityAddMode = Visibility.Visible;
            VisibilityNoEditMode = Visibility.Collapsed;
            VisibilityExitMode = Visibility.Visible;

        }
        
        public ICommand AddProduct => new RelayCommand<DataGrid>(AddProductClick);
        private void AddProductClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;

            PanelAddProduct = Visibility.Visible;
        }

        public ICommand NoEditMode => new RelayCommand<DataGrid>(NoEditModeClick);
        private void NoEditModeClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            VisibilityEditsMode = Visibility.Collapsed;
            VisibilityAddMode = Visibility.Collapsed;
            VisibilityNoEditMode = Visibility.Visible;
            VisibilityExitMode = Visibility.Collapsed;
            PanelAddProduct = Visibility.Collapsed;
            ShowInfoBar(ControlPageInfoBar.Information("Режим редактирования выключен", ""));
            if (EditProduct)
            {
                if (DataGridCollectionProduct.Columns.Count != 0)
                    DataGridCollectionProduct.Columns[0].Visibility = Visibility.Visible;
                InitializationProductCollection();
            }
            else if (EditCategory)
            {
                if (DataGridCollectionCategory.Columns.Count != 0)
                    DataGridCollectionCategory.Columns[0].Visibility = Visibility.Visible;
                InitializationCategoryCollection();
            }
            else if(IsAddMode)
            {
                if (IsVisibleProduct)
                {
                    if (DataGridCollectionProduct.Visibility == Visibility.Collapsed)
                        DataGridCollectionProduct.Visibility = Visibility.Visible;
                    InitializationProductCollection();
                }
                else
                {
                    if (DataGridCollectionCategory.Visibility == Visibility.Collapsed)
                        DataGridCollectionCategory.Visibility = Visibility.Visible;
                    InitializationCategoryCollection();
                }
            }
        }

        public ICommand VisibleProductCollection => new RelayCommand<DataGrid>(VisibleProductCollectionClick);
        private void VisibleProductCollectionClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            DataGridCollectionProduct.Visibility = Visibility.Visible;


            if (DataGridCollectionCategory != null)
                DataGridCollectionCategory.Visibility = Visibility.Collapsed;

            InitializationProductCollection();
        }

        public ICommand VisibleCategoryCollection => new RelayCommand<DataGrid>(VisibleCategoryCollectionClick);
        private void VisibleCategoryCollectionClick(DataGrid obj)
        {
            DataGridCollectionCategory = obj;
            DataGridCollectionCategory.Visibility = Visibility.Visible;

            if (DataGridCollectionCategory != null)
                DataGridCollectionProduct.Visibility = Visibility.Collapsed;

            InitializationCategoryCollection();
        }

        public ICommand RefreshTable => new RelayCommand<DataGrid>(RefreshTableClick);
        private async void RefreshTableClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;

            if (CategoryCollection.Count != 0 || ProductCollection.Count != 0)
            {
                if (VisibilityEditsMode != Visibility.Visible && VisibilityAddMode != Visibility.Visible)
                {
                    if (IsVisibleProduct)
                    {
                        InitializationProductCollection();
                    }
                    else
                    {
                        InitializationCategoryCollection();
                    }

                    ShowInfoBar(ControlPageInfoBar.Accept("Обновлено", ""));
                }
                else
                {

                    if (EditProduct)
                    {
                        var idPrimary = ProductCollection[0].idProduct;
                        ProductCollection.Clear();
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            foreach (var item in DataBaseRequstProduct.ReadingDataProduct(FindByValueProduct.idProduct, idPrimary))
                            {
                                ProductCollection.Add(ConvertProductAndProductUPGRADE.ConvertFromProductInProductUPGRADE(item));
                            }
                        });

                        ShowInfoBar(ControlPageInfoBar.Accept("Обновлено", ""));
                    }
                    else if (EditCategory)
                    {
                        await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            var idPrimary = CategoryCollection[0].idCategory;
                            CategoryCollection.Clear();
                            foreach (var item in DataBaseRequstCategory.ReadingDataCategory(FindByValueCategory.idCategory, idPrimary))
                            {
                                CategoryCollection.Add(item);
                            }
                        });

                        ShowInfoBar(ControlPageInfoBar.Accept("Обновлено", ""));
                    }
                    else
                    {
                        ShowInfoBar(ControlPageInfoBar.Warning("Невозможно обноавлять коллекцию в режиме добавления", ""));
                    }
                }
            }
            else
                ShowInfoBar(ControlPageInfoBar.Error("Все таблицы пусты", ""));

        }
    }
}
