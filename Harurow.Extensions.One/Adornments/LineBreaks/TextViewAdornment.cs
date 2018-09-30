using System;
using System.Linq;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments.LineBreaks
{
    using VisibleLineBreakLineAdornment = VisibleLineBreaks.LineAdornment;
    using WarningLineBreakLineAdornment = WarningLineBreaks.LineAdornment;

    internal sealed class TextViewAdornment : ITextViewAdornment
    {
        private IWpfTextView TextView { get; }

        private VisibleLineBreakLineAdornment VisibleLineBreak { get; }
        private WarningLineBreakLineAdornment WarningLineBreak { get; }

        public TextViewAdornment(IWpfTextView textView,
            VisibleLineBreakLineAdornment visibleLineBreak,
            WarningLineBreakLineAdornment warningLineBreak)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            VisibleLineBreak = visibleLineBreak;
            WarningLineBreak = warningLineBreak;
        }

        public void OnInitialized()
        {
            UpdateDocumentLineBreak();
            TextView.TextViewLines
                .ForEach(AddAdornment);
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            e.NewOrReformattedLines
                .ForEach(AddAdornment);
        }

        public void CleanUp()
        {
            VisibleLineBreak?.CleanUp();
            WarningLineBreak?.CleanUp();
        }

        private void UpdateDocumentLineBreak()
        {
            if (WarningLineBreak == null)
            {
                return;
            }

            // TODO: 遅延してすべて読み込んでから解析

            var grp = TextView.TextViewLines
                .Select(line => line.GetLineBreakKind())
                .Where(lb => lb != LineBreakKind.Unknown)
                .GroupBy(lb => lb)
                .ToArray();

            if (grp.Length > 1)
            {
                var docLineBreak = grp.OrderByDescending(g => g.Count())
                    .First()
                    .Key;
                WarningLineBreak.DocumentLineBreakKind = docLineBreak;
            }
        }

        private void AddAdornment(ITextViewLine line)
        {
            VisibleLineBreak?.AddAdornment(line);
            WarningLineBreak?.AddAdornment(line);
        }
    }
}