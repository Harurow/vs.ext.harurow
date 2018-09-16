using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments
{
    internal interface IAdornment
    {
        void OnInitialized();
        void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e);
        void CleanUp();
    }
}