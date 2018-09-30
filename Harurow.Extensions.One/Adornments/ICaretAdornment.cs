using System;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments
{
    internal interface ICaretAdornment : IAdornment
    {
        void OnPositionChanged(object sender, CaretPositionChangedEventArgs e);
        void OnSelectionChanged(object sender, EventArgs e);
    }
}