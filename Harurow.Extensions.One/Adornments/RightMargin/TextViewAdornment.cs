using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments.RightMargin
{
    internal sealed class TextViewAdornment : ITextViewAdornment
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private int RightMargin { get; }
        private Brush Brush { get; }

        private Image Image { get; set; }

        public TextViewAdornment(IWpfTextView textView, IAdornmentLayer layer, int rightMargin, Brush brush)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            RightMargin = rightMargin;
            Brush = brush;
        }

        public void OnInitialized()
        {
            CreateVisuals();
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (e.NewViewState.ViewportWidth != e.OldViewState.ViewportWidth ||
                e.NewViewState.ViewportHeight != e.OldViewState.ViewportHeight)
            {
                CleanUp();
                CreateVisuals();
                return;
            }

            if (e.NewViewState.ViewportRight != e.OldViewState.ViewportRight)
            {
                Canvas.SetLeft(Image, CalcLeft());
            }

            if (e.NewViewState.ViewportTop != e.OldViewState.ViewportTop)
            {
                Canvas.SetTop(Image, TextView.ViewportTop);
            }
        }

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
            Image = CreateImage();

            Canvas.SetTop(Image, TextView.ViewportTop);
            Canvas.SetLeft(Image, CalcLeft());

            AdornmentLayer.AddAdornment(null, Image);
        }

        private Image CreateImage()
        {
            var rect = new Rect(0, 0, TextView.ViewportWidth, TextView.ViewportHeight);
            var geometry = new RectangleGeometry(rect).FreezeAnd();
            var image = geometry.ToImage(Brush, null);
            Panel.SetZIndex(image, int.MinValue);
            return image;
        }

        private double CalcLeft()
        {
            var lineSource = TextView.FormattedLineSource;
            if (lineSource == null)
            {
                return -1;
            }

            var x = lineSource.BaseIndentation + lineSource.ColumnWidth * RightMargin;

            if (x < TextView.ViewportLeft)
            {
                x = TextView.ViewportLeft;
            }

            return x;
        }

    }
}