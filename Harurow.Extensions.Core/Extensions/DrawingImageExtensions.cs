using System.Windows.Controls;
using System.Windows.Media;

namespace Harurow.Extensions.Extensions
{
    public static class DrawingImageExtensions
    {
        public static Image ToImage(this DrawingImage self)
            => new Image {Source = self};
    }
}