using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.EncodingInfo.Options;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.EncodingInfo.Adronments
{
    internal sealed class EncodingNameLabelAdornment : IAdornment
    {
        private const double Opacity = 0.9;

        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private IOptionValues Options { get; }
        private OptionResources Resources { get; }
        private ITextDocument Document { get; }

        private Image EncodingNameImage { get; set; }

        public EncodingNameLabelAdornment(IWpfTextView textView, IAdornmentLayer layer,
            IOptionValues options, OptionResources resources)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Resources = resources ?? throw new ArgumentNullException(nameof(resources));

            Document = TextView.GetTextDocument();
        }

        public void Dispose()
        {
            CleanUp();
        }

        public void OnInitialized()
        {
            var encoding = Document.Encoding;

            GetEncodingAdornment(encoding, out var displayname, out var foreBrush, out var backBrush);

            var formattedText = GetFormattedText(displayname, foreBrush);
            EncodingNameImage = CreateTextImage(formattedText, foreBrush, backBrush);

            AdornmentLayer.AddAdornment(null, EncodingNameImage);

            void OnLayoutUpdated(object o, EventArgs e)
            {
                if (TextView.ViewportWidth > 0)
                {
                    Canvas.SetTop(EncodingNameImage, TextView.ViewportTop);
                    Canvas.SetLeft(EncodingNameImage, TextView.ViewportRight - EncodingNameImage.RenderSize.Width);
                    EncodingNameImage.Visibility = Visibility.Visible;
                    EncodingNameImage.LayoutUpdated -= OnLayoutUpdated;
                }
            }

            EncodingNameImage.LayoutUpdated += OnLayoutUpdated;
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (EncodingNameImage != null && EncodingNameImage.RenderSize.Width > 0)
            {
                if (e.NewViewState.ViewportTop.IsNotEquals(e.OldViewState.ViewportTop))
                {
                    Canvas.SetTop(EncodingNameImage, e.NewViewState.ViewportTop);
                }

                if (e.NewViewState.ViewportRight.IsNotEquals(e.OldViewState.ViewportRight))
                {
                    Canvas.SetLeft(EncodingNameImage, e.NewViewState.ViewportRight - EncodingNameImage.RenderSize.Width);
                }
            }
        }

        private void GetEncodingAdornment(Encoding encoding, out string displayName, out Brush foregroundBrush,
                                          out Brush backgroundBrush)
        {
            var encodingName = encoding.EncodingName;

            var fore = Resources.InfoForeBrush;
            var back = Resources.InfoBackBrush;

            if (encoding is UTF8Encoding u8)
            {
                var withBom = u8.GetPreamble().Length == 3;
                encodingName += withBom ? " with BOM" : " without BOM";
                if (!withBom && Options.IsEnabledRecommendUtf8Bom)
                {
                    fore = Resources.HintForeBrush;
                    back = Resources.HintBackBrush;
                }
            }
            else if (Options.IsEnabledWarningOtherEncoding)
            {
                fore = Resources.WarningForeBrush;
                back = Resources.WarningBackBrush;
            }

            displayName = encodingName;
            foregroundBrush = fore;
            backgroundBrush = back;
        }

        private FormattedText GetFormattedText(string text, Brush brush)
        {
            var typeface = new Typeface(SystemFonts.MessageFontFamily, FontStyles.Normal, FontWeights.Bold,
                FontStretches.Normal);
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface,
                SystemFonts.MessageFontSize, brush, null, TextFormattingMode.Ideal, 1.0);
        }

        private Image CreateTextImage(FormattedText formattedText, Brush foreBrush, Brush backBrush)
        {
            const double horzMargin = 4.0;
            const double vertMargin = 1.0;

            var textGeometry = formattedText.BuildGeometry(new Point(horzMargin, vertMargin));
            var textGeometryDrawing = textGeometry.ToGeometryDrawing(foreBrush, null);

            var rect = new Rect(0, 0,
                textGeometry.Bounds.Width + horzMargin * 2 + 1,
                textGeometry.Bounds.Bottom + vertMargin * 2 + 1);
            var backGeometry = new RectangleGeometry { Rect = rect }.FreezeAnd();

            var backGeometryDrawing = backGeometry.ToGeometryDrawing(backBrush, null);
            var drawingGroup = new DrawingGroup();
            drawingGroup.Children.Add(backGeometryDrawing);
            drawingGroup.Children.Add(textGeometryDrawing);
            var image = drawingGroup.FreezeAnd().ToImage();
            image.Opacity = Opacity;
            image.Visibility = Visibility.Hidden;
            Panel.SetZIndex(image, 100);

            void OnMouseEnter(object sender, MouseEventArgs e)
            {
                image.Visibility = Visibility.Hidden;
                image.MouseEnter -= OnMouseEnter;
            }

            image.MouseEnter += OnMouseEnter;

            return image;
        }

        private void CleanUp()
        {
            if (EncodingNameImage != null)
            {
                AdornmentLayer.RemoveAdornment(EncodingNameImage);
                EncodingNameImage = null;
            }
        }
    }
}
