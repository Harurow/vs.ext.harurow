using System;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.LineBreak.Adornments.TextViewLines
{
    internal sealed class HighlightLineBreakWarningTextViewLineAdornment
        : IHighlightLineBreakWarningTextViewLineAdornment
    {
        private IWpfTextView TextView { get; }
        private HighlightLineBreakWarningPainter Painter { get; }
        public LineBreakKind DocumentLineBreakKind { get; set; }

        public HighlightLineBreakWarningTextViewLineAdornment(IWpfTextView textView,
            HighlightLineBreakWarningPainter painter)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            Painter = painter ?? throw new ArgumentNullException(nameof(painter));
            DocumentLineBreakKind = LineBreakKind.Unknown;
        }

        public void AddAdornment(ITextViewLine line)
        {
            var lineBreakKind = line.GetLineBreakKind(TextView);

            if (DocumentLineBreakKind == lineBreakKind || DocumentLineBreakKind == LineBreakKind.Unknown)
            {
                return;
            }

            Painter.PaintHighlightLinebreakWarning(line);
        }

        private static readonly Lazy<IHighlightLineBreakWarningTextViewLineAdornment> LazyInstance =
            new Lazy<IHighlightLineBreakWarningTextViewLineAdornment>(
                () => new EmptyHighlightLineBreakWarningTextViewLineAdornment());

        public static IHighlightLineBreakWarningTextViewLineAdornment Empty
            => LazyInstance.Value;

        private sealed class EmptyHighlightLineBreakWarningTextViewLineAdornment
            : IHighlightLineBreakWarningTextViewLineAdornment
        {
            public void AddAdornment(ITextViewLine line)
            {
            }

            public LineBreakKind DocumentLineBreakKind { get; set; }
        }
    }
}