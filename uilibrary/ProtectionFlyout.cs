using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
// ReSharper disable All

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

            if (HostingPage != null)
            {
                AddDismissListners(HostingPage.TopAppBar);
                AddDismissListners(HostingPage.BottomAppBar);
            }

            UpdateFlyoutGeometry();

            // Start the update geometry timer to update the geometry periodically
            _updateFlyoutGeometryTimer.Start();

            if (IsAutoDismissEnabled)
            {
                // if auto dismiss is enabled start the dismiss timer
                _dismissTimer.Start();
            }

            Popup.IsOpen = true;

            return true;
        }

        protected void Hide()
        {
            Popup.IsOpen = false;
        }

        public bool IsAutoDismissEnabled {get; set;}

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
                Popup.ChildTransitions = new TransitionCollection();
                Popup.ChildTransitions.Add(new PaneThemeTransition() { Edge = EdgeTransitionLocation.Right} );

                // Handle Closed event for cleanup
                Popup.Closed += Popup_Closed;

                // Intercept the right click block opening/closing app bar when right clicking on the popup
                Child.RightTapped += Child_RightTapped;

                _initialized = true;
            }
        }

        private void Popup_Closed(object sender, object e)
        {
            // Unregister the appbar open/close handlers
            if (HostingPage != null)
            {
                RemoveDismissListners(HostingPage.TopAppBar);
                RemoveDismissListners(HostingPage.BottomAppBar);
            }

            // Stop the update geometry timer
            _updateFlyoutGeometryTimer.Stop();

            // Stop the dismiss timer
            _dismissTimer.Stop();

            if (Closed != null)
            {
                Closed(this, new RoutedEventArgs());
            }
        }

        private void UpdateFlyoutGeometry()
        {
            var windowRect = Window.Current.Bounds;

            Rect flyoutRect;
            var elem = windowRect.Width - FlyoutWidth;

            if (elem > 0)
            {
                flyoutRect = new Rect
                {
                    X = elem,
                    Y = 0.0,
                    Width = FlyoutWidth,
                    Height = windowRect.Height
                };
            }
            else
            {
                flyoutRect = new Rect
                {
                    X = 0.0,
                    Y = 0.0,
                    Width = windowRect.Width,
                    Height = windowRect.Height
                };
            }
            if (HostingPage != null)
            {
                if ((HostingPage.TopAppBar != null ) && (HostingPage.TopAppBar.IsOpen))
                {
                    var appBarRect = GetRectAppBar(HostingPage.TopAppBar);
                    // If the flyout overlaps with the appbar, move the flyout's top to the bottom of the appbar, such that it doesn't overlap 
                    flyoutRect.Height = flyoutRect.Height - (appBarRect.Bottom - flyoutRect.Top);
                    flyoutRect.Y = appBarRect.Bottom;                    
                }

                if ((HostingPage.BottomAppBar != null) && (HostingPage.BottomAppBar.IsOpen))
                {
                    var appBarRect = GetRectAppBar(HostingPage.TopAppBar);

                    if (flyoutRect.Bottom > appBarRect.Top)
                    {
                        // If the flyout overlaps with the appbar, move the flyout's bottom to the top of the appbar, such that it doesn't overlap 
                        flyoutRect.Height = flyoutRect.Height - (flyoutRect.Bottom - appBarRect.Top);
                    }
                }
            }

            Popup.HorizontalOffset = flyoutRect.Left;
            Popup.VerticalOffset = flyoutRect.Top;

            Popup.Width = flyoutRect.Width;
            Popup.Height = flyoutRect.Height;

            Child.Width = flyoutRect.Width;
            Child.Height = flyoutRect.Height;
        }

        private void DismissPopup(object sender, object e)
        {
            Popup.IsOpen = false;
        }

        public Page HostingPage {get; set; }

        private void _updateGeometryTimer_Tick(object sender, object e)
        {
            UpdateFlyoutGeometry();
        }

        private void _dismissTimer_Tick(object sender, object e)
        {
            if (Popup.IsOpen)
            {
                Hide();
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
                return Content as Popup;
            }
        }

        private FrameworkElement Child
        {
            get
            {
                return Popup.Child as FrameworkElement;
            }
        }

        private Rect GetRectAppBar(AppBar app)
        {
            var appBarPosition = app.TransformToVisual(null).TransformPoint(new Point(0, 0));
            return new Rect(appBarPosition, app.RenderSize);
        }

        private void AddDismissListners(AppBar app)
        {
            if (app != null)
            {
                app.Closed += DismissPopup;
                app.Opened += DismissPopup;
            }
        }

        private void RemoveDismissListners(AppBar app)
        {
            if (app != null)
            {
                app.Closed -= DismissPopup;
                app.Opened -= DismissPopup;
            }
        }

        private bool _initialized = false;
        readonly private DispatcherTimer _updateFlyoutGeometryTimer = new DispatcherTimer();
        readonly private DispatcherTimer _dismissTimer = new DispatcherTimer();

        protected const double FlyoutWidth = 400;
    }
}
