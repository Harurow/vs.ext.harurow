using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.CaretIndicator.Adronments
{
    internal sealed class LineIndicatorAdornment : ICaretIndicatorAdornment
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Pen LineIndicatorPen { get; }

        private Image LineIndicatorImage { get; set; }

        public LineIndicatorAdornment(IWpfTextView textView, IAdornmentLayer layer, Pen lineIndicatorPen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            LineIndicatorPen = lineIndicatorPen;
        }

        public void Dispose()
        {
            CleanUp();
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

        public void OnInitialized()
        {
            CreateIndicator();
        }

        public void CreateIndicator()
        {
            LineIndicatorImage = CreateHorizontalLineImage();
            AdornmentLayer.AddAdornment(null, LineIndicatorImage);
        }

        private Image CreateHorizontalLineImage()
        {
            var geometry = new LineGeometry(new Point(0, 0), new Point(TextView.ViewportWidth, 0)).FreezeAnd();
            var image = geometry.ToImage(null, LineIndicatorPen);
            Canvas.SetTop(image, GetSafeCaretBottom());
            Canvas.SetLeft(image, TextView.ViewportLeft);
            Panel.SetZIndex(image, 102);
            return image;
        }

        public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (LineIndicatorImage == null)
            {
                return;
            }

            var caretBottom = GetSafeCaretBottom();
            if (int.MinValue < caretBottom)
            {
                Canvas.SetTop(LineIndicatorImage, caretBottom);
            }
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (e.NewViewState.ViewportWidth.IsNotEquals(e.OldViewState.ViewportWidth))
            {
                CleanUp();
                CreateIndicator();
                return;
            }

            var caretBottom = GetSafeCaretBottom();
            if (int.MinValue < caretBottom)
            {
                Canvas.SetTop(LineIndicatorImage, caretBottom);
            }

            if (e.NewViewState.ViewportLeft.IsNotEquals(e.OldViewState.ViewportLeft))
            {
                Canvas.SetLeft(LineIndicatorImage, e.NewViewState.ViewportLeft);
            }
        }

        public void CleanUp()
        {
            if (LineIndicatorImage != null)
            {
                AdornmentLayer.RemoveAdornment(LineIndicatorImage);
                LineIndicatorImage = null;
            }
        }
    }
}