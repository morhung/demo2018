using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo2018.Logics.ViewModels
{
    public class MasterDetailsViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public MasterDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
