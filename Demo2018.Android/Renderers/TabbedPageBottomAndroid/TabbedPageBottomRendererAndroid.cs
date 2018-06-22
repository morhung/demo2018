using System;
using Android.Content;
using Android.Widget;
using Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Listener;
using Demo2018.Droid.Renderers.TabbedPageBottomAndroid;
using Demo2018.Views.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Utils;
using Android.Views;
using System.Linq;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(TabbedPageBottom), typeof(TabbedPageBottomRendererAndroid))]
namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid
{
    public class TabbedPageBottomRendererAndroid : VisualElementRenderer<TabbedPageBottom>, IOnTabClickListener
    {
        bool _disposed;
        BottomBar _bottomBar;
        FrameLayout _frameLayout;
        IPageController _pageController;

        public TabbedPageBottomRendererAndroid(Context context):base(context)
        {
            AutoPackage = false;
        }

        public void OnTabReSelected(int position)
        {
        }

        public void OnTabSelected(int position)
        {
            // Update view of tab selected
            SwitchContent(Element.Children[position]);
            Element.CurrentPage = Element.Children[position];
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _pageController.SendAppearing();
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            _pageController.SendDisappearing();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPageBottom> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                TabbedPageBottom bottomBarPage = e.NewElement;

                if (_bottomBar == null)
                {
                    _pageController = PageController.Create(bottomBarPage);

                    // create a view which will act as container for Page's
                    _frameLayout = new FrameLayout(Context);
                    _frameLayout.LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, GravityFlags.Fill);
                    AddView(_frameLayout, 0);

                    // create bottomBar control
                    _bottomBar = BottomBar.Attach(_frameLayout, null);
                    _bottomBar.NoTabletGoodness();

                    _bottomBar.UseFixedMode();

                    _bottomBar.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    _bottomBar.SetOnTabClickListener(this);

                    // create tab items
                    SetTabItems();
                }

                if (bottomBarPage.CurrentPage != null)
                {
                    SwitchContent(bottomBarPage.CurrentPage);
                    UpdateSelectedTabIndex(Element.CurrentPage);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(TabbedPageBottom.CurrentPage))
            {
                // Update Current Page => notify IOnTabClickListener to update view
                UpdateSelectedTabIndex(Element.CurrentPage);
            }
        }

        /// <summary>
        /// Switchs the content.
        /// </summary>
        /// <param name="view">View.</param>
        protected virtual void SwitchContent(Page view)
        {
            Context.HideKeyboard(this);

            _frameLayout.RemoveAllViews();

            if (view == null)
            {
                return;
            }

            if (Platform.GetRenderer(view) == null)
            {
                Platform.SetRenderer(view, Platform.CreateRendererWithContext(view, Context));
            }

            _frameLayout.AddView(Platform.GetRenderer(view).View);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            int width = r - l;
            int height = b - t;

            _bottomBar.Measure(
                MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));
            int tabsHeight = Math.Min(height, Math.Max(_bottomBar.MeasuredHeight, _bottomBar.MinimumHeight));

            if (width > 0 && height > 0)
            {
                _pageController.ContainerArea = new Rectangle(0, 0, Context.FromPixels(width), Context.FromPixels(_frameLayout.MeasuredHeight));

                _bottomBar.Measure(MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
                                   MeasureSpecFactory.MakeMeasureSpec(tabsHeight, MeasureSpecMode.Exactly));
                _bottomBar.Layout(0, 0, width, tabsHeight);
            }
            base.OnLayout(changed, l, t, r, b);
        }

        /// <summary>
        /// Updates the index of the selected tab.
        /// Set event Selected tab, Unselected tab to change Icon, text color
        /// </summary>
        /// <param name="page">Page.</param>
        void UpdateSelectedTabIndex(Page page)
        {
            var index = Element.Children.IndexOf(page);
            _bottomBar.SelectTabAtPosition(index);
        }

        /// <summary>
        /// Sets the tab items.
        /// </summary>
        void SetTabItems()
        {
            BottomBarTab[] tabs = Element.Children.Select(page =>
            {
                var tabIconId = ResourceManagerEx.IdFromTitle(page.Icon, ResourceManager.DrawableClass);
                return new BottomBarTab(tabIconId, page.Title);
            }).ToArray();

            if (tabs.Length > 0)
            {
                _bottomBar.SetItems(tabs);
            }
        }
    }
}
