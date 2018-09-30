using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments
{
    internal interface ITextViewAdornment : IAdornment
    {
        void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e);
    }
}