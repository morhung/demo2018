using System;
using Android.OS;
using Android.Views;
using Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Utils;

namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Listener
{
    public class NavBarMagicOnGlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private readonly BottomBar _bottomBar;
        private readonly View _outerContainer;
        private readonly int _navBarHeightCopy;
        private readonly bool _isTabletMode;

        public NavBarMagicOnGlobalLayoutListener(BottomBar bottomBar, View outerContainer, int navBarHeightCopy, bool isTabletMode)
        {
            _bottomBar = bottomBar;
            _outerContainer = outerContainer;
            _navBarHeightCopy = navBarHeightCopy;
            _isTabletMode = isTabletMode;
        }

        public void OnGlobalLayout()
        {
            int newHeight = _outerContainer.Height + _navBarHeightCopy;
            _outerContainer.LayoutParameters.Height = newHeight;

            ViewTreeObserver obs = _outerContainer.ViewTreeObserver;

            if (obs.IsAlive)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
                    obs.RemoveOnGlobalLayoutListener(this);
                else
                    obs.RemoveGlobalOnLayoutListener(this);
            }
        }
    }
}
