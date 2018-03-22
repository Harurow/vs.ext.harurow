using Harurow.Extensions.Adornments;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.CaretIndicator.Adronments
{
    internal interface ICaretIndicatorAdornment : IAdornment
    {
        void OnPositionChanged(object sender, CaretPositionChangedEventArgs e);
    }
}