using GameShop.Services;
using GameShop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GameShop.Activation
{
    /// <summary>
    /// Проверяет авторизирован ли пользователь
    /// </summary>
    internal class LoginActivationHandler : ActivationHandler
    {
       /// <summary>
       /// Вернёт bool, true если не авторизирован, false если авторизирован 
       /// </summary>
       /// <param name="args"></param>
       /// <returns></returns>
       public override bool CanHandle(object args)
        {
            return UserSessionService.IsUserAuth();
        }

        public override Task HandleAsync(object args)
        {
            Window.Current.Content = new LoginPPage();

            return Task.CompletedTask;
        }
    }
}
