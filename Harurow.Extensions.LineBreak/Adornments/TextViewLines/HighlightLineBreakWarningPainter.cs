using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.LineBreak.Adornments.TextViewLines
{
    internal sealed class HighlightLineBreakWarningPainter
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Brush HighlightLineBreakWarningBrush { get; }
        private Pen HighlightLineBreakWarningPen { get; }

        private Dictionary<Size, GeometryDrawing> Cache { get; }

        public HighlightLineBreakWarningPainter(IWpfTextView textView, IAdornmentLayer adornmentLayer,
                                                Brush highlightLineBreakWarningBrush, Pen highlightLineBreakWarningPen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = adornmentLayer ?? throw new ArgumentNullException(nameof(adornmentLayer));
            HighlightLineBreakWarningBrush = highlightLineBreakWarningBrush;
            HighlightLineBreakWarningPen = highlightLineBreakWarningPen;

            Cache = new Dictionary<Size, GeometryDrawing>();
        }

        public void PaintHighlightLinebreakWarning(ITextViewLine line)
        {
            var start = line.End.Position;
            var end = line.EndIncludingLineBreak.Position;
            var span = new SnapshotSpan(TextView.TextSnapshot, Span.FromBounds(start, end));

            var geometry = span.GetMarkerGeometry(TextView);
            if (geometry == null)
            {
                return;
            }

            var key = geometry.Bounds.Size;
            if (!Cache.TryGetValue(key, out var drawing))
            {
                drawing = geometry.ToGeometryDrawing(HighlightLineBreakWarningBrush, HighlightLineBreakWarningPen);
                Cache[key] = drawing;
            }

            var image = drawing.FreezeAnd()
                               .ToImage()
                               .SetTopLeft(geometry.Bounds.Location);

            AdornmentLayer.AddAdornment(span, null, image);
        }
    }
}