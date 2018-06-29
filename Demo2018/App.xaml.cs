using System;
using Demo2018.Logics.ViewModels;
using Demo2018.Logics.ViewModels.MainPage;
using Demo2018.Logics.ViewModels.Rentals;
using Demo2018.Logics.ViewModels.Search;
using Demo2018.Views.Screens;
using Demo2018.Views.Screens.MainPage;
using Demo2018.Views.Screens.Menu;
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
            NavigationService.NavigateAsync("/menu1/Nav/mainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        { 
            containerRegistry.RegisterForNavigation<NavigationScreen>("Nav");
            containerRegistry.RegisterForNavigation<MenuSide, MasterDetailsViewModel>("menu1");
            containerRegistry.RegisterForNavigation<MainPageScreen, MainPageViewModel>("mainPage");
            containerRegistry.RegisterForNavigation<SearchScreen, SearchViewModel>("searchPage");
            containerRegistry.RegisterForNavigation<RentalsScreen, RentalViewModel>("rentalPage");
            //containerRegistry.Register(typeof(IGoogleManager), typeof(GoogleManager));
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
