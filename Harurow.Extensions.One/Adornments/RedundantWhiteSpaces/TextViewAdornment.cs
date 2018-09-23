using System;
using System.Linq;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments.RedundantWhiteSpaces
{
    internal sealed class TextViewAdornment : ITextViewAdornment
    {
        private IWpfTextView TextView { get; }
        private ILineAdornment LineAdornment { get; }

        public TextViewAdornment(IWpfTextView textView, ILineAdornment lineAdornment)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            LineAdornment = lineAdornment ?? throw new ArgumentNullException(nameof(lineAdornment));
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
            LineAdornment.CleanUp();
        }

        private void AddAdornment(ITextViewLine line)
        {
            LineAdornment.AddAdornment(line);
        }
    }
}