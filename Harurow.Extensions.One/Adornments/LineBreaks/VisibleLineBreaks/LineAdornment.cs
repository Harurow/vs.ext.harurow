﻿using System;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments.LineBreaks.VisibleLineBreaks
{
    internal class LineAdornment : ILineAdornment
    {
        private IWpfTextView TextView { get; }
        private Painter Painter { get; }

        public LineAdornment(IWpfTextView textView, Painter painter)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            Painter = painter ?? throw new ArgumentNullException(nameof(painter));
        }

        public void AddAdornment(ITextViewLine line)
        {
            // BUG: 改行時に前行の改行コードが消える
            var lineBreakSpan = new SnapshotSpan(line.Snapshot,
                Span.FromBounds(line.End, line.EndIncludingLineBreak));

            var geometry = TextView.TextViewLines.GetMarkerGeometry(lineBreakSpan);
            if (geometry == null)
            {
                return;
            }

            var lineBreakKind = line.GetLineBreakKind();
            Painter.AddLineBreakAdornment(lineBreakSpan, geometry.Bounds, lineBreakKind);
        }

        /// <inheritdoc />
        public void CleanUp()
        {
            Painter.CleanUp();
        }
    }
}