using GameShop.DataBase;
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
using Windows.UI.Xaml.Controls;

namespace GameShop.ViewModels
{
    public class ControlPanelStaffViewModel : ObservableObject
    {
        public ObservableCollection<UserAccount> UserCollection = new ObservableCollection<UserAccount>();

        ObservableCollection<UserAccount> UserSettings = new ObservableCollection<UserAccount>();

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
        public ICommand RefreshTable => new Microsoft.Toolkit.Mvvm.Input.RelayCommand<DataGrid>(RefreshTableMethod);

        //Кнопку понижения и повышения статусов сделать видной только админам
        private async void UpStatusUserMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if (DataGridUser.SelectedItem != null)
            {
                UserAccount tempUser = (UserAccount)DataGridUser.SelectedItem;
                DataGridUser.ReleasePointerCaptures();
                DataGridUser.RemoveFocusEngagement();
                if (tempUser.Status != Status.Admin)
                {
                    if (tempUser.Status == Status.User)
                        tempUser.Status = Status.Personal;
                    else if (tempUser.Status == Status.Personal)
                        tempUser.Status = Status.Admin;

                    await Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Status, tempUser.Status, tempUser.idUser));

                    for (int i = 0; i < UserCollection.Count; i++)
                    {
                        if (tempUser.idUser == UserCollection[i].idUser)
                        {   
                            UserCollection.Insert(i, tempUser);
                            UserCollection.RemoveAt(i + 1);
                            break;
                        }
                    }
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("У пользователя максимальный статус", "Повышение невозможно"));
                }
            }
            else
            {
               ShowInfoBar(ControlPageInfoBar.Error("Не выбран пользователь", "Выберете пользователя для повышения и попробуйте снова"));
            }

        }

        private async void DownStatusUserMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if(DataGridUser.SelectedItem != null)
            {
                UserAccount tempUser = (UserAccount)DataGridUser.SelectedItem;

                if (tempUser.Status != Status.User)
                {
                    if (tempUser.Status == Status.Personal)
                        tempUser.Status = Status.User;
                    else if (tempUser.Status == Status.Admin)
                        tempUser.Status = Status.Personal;

                    await Task.Factory.StartNew(() => DataBaseRequstUser.UpdateItemInTableUser(FindByValueUser.Status, tempUser.Status, tempUser.idUser));

                    for (int i = 0; i < UserCollection.Count; i++)
                    {
                        if (UserCollection[i].idUser == tempUser.idUser)
                        {
                            UserCollection.Insert(i, tempUser);
                            UserCollection.RemoveAt(i + 1);
                            break;
                        }
                    }
                }
                else
                {
                    ShowInfoBar(ControlPageInfoBar.Error("У пользователя минимальный статус", "Понижение невозможно"));
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

            bool IsEditing = true;

            UserAccount tempUser = null;

            DataGridUser.IsReadOnly = false;

            try
            {
                tempUser = (UserAccount)DataGridUser.SelectedItem;
            }
            catch
            {
                IsEditing = false;
                ShowInfoBar(ControlPageInfoBar.Error("Не выбран пользователь", "Выберете пользователя для редактирования и попробуйте снова"));
            }

            if (IsEditing)
            {
                DataGridUser.Columns[6].Visibility = Visibility.Collapsed;
                Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    UserCollection.Clear();
                    UserCollection.Add(tempUser);
                });

                VisibilityAppBarNoEditingMode = Visibility.Collapsed;
                VisibilityAppBarEditingMode = Visibility.Visible;
            }
        }

        private void EditUserMethod(DataGrid obj)
        {
            DataGridUser = obj;
            throw new NotImplementedException();
        }

        private void DeleteUserMethod(DataGrid obj)
        {
            DataGridUser = obj;
            throw new NotImplementedException();
        }

        private void SaveChangesMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                DataGridUser.Columns[6].Visibility = Visibility.Visible;
        }

        private void NoEditingModeMethod(DataGrid obj)
        {
            DataGridUser = obj;

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
        }

        private void RefreshTableMethod(DataGrid obj)
        {
            DataGridUser = obj;

            if (isVisibleAllUser)
                InitializationAllUserCollection();
            else
                InitializationStaffUserCollection();

            if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                DataGridUser.Columns[6].Visibility = Visibility.Visible;
        }

        public void InitializationAllUserCollection()
        {
            DataGridUser.Visibility = Visibility.Visible;
            DataGridUser.UnloadingRowDetails += DataGridUser_UnloadingRowDetails;

            if (DataGridUser.Columns[6].Visibility == Visibility.Collapsed)
                DataGridUser.Columns[6].Visibility = Visibility.Visible;

            Windows.Foundation.IAsyncAction asyncAction = Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UserCollection.Clear();

                foreach (var item in DataBaseRequstUser.ReadingDataUser())
                {
                    UserCollection.Add(item);
                }
            });
        }
        public void InitializationStaffUserCollection()
        {
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
