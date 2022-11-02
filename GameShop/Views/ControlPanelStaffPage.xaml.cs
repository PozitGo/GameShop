using System;

using GameShop.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GameShop.Views
{
    public sealed partial class ControlPanelStaffPage : Page
    {
        public ControlPanelStaffViewModel ViewModel { get; } = new ControlPanelStaffViewModel();

        public ControlPanelStaffPage()
        {
            InitializeComponent();
        }
    }
}
