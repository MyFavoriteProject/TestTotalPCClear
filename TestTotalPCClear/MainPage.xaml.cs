using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TestTotalPCClear.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestTotalPCClear
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ClearViewModel clearViewModel;

        public MainPage()
        {
            this.InitializeComponent();

            this.clearViewModel = new ClearViewModel();

            this.DataContext = clearViewModel;

            Binding myBinding = new Binding();
            myBinding.Source = this.DataContext;
            myBinding.Path = new PropertyPath("StateOfScanning");
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(mltView, MultiViewControl.selectedItemShowProperty, myBinding);
        }
    }
}