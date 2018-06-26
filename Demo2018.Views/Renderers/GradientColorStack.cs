using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Demo2018.Views.Renderers
{
    public class GradientColorStack : Grid
    {
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
    }
}
