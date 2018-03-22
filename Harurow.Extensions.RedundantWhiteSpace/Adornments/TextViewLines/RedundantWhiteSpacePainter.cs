using System;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.RedundantWhiteSpace.Adornments.TextViewLines
{
    internal class RedundantWhiteSpacePainter
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Brush RedundanteWhiteSpacesHighlightBrush { get; }
        private Pen RedundantWhiteSpacesHighlightPen { get; }

        public RedundantWhiteSpacePainter(IWpfTextView textView, IAdornmentLayer adornmentLayer,
            Brush redundanteWhiteSpacesHighlightBrush, Pen redundantWhiteSpacesHighlightPen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = adornmentLayer ?? throw new ArgumentNullException(nameof(adornmentLayer));
            RedundanteWhiteSpacesHighlightBrush = redundanteWhiteSpacesHighlightBrush;
            RedundantWhiteSpacesHighlightPen = redundantWhiteSpacesHighlightPen;
        }

        public void PaintRedundantWhiteSpace(int start, int end)
        {
            var span = new SnapshotSpan(TextView.TextSnapshot, Span.FromBounds(start, end));

            var geometry = TextView.TextViewLines.GetMarkerGeometry(span);
            if (geometry == null)
            {
                return;
            }

            var image = geometry.ToImage(RedundanteWhiteSpacesHighlightBrush, RedundantWhiteSpacesHighlightPen)
                .SetTopLeft(geometry.Bounds.Location);

            AdornmentLayer.AddAdornment(span, null, image);
        }
    }
}