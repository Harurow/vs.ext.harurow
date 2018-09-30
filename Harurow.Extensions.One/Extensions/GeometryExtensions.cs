using System.Windows.Controls;
using System.Windows.Media;

namespace Harurow.Extensions.One.Extensions
{
    internal static class GeometryExtensions
    {
        public static Image ToImage(this Geometry self, Brush brush, Pen pen)
            => new GeometryDrawing(brush, pen, self).FreezeAnd().ToImage();

        public static GeometryDrawing ToGeometryDrawing(this Geometry self, Brush brush, Pen pen)
            => new GeometryDrawing(brush, pen, self).FreezeAnd();
    }
}