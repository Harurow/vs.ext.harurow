using System.Windows.Controls;
using System.Windows.Media;

namespace Harurow.Extensions.Extensions
{
    public static class DrawingGroupExtensions
    {
        public static DrawingImage ToDrawingImage(this DrawingGroup self)
            => new DrawingImage(self).FreezeAnd();

        public static Image ToImage(this DrawingGroup self)
            => new Image {Source = self.ToDrawingImage()};
    }
}