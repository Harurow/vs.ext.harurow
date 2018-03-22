using Harurow.Extensions.Adornments.TextViewLines;

namespace Harurow.Extensions.LineBreak.Adornments.TextViewLines
{
    internal interface IHighlightLineBreakWarningTextViewLineAdornment : ITextViewLineAdornment
    {
        LineBreakKind DocumentLineBreakKind { get; set; }
    }
}