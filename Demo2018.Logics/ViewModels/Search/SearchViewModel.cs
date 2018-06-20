﻿using System;
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
                    "walkthrough1",
                    "walkthrough2",
                    "walkthrough3"
                };
        }
    }
}