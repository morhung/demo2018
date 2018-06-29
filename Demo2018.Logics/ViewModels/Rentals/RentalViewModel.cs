using Demo2018.Logics.Models.Auth;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Text;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Demo2018.Logics.ViewModels.Rentals
{
    public class RentalViewModel : BaseViewModel
    {
        public ICommand login { get; set; }
        //Xamarin.Auth.OAuth2Authenticator authenticator = null;
        //public const string ClientId = "633568783691-5sfqdabsdn8a2bnie39v1s7jok3em42k.apps.googleusercontent.com";
        //public const string Scope = "email";
        //public const string RedirectUrl = "com.hungnd.Demo2018:/oauth2redirect";
        //private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        //private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        //private IGoogleManager _googleManager;
        // private readonly IPageDialogService _dialogService;

        public DelegateCommand GoogleLoginCommand { get; set; }
        public DelegateCommand GoogleLogoutCommand { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isLogedIn;

        public bool IsLogedIn
        {
            get { return _isLogedIn; }
            set { SetProperty(ref _isLogedIn, value); }
        }

        private GoogleUser _googleUser;

        public GoogleUser GoogleUser
        {
            get { return _googleUser; }
            set { SetProperty(ref _googleUser, value); }
        }
        // public RentalViewModel() { }
        public RentalViewModel(IPageDialogService pageDialogService, INavigationService navigationService) : base(navigationService)
        {
            //_googleManager = googleManager;
           // _dialogService = pageDialogService;

            IsLogedIn = false;
            GoogleLoginCommand = new DelegateCommand(GoogleLogin);
            GoogleLogoutCommand = new DelegateCommand(GoogleLogout);
        }

        private void GoogleLogout()
        {
            Xamarin.Forms.DependencyService.Get<IGoogleManager>().Logout();
            GoogleUser = new GoogleUser();
            IsLogedIn = false;
        }

        private void GoogleLogin()
        {
            //_googleManager.Login(OnLoginComplete);
            Xamarin.Forms.DependencyService.Get<IGoogleManager>().Login(OnLoginComplete);
        }

        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                IsLogedIn = true;
            }
            else
            {
                // pageDialogService.DisplayAlertAsync("Error", message, "Ok");
            }
        }

        //public void OnNavigatedFrom(NavigationParameters parameters)
        //{

        //}

        //public void OnNavigatingTo(NavigationParameters parameters)
        //{

        //}

        //public void OnNavigatedTo(NavigationParameters parameters)
        //{
        //    if (parameters.ContainsKey("title"))
        //        Title = (string)parameters["title"] + " and Prism";
        //}

    }
}
