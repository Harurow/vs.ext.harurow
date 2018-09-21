using System;
using Harurow.Extensions.Adornments.TextViewLines;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.RedundantWhiteSpace.Adornments.TextViewLines
{
    internal class RedundantWhiteSpacesTextViewLineAdornment : ITextViewLineAdornment
    {
        private IWpfTextView TextView { get; }
        private RedundantWhiteSpacePainter Painter { get; }

        public RedundantWhiteSpacesTextViewLineAdornment(IWpfTextView textView, RedundantWhiteSpacePainter painter)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            Painter = painter ?? throw new ArgumentNullException(nameof(painter));
        }

        public void AddAdornment(ITextViewLine line)
        {
            var text = TextView.TextSnapshot;
            var eof = text.Length == line.End.Position;
            var end = line.End.Position;
            if (!eof)
            {
                if (line.LineBreakLength == 0 || line.Length == 0)
                {
                    return;
                }

                end = line.End.Position;

                if (!char.IsWhiteSpace(text[end]))
                {
                    return;
                }
            }

            var startOfLine = line.Start.Position;
            var start = end;
            var i = end - 1;
            while (i >= startOfLine && char.IsWhiteSpace(text[i]))
            {
                start = i--;
            }

            Painter.PaintRedundantWhiteSpace(start, end);
        }
    }
}