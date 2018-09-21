using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Extensions
{
    public static class SnapshotSpanExtensions
    {
        public static Geometry GetLineMarkerGeometry(this SnapshotSpan self, IWpfTextView textView)
            => textView.TextViewLines.GetLineMarkerGeometry(self);

        public static Geometry GetLineMarkerGeometry(this SnapshotSpan self, IWpfTextView textView, bool clipToViewport,
                                                     Thickness padding)
            => textView.TextViewLines.GetLineMarkerGeometry(self, clipToViewport, padding);

        public static Geometry GetMarkerGeometry(this SnapshotSpan self, IWpfTextView textView, bool clipToViewport,
                                                 Thickness padding)
            => textView.TextViewLines.GetMarkerGeometry(self, clipToViewport, padding);

        public static Geometry GetMarkerGeometry(this SnapshotSpan self, IWpfTextView textView)
            => textView.TextViewLines.GetMarkerGeometry(self);

        public static Geometry GetTextMarkerGeometry(this SnapshotSpan self, IWpfTextView textView)
            => textView.TextViewLines.GetTextMarkerGeometry(self);

        public static Geometry GetTextMarkerGeometry(this SnapshotSpan self, IWpfTextView textView, bool clipToViewport,
                                                     Thickness padding)
            => textView.TextViewLines.GetTextMarkerGeometry(self, clipToViewport, padding);
    }
}