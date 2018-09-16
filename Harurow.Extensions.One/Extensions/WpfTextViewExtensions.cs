using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Harurow.Extensions.One.Extensions
{
    public static class WpfTextViewExtensions
    {
        public static void Bind(this IWpfTextView self, IDisposable disposable)
        {
            void OnClosed(object sender, EventArgs e)
            {
                disposable.Dispose();
                self.Closed -= OnClosed;
            }

            self.Closed += OnClosed;
        }

        public static void Bind(this IWpfTextView self, EventHandler<TextViewLayoutChangedEventArgs> handler)
        {
            self.LayoutChanged += handler;

            void OnClosed(object sender, EventArgs e)
            {
                self.LayoutChanged -= handler;
                self.Closed -= OnClosed;
            }

            self.Closed += OnClosed;
        }

        public static void Bind(this IWpfTextView self, EventHandler<CaretPositionChangedEventArgs> handler)
        {
            self.Caret.PositionChanged += handler;

            void OnClosed(object sender, EventArgs e)
            {
                self.Caret.PositionChanged -= handler;
                self.Closed -= OnClosed;
            }

            self.Closed += OnClosed;
        }

        public static ITextDocument GetTextDocument(this IWpfTextView self)
        {
            self.TextBuffer
                .Properties
                .TryGetProperty<ITextDocument>(typeof(ITextDocument), out var doc);

            if (doc == null)
            {
                self.TextBuffer
                    .Properties
                    .TryGetProperty<ITextBufferUndoManager>(typeof(ITextBufferUndoManager), out var txBufUndoMgr);

                if (txBufUndoMgr != null)
                {
                    txBufUndoMgr.TextBuffer
                        .Properties
                        .TryGetProperty(typeof(ITextDocument), out doc);
                }
            }

            return doc;
        }

        public static IVsTextView GetVsTextView(this IWpfTextView self)
            => self.Properties.GetProperty<IVsTextView>(typeof(IVsTextView));

        public static void Bind(this IWpfTextView self, EventHandler<EncodingChangedEventArgs> hanlder)
        {
            var doc = self.GetTextDocument();
            if (doc != null)
            {
                doc.EncodingChanged += hanlder;

                void OnClosed(object sender, EventArgs e)
                {
                    doc.EncodingChanged -= hanlder;
                    self.Closed -= OnClosed;
                }

                self.Closed += OnClosed;
            }
        }
    }
}