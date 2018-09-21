using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments.LineBreaks.WarningLineBreaks
{
    internal sealed class Painter
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }
        private Brush Brush { get; }
        private Pen Pen { get; }

        private Dictionary<Size, GeometryDrawing> Cache { get; }

        public Painter(IWpfTextView textView, IAdornmentLayer adornmentLayer, Brush brush, Pen pen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = adornmentLayer ?? throw new ArgumentNullException(nameof(adornmentLayer));
            Brush = brush;
            Pen = pen;

            Cache = new Dictionary<Size, GeometryDrawing>();
        }

        public void PaintWarning(ITextViewLine line)
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
                drawing = geometry.ToGeometryDrawing(Brush, Pen);
                Cache[key] = drawing;
            }

            var image = drawing.FreezeAnd()
                               .ToImage()
                               .SetTopLeft(geometry.Bounds.Location);

            AdornmentLayer.AddAdornment(span, typeof(Painter), image);
        }

        public void CleanUp()
        {
            AdornmentLayer.RemoveAdornmentsByTag(typeof(Painter));
        }
    }
}