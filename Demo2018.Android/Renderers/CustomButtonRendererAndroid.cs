using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Demo2018.Droid.Renderers;
using Demo2018.Views.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButtonRenderer), typeof(CustomButtonRendererAndroid))]
namespace Demo2018.Droid.Renderers
{
    class CustomButtonRendererAndroid : ButtonRenderer
    {
        public CustomButtonRendererAndroid(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                // remove event handlers
               // Control.Touch -= HandleTouch;
            }

            if (Control != null)
            {
                var innerMargin = (Element as CustomButtonRenderer)?.Padding ?? new Thickness(0, 0, 0, 0);
                Control.SetAllCaps(false);
                //Control.SetPadding((int)innerMargin.Left, (int)innerMargin.Top, (int)innerMargin.Right, (int)innerMargin.Bottom);
                //SetDisableColors();
                //Control.SetPadding(0, 0, 0, 0);
            }

            //if (e.NewElement != null)
            //{
            //    btn = e.NewElement as ButtonNoPadding;
            //    Control.Touch += HandleTouch;
            //}
        }
    }
}