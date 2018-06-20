using System;
using Demo2018.Logics.ViewModels.MainPage;
using Demo2018.Logics.ViewModels.Rentals;
using Demo2018.Logics.ViewModels.Search;
using Demo2018.Views.Screens.MainPage;
using Demo2018.Views.Screens.Rentals;
using Demo2018.Views.Screens.Search;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Demo2018
{
    public partial class App : PrismApplication
    {
        public App()
        {
            
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync("mainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPageScreen, MainPageViewModel>("mainPage");
            containerRegistry.RegisterForNavigation<SearchScreen, SearchViewModel>("searchPage");
            containerRegistry.RegisterForNavigation<RentalsScreen, RentalViewModel>("rentalPage");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
