using System;
using System.Windows.Input;
using Demo2018.Logics.Model;
using Prism.Services;
using Xamarin.Forms;

namespace Demo2018.Logics.ViewModels.Logins
{
    public class LoginViewModel : BaseViewModel
    {
        //private readonly IFacebookManager _facebookManager;
        //private readonly IPageDialogService _dialogService;

        public ICommand FacebookLoginCommand { get; set; }
        public ICommand FacebookLogoutCommand { get; set; }

        public ICommand GoogleLoginCommand { get; set; }
        public ICommand GoogleLogoutCommand { get; set; }

        private FacebookUser _facebookUser = new FacebookUser();

        public FacebookUser FacebookUser
        {
            get { return _facebookUser; }
            set { SetProperty(ref _facebookUser, value); }
        }

        private GoogleUser _googleUser;

        public GoogleUser GoogleUser
        {
            get { return _googleUser; }
            set { SetProperty(ref _googleUser, value); }
        }
        //private string _title;

        //public string Title
        //{
        //    get { return _title; }
        //    set { SetProperty(ref _title, value); }
        //}

        //private bool _isLogedIn;

        //public bool IsLogedIn
        //{
        //    get { return _isLogedIn; }
        //    set { SetProperty(ref _isLogedIn, value); }
        //}

        public LoginViewModel()
        {
            //_facebookManager = facebookManager;
            //_dialogService = dialogService;

            //IsLogedIn = false;
            FacebookLoginCommand = new Command(FacebookLogin);
            FacebookLogoutCommand = new Command(FacebookLogout);

            GoogleLoginCommand = new Command(GoogleLogin);
            GoogleLogoutCommand = new Command(GoogleLogout);
        }

        private void FacebookLogout()
        {
            Xamarin.Forms.DependencyService.Get<IFacebookManager>().Logout();
            //_facebookManager.Logout();
            //IsLogedIn = false;
        }

        private void FacebookLogin()
        {
            Xamarin.Forms.DependencyService.Get<IFacebookManager>().Login(OnLoginComplete);
            //_facebookManager.Login(OnLoginComplete);
        }

        private void OnLoginComplete(FacebookUser facebookUser, string message)
        {
            if (facebookUser != null)
            {
                FacebookUser = facebookUser;
                //IsLogedIn = true;
            }
            else
            {
               // _dialogService.DisplayAlertAsync("Error", message, "Ok");
            }
        }

        private void GoogleLogout()
        {
            Xamarin.Forms.DependencyService.Get<IGoogleManager>().Logout();
        }

        private void GoogleLogin()
        {
            Xamarin.Forms.DependencyService.Get<IGoogleManager>().Login(OnLoginCompleteGoogle);

        }

        private void OnLoginCompleteGoogle(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
            }
            else
            {
                //_dialogService.DisplayAlertAsync("Error", message, "Ok");
            }
        }
    }
}
