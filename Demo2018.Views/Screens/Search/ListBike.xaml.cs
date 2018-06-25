using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo2018.Views.Screens.Search
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListBike : Grid
	{
		public ListBike ()
		{
			InitializeComponent ();
		}

        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {

        }
    }
}