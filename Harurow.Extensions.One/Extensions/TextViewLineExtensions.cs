using Harurow.Extensions.One.Adornments.LineBreaks;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Extensions
{
    internal static class TextViewLineExtensions
    {
        public static string GetLineBreak(this ITextViewLine self)
        {
            var lineBreakLength = self.LineBreakLength;
            if (0 < lineBreakLength)
            {
                return self.Snapshot.GetText(self.EndIncludingLineBreak.Position - lineBreakLength,
                    lineBreakLength);
            }

            return null;
        }

        public static LineBreakKind GetLineBreakKind(this ITextViewLine self)
        {
            switch (self.GetLineBreak())
            {
                case "\r\n": return LineBreakKind.CrLf;
                case "\r": return LineBreakKind.Cr;
                case "\n": return LineBreakKind.Lf;
                case "\u0085": return LineBreakKind.Nel;
                case "\u2028": return LineBreakKind.Ls;
                case "\u2029": return LineBreakKind.Ps;
                default: return LineBreakKind.Unknown;
            }
        }
    }
}