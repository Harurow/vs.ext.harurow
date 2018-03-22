using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal sealed class TrackingSelection
    {
        public ITrackingSpan Span { get; }
        public bool IsReverse { get; }

        public TrackingSelection(ITextView textView)
        {
            var selection = textView.Selection;
            var span = selection.SelectedSpans[0];
            Span = textView.TextSnapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgePositive);
            IsReverse = selection.IsReversed;
        }

        public TrackingSelection(SnapshotSpan span, bool isReverse = false)
        {
            Span = span.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgePositive);
            IsReverse = isReverse;
        }
    }
}