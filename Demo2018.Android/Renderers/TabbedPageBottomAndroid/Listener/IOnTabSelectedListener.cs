using System;
namespace Demo2018.Droid.Renderers.TabbedPageBottomAndroid.Listener
{
    public interface IOnTabSelectedListener
    {
        /// <summary>
        /// The method being called when currently visible <see cref="BottomBarTab"/> changes.
        /// This listener won't be fired until the user changes the selected item the
        /// first time. So you won't get this event when you're just initialized the BottomBar.
        /// </summary>
        /// <param name="position">position the new visible <see cref="BottomBarTab"/>.</param>
        void OnItemSelected(int position);
    }
}
