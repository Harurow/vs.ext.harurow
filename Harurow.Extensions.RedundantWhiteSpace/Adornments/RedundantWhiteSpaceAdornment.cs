using System;
using System.Linq;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.Adornments.TextViewLines;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.RedundantWhiteSpace.Adornments
{
    internal sealed class RedundantWhiteSpaceAdornment : IAdornment
    {
        private IWpfTextView TextView { get; }

        private ITextViewLineAdornment RedundantWhiteSpace { get; }
        private IDisposable DocumentLineBreakDisposer { get; set; }

        public RedundantWhiteSpaceAdornment(IWpfTextView textView, ITextViewLineAdornment redundantWhiteSpace)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));

            RedundantWhiteSpace = redundantWhiteSpace ?? throw new ArgumentNullException(nameof(redundantWhiteSpace));
        }

        public void Dispose()
        {
            DocumentLineBreakDisposer?.Dispose();
            DocumentLineBreakDisposer = null;
        }

        public void OnInitialized()
        {
            TextView.TextViewLines
                .ForEach(AddAronment);
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            e.NewOrReformattedLines
                .ForEach(AddAronment);
        }

        private void AddAronment(ITextViewLine line)
        {
            RedundantWhiteSpace.AddAdronment(line);
        }
    }
}