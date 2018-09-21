using System;
using Harurow.Extensions.Adornments.TextViewLines;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.LineBreak.Adornments.TextViewLines
{
    internal class VisibleLineBreakTextViewLineAdornment : ITextViewLineAdornment
    {
        private IWpfTextView TextView { get; }
        private VisibleLineBreakPainter Painter { get; }

        public VisibleLineBreakTextViewLineAdornment(IWpfTextView textView, VisibleLineBreakPainter painter)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            Painter = painter ?? throw new ArgumentNullException(nameof(painter));
        }

        public void AddAdornment(ITextViewLine line)
        {
            var start = line.End.Position;
            var end = line.EndIncludingLineBreak.Position;
            var span = new SnapshotSpan(TextView.TextSnapshot, Span.FromBounds(start, end));

            var boundsGeometory = TextView.TextViewLines.GetMarkerGeometry(span);
            if (boundsGeometory == null)
            {
                return;
            }

            var lineBreakKind = line.GetLineBreakKind(TextView);

            Painter.AddLineBreakAdornment(span, boundsGeometory.Bounds, lineBreakKind);
        }
    }
}