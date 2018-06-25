using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Demo2018.Views.Renderers
{
    public class CustomButtonRenderer : Button
    {
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create("Padding", typeof(Thickness), typeof(CustomButtonRenderer), new Thickness(0,0,0,0), BindingMode.TwoWay);
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
    }
}
