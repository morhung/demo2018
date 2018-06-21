using System;
using Demo2018.iOS.Renderers;
using Demo2018.Views.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPageBottom), typeof(TabbedPageBottomRendererIOS))]
namespace Demo2018.iOS.Renderers
{
    public class TabbedPageBottomRendererIOS : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (TabBar == null || TabBar.Items == null)
                return;

            //var tabs = Element as TabbedPageBottom;
            // Code that uses features from iOS 10 and later
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Set Color Text When Selected Tab
                TabBar.TintColor = Color.FromHex("#FBFBFB").ToUIColor();
                // Set Color Text When UnSelected Tab
                TabBar.UnselectedItemTintColor = Color.FromHex("#4A4A4A").ToUIColor();
            }
            else // Code to support earlier iOS versions
            {
                // Set Color Text When Selected Tab
                UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = Color.FromHex("#4A4A4A").ToUIColor() }, UIControlState.Normal);
                // Set Color Text When UnSelected Tab
                UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = Color.FromHex("#FBFBFB").ToUIColor() }, UIControlState.Selected);
            }

            TabBar.BarTintColor = UIColor.Black;
            TabBar.BackgroundColor = UIColor.Black;
        } 
    }
}
