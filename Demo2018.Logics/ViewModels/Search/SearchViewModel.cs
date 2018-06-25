using System;
using System.Collections.Generic;

namespace Demo2018.Logics.ViewModels.Search
{
    public class ListBike
    {
        string price = string.Empty;
        string name = string.Empty;
        string nameJP = string.Empty;
        string urlImg = string.Empty;
        public string Price { get => price; set => price = value; }
        public string Name { get => name; set => name = value; }
        public string NameJP { get => nameJP; set => nameJP = value; }
        public string UrlImg { get => urlImg; set => urlImg = value; }
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

        List<ListBike> _listBike2;
        public List<ListBike> ListBike2
        {
            get { return _listBike2; }
            set { SetProperty(ref _listBike2, value); }
        }

        List<ListBike> _listBike3;
        public List<ListBike> ListBike3
        {
            get { return _listBike3; }
            set { SetProperty(ref _listBike3, value); }
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
                new ListBike { UrlImg = "ads1.jpg", Name = "bianchi", Price="$600", NameJP="渋谷区"},
                new ListBike { UrlImg = "ads2.jpg", Name = "bianchi2", Price="$800", NameJP="渋谷区2"},
            };

            ListBike2 = new List<ListBike>
            {
                new ListBike { UrlImg = "ads3.jpg", Name = "bianchi Aria", Price="$500", NameJP="渋田区3"},
                new ListBike { UrlImg = "ads4.jpg", Name = "bianchi Aria2", Price="$800", NameJP="大田区2"},
            };

            ListBike3 = new List<ListBike>
            {
                new ListBike { UrlImg = "ads2.jpg", Name = "bianchi", Price="逞草仍下酐情渚宓愚柞 邺鄯灬鬓\n雷門", NameJP="渋谷区"},
                new ListBike { UrlImg = "ads3.jpg", Name = "bianchi2", Price="$800", NameJP="渋谷区2"},
            };

        }
    }
}
