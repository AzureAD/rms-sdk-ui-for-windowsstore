namespace Microsoft.RightsManagement.UILibrary
{
    using System;
    using System.Text;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;

    internal static class UIUtils
    {

        public static double ControlMarginWidth(FrameworkElement control)
        {
            return control.Margin.Left + control.Margin.Right;
        }

        public static Size ControlSize(FrameworkElement control)
        {
            if (control.Visibility == Visibility.Collapsed)
            {
                return new Size(0.0, 0.0);
            }

            // Call measure to update the DesiredSize according to the content of the control
            control.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            return control.DesiredSize;
        }
    }
}
