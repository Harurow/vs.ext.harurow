using System.Windows;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Extensions
{
    internal static class AdornmentLayerExtensions
    {
        public static void AddAdornment(this IAdornmentLayer self, object tag, UIElement adornment,
            AdornmentRemovedCallback removedCallback = null)
        {
            self.AddAdornment(AdornmentPositioningBehavior.OwnerControlled, null, tag, adornment, null);
        }
    }
}