using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments.CaretIndicators
{
    internal sealed class ColumnIndicatorAdornment : ICaretIndicatorAdornment
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Pen Pen { get; }

        private Image Image { get; set; }

        public ColumnIndicatorAdornment(IWpfTextView textView, IAdornmentLayer layer, Pen pen)
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
            if (e.NewViewState.ViewportHeight != e.OldViewState.ViewportHeight)
            {
                CleanUp();
                CreateVisuals();
                return;
            }

            if (e.VerticalTranslation)
            {
                Canvas.SetTop(Image, e.NewViewState.ViewportTop);
            }

            Canvas.SetLeft(Image, TextView.Caret.Left);
        }

        /// <inheritdoc />
        public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            Canvas.SetLeft(Image, TextView.Caret.Left);
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
            var geo = new LineGeometry(new Point(0, 0), new Point(0, TextView.ViewportHeight)).FreezeAnd();
            var img = geo.ToImage(null, Pen);
            Canvas.SetTop(img, TextView.ViewportTop);
            Canvas.SetLeft(img, TextView.Caret.Left);
            Panel.SetZIndex(img, 101);
            Image = img;
            AdornmentLayer.AddAdornment(typeof(LineIndicatorAdornment), Image);
        }
    }
}