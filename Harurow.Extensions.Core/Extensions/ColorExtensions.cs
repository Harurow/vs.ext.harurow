using System.Windows.Media;

namespace Harurow.Extensions.Extensions
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color self, byte alpha)
            => Color.FromArgb(alpha, self.R, self.G, self.B);
    }
}