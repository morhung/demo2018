using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;

namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid
{
    internal static class MiscUtils
    {
        public static int GetColor(Context context, int color)
        {
            var tv = new TypedValue();
            context.Theme.ResolveAttribute(color, tv, true);
            return tv.Data;
        }

        /// <summary>
        /// Converts dps to pixels nicely.
        /// </summary>
        /// <returns>dimension in pixels</returns>
        /// <param name="context">Context for getting the resources</param>
        /// <param name="dp">dimension in dps</param>
        public static int DpToPixel(Context context, float dp)
        {
            var resources = context.Resources;
            var metrics = resources.DisplayMetrics;

            try
            {
                return (int)(dp * ((int)metrics.DensityDpi / 160f));
            }
            catch (Java.Lang.NoSuchFieldError)
            {
                return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, metrics);
            }

        }

        /// <summary>
        /// Gets the width of the screen.
        /// </summary>
        /// <returns>The screen width.</returns>
        /// <param name="context">Context to get resources and device specific display metrics.</param>
        public static int GetScreenWidth(Context context)
        {
            var displayMetrics = context.Resources.DisplayMetrics;
            return (int)(displayMetrics.WidthPixels / displayMetrics.Density);
        }

        /// <summary>
        /// A hacky method for inflating menus from xml resources to an array of BottomBarTabs.
        /// </summary>
        /// <returns>an Array of BottomBarTabs.</returns>
        /// <param name="activity">the activity context for retrieving the MenuInflater.</param>
        /// <param name="menuRes">the xml menu resource to inflate.</param>
        public static BottomBarTab[] InflateMenuFromResource(Activity activity, int menuRes)
        {
            // A bit hacky, but hey hey what can I do
            var popupMenu = new PopupMenu(activity, null);
            var menu = popupMenu.Menu;
            activity.MenuInflater.Inflate(menuRes, menu);

            int menuSize = menu.Size();
            var tabs = new BottomBarTab[menuSize];

            for (int i = 0; i < menuSize; i++)
            {
                var item = menu.GetItem(i);

                BottomBarTab tab = new BottomBarTab(item.Icon, item.TitleFormatted.ToString());
                tab.IsEnabled = item.IsEnabled;
                tab.IsVisible = item.IsVisible;
                tab.Id = item.ItemId;
                tabs[i] = tab;
            }

            return tabs;
        }

        /// <summary>
        /// A convenience method for setting text appearance.
        /// </summary>
        /// <param name="textView">TextView which textAppearance to modify.</param>
        /// <param name="resId">style resource for the text appearance.</param>
        public static void SetTextAppearance(Android.Widget.TextView textView, int resId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                textView.SetTextAppearance(resId);
            else
                textView.SetTextAppearance(textView.Context, resId);
        }

        /// <summary>
        /// Determine if the current UI Mode is Night Mode.
        /// </summary>
        /// <returns><c>true</c>, if the night mode is enabled, <c>false</c> otherwise.</returns>
        /// <param name="context">Context to get the configuration.</param>
        public static bool IsNightMode(Context context)
        {
            return context.Resources.Configuration.UiMode == UiMode.NightYes;
        }
    }
}
