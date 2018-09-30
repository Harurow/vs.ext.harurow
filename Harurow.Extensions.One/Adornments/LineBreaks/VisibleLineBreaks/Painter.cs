using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Adornments.LineBreaks.VisibleLineBreaks
{
    internal class Painter
    {
        private IAdornmentLayer AdornmentLayer { get; }
        private Brush Brush { get; }
        private Pen Pen { get; }

        private Dictionary<(Size Size, LineBreakKind Kind), GeometryDrawing> Cache { get; }

        public Painter(IAdornmentLayer adornmentLayer, Brush brush, Pen pen)
        {
            AdornmentLayer = adornmentLayer ?? throw new ArgumentNullException(nameof(adornmentLayer));
            Brush = brush;
            Pen = pen;

            Cache = new Dictionary<(Size Size, LineBreakKind Kind), GeometryDrawing>();
        }

        public void AddLineBreakAdornment(SnapshotSpan span, Rect bounds, LineBreakKind lineBreakKind)
        {
            if (lineBreakKind == LineBreakKind.Unknown)
            {
                return;
            }

            var key = (bounds.Size, lineBreakKind);
            if (!Cache.TryGetValue(key, out var drawing))
            {
                drawing = CreateGeometryDrawing(key);
                Cache[key] = drawing;
            }

            var image = drawing.ToImage()
                               .SetTopLeft(bounds.Location);

            AdornmentLayer.AddAdornment(span, typeof(Painter), image);
        }

        public void CleanUp()
        {
            AdornmentLayer.RemoveAdornmentsByTag(typeof(Painter));
        }

        private GeometryDrawing CreateGeometryDrawing((Size Size, LineBreakKind Kind) key)
        {
            PathFigure pathFigure = null;
            Pen pen = null;
            Brush brush = null;

            switch (key.Kind)
            {
                case LineBreakKind.CrLf:
                    pathFigure = GetCrLfPathFigure(key.Size);
                    brush = Brush;
                    break;
                case LineBreakKind.Cr:
                    pathFigure = GetCrPathFigure(key.Size);
                    brush = Brush;
                    break;
                case LineBreakKind.Lf:
                    pathFigure = GetLfPathFigure(key.Size);
                    brush = Brush;
                    break;
                case LineBreakKind.Nel:
                    pathFigure = GetNelPathFigure(key.Size);
                    brush = Brush;
                    break;
                case LineBreakKind.Ls:
                    pathFigure = GetLfPathFigure(key.Size);
                    pen = Pen;
                    break;
                case LineBreakKind.Ps:
                    pathFigure = GetCrLfPathFigure(key.Size);
                    pen = Pen;
                    break;
            }

            var bottomRight = new Point(key.Size.Width, key.Size.Height);
            var bottomRightSegment = new PathSegment[] { new LineSegment(bottomRight, false), };
            var baseFigure = new PathFigure(new Point(0, 0), bottomRightSegment, false);

            var pathGeometry = new PathGeometry(new[]
            {
                baseFigure, pathFigure
            }.Where(i => i != null));

            return new GeometryDrawing(brush, pen, pathGeometry).FreezeAnd();
        }

        private PathFigure GetCrLfPathFigure(Size size)
        {
            var w = size.Width;
            var h = size.Height;
            var u = w * 0.4;
            var cx = w * 0.5;
            var cy = h * 0.6;

            return new PathFigure
            (
                new Point(cx - u * 1, cy),
                new PathSegment[]
                {
                    new LineSegment(new Point(cx, cy - u * 1), true),
                    new LineSegment(new Point(cx, cy - 0.5), true),
                    new LineSegment(new Point(w - 1, cy - 0.5), true),
                    new LineSegment(new Point(w - 1, cy - u * 1.5), true),
                    new LineSegment(new Point(w, cy - u * 1.5), true),
                    new LineSegment(new Point(w, cy + 0.5), true),
                    new LineSegment(new Point(cx, cy + 0.5), true),
                    new LineSegment(new Point(cx, cy + u * 1), true),
                }, true
            );
        }

        private PathFigure GetCrPathFigure(Size size)
        {
            var w = size.Width;
            var h = size.Height;
            var u = w * 0.4;
            var cx = w * 0.5;
            var cy = h * 0.5;

            return new PathFigure
            (
                new Point(cx - u * 1, cy),
                new PathSegment[]
                {
                    new LineSegment(new Point(cx, cy - u * 1), true),
                    new LineSegment(new Point(cx, cy - 0.5), true),
                    new LineSegment(new Point(w, cy - 0.5), true),
                    new LineSegment(new Point(w, cy + 0.5), true),
                    new LineSegment(new Point(cx, cy + 0.5), true),
                    new LineSegment(new Point(cx, cy + u * 1), true),
                }, true
            );
        }

        private PathFigure GetLfPathFigure(Size size)
        {
            var w = size.Width;
            var h = size.Height;
            var u = w * 0.4;
            var cx = w * 0.5 + 0.5;
            var cy = h * 0.5;

            return new PathFigure
            (
                new Point(cx, cy + u * 1.5),
                new PathSegment[]
                {
                    new LineSegment(new Point(cx - u, cy + u * 0.5), true),
                    new LineSegment(new Point(cx - 0.5, cy + u * 0.5), true),
                    new LineSegment(new Point(cx - 0.5, cy - u * 1.5), true),
                    new LineSegment(new Point(cx + 0.5, cy - u * 1.5), true),
                    new LineSegment(new Point(cx + 0.5, cy + u * 0.5), true),
                    new LineSegment(new Point(cx + u, cy + u * 0.5), true),
                }, true
            );
        }

        private PathFigure GetNelPathFigure(Size size)
        {
            var w = size.Width;
            var h = size.Height;
            var u = w * 0.4;

            return new PathFigure
            (
                new Point(0.5, h),
                new PathSegment[]
                {
                    new LineSegment(new Point(0.5, h - u * 1.2), true),

                    new LineSegment(new Point(0.5 + u * 0.6 - 0.3536, h - u * 0.6 - 0.3536), true),
                    new LineSegment(new Point(0.5 + u * 2.1 - 0.3536, h - u * 2.1 - 0.3536), true),
                    new LineSegment(new Point(0.5 + u * 2.1 + 0.3536, h - u * 2.1 + 0.3536), true),
                    new LineSegment(new Point(0.5 + u * 0.6 + 0.3536, h - u * 0.6 + 0.3536), true),

                    new LineSegment(new Point(0.5 + u * 1.2, h), true),
                }, true
            );
        }
    }
}