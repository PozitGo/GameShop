using System;

using GameShop.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using NPOI.POIFS.Crypt.Dsig;
using Windows.UI.Xaml.Controls;
using static GameShop.ViewModels.ControlPanelViewModel;

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
