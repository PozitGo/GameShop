﻿using GalaSoft.MvvmLight.Command;
using GameShop.Convert;
using GameShop.DataBase;
using GameShop.DataBase.DataBaseRequstInTable;
using GameShop.Enum;
using GameShop.Model;
using GameShop.ViewModels.InfoBars;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Org.BouncyCastle.Crypto.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GameShop.ViewModels
{
    public class ProductControlPanelViewModel : ObservableObject
    {
        public ObservableCollection<ModelUPGRADEProduct> ProductCollection = new ObservableCollection<ModelUPGRADEProduct>();

        public ObservableCollection<Category> CategoryCollection = new ObservableCollection<Category>();

        public ObservableCollection<string> NameCategory = new ObservableCollection<string>();

        public List<byte[]> EncryptedImages = new List<byte[]>();

        public ObservableCollection<BitmapImage> ImageAddProduct = new ObservableCollection<BitmapImage>();

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

        private Visibility _VisibilitySaveButton;
        public Visibility VisibilitySaveButton
        {
            get => _VisibilitySaveButton;
            set => SetProperty(ref _VisibilitySaveButton, value);
        }

        private string _AddTextBoxTextNameProduct;
        public string AddTextBoxTextNameProduct
        {
            get => _AddTextBoxTextNameProduct;
            set => SetProperty(ref _AddTextBoxTextNameProduct, value);
        }

        private string _AddTextBoxTextManufacturer;
        public string AddTextBoxTextManufacturer
        {
            get => _AddTextBoxTextManufacturer;
            set => SetProperty(ref _AddTextBoxTextManufacturer, value);
        }

        private string _AddTextBoxTextPrice;
        public string AddTextBoxTextPrice
        {
            get => _AddTextBoxTextPrice;
            set => SetProperty(ref _AddTextBoxTextPrice, value);
        }

        private string _AddComboBoxTextNameCategory;
        public string AddComboBoxTextNameCategory
        {
            get => _AddComboBoxTextNameCategory;
            set => SetProperty(ref _AddComboBoxTextNameCategory, value);
        }

        private string _AddTextBoxTextDescription;
        public string AddTextBoxTextDescription
        {
            get => _AddTextBoxTextDescription;
            set => SetProperty(ref _AddTextBoxTextDescription, value);
        }

        public ProductControlPanelViewModel()
        {
            VisibilityEditsMode = Visibility.Collapsed;
            VisibilityAddMode = Visibility.Collapsed;
            VisibilityExitMode = Visibility.Collapsed;
            VisibilityNoEditMode = Visibility.Visible;
            PanelAddProduct = Visibility.Collapsed;
            VisibilitySaveButton = Visibility.Collapsed;

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

        private async void InitializationNameCategoryCollection()
        {
            NameCategory.Clear();

            foreach (var item in UniversalRequst.ReadingAllToIdFromTableString("category", "NameCategory")) await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => NameCategory.Add(item));
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
                VisibilitySaveButton = Visibility.Visible;

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
            VisibilitySaveButton = Visibility.Visible;

        }

        public ICommand AddProduct => new RelayCommand<DataGrid>(AddProductClick);
        private void AddProductClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;

            InitializationNameCategoryCollection();

            PanelAddProduct = Visibility.Visible;
        }

        public ICommand AddPhotoInProduct => new RelayCommand<DataGrid>(AddPhotoInProductClick);
        private async void AddPhotoInProductClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            Image image = new Image();

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                byte[] fileBytes = null;

                using (var stream = await file.OpenReadAsync())
                {
                    fileBytes = new byte[stream.Size];
                    using (var reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(fileBytes);
                    }
                }
                
                EncryptedImages.Add(fileBytes);

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Clear());

                for (int i = 0; i < EncryptedImages.Count; i++) await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Add(ImageConverter.GetBitmapAsync(EncryptedImages[i])));
            }
    }

        public ICommand AddCategory => new RelayCommand<DataGrid>(AddCategoryClick);
        private void AddCategoryClick(DataGrid obj)
        {
            DataGridCollectionCategory = obj;
            CategoryCollection.Clear();
            CategoryCollection.Add(new Category());

            DataGridCollectionCategory.IsReadOnly = false;

            if (DataGridCollectionCategory.Columns.Count != 0)
                DataGridCollectionCategory.Columns[0].Visibility = Visibility.Collapsed;

            DataGridCollectionCategory.Visibility = Visibility.Visible;
        }

        public ICommand NoEditMode => new RelayCommand<DataGrid>(NoEditModeClick);
        private async void NoEditModeClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            VisibilityEditsMode = Visibility.Collapsed;
            VisibilityAddMode = Visibility.Collapsed;
            VisibilityNoEditMode = Visibility.Visible;
            VisibilityExitMode = Visibility.Collapsed;
            PanelAddProduct = Visibility.Collapsed;
            VisibilitySaveButton = Visibility.Collapsed;
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
            else if (IsAddMode)
            {
                if (IsVisibleProduct)
                {
                    if (DataGridCollectionProduct.Visibility == Visibility.Collapsed)
                        DataGridCollectionProduct.Visibility = Visibility.Visible;

                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Clear());
                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => EncryptedImages.Clear());
                    
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

        public ICommand SaveButton => new RelayCommand<DataGrid>(SaveButtonClick);
        private void SaveButtonClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;

            if(IsAddMode)
            {
                if (AddTextBoxTextNameProduct != null && AddTextBoxTextNameProduct != "" && AddTextBoxTextManufacturer != null && AddTextBoxTextManufacturer != "" && AddTextBoxTextPrice != null && AddTextBoxTextPrice != "" && AddTextBoxTextDescription != null && AddTextBoxTextDescription != "")
                {
                    if (int.Parse(AddTextBoxTextPrice) > 0)
                    {
                        bool ResultSavePhoto = true;
                        if (AddComboBoxTextNameCategory != null && AddComboBoxTextNameCategory != "")
                        {
                            Product product = new Product();
                            Task task = Task.Factory.StartNew(() =>
                            {
                                product.idProduct = DataBaseRequstProduct.MaxIdProduct() + 1;
                                product.Name = AddTextBoxTextNameProduct;
                                product.Price = double.Parse(AddTextBoxTextPrice);
                                product.Manufacturer = AddTextBoxTextManufacturer;
                                product.BasicDescription = AddTextBoxTextDescription;

                                for (int i = 0; i < NameCategory.Count; i++)
                                {
                                    if (NameCategory[0] == AddComboBoxTextNameCategory)
                                    {
                                        product.idCategory = i + 1;
                                        break;
                                    }

                                }

                                DataBaseRequstProduct.SaveNewItemProductByDB(product);
                                if (EncryptedImages.Count != 0)
                                    ResultSavePhoto = DataBasePhotoRequst.SavePhoto(EncryptedImages, product.idProduct);
                            });

                            task.Wait();

                            IsVisibleProduct = true;
                            NoEditModeClick(obj);

                            if (ResultSavePhoto)
                            {
                                ShowInfoBar(ControlPageInfoBar.Accept("Товар добавлен", ""));
                            }
                            else
                            {
                                ShowInfoBar(ControlPageInfoBar.Warning("Одна или несколько фотографий не были добавлены", "Размер бинарных данных больше 65кб"));
                            }
                        }
                        else
                        {
                            ShowInfoBar(ControlPageInfoBar.Error("Не выбрана категория", ""));
                        }
                    }
                    else
                    {
                        ShowInfoBar(ControlPageInfoBar.Error("Цена не может быть меньше 0", ""));
                    }
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Не все поля заполнены", ""));
                }
            }
            else if(EditProduct || EditCategory)
            {
                
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
