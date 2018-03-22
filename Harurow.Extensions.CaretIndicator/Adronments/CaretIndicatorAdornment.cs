using System;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.CaretIndicator.Adronments
{
    internal static class CaretIndicatorAdornment
    {
        private static readonly Lazy<ICaretIndicatorAdornment> LazyInstance =
            new Lazy<ICaretIndicatorAdornment>(() => new EmptyCaretAdornment());

        private sealed class EmptyCaretAdornment : ICaretIndicatorAdornment
        {
            public void Dispose()
            {
            }

            public void OnInitialized()
            {
            }

            public void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
            {
            }

            public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
            {
            }
        }

        public static ICaretIndicatorAdornment Empty
            => LazyInstance.Value;
    }
}