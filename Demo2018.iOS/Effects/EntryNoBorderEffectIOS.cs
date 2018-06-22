using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Demo2018.iOS.Effects;
using UIKit;

[assembly: ResolutionGroupName("Demo2018")]
[assembly: ExportEffect(typeof(EntryNoBorderEffectIOS), "EntryNoBorder")]
namespace Demo2018.iOS.Effects
{
    public class EntryNoBorderEffectIOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
            {
                UITextField entry = (UITextField)Control;
                entry.Layer.BorderWidth = 0;
                entry.BorderStyle = UITextBorderStyle.None;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
