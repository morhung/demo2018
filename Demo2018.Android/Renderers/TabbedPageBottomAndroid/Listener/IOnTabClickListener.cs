using System;
namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Listener
{
    public interface IOnTabClickListener
    {
        /// <summary>
        /// The method being called when currently visible <see cref="BottomBarTab"/> changes.
        /// This listener is fired for the first time after the items have been set and
        /// also after a configuration change, such as when screen orientation changes
        /// from portrait to landscape.
        /// </summary>
        /// <param name="position">position the new visible <see cref="BottomBarTab"/></param>
        void OnTabSelected(int position);

        /// <summary>
        /// The method being called when currently visible <see cref="BottomBarTab"/> is
        /// reselected. Use this method for scrolling to the top of your content,
        /// as recommended by the Material Design spec
        /// </summary>
        /// <param name="position">osition the <see cref="BottomBarTab"/> that was reselected.</param>
        void OnTabReSelected(int position);
    }
}
