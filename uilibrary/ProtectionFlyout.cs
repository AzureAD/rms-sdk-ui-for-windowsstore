using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.RightsManagement.UILibrary
{
    public class ProtectionFlyout : UserControl
    {
        public ProtectionFlyout()
        {
        }

        protected bool Show()
        {
            // Initialize if needed, i.e. listen to various events, set up the animation
            Initialize();

            // The popup is not dismissed when an appbar is opened or close. So we need to listen to appbar close/open events manually to dismiss our popup.
            _allAppBars = this.GetAllAppBarsInApp();

            foreach (var appBar in _allAppBars)
            {
                appBar.Opened += DismissPopup;
                appBar.Closed += DismissPopup;
            }

            this.UpdateFlyoutGeometry();

            // Start the update geometry timer to update the geometry periodically
            _updateFlyoutGeometryTimer.Start();

            if (this.IsAutoDismissEnabled)
            {
                // if auto dismiss is enabled start the dismiss timer
                _dismissTimer.Start();
            }

            this.Popup.IsOpen = true;

            return true;
        }

        protected void Hide()
        {
            this.Popup.IsOpen = false;
        }

        public bool IsAutoDismissEnabled
        {
            get;
            set;
        }

        public event RoutedEventHandler Closed;

        private void Initialize()
        {
            if (!_initialized)
            {
                // Set up the update geometry timer
                _updateFlyoutGeometryTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                _updateFlyoutGeometryTimer.Tick += _updateGeometryTimer_Tick;

                // Set up the dismiss timer
                _dismissTimer.Interval = new TimeSpan(0, 0, 5);
                _dismissTimer.Tick += _dismissTimer_Tick;

                // Add the open/close animation
                this.Popup.ChildTransitions = new TransitionCollection();
                this.Popup.ChildTransitions.Add(new PaneThemeTransition() { Edge = EdgeTransitionLocation.Right} );

                // Handle Closed event for cleanup
                this.Popup.Closed += Popup_Closed;

                // Intercept the right click block opening/closing app bar when right clicking on the popup
                this.Child.RightTapped += Child_RightTapped;

                _initialized = true;
            }
        }

        private void Popup_Closed(object sender, object e)
        {
            // Unregister the appbar open/close handlers
            foreach (var appBar in _allAppBars)
            {
                appBar.Opened -= DismissPopup;
                appBar.Closed -= DismissPopup;
            }

            // Stop the update geometry timer
            _updateFlyoutGeometryTimer.Stop();

            // Stop the dismiss timer
            _dismissTimer.Stop();

            if (this.Closed != null)
            {
                this.Closed(this, new RoutedEventArgs());
            }
        }

        private void UpdateFlyoutGeometry()
        {
            var windowRect = Window.Current.Bounds;

            var flyoutRect = new Rect 
            {
                X = windowRect.Width - FlyoutWidth,
                Y = 0.0,
                Width = FlyoutWidth,
                Height = windowRect.Height
            };

            foreach (var appBar in _allAppBars)
            {
                if (appBar.IsOpen)
                {
                    var appBarPosition = appBar.TransformToVisual(null).TransformPoint(new Point(0, 0));
                    var appBarRect = new Rect(appBarPosition, appBar.RenderSize);

                    // If the top of the app bar is < 3 then we assume that it is a top app bar.
                    // If the bottom of the app bar is > screen height - 3, then we assume that it is a bottom app bar.
                    const double tolerance = 3;

                    if (appBarPosition.Y <= tolerance)
                    {
                        // If this is a top app bar make sure that the upper part of the flyout doesn't overlap with the app bar

                        if (flyoutRect.Top < appBarRect.Bottom)
                        {
                            // If the flyout overlaps with the appbar, move the flyout's top to the bottom of the appbar, such that it doesn't overlap 
                            flyoutRect.Height = flyoutRect.Height - (appBarRect.Bottom - flyoutRect.Top);
                            flyoutRect.Y = appBarRect.Bottom;
                        }
                    }
                    else if (appBarRect.Bottom >= windowRect.Height - tolerance)
                    {
                        // If this is a bottom app bar make sure that the lower part of the flyout doesn't overlap with the app bar

                        if (flyoutRect.Bottom > appBarRect.Top)
                        {
                            // If the flyout overlaps with the appbar, move the flyout's bottom to the top of the appbar, such that it doesn't overlap 
                            flyoutRect.Height = flyoutRect.Height - (flyoutRect.Bottom - appBarRect.Top);
                        }
                    }
                }
            }

            this.Popup.HorizontalOffset = flyoutRect.Left;
            this.Popup.VerticalOffset = flyoutRect.Top;

            this.Popup.Width = flyoutRect.Width;
            this.Popup.Height = flyoutRect.Height;

            this.Child.Width = flyoutRect.Width;
            this.Child.Height = flyoutRect.Height;
        }

        private void DismissPopup(object sender, object e)
        {
            this.Popup.IsOpen = false;
        }

        private List<AppBar> GetAllAppBarsInApp()
        {
            var appBars = new List<AppBar>();
            this.GetAllAppBars(Window.Current.Content, appBars);
            return appBars;
        }

        private void GetAllAppBars(DependencyObject root, List<AppBar> appBars)
        {
            var page = root as Page;

            // If the root is a page add its app bars to the list
            if (page != null)
            {
                if (page.BottomAppBar != null)
                {
                    appBars.Add(page.BottomAppBar);
                }

                if (page.TopAppBar != null)
                {
                    appBars.Add(page.TopAppBar);
                }
            }

            // Call this function recursively for the children of the root
            int childrenCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childrenCount; i++)
            {
                this.GetAllAppBars(VisualTreeHelper.GetChild(root, i), appBars);
            }
        }

        private void _updateGeometryTimer_Tick(object sender, object e)
        {
            this.UpdateFlyoutGeometry();
        }

        private void _dismissTimer_Tick(object sender, object e)
        {
            if (this.Popup.IsOpen)
            {
                this.Hide();
            }
        }

        private void Child_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            // Don't allow right click to go through. It will open the app bar and close our popup.
            e.Handled = true;
        }

        private Popup Popup
        {
            get
            {
                return base.Content as Popup;
            }
        }

        private FrameworkElement Child
        {
            get
            {
                return this.Popup.Child as FrameworkElement;
            }
        }


        private bool _initialized = false;
        private List<AppBar> _allAppBars = null;
        readonly private DispatcherTimer _updateFlyoutGeometryTimer = new DispatcherTimer();
        readonly private DispatcherTimer _dismissTimer = new DispatcherTimer();

        protected const double FlyoutWidth = 400;
    }
}
