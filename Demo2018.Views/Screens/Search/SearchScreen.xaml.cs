using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Demo2018.Views.Screens.Search
{
    public partial class SearchScreen : BaseScreen
    {
        public SearchScreen()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("clicked");
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            Debug.WriteLine("tapped");
        }

        async private void OnTapGestureRecognizerTapped1(object sender, EventArgs e)
        {
            Debug.WriteLine("tapped");
            StackLayout btn = (StackLayout)sender;

            btn.BackgroundColor = Color.FromHex("#e6e6e6");
            await Task.Delay(100);
            btn.BackgroundColor = Color.FromHex("#ffffff");
        }
    }
}
