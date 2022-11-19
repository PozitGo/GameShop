using GalaSoft.MvvmLight.Command;
using GameShop.DataBase;
using GameShop.Enum;
using GameShop.Model;
using GameShop.Services;
using GameShop.ViewModels.InfoBars;
using GameShop.Views;
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
    public class ControlPanelStaffViewModel : ObservableObject
    {
        public ObservableCollection<UserAccount> UserCollection = new ObservableCollection<UserAccount>();

        UserAccount UserSettings;

        DataGrid DataGridUser = new DataGrid();

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

        public Visibility _VisibilityAppBarEditingMode;
        public Visibility VisibilityAppBarEditingMode
        {
            get => _VisibilityAppBarEditingMode;
            set => SetProperty(ref _VisibilityAppBarEditingMode, value);
        }

        public Visibility _VisibilityAppBarNoEditingMode;
        public Visibility VisibilityAppBarNoEditingMode
        {
            get => _VisibilityAppBarNoEditingMode;
            set => SetProperty(ref _VisibilityAppBarNoEditingMode, value);
        }

        bool isVisibleAllUser;
        public ControlPanelStaffViewModel()
        {
            VisibilityAppBarNoEditingMode = Visibility.Visible;
            VisibilityAppBarEditingMode = Visibility.Collapsed;
        }

        public void ShowInfoBar(InfoBar bar)
        {
            TitleInfoBar = bar.Title;
            MessageInfoBar = bar.Message;
            SeverityInfoBar = bar.Severity;
            InfoBarIsOpen = bar.IsOpen;
        }

        public ICommand UpStatusUser => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(UpStatusUserMethod);
        public ICommand DownStatusUser => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(DownStatusUserMethod);
        public ICommand DisplayAllUsers => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(DisplayAllUsersMethod);
        public ICommand DisplayStaffUsers => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(DisplayStaffUsersMethod);
        public ICommand EditUser => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(EditUserMethod);
        public ICommand DeleteUser => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(DeleteUserMethod);
        public ICommand EditingMode => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(EditingModeMethod);
        public ICommand SaveChanges => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(SaveChangesMethod);
        public ICommand NoEditingMode => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(NoEditingModeMethod);
        public ICommand RefreshTable => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(RefreshTableClick);

        public ICommand NavigateToOrder => new RelayCommand(NavigateToOrderClick);

        private void NavigateToOrderClick() => NavigationService.Navigate(typeof(ControlPanelPage));

        public ICommand NavigateToProductPage => new RelayCommand(NavigateToProductPageClick);

        private void NavigateToProductPageClick() => NavigationService.Navigate(typeof(ProductControlPanelPage));
        private void UpStatusUserMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if (DataGridUser.SelectedItem != null)
            {
                int Index = DataGridUser.SelectedIndex;
                UserAccount tempUser = (UserAccount)DataGridUser.SelectedItem;

                if (tempUser.Status != Status.Admin)
                {
                    UserCollection.Remove((UserAccount)DataGridUser.SelectedItem);
                    tempUser.Status++;
                    Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Status, tempUser.Status, tempUser.idUser));

                    UserCollection.Insert(Index, new UserAccount(tempUser.idUser, tempUser.Login, tempUser.PhoneNumber, tempUser.Name, tempUser.Surname, tempUser.Email, tempUser.PathAvatar, tempUser.Status));

                    ShowInfoBar(ControlPageInfoBar.Accept($"Статус {tempUser.Login} успешно повышен", ""));
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error($"У {tempUser.Login} максимальный статус", "Повышение невозможно"));
                }
            }
            else
            {
                ShowInfoBar(ControlPageInfoBar.Error("Не выбран пользователь", "Выберете пользователя для повышения и попробуйте снова"));
            }

        }

        private bool IsVisibleStaffCollection;

        private bool isDelete;
        private bool isEdit;

        private void DownStatusUserMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if (DataGridUser.SelectedItem != null)
            {
                int Index = DataGridUser.SelectedIndex;
                UserAccount tempUser = (UserAccount)DataGridUser.SelectedItem;

                if (tempUser.Status != Status.User)
                {
                    UserCollection.Remove((UserAccount)DataGridUser.SelectedItem);
                    tempUser.Status--;
                    Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Status, tempUser.Status, tempUser.idUser));

                    UserCollection.Insert(Index, new UserAccount(tempUser.idUser, tempUser.Login, tempUser.PhoneNumber, tempUser.Name, tempUser.Surname, tempUser.Email, tempUser.PathAvatar, tempUser.Status));

                    if (IsVisibleStaffCollection && tempUser.Status == Status.User)
                    {
                        UserCollection.Remove((UserAccount)DataGridUser.SelectedItem);
                    }

                    ShowInfoBar(ControlPageInfoBar.Accept($"Статус {tempUser.Login} успешно понижен", ""));
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error($"У {tempUser.Login} минимальный статус", "Понижение невозможно"));
                }
            }
            else
            {
                ShowInfoBar(ControlPageInfoBar.Error("Не выбран пользователь", "Выберете пользователя для понижения и попробуйте снова"));
            }
        }

        private void DisplayAllUsersMethod(DataGrid obj)
        {
            DataGridUser = obj;
            isVisibleAllUser = true;
            InitializationAllUserCollection();
        }

        private void DisplayStaffUsersMethod(DataGrid obj)
        {
            DataGridUser = obj;
            isVisibleAllUser = false;
            InitializationStaffUserCollection();
        }

        private void EditingModeMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if (DataGridUser.SelectedItem != null)
            {
                UserAccount tempUser = (UserAccount)DataGridUser.SelectedItem;

                DataGridUser.Columns[6].Visibility = Visibility.Collapsed;
                Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    UserCollection.Clear();
                    UserCollection.Add(tempUser);
                });

                VisibilityAppBarNoEditingMode = Visibility.Collapsed;
                VisibilityAppBarEditingMode = Visibility.Visible;

                ShowInfoBar(ControlPageInfoBar.Information("Режим редактирования включен", ""));
            }
            else
            {
                ShowInfoBar(ControlPageInfoBar.Error("Не выбран пользователь", "Выберете пользователя для редактирования и попробуйте снова"));
            }
        }

        private void EditUserMethod(DataGrid obj)
        {
            DataGridUser = obj;
            isEdit = true;
            isDelete = false;

            DataGridUser.IsReadOnly = false;
            DataGridUser.Columns[0].Visibility = Visibility.Collapsed;
            DataGridUser.Columns[6].Visibility = Visibility.Collapsed;

            UserSettings = new UserAccount(UserCollection[0].idUser, UserCollection[0].Login, UserCollection[0].PhoneNumber, UserCollection[0].Name, UserCollection[0].Surname, UserCollection[0].Email, UserCollection[0].PathAvatar, UserCollection[0].Status);
        }

        private ReturnResultEditUser EditingUser()
        {
            int CountEditDate = 0;

            if (UserCollection[0].Login != UserSettings.Login)
            {
                if (!DataBaseAuthorization.CheckSpace(UserCollection[0].Login) && UserCollection[0].Login != null && UserCollection[0].Login != "")
                {
                    if (!DataBaseAuthorization.LoginCheck(UserCollection[0].Login))
                    {
                        UserSettings.Login = UserCollection[0].Login;

                        Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Login, UserSettings.Login, UserSettings.idUser));
                        CountEditDate++;
                    }
                    else
                    {
                        ShowInfoBar(ControlPageInfoBar.Error($"Логин {UserCollection[0].Login} занят", "Введите другой и повторите попытку"));
                        return ReturnResultEditUser.DataRegistered;
                    }
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Некорректно введены данные", "Повторите попытку"));
                    return ReturnResultEditUser.IncorrectData;
                }
            }

            if (UserCollection[0].Name != UserSettings.Name)
            {
                if (!DataBaseAuthorization.CheckSpace(UserCollection[0].Name) && UserCollection[0].Name != null && UserCollection[0].Name != "")
                {
                    UserSettings.Name = UserCollection[0].Name;

                    Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Name, UserSettings.Name, UserSettings.idUser));
                    CountEditDate++;
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Некорректно введены данные", "Повторите попытку"));
                    return ReturnResultEditUser.IncorrectData;
                }
            }

            if (UserCollection[0].Surname != UserSettings.Surname)
            {
                if (!DataBaseAuthorization.CheckSpace(UserCollection[0].Surname) && UserCollection[0].Surname != null && UserCollection[0].Surname != "")
                {
                    UserSettings.Surname = UserCollection[0].Surname;

                    Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Surname, UserSettings.Surname, UserSettings.idUser));
                    CountEditDate++;
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Некорректно введены данные", "Повторите попытку"));
                    return ReturnResultEditUser.IncorrectData;
                }
            }

            if (UserCollection[0].PhoneNumber != UserSettings.PhoneNumber)
            {
                if (!DataBaseAuthorization.CheckSpace(UserCollection[0].PhoneNumber) && UserCollection[0].PhoneNumber != null && UserCollection[0].PhoneNumber != "")
                {
                    if (!DataBaseAuthorization.PhoneNumberCheck(UserCollection[0].PhoneNumber))
                    {
                        UserSettings.PhoneNumber = UserCollection[0].PhoneNumber;

                        Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.PhoneNumber, UserSettings.PhoneNumber, UserSettings.idUser));
                        CountEditDate++;
                    }
                    else
                    {
                        ShowInfoBar(ControlPageInfoBar.Error($"Номер телефона {UserCollection[0].PhoneNumber} занят", "Введите другой и повторите попытку"));
                        return ReturnResultEditUser.DataRegistered;
                    }
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Некорректно введены данные", "Повторите попытку"));
                    return ReturnResultEditUser.IncorrectData;
                }
            }

            if (UserCollection[0].Email != UserSettings.Email)
            {
                if (!DataBaseAuthorization.CheckSpace(UserCollection[0].Email) && UserCollection[0].Email != null && UserCollection[0].Email != "")
                {
                    if (!DataBaseAuthorization.EmailCheck(UserCollection[0].Email))
                    {
                        UserSettings.Email = UserCollection[0].Email;

                        Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Email, UserSettings.Email, UserSettings.idUser));
                        CountEditDate++;
                    }
                    else
                    {
                        ShowInfoBar(ControlPageInfoBar.Error($"E-mail {UserCollection[0].Email} занят", "Введите другой и повторите попытку"));
                        return ReturnResultEditUser.DataRegistered;
                    }
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("Некорректно введены данные", "Повторите попытку"));
                    return ReturnResultEditUser.IncorrectData;
                }
            }

            if (CountEditDate == 0)
            {
                ShowInfoBar(ControlPageInfoBar.Warning("Данные остались в прежнем виде", "По этому не были изменены"));
                return ReturnResultEditUser.NoChange;
            }

            ShowInfoBar(ControlPageInfoBar.Accept("Данные успешно изменены", ""));
            return ReturnResultEditUser.Success;
        }
        private void DeleteUserMethod(DataGrid obj)
        {
            DataGridUser = obj;
            isEdit = false;
            isDelete = true;
        }

        private async void SaveChangesMethod(DataGrid obj)
        {
            DataGridUser = obj;
            
            if(isEdit || isDelete)
            {
                DataGridUser.IsReadOnly = true;
                DataGridUser.Columns[0].Visibility = Visibility.Visible;

                if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                    DataGridUser.Columns[6].Visibility = Visibility.Visible;

                if (isEdit)
                {
                    ReturnResultEditUser Editing = EditingUser();

                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        UserCollection.Remove((UserAccount)DataGridUser.SelectedItem);
                        UserCollection.Add(UserSettings);
                    });

                    isEdit = false;
                }
                else if (isDelete)
                {
                    DataBaseRequstUser.DeleteUser(UserCollection[0].idUser);
                    ShowInfoBar(ControlPageInfoBar.Accept($"Пользователь {UserCollection[0].Login} успешно удалён.", ""));
                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        UserCollection.Remove(UserCollection[0]);
                    });
                    
                    isDelete = false;
                }
            }
            else
            {
                ShowInfoBar(ControlPageInfoBar.Warning("Вы не выбрали ни одно действие", ""));
            }
        }

        private void NoEditingModeMethod(DataGrid obj)
        {
            DataGridUser = obj;

            DataGridUser.Columns[0].Visibility = Visibility.Visible;
            DataGridUser.IsReadOnly = true;

            UserCollection.Clear();

            if (isVisibleAllUser)
            {
                InitializationAllUserCollection();
            }
            else
            {
                InitializationStaffUserCollection();
            }

            VisibilityAppBarNoEditingMode = Visibility.Visible;
            VisibilityAppBarEditingMode = Visibility.Collapsed;

            ShowInfoBar(ControlPageInfoBar.Information("Режим редактирования выключен", ""));
        }

        private async void RefreshTableClick(DataGrid obj)
        {
            DataGridUser = obj;

            if (VisibilityAppBarEditingMode != Visibility.Visible)
            {
                UserCollection.Clear();

                if (isVisibleAllUser)
                {
                    InitializationAllUserCollection();
                }
                else
                {
                    InitializationStaffUserCollection();
                }

                if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                    DataGridUser.Columns[6].Visibility = Visibility.Visible;
            }
            else
            {
                var idUser = UserCollection[0].idUser;
                await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    UserCollection.Clear();
                    foreach (var item in DataBaseRequstUser.ReadingDataUser(FindByValueUser.idUser, idUser))
                    {
                        UserCollection.Add(item);
                    }
                });
            }

            ShowInfoBar(ControlPageInfoBar.Accept("Обновлено", ""));
        }

        public void InitializationAllUserCollection()
        {
            IsVisibleStaffCollection = false;

            DataGridUser.Visibility = Visibility.Visible;
            DataGridUser.UnloadingRowDetails += DataGridUser_UnloadingRowDetails;

            if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                DataGridUser.Columns[6].Visibility = Visibility.Visible;

            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UserCollection.Clear();


                foreach (var item in DataBaseRequstUser.ReadingDataUser(FindByValueUser.Status, Status.Admin))
                {
                    UserCollection.Add(item);
                }

                foreach (var item in DataBaseRequstUser.ReadingDataUser(FindByValueUser.Status, Status.Personal))
                {
                    UserCollection.Add(item);
                }

                foreach (var item in DataBaseRequstUser.ReadingDataUser(FindByValueUser.Status, Status.User))
                {
                    UserCollection.Add(item);
                }
            });
        }
        public void InitializationStaffUserCollection()
        {
            IsVisibleStaffCollection = true;

            DataGridUser.Visibility = Visibility.Visible;
            DataGridUser.UnloadingRowDetails += DataGridUser_UnloadingRowDetails;

            if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                DataGridUser.Columns[6].Visibility = Visibility.Visible;

            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UserCollection.Clear();

                foreach (var item in DataBaseRequstUser.ReadingDataUser(FindByValueUser.Status, Status.Admin))
                {
                    UserCollection.Add(item);
                }

                foreach (var item in DataBaseRequstUser.ReadingDataUser(FindByValueUser.Status, Status.Personal))
                {
                    UserCollection.Add(item);
                }
            });
        }
        private void DataGridUser_UnloadingRowDetails(object sender, DataGridRowDetailsEventArgs e) => e.Row.DetailsVisibility = Visibility.Collapsed;
    }
}
