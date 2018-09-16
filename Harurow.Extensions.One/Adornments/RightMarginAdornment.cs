using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments
{
    internal sealed class RightMarginAdornment : IAdornment
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private int RightMargin { get; }
        private Brush RightMarginBrush { get; }

        private Image RightMarginImage { get; set; }

        public RightMarginAdornment(IWpfTextView textView, IAdornmentLayer layer, int rightMargin, Brush brush)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            RightMargin = rightMargin;
            RightMarginBrush = brush;
        }

        public void OnInitialized()
        {
            CreateRightMargin();
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (e.NewViewState.ViewportWidth != e.OldViewState.ViewportWidth ||
                e.NewViewState.ViewportHeight != e.OldViewState.ViewportHeight)
            {
                CleanUp();
                CreateRightMargin();
                return;
            }

            if (e.NewViewState.ViewportRight != e.OldViewState.ViewportRight)
            {
                Canvas.SetLeft(RightMarginImage, CalcLeft());
            }

            if (e.NewViewState.ViewportTop != e.OldViewState.ViewportTop)
            {
                Canvas.SetTop(RightMarginImage, TextView.ViewportTop);
            }
        }

        public void CleanUp()
        {
            if (RightMarginImage != null)
            {
                AdornmentLayer.RemoveAdornment(RightMarginImage);
                RightMarginImage = null;
            }
        }

        private void CreateRightMargin()
        {
            RightMarginImage = CreateRightMarginImage();

            Canvas.SetTop(RightMarginImage, TextView.ViewportTop);
            Canvas.SetLeft(RightMarginImage, CalcLeft());

            AdornmentLayer.AddAdornment(null, RightMarginImage);
        }

        private Image CreateRightMarginImage()
        {
            var rect = new Rect(0, 0, TextView.ViewportWidth, TextView.ViewportHeight);
            var geometry = new RectangleGeometry(rect).FreezeAnd();
            var image = geometry.ToImage(RightMarginBrush, null);
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