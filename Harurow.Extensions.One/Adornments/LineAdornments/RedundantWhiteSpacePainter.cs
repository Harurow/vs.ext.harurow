using System;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments.LineAdornments
{
    internal class RedundantWhiteSpacePainter
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Brush Brush { get; }
        private Pen Pen { get; }

        public RedundantWhiteSpacePainter(IWpfTextView textView, IAdornmentLayer adornmentLayer, Brush brush, Pen pen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = adornmentLayer ?? throw new ArgumentNullException(nameof(adornmentLayer));
            Brush = brush;
            Pen = pen;
        }

        public void PaintRedundantWhiteSpace(int start, int end)
        {
            var span = new SnapshotSpan(TextView.TextSnapshot, Span.FromBounds(start, end));

            var geometry = TextView.TextViewLines.GetMarkerGeometry(span);
            if (geometry == null)
            {
                return;
            }

            var image = geometry.ToImage(Brush, Pen)
                .SetTopLeft(geometry.Bounds.Location);

            AdornmentLayer.AddAdornment(span, typeof(RedundantWhiteSpacePainter), image);
        }

        public void CleanUp()
        {
            AdornmentLayer.RemoveAdornmentsByTag(typeof(RedundantWhiteSpacePainter));
        }
    }
}