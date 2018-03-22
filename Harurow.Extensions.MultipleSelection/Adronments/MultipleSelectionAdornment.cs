using System;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.MultipleSelection.Behaviors.Filters;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.MultipleSelection.Adronments
{
    internal sealed class MultipleSelectionAdornment : ITrackingSelectionAdornment
    {
        private TrackingSelectionCollection TrackingSelections { get; }
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private Brush CaretsBrush { get; }
        private Brush CaretsSelectionsBrush { get; }
        private Pen CaretsSelectionsPen { get; }

        public MultipleSelectionAdornment(IWpfTextView textView, IAdornmentLayer layer,
                                         TrackingSelectionCollection trackingSelections, Brush caretsBrush,
                                         Brush caretsSelectionsBrush, Pen caretsSelectionsPen)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            AdornmentLayer = layer ?? throw new ArgumentNullException(nameof(layer));
            TrackingSelections = trackingSelections ?? throw new ArgumentNullException(nameof(trackingSelections));
            CaretsBrush = caretsBrush;
            CaretsSelectionsBrush = caretsSelectionsBrush;
            CaretsSelectionsPen = caretsSelectionsPen;
        }

        public void Dispose()
        {
            CleanUp();
        }

        public void OnInitialized()
        {
            CreateMutiCaretsSelections();
        }

        private void CreateMutiCaretsSelections()
        {
            AdornmentLayer.RemoveAllAdornments();
            foreach (var selection in TrackingSelections)
            {
                var span = selection.Span.GetSpan(TextView.TextSnapshot);
                AddSelection(span);
                AddCaret(selection.IsReverse
                    ? span.Start
                    : span.End);
            }
        }

        public void OnTrackingSelectionsChanged(object sender, TrackingSelectionsEventArgs e)
        {
            CreateMutiCaretsSelections();
        }

        public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            CreateMutiCaretsSelections();
        }

        private void AddSelection(SnapshotSpan span)
        {
            var geometry = TextView.TextViewLines.GetLineMarkerGeometry(span);
            if (geometry != null)
            {
                var image = geometry.ToImage(CaretsSelectionsBrush, CaretsSelectionsPen);
                image.SetTopLeft(geometry.Bounds.Location);

                AdornmentLayer.AddAdornment(span, null, image);
            }
        }

        private void AddCaret(SnapshotPoint point)
        {
            var span = new SnapshotSpan(point, 1);
            var geo = TextView.TextViewLines.GetLineMarkerGeometry(span);

            if (geo != null)
            {
                var rect = new Rect(0, 0, 3, geo.Bounds.Height);

                var geometry = new RectangleGeometry(rect);
                var image = geometry.ToImage(CaretsBrush, null);
                image.SetTopLeft(geo.Bounds.Location);
                AdornmentLayer.AddAdornment(span, null, image);
            }
        }

        private void CleanUp()
        {
            AdornmentLayer.RemoveAllAdornments();
        }
    }
}