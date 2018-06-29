using System;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace Demo2018.Logics.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {
        private INavigationService navigationService;
       // private IPageDialogService pageDialogService;
        public BaseViewModel()
        {
        }

        public BaseViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
    //        this.pageDialogService = pageDialogService;
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
