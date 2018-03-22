using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.CaretIndicator.Adronments
{
    internal sealed class ColumnIndicatorAdornment : ICaretIndicatorAdornment
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Pen ColumnIndicatorPen { get; }

        private double CaretLeft { get; set; }
        private Image ColumnIndicatorImage { get; set; }

        public ColumnIndicatorAdornment(IWpfTextView textView, IAdornmentLayer layer, Pen columnIndicatorPen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            ColumnIndicatorPen = columnIndicatorPen;
        }

        public void Dispose()
        {
            CleanUp();
        }

        public void OnInitialized()
        {
            CreateIndicator();
        }

        private void CreateIndicator()
        {
            ColumnIndicatorImage = CreateVirticalLineImage();
            CaretLeft = TextView.Caret.Left - 1;
            AdornmentLayer.AddAdornment(null, ColumnIndicatorImage);
        }

        private Image CreateVirticalLineImage()
        {
            var geometry = new LineGeometry(new Point(0, 0), new Point(0, TextView.ViewportHeight)).FreezeAnd();
            var image = geometry.ToImage(null, ColumnIndicatorPen);
            Canvas.SetTop(image, TextView.ViewportTop);
            Canvas.SetLeft(image, TextView.Caret.Left);
            Panel.SetZIndex(image, 101);
            return image;
        }

        public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (ColumnIndicatorImage != null && CaretLeft.IsNotEquals(TextView.Caret.Left))
            {
                Canvas.SetLeft(ColumnIndicatorImage, TextView.Caret.Left);
                CaretLeft = TextView.Caret.Left - 1;
            }
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (e.NewViewState.ViewportHeight.IsNotEquals(e.OldViewState.ViewportHeight))
            {
                CleanUp();
                CreateIndicator();
                return;
            }

            if (e.NewViewState.ViewportTop.IsNotEquals(e.OldViewState.ViewportTop))
            {
                Canvas.SetTop(ColumnIndicatorImage, TextView.ViewportTop);
            }

            if (e.NewViewState.ViewportLeft.IsNotEquals(e.OldViewState.ViewportLeft) ||
                CaretLeft.IsNotEquals(TextView.Caret.Left))
            {
                Canvas.SetLeft(ColumnIndicatorImage, TextView.Caret.Left);
                CaretLeft = TextView.Caret.Left - 1;
            }
        }

        private void CleanUp()
        {
            if (ColumnIndicatorImage != null)
            {
                AdornmentLayer.RemoveAdornment(ColumnIndicatorImage);
                ColumnIndicatorImage = null;
                CaretLeft = 0;
            }
        }
    }
}