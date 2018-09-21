using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.Adornments.TextViewLines
{
    public interface ITextViewLineAdornment
    {
        void AddAdornment(ITextViewLine line);
    }
}