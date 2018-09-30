using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.One.Adornments
{
    public interface ILineAdornment
    {
        void AddAdornment(ITextViewLine line);
        void CleanUp();
    }
}