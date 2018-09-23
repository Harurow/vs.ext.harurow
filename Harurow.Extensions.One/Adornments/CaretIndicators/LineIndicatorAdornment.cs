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
                Canvas.SetLeft(Image, e.NewViewState.ViewportLeft);
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
                Canvas.SetTop(Image, y);
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
            Canvas.SetTop(img, GetSafeCaretBottom());
            Canvas.SetLeft(img, TextView.ViewportLeft);
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