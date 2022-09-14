using GameShop.Views;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GameShop.Activation
{//Класс отвечает за проверку пользовательских сессий
    internal class UserSessionHandler : ActivationHandler
    {
        //Вернёт true если у пользователя больше 10 входов
        public override bool CanHandle(object args)
        {
            //Тут обращаемся к UserSessionService за инфой
            return true;
        }

        public override Task HandleAsync(object args)
        {
            Window.Current.Content = new LoginPPage();

            return Task.CompletedTask;
        }
    }
}
