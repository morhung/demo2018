using System;
using Android.Graphics.Drawables;

namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid
{
    public class BottomBarTab : BottomBarItemBase
    {
        public int Id = -1;

        /// <summary>
        /// Creates a new Tab for the BottomBar
        /// </summary>
        /// <param name="iconResource">a resource for the Tab icon.</param>
        /// <param name="title">title for the Tab.</param>
        public BottomBarTab(int iconResource, String title)
        {
            this._iconResource = iconResource;
            this._title = title;
        }

        /// <summary>
        /// Creates a new Tab for the BottomBar.
        /// </summary>
        /// <param name="icon">an icon for the Tab.</param>
        /// <param name="title">title title for the Tab.</param>
        public BottomBarTab(Drawable icon, String title)
        {
            this._icon = icon;
            this._title = title;
        }

        /// <summary>
        /// Creates a new Tab for the BottomBar.
        /// </summary>
        /// <param name="icon">an icon for the Tab.</param>
        /// <param name="titleResource">resource for the title.</param>
        public BottomBarTab(Drawable icon, int titleResource)
        {
            this._icon = icon;
            this._titleResource = titleResource;
        }

        /// <summary>
        /// Creates a new Tab for the BottomBar.
        /// </summary>
        /// <param name="iconResource">a resource for the Tab icon.</param>
        /// <param name="titleResource">resource for the title.</param>
        public BottomBarTab(int iconResource, int titleResource)
        {
            this._iconResource = iconResource;
            this._titleResource = titleResource;
        }
    }
}
