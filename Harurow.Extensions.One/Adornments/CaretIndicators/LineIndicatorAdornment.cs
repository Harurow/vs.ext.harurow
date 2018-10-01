using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Editor;

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

            if (e.HorizontalTranslation)
            {
                if (Left != e.NewViewState.ViewportLeft)
                {
                    Canvas.SetLeft(Image, e.NewViewState.ViewportLeft);
                    Left = e.NewViewState.ViewportLeft;
                }
            }

            var y = GetSafeCaretBottom();
            if (int.MinValue < y)
            {
                if (Top != y)
                {
                    Canvas.SetTop(Image, y);
                    Top = y;
                }
            }
        }

        /// <inheritdoc />
        public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            var y = GetSafeCaretBottom();
            if (int.MinValue < y)
            {
                if (Top != y)
                {
                    Canvas.SetTop(Image, y);
                    Top = y;
                }
            }
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
            var img = geo.ToImage(null, Pen);
            Top = GetSafeCaretBottom();
            Left = TextView.ViewportLeft;
            Canvas.SetTop(img, Top);
            Canvas.SetLeft(img, Left);
            Panel.SetZIndex(img, 102);
            Image = img;
            AdornmentLayer.AddAdornment(typeof(LineIndicatorAdornment), Image);
        }

        private double GetSafeCaretBottom()
        {
            try
            {
                return TextView.Caret.Bottom + 1;
            }
            catch (InvalidOperationException)
            {
                return int.MinValue;
            }
        }
    }
}