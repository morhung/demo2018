using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;

namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid
{
    public class BottomBarItemBase
    {
        protected int _iconResource;
        protected Drawable _icon;
        protected int _titleResource;
        protected String _title;
        protected int _color;
        protected bool _isEnabled = true;
        protected bool _isVisible = true;

        public bool IsEnabled
        {
            get
            {
                return this._isEnabled;
            }
            set
            {
                this._isEnabled = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                this._isVisible = value;
            }
        }

        public Drawable GetIcon(Context context)
        {
            return this._iconResource != 0 ? AppCompatDrawableManager.Get().GetDrawable(context, this._iconResource) : this._icon;
        }

        public String GetTitle(Context context)
        {
            return this._titleResource != 0 ? context.GetString(this._titleResource) : this._title;
        }
    }
}
