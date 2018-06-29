using Demo2018.Views.Screens.MainPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo2018.Views.Screens.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuSide : MasterDetailPage
	{
		public MenuSide ()
		{
			InitializeComponent ();
            Detail = new MainPageScreen();
        }
	}
}