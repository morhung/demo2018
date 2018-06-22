﻿using Android.Content;
using Android.Content.Res;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Demo2018.Droid.Renderers;
using Demo2018.Views.Renderers;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HorizontalViewNative), typeof(AndroidHorizontalViewRenderer))]
namespace Demo2018.Droid.Renderers
{
    public class AndroidHorizontalViewRenderer : ViewRenderer<HorizontalViewNative, RecyclerView>
    {
        public AndroidHorizontalViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Element.ItemsSource))
            {
                var adapter = new RecycleViewAdapter(Element);
                Control.SetAdapter(adapter);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HorizontalViewNative> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var recyclerView = new RecyclerView(Context);

                recyclerView.SetLayoutManager(new LinearLayoutManager(Context, OrientationHelper.Horizontal, false));

                SetNativeControl(recyclerView);

                var adapter = new RecycleViewAdapter(Element);
                Control.SetAdapter(adapter);
            }
        }
    }

    public class RecycleViewAdapter : RecyclerView.Adapter
    {
        private readonly HorizontalViewNative _view;

        private readonly IList _dataSource;

        public override int ItemCount => (_dataSource != null ? _dataSource.Count : 0);

        public override long GetItemId(int position)
        {
            return position;
        }

        public RecycleViewAdapter(HorizontalViewNative view)
        {
            _view = view;
            _dataSource = view.ItemsSource?.Cast<object>()?.ToList();
            HasStableIds = true;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = (RecycleViewHolder)holder;
            var dataContext = _dataSource[position];
            if (dataContext != null)
            {
                var dataTemplate = _view.ItemTemplate;
                ViewCell viewCell;
                var selector = dataTemplate as DataTemplateSelector;
                if (selector != null)
                {
                    var template = selector.SelectTemplate(_dataSource[position], _view.Parent);
                    viewCell = template.CreateContent() as ViewCell;
                }
                else
                {
                    viewCell = dataTemplate?.CreateContent() as ViewCell;
                }

                item.UpdateUi(viewCell, dataContext, _view);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var contentFrame = new FrameLayout(parent.Context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.MatchParent)
                {
                    Height = (int)(_view.ItemHeight * Resources.System.DisplayMetrics.Density),
                    Width = (int)(_view.ItemWidth * Resources.System.DisplayMetrics.Density)
                }
            };

            contentFrame.DescendantFocusability = DescendantFocusability.AfterDescendants;
            var viewHolder = new RecycleViewHolder(contentFrame);
            return viewHolder;
        }
    }

    public class RecycleViewHolder : RecyclerView.ViewHolder
    {
        public RecycleViewHolder(Android.Views.View itemView) : base(itemView)
        {
            ItemView = itemView;
        }

        public void UpdateUi(ViewCell viewCell, object dataContext, HorizontalViewNative view)
        {
            var contentLayout = (FrameLayout)ItemView;

            viewCell.BindingContext = dataContext;
            viewCell.Parent = view;

            var metrics = Resources.System.DisplayMetrics;
            var height = (int)((view.ItemHeight + viewCell.View.Margin.Top + viewCell.View.Margin.Bottom) * metrics.Density);
            var width = (int)((view.ItemWidth + viewCell.View.Margin.Left + viewCell.View.Margin.Right) * metrics.Density);

            viewCell.View.Layout(new Rectangle(0, 0, view.ItemWidth, view.ItemHeight));

            // Layout Android View
            var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            {
                Height = height,
                Width = width
            };

            if (Platform.GetRenderer(viewCell.View) == null)
            {
                Platform.SetRenderer(viewCell.View, Platform.CreateRenderer(viewCell.View));
            }
            var renderer = Platform.GetRenderer(viewCell.View);


            var viewGroup = renderer.View;
            viewGroup.LayoutParameters = layoutParams;
            viewGroup.Layout(0, 0, width, height);

            contentLayout.RemoveAllViews();
            contentLayout.AddView(viewGroup);
        }
    }
}