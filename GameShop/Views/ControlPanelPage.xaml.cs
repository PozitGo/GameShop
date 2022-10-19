using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameShop.Views
{
    public sealed partial class ControlPanelPage : Page
    {
        public ControlPanelViewModel ViewModel { get; } = new ControlPanelViewModel();

        public ControlPanelPage()
        {
            InitializeComponent();
        }
    }
}
