using System.Windows.Controls;
using System.Windows.Media;

namespace Harurow.Extensions.Extensions
{
    public static class GeometryDrawingExtensions
    {
        public static Image ToImage(this GeometryDrawing drawing)
            => new Image { Source = new DrawingImage(drawing).FreezeAnd() };
    }
}