using Model;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalAssetCreator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;
            this.MinWidth = 800;
            this.MinHeight = 800;
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ContentGrid.UpdateLayout();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if(cmbTheme.SelectedItem != null)
            {
                var viewModel = (MainViewModel)this.DataContext;

                viewModel.ChosenTheme = (ThemeChosen) Enum.Parse(typeof(ThemeChosen), cmbTheme.SelectedItem as string);                
            }
        }
    }
}