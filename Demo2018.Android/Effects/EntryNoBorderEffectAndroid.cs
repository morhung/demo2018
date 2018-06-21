using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Demo2018.Droid.Effects;
using Android.Widget;

[assembly: ResolutionGroupName("Demo2018")]
[assembly: ExportEffect(typeof(EntryNoBorderEffectAndroid), "EntryNoBorder")]
namespace Demo2018.Droid.Effects
{
    public class EntryNoBorderEffectAndroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
            {
                TextView textView = (TextView)Control;
                textView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
