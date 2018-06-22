using System;
using System.Collections.Generic;

namespace Demo2018.Logics.ViewModels.Search
{
    public class ListBike
    {
        string urlImg = string.Empty;
        string name = string.Empty;

        public string UrlImg { get => urlImg; set => urlImg = value; }
        public string Name { get => name; set => name = value; }
    }
    public class SearchViewModel : BaseViewModel
    {
        List<String> _myItemsSource;
        
        public List<String> MyItemsSource
        {
            get { return _myItemsSource; }
            set { SetProperty(ref _myItemsSource, value); }
        }

        List<ListBike> _listBike;
        public List<ListBike> ListBike
        {
            get { return _listBike; }
            set { SetProperty(ref _listBike, value); }
        }

        public SearchViewModel()
        {
            MyItemsSource = new List<String>
            {
                "ads1.jpg",
                "ads2.jpg",
                "ads3.jpg",
                "ads4.jpg"
            };
            ListBike = new List<ListBike>
            {
                new ListBike { UrlImg = "1", Name = "bike 1"},
                new ListBike { UrlImg = "2", Name = "bike 2"},
                new ListBike { UrlImg = "3", Name = "bike 3"},
                new ListBike { UrlImg = "4", Name = "bike 4"},
                new ListBike { UrlImg = "5", Name = "bike 5"},
                new ListBike { UrlImg = "6", Name = "bike 6"},
                new ListBike { UrlImg = "7", Name = "bike 7"},
                new ListBike { UrlImg = "8", Name = "bike 8"},
            };

            
            
        }
    }
}
