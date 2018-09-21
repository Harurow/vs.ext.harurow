using System;
using Harurow.Extensions.One.Adornments.LineBreaks.VisibleLineBreaks;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments.LineBreaks.WarningLineBreaks
{
    internal sealed class WarningLineBreakLineAdornment : ILineAdornment
    {
        public LineBreakKind DocumentLineBreakKind { get; set; }

        private IWpfTextView TextView { get; }
        private Painter Painter { get; }

        public WarningLineBreakLineAdornment(IWpfTextView textView, Painter painter)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            Painter = painter ?? throw new ArgumentNullException(nameof(painter));
            DocumentLineBreakKind = LineBreakKind.Unknown;
        }

        public void AddAdornment(ITextViewLine line)
        {
            var lineBreakKind = line.GetLineBreakKind(TextView);

            if (DocumentLineBreakKind == lineBreakKind ||
                DocumentLineBreakKind == LineBreakKind.Unknown)
            {
                return;
            }

            Painter.PaintWarning(line);
        }

        /// <inheritdoc />
        public void CleanUp()
        {
            Painter.CleanUp();
        }
    }
}