using System.Text;
using System.Windows.Media;

namespace Harurow.Extensions.One.Extensions
{
    internal static class EncodingExtensions
    {
        public static bool IsUtf8WithBom(this Encoding self)
            => self is UTF8Encoding u8 && u8.GetPreamble().Length == 3;

        public static string GetEncodingName(this Encoding self)
        {
            var encodingName = self.EncodingName;

            if (encodingName.StartsWith("Unicode (UTF-8)"))
            {
                encodingName = $"UTF-8";
            }

            if (self is UTF8Encoding u8)
            {
                var withBom = u8.GetPreamble().Length == 3;
                encodingName += withBom ? " with BOM" : " without BOM";
            }

            return encodingName;
        }

        public static Brush GetBackground(this Encoding self)
        {
            if (self is UTF8Encoding u8)
            {
                var withBom = u8.GetPreamble().Length == 3;
                if (withBom)
                {
                    return null;
                }
                return Brushes.ForestGreen;
            }

            return Brushes.DarkOrange;
        }
    }
}