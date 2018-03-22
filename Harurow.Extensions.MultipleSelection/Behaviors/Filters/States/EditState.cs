using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio.Text;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters.States
{
    internal sealed class EditState : AbstractState
    {
        private const string ClipboardSeparator = "\r\n";

        private bool InEditing { get; set; }

        public EditState(IMultipleSelectionStateManager stateContext, ICommandFilterExecContext execContext)
            : base(stateContext, "Edit")
        {
            Exec(execContext);
        }

        public override bool Exec(ICommandFilterExecContext execContext)
        {
            if (execContext.IsCancel() || execContext.IsNotSupport())
            {
                StateContext.ResetState();
                return false;
            }

            if (execContext.IsCopy())
            {
                Copy();
                return true;
            }

            if (execContext.IsCut())
            {
                Cut();
                return true;
            }

            if (execContext.IsPaste())
            {
                Paste(execContext);
                return true;
            }

            if (execContext.IsEdit() || execContext.IsMove() || execContext.IsExtendSelection())
            {
                EditOrMove(execContext);
                return true;
            }

            return false;
        }

        public override void OnCaretPositionChanged()
        {
            if (!InEditing)
            {
                StateContext.ResetState();
            }
        }

        private void Copy()
        {
            var body = string.Join(ClipboardSeparator,
                StateContext.TrackingSelections
                            .Select(selection => selection.Span.GetText(TextView.TextSnapshot)));
            Clipboard.SetText(body);
        }

        private void Cut()
        {
            Copy();
            Edit("切り取り", span => TextView.TextBuffer.Delete(span));
        }

        private void Paste(ICommandFilterExecContext execContext)
        {
            var numOfSel = StateContext.TrackingSelections.Count;
            if (numOfSel == 1)
            {
                Edit("貼り付け", _ => execContext.ExecNextTarget());
                return;
            }

            if (numOfSel > 1)
            {
                var raw = Clipboard.GetText();
                var textList = raw.Split(new[] {ClipboardSeparator}, StringSplitOptions.None);

                if (numOfSel == textList.Length)
                {
                    var no = 0;
                    Edit("貼り付け", span => TextView.TextBuffer.Replace(span, textList[no++]));
                }
                else
                {
                    Edit("貼り付け", _ => execContext.ExecNextTarget());
                }
            }
        }

        private void EditOrMove(ICommandFilterExecContext execContext)
        {
            Edit("編集", _ => execContext.ExecNextTarget());
        }

        private void Edit(string undoName, Action<SnapshotSpan> action)
        {
            try
            {
                InEditing = true;

                var newSelections = new List<TrackingSelection>();

                Edit(undoName, () =>
                {
                    foreach (var selection in StateContext.TrackingSelections)
                    {
                        var span = selection.Span.GetSpan(TextView.TextSnapshot);

                        TextView.Selection.Select(span, selection.IsReverse);
                        TextView.Caret.MoveTo(selection.IsReverse ? span.Start : span.End);

                        action(span);

                        newSelections.Add(new TrackingSelection(TextView));
                    }
                });

                StateContext.TrackingSelections.Set(newSelections);
            }
            finally
            {
                InEditing = false;
            }
        }
    }
}