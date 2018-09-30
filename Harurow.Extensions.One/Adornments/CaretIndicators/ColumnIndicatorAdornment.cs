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

        private double Left { get; set; }
        private double Top { get; set; }
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
                if (Top != e.NewViewState.ViewportTop)
                {
                    Canvas.SetTop(Image, e.NewViewState.ViewportTop);
                    Top = e.NewViewState.ViewportTop;
                }
            }

            if (Left != TextView.Caret.Left)
            {
                Canvas.SetLeft(Image, TextView.Caret.Left);
                Left = TextView.Caret.Left;
            }
        }

        /// <inheritdoc />
        public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            if (Left != TextView.Caret.Left)
            {
                Canvas.SetLeft(Image, TextView.Caret.Left);
                Left = TextView.Caret.Left;
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
            var geo = new LineGeometry(new Point(0, 0), new Point(0, TextView.ViewportHeight)).FreezeAnd();
            var img = geo.ToImage(null, Pen);
            Top = TextView.ViewportTop;
            Left = TextView.Caret.Left;
            Canvas.SetTop(img, Top);
            Canvas.SetLeft(img, Left);
            Panel.SetZIndex(img, 101);
            Image = img;
            AdornmentLayer.AddAdornment(typeof(LineIndicatorAdornment), Image);
        }
    }
}