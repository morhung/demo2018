using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace Demo2018.Logics.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {
        public BaseViewModel()
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
