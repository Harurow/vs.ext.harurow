using System;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Adornments
{
    public interface IAdornment : IDisposable
    {
        void OnInitialized();
        void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e);
    }
}