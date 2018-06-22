using System;
using System.Collections.Generic;

namespace Demo2018.Logics.ViewModels.Search
{
    public class SearchViewModel : BaseViewModel
    {
        List<String> _myItemsSource;
        public List<String> MyItemsSource
        {
            get { return _myItemsSource; }
            set { SetProperty(ref _myItemsSource, value); }
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
        }
    }
}
