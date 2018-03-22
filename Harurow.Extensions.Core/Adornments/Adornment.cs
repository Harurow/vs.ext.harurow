using System;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Adornments
{
    public static class Adornment
    {
        private static readonly Lazy<IAdornment> LazyInstance =
            new Lazy<IAdornment>(() => new EmptyAdornment());

        public static IAdornment Empty
            => LazyInstance.Value;

        private sealed class EmptyAdornment : IAdornment
        {
            public void Dispose()
            {
            }

            public void OnInitialized()
            {
            }

            public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
            {
            }
        }
    }
}