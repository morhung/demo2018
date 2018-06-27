using System;
using System.Collections.Generic;
using Demo2018.Views.Screens.MainPage;
using Demo2018.Views.Screens.Search;
using Xamarin.Forms;

namespace Demo2018.Views.Screens.Menu
{
    public partial class MenuSide : MasterDetailPage
    {
        public MenuSide()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Detail = new MainPageScreen();
        }
    }
}
