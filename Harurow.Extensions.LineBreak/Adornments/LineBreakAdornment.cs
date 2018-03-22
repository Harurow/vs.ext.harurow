using System;
using System.Linq;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.Adornments.TextViewLines;
using Harurow.Extensions.LineBreak.Adornments.TextViewLines;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.LineBreak.Adornments
{
    internal sealed class LineBreakAdornment : IAdornment
    {
        private IWpfTextView TextView { get; }

        private ITextViewLineAdornment VisibleLineBreak { get; }
        private IHighlightLineBreakWarningTextViewLineAdornment HighlightLineBreakWarning { get; }

        public LineBreakAdornment(IWpfTextView textView, ITextViewLineAdornment visibleLineBreak,
            IHighlightLineBreakWarningTextViewLineAdornment highlightLineBreakWarning)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));

            VisibleLineBreak = visibleLineBreak ?? throw new ArgumentNullException(nameof(visibleLineBreak));
            HighlightLineBreakWarning = highlightLineBreakWarning ??
                                        throw new ArgumentNullException(nameof(highlightLineBreakWarning));
        }

        public void Dispose()
        {
        }

        public void OnInitialized()
        {
            UpdateDocumentLineBreak();

            TextView.TextViewLines
                .ForEach(AddAronment);
        }

        private void UpdateDocumentLineBreak()
        {
            var grp = TextView.TextViewLines
                .Select(line => line.GetLineBreakKind(TextView))
                .Where(lb => lb != LineBreakKind.Unknown)
                .GroupBy(lb => lb)
                .ToArray();

            if (grp.Length > 1)
            {
                HighlightLineBreakWarning.DocumentLineBreakKind = grp.OrderByDescending(g => g.Count())
                    .First()
                    .Key;
            }
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            e.NewOrReformattedLines
                .ForEach(AddAronment);
        }

        private void AddAronment(ITextViewLine line)
        {
            VisibleLineBreak.AddAdronment(line);
            HighlightLineBreakWarning.AddAdronment(line);
        }
    }
}