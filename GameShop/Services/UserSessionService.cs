﻿using GameShop.DataBase;
using GameShop.Model;
using Microsoft.Toolkit.Uwp.Helpers;

namespace GameShop.Services
{
    public static class UserSessionService
    {
        private static readonly ApplicationDataStorageHelper helper = ApplicationDataStorageHelper.GetCurrent(new JsonObjectSerializer());

        public const string Key = "UserAuth";
        public static bool IsUserAuth()
        {
            var userSessionData = helper.Read<UserSessionModel>(Key);

            if (userSessionData == null)
                return true;

            if (userSessionData.CountOfSession < 10 && DataBaseAuthorization.LogUser(userSessionData.Login, userSessionData.Password))
            {
                userSessionData.CountOfSession++;
                helper.Save(Key, userSessionData);

                return false;
            }
            else
            {
                return true;
            }
        }

        public static void SaveUserSession(string Login, string Password)
        {
            UserSessionModel user = new UserSessionModel() { Login = Login, Password = Password, CountOfSession = 0 };
            helper.Save(Key, user);
        }
    }
}
