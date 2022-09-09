using GameShop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GameShop.Activation
{
    internal class LoginActivationHandler : ActivationHandler
    {
        public override bool CanHandle(object args)
        {
            //Если авторизован
            return true;
        }

        public override Task HandleAsync(object args)
        {
            Window.Current.Content = new LoginPPage();

            return Task.CompletedTask;
        }
    }
}
