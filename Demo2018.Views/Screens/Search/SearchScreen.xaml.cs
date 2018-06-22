using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Demo2018.Views.Screens.Search
{
    public partial class SearchScreen : BaseScreen
    {
        public SearchScreen()
        {
            InitializeComponent();
            var tapGestureRecognizer = new TapGestureRecognizer();
        }

        async void BikeButtonTapped(object sender, System.EventArgs e)
        {
            stBikeButton.Opacity = 1;
            await stBikeButton.FadeTo(0.5, 100);
            await stBikeButton.FadeTo(1, 100);
        }

        async void LocationButtonTapped(object sender, System.EventArgs e)
        {
            stLocationButton.Opacity = 1;
            await stLocationButton.FadeTo(0.5, 100);
            await stLocationButton.FadeTo(1, 100);
        }

        async void CheckListButtonTapped(object sender, System.EventArgs e)
        {
            stCheckListButton.Opacity = 1;
            await stCheckListButton.FadeTo(0.5, 100);
            await stCheckListButton.FadeTo(1, 100);
        }
    }
}
