using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.LineBreak.Adornments.TextViewLines
{
    internal class VisibleLineBreakPainter
    {
        private IAdornmentLayer AdornmentLayer { get; }
        private Brush VisibleLineBreakBrush { get; }
        private Pen VisibleLineBreakPen { get; }

        private Dictionary<Tuple<Size, LineBreakKind>, GeometryDrawing> Cache { get; }

        public VisibleLineBreakPainter(IAdornmentLayer adornmentLayer,
                                       Brush visibleLineBreakBrush, Pen visibleLineBreakPen)
        {
            AdornmentLayer = adornmentLayer ?? throw new ArgumentNullException(nameof(adornmentLayer));
            VisibleLineBreakBrush = visibleLineBreakBrush;
            VisibleLineBreakPen = visibleLineBreakPen;

            Cache = new Dictionary<Tuple<Size, LineBreakKind>, GeometryDrawing>();
        }

        public void AddLineBreakAdornment(SnapshotSpan span, Rect bounds, LineBreakKind lineBreakKind)
        {
            var key = Tuple.Create(bounds.Size, lineBreakKind);
            if (!Cache.TryGetValue(key, out var drawing))
            {
                drawing = CreateGeometryDrawing(bounds.Size, lineBreakKind).FreezeAnd();
                Cache[key] = drawing;
            }
            var image = drawing.FreezeAnd()
                               .ToImage()
                               .SetTopLeft(bounds.Location);

            AdornmentLayer.AddAdornment(span, null, image);
        }

        private GeometryDrawing CreateGeometryDrawing(Size size, LineBreakKind lineBreakKind)
        {
            PathFigure pathFigure = null;
            Pen pen = null;
            Brush brush = null;

            switch (lineBreakKind)
            {
                case LineBreakKind.CrLf:
                    pathFigure = GetCrLfPathFigure(size);
                    brush = VisibleLineBreakBrush;
                    break;
                case LineBreakKind.Cr:
                    pathFigure = GetCrPathFigure(size);
                    brush = VisibleLineBreakBrush;
                    break;
                case LineBreakKind.Lf:
                    pathFigure = GetLfPathFigure(size);
                    brush = VisibleLineBreakBrush;
                    break;
                case LineBreakKind.Nel:
                    pathFigure = GetNelPathFigure(size);
                    brush = VisibleLineBreakBrush;
                    break;
                case LineBreakKind.Ls:
                    pathFigure = GetLfPathFigure(size);
                    pen = VisibleLineBreakPen;
                    break;
                case LineBreakKind.Ps:
                    pathFigure = GetCrLfPathFigure(size);
                    pen = VisibleLineBreakPen;
                    break;
            }

            var bottomRight = new Point(size.Width, size.Height);
            var bottomRightSegment = new PathSegment[] { new LineSegment(bottomRight, false), };
            var baseFigure = new PathFigure(new Point(0, 0), bottomRightSegment, false);

            var pathGeometory = new PathGeometry(new[]
            {
                baseFigure, pathFigure
            }.Where(i => i != null));

            return new GeometryDrawing(brush, pen, pathGeometory).FreezeAnd();
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