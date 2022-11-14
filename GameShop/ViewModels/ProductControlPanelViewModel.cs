using GalaSoft.MvvmLight.Command;
using GameShop.Convert;
using GameShop.DataBase;
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
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameShop.ViewModels
{
    public class ProductControlPanelViewModel : ObservableObject
    {
        public ObservableCollection<ModelUPGRADEProduct> ProductCollection = new ObservableCollection<ModelUPGRADEProduct>();

        public ObservableCollection<Category> CategoryCollection = new ObservableCollection<Category>();

        public ObservableCollection<string> NameCategory = new ObservableCollection<string>();

        public ObservableCollection<ImageAddProduct> ImageAddProduct = new ObservableCollection<ImageAddProduct>();

        DataGrid DataGridCollectionProduct = new DataGrid();

        DataGrid DataGridCollectionCategory = new DataGrid();

        bool EditProduct;
        bool EditCategory;
        bool IsAddMode;
        bool isAddCategory;
        bool isAddProduct;

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

        public string _PlaceholderTextComboBoxCategory;
        public string PlaceholderTextComboBoxCategory
        {
            get => _PlaceholderTextComboBoxCategory;
            set => SetProperty(ref _PlaceholderTextComboBoxCategory, value);
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

        public async void DefaultPhoto()
        {
            Windows.Storage.StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));
            ;
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
            byte[] temp = new byte[1] { 1 };
            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Add(new ImageAddProduct(temp, ImageConverter.GetBitmapAsync(fileBytes))));
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
                    DefaultPhoto();

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

        public ICommand EditSelectedItem => new RelayCommand<DataGrid>(EditSelectedItemButtonClick);

        static ModelUPGRADEProduct OldItemProduct = null;
        static Category OldItemCategory = null;
        public static void UpdateItemProduct(ModelUPGRADEProduct OldItem, ModelUPGRADEProduct NewItem)
        {
            if (OldItem.Name != NewItem.Name)
            {
                Task.Factory.StartNew(() => DataBaseRequstProduct.UpdateItemInTableProduct(FindByValueProduct.Name, NewItem.Name, NewItem.idProduct));
            }

            if (OldItem.Price != NewItem.Price)
            {
                Task.Factory.StartNew(() => DataBaseRequstProduct.UpdateItemInTableProduct(FindByValueProduct.Price, NewItem.Price, NewItem.idProduct));
            }

            if (OldItem.idCategory != NewItem.idCategory)
            {
                Task.Factory.StartNew(() => DataBaseRequstProduct.UpdateItemInTableProduct(FindByValueProduct.idCategory, NewItem.idCategory, NewItem.idProduct));
            }

            if (OldItem.BasicDescription != NewItem.BasicDescription)
            {
                Task.Factory.StartNew(() => DataBaseRequstProduct.UpdateItemInTableProduct(FindByValueProduct.BasicDescription, NewItem.BasicDescription, NewItem.idProduct));
            }

            if (OldItem.Manufacturer != NewItem.Manufacturer)
            {
                Task.Factory.StartNew(() => DataBaseRequstProduct.UpdateItemInTableProduct(FindByValueProduct.Manufacturer, NewItem.Manufacturer, NewItem.idProduct));
            }

        }

        public static void UpdateItemCategory(Category OldItem, Category NewItem)
        {
            if (OldItem.NameCategory != NewItem.NameCategory)
            {
                Task.Factory.StartNew(() => DataBaseRequstCategory.UpdateItemInTableCategory(FindByValueCategory.NameCategory, NewItem.NameCategory, NewItem.idCategory));
            }

            if (OldItem.Description != NewItem.Description)
            {
                Task.Factory.StartNew(() => DataBaseRequstCategory.UpdateItemInTableCategory(FindByValueCategory.Description, NewItem.Description, NewItem.idCategory));
            }
        }
        private async void EditSelectedItemButtonClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;


            if (EditProduct || EditCategory)
            {
                if (EditProduct)
                {
                    PanelAddProduct = Visibility.Visible;
                    DataGridCollectionProduct.Visibility = Visibility.Collapsed;
                    DataGridCollectionCategory.Visibility = Visibility.Collapsed;

                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Clear());
                    InitializationNameCategoryCollection();

                    OldItemProduct = new ModelUPGRADEProduct(ProductCollection[0]);

                    AddTextBoxTextNameProduct = ProductCollection[0].Name;
                    AddTextBoxTextManufacturer = ProductCollection[0].Manufacturer;
                    AddTextBoxTextPrice = ProductCollection[0].Price.ToString();
                    PlaceholderTextComboBoxCategory = ProductCollection[0].NameCategory;
                    AddTextBoxTextDescription = ProductCollection[0].BasicDescription;

                    if (ProductCollection[0].PhotoProducts == null)
                    {
                        DefaultPhoto();
                    }
                    else
                    {
                        for (int i = 0; i < ProductCollection[0].PhotoProducts.Count; i++)
                        {
                            for (int j = 0; j < ProductCollection[0].PhotoProducts[i].BitPhotoProducts.Count; j++)
                            {
                                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Add(new ImageAddProduct(null, ProductCollection[0].PhotoProducts[i].BitPhotoProducts[i])));
                            }
                        }
                    }
                }
                else if (EditCategory)
                {
                    if (DataGridCollectionCategory.Columns.Count != 0)
                        DataGridCollectionCategory.Columns[0].Visibility = Visibility.Collapsed;

                    DataGridCollectionCategory.IsReadOnly = false;

                    OldItemCategory = new Category(CategoryCollection[0].idCategory, CategoryCollection[0].NameCategory, CategoryCollection[0].Description);
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

        public ICommand AddProduct => new RelayCommand<DataGrid>(AddProductClickAsync);
        private void AddProductClickAsync(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            isAddProduct = true;
            isAddCategory = false;

            InitializationNameCategoryCollection();

            DefaultPhoto();

            PanelAddProduct = Visibility.Visible;
        }

        public ICommand AddPhotoInProduct => new RelayCommand<DataGrid>(AddPhotoInProductClick);
        private async void AddPhotoInProductClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            byte[] fileBytes = null;
            if (file != null)
            {
                using (var stream = await file.OpenReadAsync())
                {
                    fileBytes = new byte[stream.Size];
                    using (var reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(fileBytes);
                    }
                }

                if (ImageAddProduct.Count != 0)
                    if (ImageAddProduct[0].ByteImage != null)
                        if (ImageAddProduct.Count == 1 && ImageAddProduct[0].ByteImage[0] == 1)
                        {
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Clear());
                        }

                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Add(new ImageAddProduct(fileBytes, ImageConverter.GetBitmapAsync(fileBytes))));
            }
        }

        public ICommand DeletePhotoInProduct => new RelayCommand<DataGrid>(DeletePhotoInProductClick);
        private async void DeletePhotoInProductClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;
            Image image = new Image();

            bool isClosed = false;
            TextBox box = new TextBox();
            box.Header = "Введите номер фотографии";
            box.Visibility = Visibility.Visible;
            int NumberPhoto = 0;
            ContentDialog DeletePhoto = new ContentDialog()
            {
                Title = "Удаление фотографии",
                CloseButtonText = "Отмена",
                CloseButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(() => { isClosed = true; }),
                PrimaryButtonText = "Удалить",

                PrimaryButtonCommand = new Microsoft.Toolkit.Mvvm.Input.RelayCommand(async () =>
                {
                    int.TryParse(box.Text, out NumberPhoto);

                    if (NumberPhoto != 0 && !isClosed && NumberPhoto <= ImageAddProduct.Count)
                    {

                        if (ImageAddProduct[NumberPhoto - 1].ByteImage == null)
                        {
                            await Task.Factory.StartNew(() =>
                            {
                                int idPhoto = ProductCollection[0].PhotoProducts[NumberPhoto - 1].idPhoto;
                                DataBasePhotoRequst.DeletePhoto(idPhoto);
                            });

                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ProductCollection[0].PhotoProducts.RemoveAt(NumberPhoto - 1));

                            if (ProductCollection[0].PhotoProducts.Count <= 0)
                            {
                                DefaultPhoto();
                            }

                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.RemoveAt(NumberPhoto - 1));
                        }
                        else
                        {
                            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.RemoveAt(NumberPhoto - 1));

                            try
                            {
                                if (ProductCollection[0].PhotoProducts.Count <= 0)
                                {
                                    DefaultPhoto();
                                }
                            }
                            catch (Exception)
                            {
                                if (ImageAddProduct.Count == 0)
                                    DefaultPhoto();
                            }
                        }

                    }
                    else
                        ShowInfoBar(ControlPageInfoBar.Error("Номер фотографии введён некорректно", ""));
                }),
            };

            DeletePhoto.Content = box;
            await DeletePhoto.ShowAsync();
        }

        public ICommand DeleteItemButton => new RelayCommand<DataGrid>(DeleteItemButtonClick);
        private void DeleteItemButtonClick(DataGrid obj)
        {
            DataGridCollectionProduct = obj;

            if (EditProduct)
            {
                Task.Factory.StartNew(() => DataBaseRequstProduct.DeleteProduct(ProductCollection[0].idProduct));

                DataGridCollectionProduct.Visibility = Visibility.Visible;
                if (DataGridCollectionCategory != null)
                    DataGridCollectionCategory.Visibility = Visibility.Collapsed;
            }
            else
            {
                Task.Factory.StartNew(() => DataBaseRequstCategory.DeleteCategory(CategoryCollection[0].idCategory));

                DataGridCollectionCategory.Visibility = Visibility.Visible;
                DataGridCollectionProduct.Visibility = Visibility.Collapsed;
            }


            ShowInfoBar(ControlPageInfoBar.Error("Удалено", ""));
            NoEditModeClick(obj);

        }


        public ICommand AddCategory => new RelayCommand<DataGrid>(AddCategoryClick);
        private void AddCategoryClick(DataGrid obj)
        {
            DataGridCollectionCategory = obj;
            CategoryCollection.Clear();
            CategoryCollection.Add(new Category());
            isAddCategory = true;
            isAddProduct = false;

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
                if (isAddProduct)
                {
                    if (DataGridCollectionProduct.Visibility == Visibility.Collapsed)
                        DataGridCollectionProduct.Visibility = Visibility.Visible;

                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ImageAddProduct.Clear());

                    isAddProduct = false;
                    IsAddMode = false;
                    InitializationProductCollection();
                }
                else
                {
                    isAddCategory = false;
                    IsAddMode = false;
                    DataGridCollectionCategory.IsReadOnly = true;
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

            if (IsAddMode)
            {
                if (isAddProduct)
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
                                        if (NameCategory[i] == AddComboBoxTextNameCategory)
                                        {
                                            product.idCategory = i + 1;
                                            break;
                                        }

                                    }

                                    DataBaseRequstProduct.SaveNewItemProductByDB(product);
                                    if (ImageAddProduct.Count != 0)
                                    {
                                        for (int i = 0; i < ImageAddProduct.Count; i++)
                                        {
                                            ResultSavePhoto = DataBasePhotoRequst.SavePhoto(ImageAddProduct[i].ByteImage, product.idProduct);
                                        }
                                    }

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
                else if (isAddCategory)
                {
                    if (CategoryCollection[0].NameCategory != null && CategoryCollection[0].NameCategory != "" && CategoryCollection[0].Description != null && CategoryCollection[0].Description != "")
                    {
                        CategoryCollection[0].idCategory = DataBaseRequstCategory.MaxIdCategory() + 1;

                        DataBaseRequstCategory.SaveNewItemCategoryByDB(CategoryCollection[0]);
                        NoEditModeClick(obj);
                        ShowInfoBar(ControlPageInfoBar.Accept("Категория товара добавлена", ""));

                        if (DataGridCollectionCategory.Columns.Count != 0 && DataGridCollectionCategory.Columns[0].Visibility == Visibility.Collapsed)
                            DataGridCollectionCategory.Columns[0].Visibility = Visibility.Visible;
                    }
                    else
                        ShowInfoBar(ControlPageInfoBar.Error("Не все поля заполнены", ""));
                }

            }
            else if (EditProduct || EditCategory)
            {
                if (EditProduct)
                {
                    for (int i = 0; i < ImageAddProduct.Count; i++)
                    {
                        if (ImageAddProduct[i].ByteImage != null)
                        {
                            DataBasePhotoRequst.SavePhoto(ImageAddProduct[i].ByteImage, ProductCollection[0].idProduct);
                        }
                    }

                    ProductCollection[0].Name = AddTextBoxTextNameProduct;
                    ProductCollection[0].Manufacturer = AddTextBoxTextManufacturer;
                    ProductCollection[0].Price = double.Parse(AddTextBoxTextPrice);

                    if (AddComboBoxTextNameCategory != null)
                    {
                        for (int i = 0; i < NameCategory.Count; i++)
                        {
                            if (NameCategory[i] == AddComboBoxTextNameCategory)
                            {
                                ProductCollection[0].idCategory = i + 1;
                            }
                        }
                    }

                    ProductCollection[0].BasicDescription = AddTextBoxTextDescription;

                    Task task = Task.Factory.StartNew(() => UpdateItemProduct(OldItemProduct, ProductCollection[0]));

                    DataGridCollectionProduct.Visibility = Visibility.Visible;

                    if (DataGridCollectionCategory != null)
                        DataGridCollectionCategory.Visibility = Visibility.Collapsed;

                    task.Wait();
                    NoEditModeClick(obj);

                    ShowInfoBar(ControlPageInfoBar.Accept("Сохранено", ""));
                }
                else
                {
                    Task task = Task.Factory.StartNew(() => UpdateItemCategory(OldItemCategory, CategoryCollection[0]));

                    DataGridCollectionProduct.Visibility = Visibility.Collapsed;

                    if (DataGridCollectionCategory != null)
                        DataGridCollectionCategory.Visibility = Visibility.Visible;

                    DataGridCollectionCategory.IsReadOnly = true;
                    task.Wait();
                    NoEditModeClick(obj);

                    ShowInfoBar(ControlPageInfoBar.Accept("Сохранено", ""));
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
