using System;
using Microsoft.VisualStudio.Text.Formatting;

namespace Harurow.Extensions.Adornments.TextViewLines
{
    public static class TextViewLineAdornment
    {
        private static readonly Lazy<ITextViewLineAdornment> LazyInstance =
            new Lazy<ITextViewLineAdornment>(() => new EmptyTextViewLineAdornment());

        public static ITextViewLineAdornment Empty
            => LazyInstance.Value;

        private sealed class EmptyTextViewLineAdornment : ITextViewLineAdornment
        {
            public void AddAdornment(ITextViewLine line)
            {
            }
        }
    }
}