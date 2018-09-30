using System.Windows.Media;

namespace Harurow.Extensions.One.Extensions
{
    internal static class ColorExtensions
    {
        public static Color SetAlpha(this Color self, byte alpha)
            => Color.FromArgb(alpha, self.R, self.G, self.B);
    }
}