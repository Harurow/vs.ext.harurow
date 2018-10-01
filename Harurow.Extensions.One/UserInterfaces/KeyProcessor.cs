using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using EnvDTE80;
using Harurow.Extensions.One.Options;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.UserInterfaces
{
    internal class KeyProcessor : Microsoft.VisualStudio.Text.Editor.KeyProcessor
    {
        private IWpfTextView TextView { get; }
        private CompositeDisposable Disposable { get; }
        private Subject<KeyEvents> Subject { get; }

        private ReactiveProperty<bool> IsInGoThere { get; }

        private TimeSpan ValidInterval { get; } = TimeSpan.FromMilliseconds(600);

        private bool IsDoubleKeyDown(KeyEvents prev, KeyEvents cur)
            => prev != null && cur != null &&
               !prev.Args.Handled && !cur.Args.Handled &&
               prev.Args.IsDown && cur.Args.IsDown &&
               !prev.Args.IsRepeat && !cur.Args.IsRepeat &&
               prev.Args.Key == cur.Args.Key &&
               (cur.RaiseDateTime - prev.RaiseDateTime) < ValidInterval;

        public KeyProcessor(IWpfTextView textView)
        {
            Disposable = new CompositeDisposable();

            var values = new OptionValues();
            values.LoadSettingsFromStorage();

            TextView = textView;
            TextView.Closed += OnClosed;

            Subject = new Subject<KeyEvents>().AddTo(Disposable);
            IsInGoThere = new ReactiveProperty<bool>().AddTo(Disposable);

            if (values.IsEnableGoThere)
            {
                Subject.Zip(Subject.Skip(1), (prev, cur) => (Prev: prev, Cur: cur))
                    .Where(x => IsDoubleKeyDown(x.Prev, x.Cur))
                    .Select(x => x.Cur.Args)
                    .Subscribe(e =>
                    {
                        Debug.WriteLine($"** double key touch {e.Key} **");
                        if (e.Key == Key.LeftShift)
                        {
                            IsInGoThere.Value = !IsInGoThere.Value;
                        }
                    })
                    .AddTo(Disposable);
            }

            IsInGoThere.Subscribe(x =>
            {
                Debug.WriteLine($"** Go There is {x}. **");
            }).AddTo(Disposable);

            TextView.Properties.AddProperty(typeof(KeyProcessor), this);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.Closed -= OnClosed;
            Disposable.Dispose();
        }

        private void MoveDown()
        {

            var startingPos = TextView.Caret.Position;
            var buffer = TextView.TextBuffer;
            var currentSnapshot = buffer.CurrentSnapshot;
            var start = startingPos.BufferPosition;

            var previousLine = start.GetContainingLine();
            var startLineNo = currentSnapshot.GetLineNumberFromPosition(start.Position);

            var line = currentSnapshot.GetLineFromLineNumber(startLineNo + 1);
            var finalPosition = new VirtualSnapshotPoint(line, TextView.Caret.Position.VirtualSpaces);
            TextView.Caret.MoveTo(finalPosition);

            TextView.Caret.EnsureVisible();

            /*

            for (var i = firstLine; 0 <= i && i < currentSnapshot.LineCount; i ++)
            {
                var line = currentSnapshot.GetLineFromLineNumber(i);
                
                string lineContents = line.GetTextIncludingLineBreak();
                bool lineIsBlank = string.IsNullOrWhiteSpace(lineContents);
                if (lineIsBlank)
                {
                    if (!previousLineIsBlank && i != firstLine)
                    {
                        // found our next blank line beyond our text block
                        targetLine = (JumpOutsideEdge) ? line : previousLine;
                        break;
                    }
                }
                else if (!SkipClosestEdge && previousLineIsBlank && i != firstLine)
                {
                    // found our text block, go to the blank line right before it
                    targetLine = (JumpOutsideEdge) ? previousLine : line;
                    break;
                }

                previousLine = line;
                previousLineIsBlank = lineIsBlank;
            }

            if (targetLine != null)
            {
                VirtualSnapshotPoint finalPosition;
                if (JumpOutsideEdge)
                {
                    // move the caret to the blank line indented with the appropriate number of virtual spaces
                    int? virtualSpaces = SmartIndentation.GetDesiredIndentation(TextView, targetLine);
                    finalPosition = new VirtualSnapshotPoint(targetLine.Start, virtualSpaces.GetValueOrDefault());
                    if (!finalPosition.IsInVirtualSpace)
                    {
                        // our line has some 'meaningful' whitespace, go to end instead
                        finalPosition = new VirtualSnapshotPoint(targetLine.End);
                    }
                }
                else
                {
                    string lineString = targetLine.GetTextIncludingLineBreak();
                    int offset = lineString.TakeWhile(c => char.IsWhiteSpace(c)).Count();
                    finalPosition = new VirtualSnapshotPoint(targetLine, offset);
                }
                TextView.Caret.MoveTo(finalPosition);
            }
            else
            {
                // we found no suitable line so just choose BOF or EOF depending on the direction we were moving
                if (direction == JumpDirection.Up)
                {
                    TextView.Caret.MoveTo(previousLine.Start);
                }
                else
                {
                    TextView.Caret.MoveTo(previousLine.End);
                }
            }

            // scroll our view to the new caret position
            TextView.Caret.EnsureVisible();
            */
        }

        /// <inheritdoc />
        public override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);

            if (IsInGoThere.Value)
            {
                // h: left
                // j: down
                // k: up
                // l: right
                if (Keyboard.Modifiers == ModifierKeys.None)
                {
                    switch (e.Key)
                    {
                        case Key.H:
//                            VSCommands.Browse."");
                            break;
                        case Key.J:
                            //var next = line.EndIncludingLineBreak.Position + 1;
                            //TextView.TextViewLines.GetTextViewLineContainingBufferPosition(TextView.Caret.ContainingTextViewLine)
                            MoveDown();
                            break;
                        case Key.K:
                            break;
                        case Key.L:
                            break;
                        case Key.Enter:
                            IsInGoThere.Value = false;
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