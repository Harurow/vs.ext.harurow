using System;
using Harurow.Extensions.One.Adornments.LineBreaks;

namespace Harurow.Extensions.One.Extensions
{
    internal static class LineBreakKindExtensions
    {
        public static string GetName(this LineBreakKind self)
        {
            switch (self)
            {
                case LineBreakKind.Unknown:
                    return "";
                case LineBreakKind.CrLf:
                    return "CR/LF";
                case LineBreakKind.Cr:
                    return "CR";
                case LineBreakKind.Lf:
                    return "LF";
                case LineBreakKind.Nel:
                    return "NEL";
                case LineBreakKind.Ls:
                    return "LS";
                case LineBreakKind.Ps:
                    return "PS";
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
    }
}