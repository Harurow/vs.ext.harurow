using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.LineBreak.Adornments.TextViewLines
{
    internal static class TextViewLineExtensions
    {
        public static LineBreakKind GetLineBreakKind(this ITextViewLine self, IWpfTextView textView)
        {
            switch (self.GetLineBreak(textView))
            {
                case "\r\n": return LineBreakKind.CrLf;
                case "\r": return LineBreakKind.Cr;
                case "\n": return LineBreakKind.CrLf;
                case "\u0085": return LineBreakKind.Nel;
                case "\u2028": return LineBreakKind.Ls;
                case "\u2029": return LineBreakKind.Ps;
                default: return LineBreakKind.Unknown;
            }
        }
    }
}