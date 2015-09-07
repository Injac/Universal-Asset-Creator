using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Model.ViewModel;

namespace UniversalAssetCreator.Views
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
                ServiceLocator.SetLocatorProvider(()=>SimpleIoc.Default);
                SimpleIoc.Default.Register<MainViewModel>();
        }

        public  MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
