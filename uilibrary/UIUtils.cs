namespace Microsoft.RightsManagement.UILibrary
{
    using System;
    using System.Text;
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;

    internal static class UIUtils
    {
        public static bool IsControlInUpperHalf(UIElement control)
        {
            var controlTransform = control.TransformToVisual(null);
            var controlPoint = controlTransform.TransformPoint(new Point(0, 0));

            return controlPoint.Y < Window.Current.Bounds.Height / 2.0;
        }

        public static String WrapString(String str, int width)
        {
            var wrappedBuilder = new StringBuilder();

            while (str.Length > width)
            {
                String str1, str2;

                BreakString(str, width, out str1, out str2);

                wrappedBuilder.Append(str1);

                str = str2;
            }

            // append the last piece
            wrappedBuilder.Append(str);

            return wrappedBuilder.ToString();
        }

        private static void BreakString(String str, int breakIndex, out String line1, out String line2)
        {
            var lineEndIndex = breakIndex;

            // find a space-character from where to break
            while (!Char.IsWhiteSpace(str[lineEndIndex]) && lineEndIndex > 0)
            {
                --lineEndIndex;
            }

            // skip space characters
            while (Char.IsWhiteSpace(str[lineEndIndex]) && lineEndIndex > 0)
            {
                --lineEndIndex;
            }

            // if there is no space to break, then break in the middle of the word
            if (lineEndIndex == 0)
            {
                lineEndIndex = breakIndex;
            }

            line1 = str.Substring(0, lineEndIndex + 1);

            var nextLineStartIndex = lineEndIndex + 1;

            // skip space
            while (nextLineStartIndex < str.Length && Char.IsWhiteSpace(str[nextLineStartIndex]))
            {
                ++nextLineStartIndex;
            }

            // if there is nothing left then it is an empty string
            if (nextLineStartIndex >= str.Length)
            {
                line2 = String.Empty;
            }
            else
            {
                line2 = str.Substring(nextLineStartIndex);

                // add a newline to the end of line1, because it is not the last line
                line1 += "\n";
            }
        }

        public static void PositionPopup(Popup popup, UIElement anchor, UIElement popupContent)
        {
            var anchorTransform = anchor.TransformToVisual(null);
            var anchorPoint = anchorTransform.TransformPoint(new Point(0, 0));

            // measure the contents of the popup
            popupContent.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            var popupWidth = popupContent.DesiredSize.Width;
            var popupHeight = popupContent.DesiredSize.Height;

            var anchorWidth = anchor.RenderSize.Width;
            var anchorHeight = anchor.RenderSize.Height;

            // try to align the centers of the popup and the anchor
            var x = anchorPoint.X - (popupWidth - anchorWidth) / 2.0;

            x = Math.Max(x, 0.0);
            x = Math.Min(x, Window.Current.Bounds.Width - popupWidth);

            var y = 0.0;

            if (anchorPoint.Y < Window.Current.Bounds.Height / 2.0)
            {
                // if anchor is in the top half of the screen
                // then position the popup below the anchor
                y = anchorPoint.Y + anchorHeight;
            }
            else
            {
                // if anchor is in the bottom half of the screen
                // then position the popup above the anchor
                y = anchorPoint.Y - popupHeight;
            }

            popup.HorizontalOffset = x;
            popup.VerticalOffset = y;
        }

        public static void PositionPopup(Popup popup, Point anchorPoint, UIElement popupContent)
        {
            // measure the contents of the popup
            popupContent.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            var popupWidth = popupContent.DesiredSize.Width;
            var popupHeight = popupContent.DesiredSize.Height;

            var x = anchorPoint.X - popupWidth / 2.0;

            x = Math.Max(x, 0.0);
            x = Math.Min(x, Window.Current.Bounds.Width - popupWidth);

            var y = 0.0;

            if (anchorPoint.Y < Window.Current.Bounds.Height / 2.0)
            {
                // if anchor point is in the top half of the screen
                // then position the popup below the anchor
                y = anchorPoint.Y;
            }
            else
            {
                // if anchor is in the bottom half of the screen
                // then position the popup above the anchor point
                y = anchorPoint.Y - popupHeight;
            }

            popup.HorizontalOffset = x;
            popup.VerticalOffset = y;
        }

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
