using System;
using System.Collections.Generic;
using System.Linq;
using Android.Annotation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V4.View.Animation;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Listener;

namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid
{
    public class BottomBar : RelativeLayout, View.IOnClickListener, View.IOnLongClickListener
    {
        private const String STATE_CURRENT_SELECTED_TAB = "STATE_CURRENT_SELECTED_TAB";
        private const String STATE_BADGE_STATES_BUNDLE = "STATE_BADGE_STATES_BUNDLE";
        private const String TAG_BOTTOM_BAR_VIEW_INACTIVE = "BOTTOM_BAR_VIEW_INACTIVE";
        private const String TAG_BOTTOM_BAR_VIEW_ACTIVE = "BOTTOM_BAR_VIEW_ACTIVE";
        private const String TAG_BADGE = "BOTTOMBAR_BADGE_";

        private Context _context;
        private bool _ignoreTabletLayout;
        private bool _isTabletMode;

        private View _backgroundView;
        private View _backgroundOverlay;
        private View _shadowView;
        private View _tabletRightBorder;

        private Color _primaryColor;
        private Color _inActiveColor;

        private Color _whiteColor;

        private int _screenWidth;
        private int _tenDp;
        private int _sixDp;
        private int _sixteenDp;
        private int _eightDp;
        private int _maxFixedItemWidth;
        private int _maxInActiveShiftingItemWidth;
        private int _inActiveShiftingItemWidth;
        private int _activeShiftingItemWidth;

        private Object _listener;
        private Object _menuListener;

        private bool _isShiftingMode;

        private Java.Lang.Object _fragmentManager;
        private int _fragmentContainer;

        private BottomBarItemBase[] _items;

        private int _pendingTextAppearance = -1;
        private Typeface _pendingTypeface;

        private static readonly IInterpolator INTERPOLATOR = new LinearOutSlowInInterpolator();

        // For fragment state restoration
        private bool _shouldUpdateFragmentInitially;

        protected View PendingUserContentView { get; set; }
        protected ViewGroup UserContainer { get; set; }

        private bool _drawBehindNavBar;
        public bool DrawBehindNavBar
        {
            get { return _drawBehindNavBar; }
            set
            {
                if (_items != null)
                {
                    throw new Java.Lang.UnsupportedOperationException("This BottomBar already has items! " +
                        "You must call noNavBarGoodness() before setting the items, preferably " +
                        "right after attaching it to your layout.");
                }

                _drawBehindNavBar = value;
            }
        }

        public BottomBarItemBase[] Items { get { return _items; } }

        public ViewGroup ItemContainer { get; private set; }

        /// <summary>
        /// Get the actual BottomBar that has the tabs inside it for whatever what you may want
        /// to do with it.
        /// </summary>
        /// <value>The BottomBar</value>
        public ViewGroup OuterContainer { get; protected set; }

        /// <summary>
        /// Gets the current tab position.
        /// </summary>
        /// <value>the position of currently selected tab.</value>
        public int CurrentTabPosition { get; private set; }

        private int _maxFixedTabCount = 3;
        /// <summary>
        /// Set the maximum number of tabs, after which the tabs should be shifting ones with a background color.
        /// NOTE: You must call this method before setting any items.
        /// </summary>
        /// <value>count maximum number of fixed tabs.</value>
        public int MaxFixedTabCount
        {
            get { return _maxFixedTabCount; }
            set
            {
                if (_items != null)
                    throw new InvalidOperationException("This BottomBar already has items! " +
                        "You must set MaxFixedTabCount before specifying any items.");
                _maxFixedTabCount = value;
            }
        }

        public bool Hidden { get; private set; }

        public EventHandler BarWillShow;
        public EventHandler BarDidShow;

        public EventHandler BarWillHide;
        public EventHandler BarDidHide;


        /// <summary>
        /// Attach the specified view and savedInstanceState.
        /// </summary>
        /// <returns>The attach.</returns>
        /// <param name="view">View.</param>
        /// <param name="savedInstanceState">Saved instance state.</param>
        public static BottomBar Attach(View view, Bundle savedInstanceState)
        {
            BottomBar bottomBar = new BottomBar(view.Context);
            bottomBar.OnRestoreInstanceState(savedInstanceState);

            ViewGroup contentView = (ViewGroup)view.Parent;

            if (contentView != null)
            {
                View oldLayout = contentView.GetChildAt(0);
                contentView.RemoveView(oldLayout);

                bottomBar.PendingUserContentView = oldLayout;
                contentView.AddView(bottomBar, 0);
            }
            else
            {
                bottomBar.PendingUserContentView = view;
            }

            return bottomBar;
        }


        /// <summary>
        /// Sets the fragment items.
        /// </summary>
        /// <param name="fragmentManager">Fragment manager.</param>
        /// <param name="containerResource">Container resource.</param>
        /// <param name="fragmentItems">Fragment items.</param>
        [Obsolete("Deprecated")]
        public void SetFragmentItems(Android.App.FragmentManager fragmentManager, int containerResource, BottomBarFragment[] fragmentItems)
        {
            if (fragmentItems.Length > 0)
            {
                int index = 0;

                foreach (var fragmentItem in fragmentItems)
                {
                    if (fragmentItem.Fragment == null
                        && fragmentItem.SupportFragment != null)
                    {
                        throw new ArgumentException("Conflict: cannot use android.app.FragmentManager " +
                            "to handle a android.support.v4.app.Fragment object at position " + index +
                            ". If you want BottomBar to handle support Fragments, use getSupportFragment" +
                            "Manager() instead of getFragmentManager().");
                    }

                    index++;
                }
            }

            ClearItems();
            _fragmentManager = fragmentManager;
            _fragmentContainer = containerResource;
            _items = fragmentItems;
            UpdateItems(_items);
        }

        /// <summary>
        /// Sets the fragment items.
        /// </summary>
        /// <param name="fragmentManager">Fragment manager.</param>
        /// <param name="containerResource">Container resource.</param>
        /// <param name="fragmentItems">Fragment items.</param>
        [Obsolete("Deprecated")]
        public void SetFragmentItems(Android.Support.V4.App.FragmentManager fragmentManager, int containerResource, BottomBarFragment[] fragmentItems)
        {
            if (fragmentItems.Length > 0)
            {
                int index = 0;

                foreach (var fragmentItem in fragmentItems)
                {
                    if (fragmentItem.SupportFragment == null
                        && fragmentItem.Fragment != null)
                    {
                        throw new ArgumentException("Conflict: cannot use android.support.v4.app.FragmentManager " +
                            "to handle a android.app.Fragment object at position " + index +
                            ". If you want BottomBar to handle normal Fragments, use getFragment" +
                            "Manager() instead of getSupportFragmentManager().");
                    }

                    index++;
                }
            }
            ClearItems();
            _fragmentManager = fragmentManager;
            _fragmentContainer = containerResource;
            _items = fragmentItems;
            UpdateItems(_items);
        }

        /// <summary>
        /// Sets the items.
        /// </summary>
        /// <param name="bottomBarTabs">Bottom bar tabs.</param>
        public void SetItems(BottomBarTab[] bottomBarTabs)
        {
            ClearItems();
            _items = bottomBarTabs;
            UpdateItems(_items);
        }

        /// <summary>
        /// Set items for this BottomBar from an XML menu resource file.
        /// When setting more than 3 items, only the icons will show by
        /// default, but the selected item will have the text visible.
        /// </summary>
        /// <param name="menuRes">the menu resource to inflate items from.</param>
        public void SetItems(int menuRes)
        {
            ClearItems();
            _items = MiscUtils.InflateMenuFromResource((Activity)Context, menuRes);
            UpdateItems(_items);
        }

        /// <summary>
        /// Sets the items from menu.
        /// </summary>
        /// <param name="menuRes">Menu res.</param>
        /// <param name="listener">Listener.</param>
        //[Obsolete("Deprecated")]
        //public void SetItemsFromMenu(int menuRes, IOnMenuTabSelectedListener listener)
        //{
        //    ClearItems();
        //    _items = MiscUtils.InflateMenuFromResource((Activity)Context, menuRes);
        //    _menuListener = listener;
        //    UpdateItems(_items);
        //}

        /// <summary>
        /// Sets the items from menu.
        /// </summary>
        /// <param name="menuRes">Menu res.</param>
        /// <param name="listener">Listener.</param>
        //[Obsolete("Deprecated")]
        //public void SetItemsFromMenu(int menuRes, IOnMenuTabClickListener listener)
        //{
        //    ClearItems();
        //    _items = MiscUtils.InflateMenuFromResource((Activity)Context, menuRes);
        //    _menuListener = listener;
        //    UpdateItems(_items);

        //    if (_items != null && _items.Length > 0 && _items is BottomBarTab[])
        //        listener.OnMenuTabSelected(((BottomBarTab)_items[CurrentTabPosition]).Id);
        //}

        /// <summary>
        /// Sets the on item selected listener.
        /// </summary>
        /// <param name="listener">Listener.</param>
        [Obsolete("Deprecated")]
        public void SetOnItemSelectedListener(IOnTabSelectedListener listener)
        {
            _listener = listener;
        }

        /// <summary>
        /// Set a listener that gets fired when the selected tab changes.
        /// Note: If listener is set after items are added to the BottomBar, OnTabSelected 
        /// will be immediately called for the currently selected tab
        /// </summary>
        /// <param name="listener">a listener for monitoring changes in tab selection.</param>
        public void SetOnTabClickListener(IOnTabClickListener listener)
        {
            _listener = listener;

            if (_listener != null && _items != null && _items.Length > 0)
            {
                if (_items[CurrentTabPosition].IsEnabled)
                {
                    listener.OnTabSelected(CurrentTabPosition);
                }
            }
        }

        //public void SetOnMenuTabClickListener(IOnMenuTabClickListener listener)
        //{
        //    _menuListener = listener;

        //    if (_menuListener != null && _items != null && _items.Length > 0)
        //    {
        //        var tab = (BottomBarTab)_items[CurrentTabPosition];
        //        if (tab != null && tab.IsEnabled)
        //        {
        //            listener.OnMenuTabSelected(tab.Id);
        //        }
        //    }
        //}

        /// <summary>
        /// Selects the tab at position.
        /// </summary>
        /// <param name="position">Position.</param>
        public void SelectTabAtPosition(int position)
        {
            if (_items == null || _items.Length == 0)
            {
                throw new InvalidOperationException("Can't select tab at " +
                    "position " + position + ". This BottomBar has no items set yet.");
            }
            else if (position > _items.Length - 1 || position < 0)
            {
                throw new ArgumentOutOfRangeException("Can't select tab at position " +
                    position + ". This BottomBar has no items at that position.");
            }

            var oldTab = ItemContainer.FindViewWithTag(TAG_BOTTOM_BAR_VIEW_ACTIVE);
            var newTab = ItemContainer.GetChildAt(position);

            UnselectTab(oldTab);
            SelectTab(newTab);

            UpdateSelectedTab(position);
            ShiftingMagic(oldTab, newTab);
        }

        /// <summary>
        /// Hide the BottomBar with or without animation.
        /// </summary>
        public void Hide(bool animated)
        {
            if (!animated)
                SetBarVisibility(ViewStates.Gone);

            Hidden = true;
        }

        /// <summary>
        /// Show the BottomBar with or without animation.
        /// </summary>
        public void Show(bool animated)
        {
            if (!animated)
                SetBarVisibility(ViewStates.Visible);

            Hidden = false;
        }

        /// <summary>
        /// Always show the titles and icons also on inactive tabs, even if there's more than three of them.
        /// </summary>
        public void UseFixedMode()
        {
            if (_items != null)
                throw new InvalidOperationException("This BottomBar already has items! " +
                    "You must call the UseFixedMode() method before specifying any items.");
            _maxFixedTabCount = -1;
        }

        private void SetBarVisibility(ViewStates visibility)
        {
            if (OuterContainer != null)
                OuterContainer.Visibility = visibility;

            if (_backgroundView != null)
                _backgroundView.Visibility = visibility;

            if (_backgroundOverlay != null)
                _backgroundOverlay.Visibility = visibility;
        }


        /// <summary>
        /// Call this method in your Activity's onSaveInstanceState to keep the BottomBar's state on configuration change.
        /// </summary>
        /// <param name="outState">the Bundle to save data to.</param>
        public void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(STATE_CURRENT_SELECTED_TAB, CurrentTabPosition);

            if (_fragmentManager != null
                && _fragmentContainer != 0
                && _items != null
                && _items is BottomBarFragment[])
            {
                BottomBarFragment bottomBarFragment = (BottomBarFragment)_items[CurrentTabPosition];

                if (bottomBarFragment.Fragment != null)
                {
                    bottomBarFragment.Fragment.OnSaveInstanceState(outState);
                }
                else if (bottomBarFragment.SupportFragment != null)
                {
                    bottomBarFragment.SupportFragment.OnSaveInstanceState(outState);
                }
            }
        }

        /// <summary>
        /// Hide the shadow that's normally above the BottomBar.
        /// </summary>
        public void HideShadow()
        {
            if (_shadowView != null)
            {
                _shadowView.Visibility = ViewStates.Gone;
            }
        }

        /// <summary>
        /// Prevent the BottomBar drawing behind the Navigation Bar and making it transparent. 
        /// Must be called before setting items.
        /// </summary>
        public void NoNavBarGoodness()
        {
            if (_items != null)
            {
                throw new Java.Lang.UnsupportedOperationException("This BottomBar already has items! " +
                    "You must call noNavBarGoodness() before setting the items, preferably " +
                    "right after attaching it to your layout.");
            }

            DrawBehindNavBar = false;
        }

        /// <summary>
        /// Force the BottomBar to behave exactly same on tablets and phones,
        /// instead of showing a left menu on tablets.
        /// </summary>
        public void NoTabletGoodness()
        {
            if (_items != null)
            {
                throw new Java.Lang.UnsupportedOperationException("This BottomBar already has items! " +
                    "You must call noTabletGoodness() before setting the items, preferably " +
                    "right after attaching it to your layout.");
            }

            _ignoreTabletLayout = true;
        }

        /// <summary>
        /// Get this BottomBar's height (or width), depending if the BottomBar
        /// is on the bottom (phones) or the left (tablets) of the screen.
        /// </summary>
        /// <param name="listener">listener <see cref="IOnSizeDeterminedListener"/> to get the size when it's ready.</param>
        public void GetBarSize(IOnSizeDeterminedListener listener)
        {
            int sizeCandidate = _isTabletMode ? OuterContainer.Width : OuterContainer.Height;

            if (sizeCandidate == 0)
            {
                OuterContainer.ViewTreeObserver.AddOnGlobalLayoutListener(new BarSizeOnGlobalLayoutListener(listener, _isTabletMode, OuterContainer));
                return;
            }

            listener.OnSizeReady(sizeCandidate);
        }

        public BottomBar(Context context)
            : base(context)
        {
            Init(context, null, 0, 0);
        }

        public BottomBar(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Init(context, attrs, 0, 0);
        }

        public BottomBar(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {

            Init(context, attrs, defStyleAttr, 0);
        }

        [TargetApiAttribute(Value = (int)BuildVersionCodes.Lollipop)]
        public BottomBar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs, defStyleAttr, defStyleRes);
        }

        public BottomBar(Context context, Color backgroundColor, Color activeColor)
            : base(context)
        {
            _whiteColor = activeColor;
            _primaryColor = backgroundColor;

            Init(context, null, 0, 0, true);
        }

        private void Init(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes, bool colorsInitialized = false)
        {
            _context = context;

            if (!colorsInitialized)
            {
                _whiteColor = new Color(ContextCompat.GetColor(Context, Resource.Color.white));
                _primaryColor = new Color(MiscUtils.GetColor(Context, Resource.Attribute.colorPrimary));
            }

            _inActiveColor = new Color(ContextCompat.GetColor(Context, Resource.Color.bb_inActiveBottomBarItemColor));

            _screenWidth = MiscUtils.GetScreenWidth(_context);
            _tenDp = MiscUtils.DpToPixel(_context, 10);
            _sixteenDp = MiscUtils.DpToPixel(_context, 16);
            _sixDp = MiscUtils.DpToPixel(_context, 6);
            _eightDp = MiscUtils.DpToPixel(_context, 8);
            _maxFixedItemWidth = MiscUtils.DpToPixel(_context, 168);
            _maxInActiveShiftingItemWidth = MiscUtils.DpToPixel(_context, 96);
        }

        private void InitializeViews()
        {
            _isTabletMode = !_ignoreTabletLayout && _context.Resources.GetBoolean(Resource.Boolean.bb_bottom_bar_is_tablet_mode);

            ViewCompat.SetElevation(this, MiscUtils.DpToPixel(_context, 8));

            View rootView = Inflate(_context,
                                    _isTabletMode ? Resource.Layout.bb_bottom_bar_item_container_tablet : Resource.Layout.bb_bottom_bar_item_container,
                this);
            _tabletRightBorder = rootView.FindViewById(Resource.Id.bb_tablet_right_border);

            UserContainer = (ViewGroup)rootView.FindViewById(Resource.Id.bb_user_content_container);
            _shadowView = rootView.FindViewById(Resource.Id.bb_bottom_bar_shadow);

            OuterContainer = (ViewGroup)rootView.FindViewById(Resource.Id.bb_bottom_bar_outer_container);
            ItemContainer = (ViewGroup)rootView.FindViewById(Resource.Id.bb_bottom_bar_item_container);

            _backgroundView = rootView.FindViewById(Resource.Id.bb_bottom_bar_background_view);
            _backgroundOverlay = rootView.FindViewById(Resource.Id.bb_bottom_bar_background_overlay);

            if (PendingUserContentView != null)
            {
                var param = PendingUserContentView.LayoutParameters;

                if (param == null)
                {
                    param = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                }

                UserContainer.AddView(PendingUserContentView, 0, param);
                PendingUserContentView = null;
            }
        }

        public void OnClick(View v)
        {
            HandleClick(v);
        }

        public void HandleClick(View v)
        {
            if (v.Tag.Equals(TAG_BOTTOM_BAR_VIEW_INACTIVE))
            {
                var oldTab = FindViewWithTag(TAG_BOTTOM_BAR_VIEW_ACTIVE);

                UnselectTab(oldTab);
                SelectTab(v);

                ShiftingMagic(oldTab, v);
            }
            UpdateSelectedTab(FindItemPosition(v));
        }

        private void ShiftingMagic(View oldTab, View newTab)
        {
            if (!_isTabletMode && _isShiftingMode)
            {
                if (oldTab is FrameLayout)
                    oldTab = ((FrameLayout)oldTab).GetChildAt(0);
                if (newTab is FrameLayout)
                    newTab = ((FrameLayout)newTab).GetChildAt(0);
            }
        }

        private void UpdateSelectedTab(int newPosition)
        {
            var notifyMenuListener = _menuListener != null && _items is BottomBarTab[];
            var notifyRegularListener = _listener != null;

            if (newPosition != CurrentTabPosition)
            {
                CurrentTabPosition = newPosition;

                if (notifyRegularListener)
                    NotifyRegularListener(_listener, false, CurrentTabPosition);

                //if (notifyMenuListener)
                    //NotifyMenuListener(_menuListener, false, ((BottomBarTab)_items[CurrentTabPosition]).Id);

                UpdateCurrentFragment();
            }
            else
            {
                if (notifyRegularListener)
                    NotifyRegularListener(_listener, true, CurrentTabPosition);

                //if (notifyMenuListener && _menuListener is IOnMenuTabClickListener)
                    //NotifyMenuListener(_menuListener, true, ((BottomBarTab)_items[CurrentTabPosition]).Id);
            }
        }

        private void NotifyRegularListener(Object listener, bool isReselection, int position)
        {
            if (listener is IOnTabClickListener)
            {
                var onTabClickListener = (IOnTabClickListener)listener;
                if (!isReselection)
                    onTabClickListener.OnTabSelected(position);
                else
                    onTabClickListener.OnTabReSelected(position);
            }
            else if (_listener is IOnTabSelectedListener)
            {
                var onTabSelectedListener = (IOnTabSelectedListener)listener;
                if (!isReselection)
                    onTabSelectedListener.OnItemSelected(position);
            }
        }

        //private void NotifyMenuListener(Object listener, bool isReselection, int menuItemId)
        //{
        //    if (listener is IOnMenuTabClickListener)
        //    {
        //        var onMenuTabClickListener = (IOnMenuTabClickListener)listener;
        //        if (!isReselection)
        //            onMenuTabClickListener.OnMenuTabSelected(menuItemId);
        //        else
        //            onMenuTabClickListener.OnMenuTabReSelected(menuItemId);
        //    }
        //    else if (_listener is IOnMenuTabSelectedListener)
        //    {
        //        var onMenuTabSelectedListener = (IOnMenuTabSelectedListener)listener;
        //        if (!isReselection)
        //            onMenuTabSelectedListener.OnMenuItemSelected(menuItemId);
        //    }
        //}

        public bool OnLongClick(View v)
        {
            return HandleLongClick(v);
        }

        private bool HandleLongClick(View v)
        {
            if ((_isShiftingMode || _isTabletMode) && v.Tag.Equals(TAG_BOTTOM_BAR_VIEW_INACTIVE))
            {
                Toast.MakeText(_context, _items[FindItemPosition(v)].GetTitle(_context), ToastLength.Short).Show();
            }

            return true;
        }

        private void UpdateItems(BottomBarItemBase[] bottomBarItems)
        {
            if (ItemContainer == null)
                InitializeViews();

            int index = 0;
            int biggestWidth = 0;
            _isShiftingMode = MaxFixedTabCount >= 0 && MaxFixedTabCount < bottomBarItems.Length;

            _backgroundView.SetBackgroundColor(_primaryColor);

            if (_context is Activity)
            {
                NavBarMagic((Activity)_context, this);
            }


            var listOfBottomBarItems = new List<BottomBarItemBase>(bottomBarItems)?.Where(i => i.IsVisible)?.ToList();
            View[] viewsToAdd = new View[listOfBottomBarItems.Count];

            foreach (var bottomBarItemBase in listOfBottomBarItems)
            {
                int layoutResource;

                if (_isShiftingMode && !_isTabletMode)
                {
                    layoutResource = Resource.Layout.bb_bottom_bar_item_shifting;
                }
                else
                {
                    layoutResource = _isTabletMode ? Resource.Layout.bb_bottom_bar_item_fixed_tablet : Resource.Layout.bb_bottom_bar_item_fixed;
                }

                View bottomBarTab = View.Inflate(_context, layoutResource, null);
                var icon = (AppCompatImageView)bottomBarTab.FindViewById(Resource.Id.bb_bottom_bar_icon);
                bottomBarTab.Enabled = bottomBarItemBase.IsEnabled;

                icon.SetImageDrawable(bottomBarItemBase.GetIcon(_context));
                icon.SetTag(Resource.Id.TAG_IMAGE_NAME, "iconName" + index.ToString());//todo add 

                if (!_isTabletMode)
                {
                    TextView title = (TextView)bottomBarTab.FindViewById(Resource.Id.bb_bottom_bar_title);
                    title.Text = bottomBarItemBase.GetTitle(_context);

                    if (_pendingTextAppearance != -1)
                    {
                        MiscUtils.SetTextAppearance(title, _pendingTextAppearance);
                    }

                    if (!bottomBarItemBase.IsEnabled)
                    {
                        title.Alpha = 0.5F;
                    }

                    if (_pendingTypeface != null)
                    {
                        title.Typeface = (_pendingTypeface);
                    }
                }

                if (bottomBarItemBase is BottomBarTab)
                {
                    bottomBarTab.Id = (((BottomBarTab)bottomBarItemBase).Id);
                }

                if (index == CurrentTabPosition && bottomBarItemBase.IsEnabled)
                {
                    SelectTab(bottomBarTab);
                }
                else
                {
                    UnselectTab(bottomBarTab);
                }

                if (!_isTabletMode)
                {
                    if (bottomBarTab.Width > biggestWidth)
                    {
                        biggestWidth = bottomBarTab.Width;
                    }

                    viewsToAdd[index] = bottomBarTab;
                }
                else
                {
                    ItemContainer.AddView(bottomBarTab);
                }

                bottomBarTab.SetOnClickListener(this);
                bottomBarTab.SetOnLongClickListener(this);
                index++;
            }

            if (!_isTabletMode)
            {
                int proposedItemWidth = Math.Min(
                                            MiscUtils.DpToPixel(_context, _screenWidth / bottomBarItems.Length),
                                            _maxFixedItemWidth
                                        );

                _inActiveShiftingItemWidth = (int)(proposedItemWidth * 0.9);
                _activeShiftingItemWidth = (int)(proposedItemWidth + (proposedItemWidth * (bottomBarItems.Length * 0.1)));

                var height = (int)Math.Round(_context.Resources.GetDimension(Resource.Dimension.bb_height));
                foreach (var bottomBarView in viewsToAdd)
                {
                    LinearLayout.LayoutParams param;

                    if (_isShiftingMode)
                    {
                        if (TAG_BOTTOM_BAR_VIEW_ACTIVE.Equals(bottomBarView.Tag))
                            param = new LinearLayout.LayoutParams(_activeShiftingItemWidth, height);
                        else
                            param = new LinearLayout.LayoutParams(_inActiveShiftingItemWidth, height);
                    }
                    else
                        param = new LinearLayout.LayoutParams(proposedItemWidth, height);


                    bottomBarView.LayoutParameters = param;
                    ItemContainer.AddView(bottomBarView);
                }
            }

            if (_pendingTextAppearance != -1)
            {
                _pendingTextAppearance = -1;
            }

            if (_pendingTypeface != null)
            {
                _pendingTypeface = null;
            }
        }

        private void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            if (savedInstanceState != null)
            {
                CurrentTabPosition = savedInstanceState.GetInt(STATE_CURRENT_SELECTED_TAB, -1);

                if (CurrentTabPosition == -1)
                {
                    CurrentTabPosition = 0;
                    Log.Error("BottomBar", "You must override the Activity's onSave" +
                        "InstanceState(Bundle outState) and call BottomBar.onSaveInstanc" +
                        "eState(outState) there to restore the state properly.");
                }

                _shouldUpdateFragmentInitially = true;
            }
        }

        /// <summary>
        /// Selects the tab.
        /// </summary>
        /// <param name="tab">Tab.</param>
        private void SelectTab(View tab)
        {
            tab.Tag = TAG_BOTTOM_BAR_VIEW_ACTIVE;
            var icon = (AppCompatImageView)tab.FindViewById(Resource.Id.bb_bottom_bar_icon);
            TextView title = (TextView)tab.FindViewById(Resource.Id.bb_bottom_bar_title);

            int tabPosition = FindItemPosition(tab);

            if (icon != null)
            {
                if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName0")
                {
                    icon.SetImageResource(Resource.Drawable.search2);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName1")
                {
                    //icon.SetImageResource(Resource.Drawable.order);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName2")
                {
                    //icon.SetImageResource(Resource.Drawable.chart);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName3")
                {
                    icon.SetImageResource(Resource.Drawable.bike_menu);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName4")
                {
                    icon.SetImageResource(Resource.Drawable.profile);
                }

            }

            if (title != null)
                //title.SetTextColor(_whiteColor);
                title.SetTextColor(Color.ParseColor("#fbfbfb"));
        }

        /// <summary>
        /// Unselects the tab.
        /// </summary>
        /// <param name="tab">Tab.</param>
        private void UnselectTab(View tab)
        {
            tab.Tag = (TAG_BOTTOM_BAR_VIEW_INACTIVE);

            var icon = (AppCompatImageView)tab.FindViewById(Resource.Id.bb_bottom_bar_icon);
            TextView title = (TextView)tab.FindViewById(Resource.Id.bb_bottom_bar_title);

            int tabPosition = FindItemPosition(tab);

            if (icon != null)
            {
                if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName0")
                {
                    icon.SetImageResource(Resource.Drawable.search2);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName1")
                {
                    //icon.SetImageResource(Resource.Drawable.order_2);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName2")
                {
                    //icon.SetImageResource(Resource.Drawable.chart_2);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName3")
                {
                    icon.SetImageResource(Resource.Drawable.bike_menu);
                }
                else if (icon.GetTag(Resource.Id.TAG_IMAGE_NAME).ToString() == "iconName4")
                {
                    icon.SetImageResource(Resource.Drawable.profile);
                }

            }

            if (title != null)
                title.SetTextColor(Color.ParseColor("#808080"));
        }

        private int FindItemPosition(View viewToFind)
        {
            int position = 0;

            for (int i = 0; i < ItemContainer.ChildCount; i++)
            {
                View candidate = ItemContainer.GetChildAt(i);

                if (candidate.Equals(viewToFind))
                {
                    position = i;
                    break;
                }
            }

            return position;
        }

        private void UpdateCurrentFragment()
        {
            if (!_shouldUpdateFragmentInitially && _fragmentManager != null
                && _fragmentContainer != 0
                && _items != null
                && _items is BottomBarFragment[])
            {
                var newFragment = ((BottomBarFragment)_items[CurrentTabPosition]);

                if (_fragmentManager is Android.Support.V4.App.FragmentManager
                    && newFragment.SupportFragment != null)
                {
                    ((Android.Support.V4.App.FragmentManager)_fragmentManager).BeginTransaction()
                        .Replace(_fragmentContainer, newFragment.SupportFragment)
                        .Commit();
                }
                else if (_fragmentManager is Android.App.FragmentManager
                         && newFragment.Fragment != null)
                {
                    ((Android.App.FragmentManager)_fragmentManager).BeginTransaction()
                        .Replace(_fragmentContainer, newFragment.Fragment)
                        .Commit();
                }
            }

            _shouldUpdateFragmentInitially = false;
        }

        private void ClearItems()
        {
            if (ItemContainer != null)
            {
                ItemContainer.RemoveAllViews();
            }

            if (_fragmentManager != null)
            {
                _fragmentManager = null;
            }

            if (_fragmentContainer != 0)
            {
                _fragmentContainer = 0;
            }

            if (_items != null)
            {
                _items = null;
            }
        }

        /// <summary>
        /// Navs the bar magic.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="bottomBar">Bottom bar.</param>
        private static void NavBarMagic(Activity activity, BottomBar bottomBar)
        {
            var res = activity.Resources;

            int softMenuIdentifier = res.GetIdentifier("config_showNavigationBar", "bool", "android");
            int navBarIdentifier = res.GetIdentifier("navigation_bar_height", "dimen", "android");
            int navBarHeight = 0;

            if (navBarIdentifier > 0)
            {
                navBarHeight = res.GetDimensionPixelSize(navBarIdentifier);
            }

            if (!bottomBar.DrawBehindNavBar || navBarHeight == 0)
            {
                return;
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich
                && ViewConfiguration.Get(activity).HasPermanentMenuKey)
            {
                return;
            }

            if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBeanMr1 &&
                (!(softMenuIdentifier > 0 && res.GetBoolean(softMenuIdentifier))))
                return;

            /* Copy-paste coding made possible by: http://stackoverflow.com/a/14871974/940036 */
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
            {
                var d = activity.WindowManager.DefaultDisplay;

                DisplayMetrics realDisplayMetrics = new DisplayMetrics();
                d.GetRealMetrics(realDisplayMetrics);

                int realHeight = realDisplayMetrics.HeightPixels;
                int realWidth = realDisplayMetrics.WidthPixels;

                DisplayMetrics displayMetrics = new DisplayMetrics();
                d.GetMetrics(displayMetrics);

                int displayHeight = displayMetrics.HeightPixels;
                int displayWidth = displayMetrics.WidthPixels;

                var hasSoftwareKeys = (realWidth - displayWidth) > 0
                                      || (realHeight - displayHeight) > 0;

                if (!hasSoftwareKeys)
                {
                    return;
                }
            }
            /* End of delicious copy-paste code */

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat
                && res.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                activity.Window.Attributes.Flags |= WindowManagerFlags.TranslucentNavigation;

                View outerContainer = bottomBar.OuterContainer;
                int navBarHeightCopy = navBarHeight;
                bottomBar.ViewTreeObserver.AddOnGlobalLayoutListener(new NavBarMagicOnGlobalLayoutListener(bottomBar, outerContainer, navBarHeightCopy, bottomBar._isTabletMode));
            }
        }
    }
}
