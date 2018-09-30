using System;
using System.Linq;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments.LineBreaks
{
    using VisibleLineBreakLineAdornment = VisibleLineBreaks.LineAdornment;

    internal sealed class TextViewAdornment : ITextViewAdornment
    {
        private IWpfTextView TextView { get; }

        private VisibleLineBreakLineAdornment VisibleLineBreak { get; }

        public TextViewAdornment(IWpfTextView textView,
            VisibleLineBreakLineAdornment visibleLineBreak)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            VisibleLineBreak = visibleLineBreak;
        }

        public void OnInitialized()
        {
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
        }

        private void AddAdornment(ITextViewLine line)
        {
            VisibleLineBreak?.AddAdornment(line);
        }
    }
}