using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Editor;
using static System.Double;

namespace Harurow.Extensions.One.Adornments.CaretIndicators
{
    internal sealed class LineIndicatorAdornment : ICaretIndicatorAdornment
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Pen Pen { get; }

        private double Left { get; set; }
        private double Top { get; set; }
        private Image Image { get; set; }

        public LineIndicatorAdornment(IWpfTextView textView, IAdornmentLayer layer, Pen pen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            Pen = pen;
        }

        /// <inheritdoc />
        public void OnInitialized()
        {
            CreateVisuals();
        }

        /// <inheritdoc />
        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (e.NewViewState.ViewportWidth != e.OldViewState.ViewportWidth)
            {
                CleanUp();
                CreateVisuals();
                return;
            }

            SetPosition(TextView.ViewportLeft, GetSafeCaretBottom());
        }

        /// <inheritdoc />
        public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            SetPosition(TextView.ViewportLeft, GetSafeCaretBottom());
        }

        /// <inheritdoc />
        public void OnSelectionChanged(object sender, EventArgs e)
        {
            if (Image != null)
            {
                if (TextView.Selection.IsEmpty)
                {
                    if (Image.Visibility == Visibility.Hidden)
                    {
                        Image.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (Image.Visibility == Visibility.Visible)
                    {
                        Image.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        /// <inheritdoc />
        public void CleanUp()
        {
            if (Image != null)
            {
                AdornmentLayer.RemoveAdornment(Image);
                Image = null;
            }
        }

        private void CreateVisuals()
        {
            var geo = new LineGeometry(new Point(0, 0), new Point(TextView.ViewportWidth, 0)).FreezeAnd();
            Image = geo.ToImage(null, Pen);
            Panel.SetZIndex(Image, 102);
            SetPosition(TextView.ViewportLeft, GetSafeCaretBottom());
            AdornmentLayer.AddAdornment(typeof(LineIndicatorAdornment), Image);
        }

        private void SetPosition(double x, double y)
        {
            var nextVisible = TextView.Selection.IsEmpty;

            if (x != Left)
            {
                Canvas.SetLeft(Image, x);
                Left = x;
            }

            if (y != Top)
            {
                if (y != NaN)
                {
                    Canvas.SetTop(Image, y);
                }
                else
                {
                    nextVisible = false;
                }
                Top = y;
            }

            if (nextVisible && Image.Visibility != Visibility.Visible)
            {
                Image.Visibility = Visibility.Visible;
            }
            else if (!nextVisible && Image.Visibility == Visibility.Visible)
            {
                Image.Visibility = Visibility.Hidden;
            }
        }

    private double GetSafeCaretBottom()
        {
            try
            {
                return TextView.Caret.Bottom + 1;
            }
            catch (InvalidOperationException)
            {
                return NaN;
            }
        }
    }
}