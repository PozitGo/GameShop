using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GameShop.DataBase;
using GameShop.Enum;
using GameShop.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Relational;
using Windows.UI.Xaml.Media.Animation;
using static GameShop.DataBase.DataBaseRequestOrder;

namespace GameShop.ViewModels
{
    public class ControlPanelViewModel : ObservableObject
    {
        public ObservableCollection<Order> OrderCollection = new ObservableCollection<Order>();

        public ObservableCollection<Order> OrderCollectionSettings = new ObservableCollection<Order>();

        public ObservableCollection<Order> CheckCollection = new ObservableCollection<Order>();

        public ObservableCollection<Order> CheckCollectionSettings = new ObservableCollection<Order>();

        public ReadingDataOrderInCollection ReadingDataCheckInCollectionHandeler { get; set; }

        public ReadingDataOrderInCollection ReadingDataOrderInCollectionHandeler { get; set; }

        public SaveNewItemOrderByDBDelegate SaveNewItemOrderByDBHandler { get; set; }
        
        public SaveNewItemOrderByDBDelegate SaveNewItemCheckByDBHandler { get; set; }

        private Task InitializationCollectionOrder()
        {
            OrderCollection.Clear();

            ReadingDataOrderInCollectionHandeler = new ReadingDataOrderInCollection(ReadingDataOrder);

            return Task.Factory.StartNew(() =>
            {
                foreach (var item in ReadingDataOrderInCollectionHandeler())
                {
                    OrderCollection.Add(item);
                }
            });
        }
        
        private Task<bool> AddNewItemInCollectionOrder(Order item)
        {
            SaveNewItemOrderByDBHandler = new SaveNewItemOrderByDBDelegate(SaveNewItemOrderByDB);

            ReadingDataOrderInCollectionHandeler = new ReadingDataOrderInCollection(ReadingDataOrder);

            Task.Factory.StartNew(() =>
            {
                SaveNewItemOrderByDBHandler(item);
                OrderCollection.Add(item);
            });
            
            return Task<bool>.Factory.StartNew(() => true);
        }

        private Task<bool> DeleteItemInCollectionOrder(Order item)
        {
            //Проверка на существование 
            Task.Factory.StartNew(() =>
            {
                
                OrderCollection.Remove(item);
            });

            return Task<bool>.Factory.StartNew(() => true);
        }

        private Task<bool> UPDATEItemInCollectionOrder(FindByValueOrder value, int idOrder)
        {
            //Какой элемент менять, и что в нём менять 
            Task.Factory.StartNew(() =>
            {
                var temp = ReadingDataOrderInCollectionHandeler(value, idOrder);
                if (temp.Count > 0 && OrderCollection[idOrder] != null)
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

        public ControlPanelViewModel()
        {
            InitializationCollectionOrder();
        }
    }
}
