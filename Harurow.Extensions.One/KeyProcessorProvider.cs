using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One
{
    [Export(typeof(IKeyProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [Name("Key Processor")]
    internal sealed class KeyProcessorProvider
        : IKeyProcessorProvider
    {
        /// <inheritdoc />
        public KeyProcessor GetAssociatedProcessor(IWpfTextView textView)
            => new HarurowKeyProcessor(textView);
    }

    [Export(typeof(IMouseProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [Name("Mouse Processor")]
    internal sealed class MouseProcessorProvider
        : IMouseProcessorProvider
    {
        /// <inheritdoc />
        public IMouseProcessor GetAssociatedProcessor(IWpfTextView textView)
            => new HarurowMouseProcessor(textView);
    }

    internal class HarurowMouseProcessor : IMouseProcessor
    {
        private IWpfTextView TextView { get; }

        public HarurowMouseProcessor(IWpfTextView textView)
        {
            TextView = textView;
        }

        #region Implements of IMouseProcessor

        /// <inheritdoc />
        public void PreprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseRightButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseRightButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseMove(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseMove(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseWheel(MouseWheelEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseWheel(MouseWheelEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseEnter(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseEnter(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseLeave(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseLeave(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDragLeave(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDragLeave(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDragOver(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDragOver(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDragEnter(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDragEnter(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDrop(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDrop(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessQueryContinueDrag(QueryContinueDragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessQueryContinueDrag(QueryContinueDragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessGiveFeedback(GiveFeedbackEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessGiveFeedback(GiveFeedbackEventArgs e)
        {
        }

        #endregion
    }

    internal class KeyEvents
    {
        public DateTime RaiseDateTime { get; }
        public KeyEventArgs Args { get; }

        public KeyEvents(DateTime raiseDateTime, KeyEventArgs args)
        {
            RaiseDateTime = raiseDateTime;
            Args = args;
        }
    }

    internal class HarurowKeyProcessor : KeyProcessor
    {
        private IWpfTextView TextView { get; }
        private CompositeDisposable Disposable { get; }
        private Subject<KeyEvents> Subject { get; }

        private bool IsInGoToThere { get; set; }

        private TimeSpan ValidInterval { get; } = TimeSpan.FromMilliseconds(600);

        private bool IsDoubleKeyDown(KeyEvents prev, KeyEvents cur)
            => prev != null && cur != null &&
               !prev.Args.Handled && !cur.Args.Handled &&
                prev.Args.IsDown && cur.Args.IsDown &&
                !prev.Args.IsRepeat && !cur.Args.IsRepeat &&
                prev.Args.Key == cur.Args.Key &&
                (cur.RaiseDateTime - prev.RaiseDateTime) < ValidInterval;

        public HarurowKeyProcessor(IWpfTextView textView)
        {
            TextView = textView;
            TextView.Closed += OnClosed;

            Subject = new Subject<KeyEvents>();
            Disposable = new CompositeDisposable(
                Subject,
                Subject.Zip(Subject.Skip(1), (prev, cur) => (Prev: prev, Cur: cur))
                    .Where(x => IsDoubleKeyDown(x.Prev, x.Cur))
                    .Select(x => x.Cur.Args)
                    .Subscribe(e => {
                        Debug.WriteLine($"** double key touch {e.Key} **");
                        if (e.Key == Key.LeftShift)
                        {
                            IsInGoToThere = !IsInGoToThere;
                        }
                     })
            );
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.Closed -= OnClosed;
            Disposable.Dispose();
        }

        /// <inheritdoc />
        public override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);

            if (IsInGoToThere)
            {
                // h: left
                // j: down
                // k: up
                // l: right
                if (e.KeyStates == KeyStates.None)
                {
                    switch (e.Key)
                    {
                        case Key.H:
//                            VSCommands.Browse."");
                            break;
                        case Key.J:
                            break;
                        case Key.K:
                            break;
                        case Key.L:
                            break;
                        case Key.Enter:
                            IsInGoToThere = false;
                            break;
                        default:
                            return;
                    }

                    e.Handled = true;
                }
            }
            else
            {
                Subject.OnNext(new KeyEvents(DateTime.Now, e));
            }
        }
    }
}