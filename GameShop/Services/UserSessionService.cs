using GameShop.Helpers;
using GameShop.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace GameShop.Services
{
    internal class UserSessionService
    {
        public async Task<bool> UserCheckAuthAsync()
        {
            //проверяет может ли зайти в этот раз
            //await ApplicationData.Current.LocalSettings.SaveAsync("Data1", data);
            var userSessionModel = await ApplicationData.Current.LocalSettings.ReadAsync<UserSessionModel>("Data1");
            if (userSessionModel.CountOfSession < 10)
            {
                userSessionModel.CountOfSession++;
                await ApplicationData.Current.LocalSettings.SaveAsync("Data1", userSessionModel);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
