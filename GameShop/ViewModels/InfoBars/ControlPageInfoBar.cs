using Microsoft.UI.Xaml.Controls;

namespace GameShop.ViewModels.InfoBars
{
    public static class ControlPageInfoBar
    {

        public static InfoBar bar = new InfoBar();
        public static InfoBar IsNull()
        {
            bar.IsOpen = true;
            bar.Severity = InfoBarSeverity.Error;
            bar.Title = "Не все поля заполенны.";
            bar.Message = "Заполните оставшиеся поля и повторите попытку.";

            return bar;
        }

        public static InfoBar Accept()
        {
            bar.IsOpen = true;
            bar.Severity = InfoBarSeverity.Success;
            bar.Title = "Успешно";
            bar.Message = "Заполните оставшиеся поля и повторите попытку.";

            return bar;
        }

        public static InfoBar Accept(string InputTitle, string InputMessage)
        {

            bar.IsOpen = true;
            bar.Severity = InfoBarSeverity.Success;
            bar.Title = InputTitle;
            bar.Message = InputMessage;
            
            return bar;
        }

        public static InfoBar Error(string InputTitle, string InputMessage)
        {
            bar.IsOpen = true;
            bar.Severity = InfoBarSeverity.Error;
            bar.Title = InputTitle;
            bar.Message = InputMessage;

            return bar;
        }

        public static InfoBar Information(string InputTitle, string InputMessage)
        {
            bar.IsOpen = true;
            bar.Severity = InfoBarSeverity.Informational;
            bar.Title = InputTitle;
            bar.Message = InputMessage;

            return bar;
        }

        public static InfoBar Warning(string InputTitle, string InputMessage)
        {
            bar.IsOpen = true;
            bar.Severity = InfoBarSeverity.Warning;
            bar.Title = InputTitle;
            bar.Message = InputMessage;

            return bar;
        }
    }
}
