using System.Windows;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Extensions
{
    public static class AdornmentLayerExtensions
    {
        public static void AddAdornment(this IAdornmentLayer self, object tag, UIElement adornment,
            AdornmentRemovedCallback removedCallback = null)
        {
            self.AddAdornment(AdornmentPositioningBehavior.OwnerControlled,
                null, tag, adornment, null);
        }
    }
}