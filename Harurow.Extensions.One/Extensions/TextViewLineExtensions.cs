using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Extensions
{
    public static class TextViewLineExtensions
    {
        public static string GetLineBreak(this ITextViewLine self, IWpfTextView textView)
        {
            var lineBreakLength = self.LineBreakLength;
            if (0 < lineBreakLength)
            {
                return textView.TextSnapshot.GetText(self.EndIncludingLineBreak.Position - lineBreakLength,
                    lineBreakLength);
            }

            return null;
        }
    }
}