using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio.Text;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters.States
{
    internal sealed class SelectState : AbstractState
    {
        private const int BufferSize = 4 * 1024;

        private HashSet<int> CaretPositions { get; }
        private List<SnapshotSpan> MatchedSpans { get; }
        private SnapshotSpan? StartSpan { get; }
        private bool InFindNextSelected { get; set; }

        public SelectState(IMultipleSelectionStateManager stateContext, bool addSelection)
            : base(stateContext, "Select")
        {
            CaretPositions = new HashSet<int>();

            MatchedSpans = new List<SnapshotSpan>();
            MatchedSpans.AddRange(EnumerateToNextFindMatch());

            MatchedSpans.ForEach(span => Debug.WriteLine($"{span.Start.Position} - {span.End.Position}"));

            var startPoint = TextView.Selection.Start.Position;
            var startSpan = MatchedSpans.First(span => span.Contains(startPoint));

            StartSpan = null;

            if (addSelection)
            {
                AddSelectionToNextFindMatch();
            }
            else
            {
                MoveSelectionToNextFindMatch();
            }

            StartSpan = startSpan;
        }

        private IEnumerable<SnapshotSpan> EnumerateToNextFindMatch()
        {
            if (TextView.Selection.IsEmpty)
            {
                return new List<SnapshotSpan>();
            }

            var snapshot = TextView.TextSnapshot;
            var curSpan = TextView.Selection.SelectedSpans[0];
            var targetText = curSpan.GetText();

            var matches = new List<SnapshotSpan>();

            var startIndex = 0;
            while (true)
            {
                var endIndex = Math.Min(snapshot.Length, startIndex + BufferSize);
                var text = snapshot.GetText(startIndex, endIndex - startIndex);

                var offset = 0;
                while (true)
                {
                    var findIndex = text.IndexOf(targetText, offset, StringComparison.Ordinal);
                    if (findIndex == -1)
                    {
                        break;
                    }

                    matches.Add(new SnapshotSpan(snapshot, startIndex + findIndex, targetText.Length));
                    offset = findIndex + targetText.Length;
                }

                if (endIndex == snapshot.Length)
                {
                    break;
                }

                startIndex = endIndex - targetText.Length + 1;
            }

            return matches;
        }

        public override bool Exec(ICommandFilterExecContext execContext)
        {
            if (execContext.IsCancel() || execContext.IsNotSupport())
            {
                SetStateToNormal();
                return false;
            }

            if (execContext.IsEdit() || execContext.IsMove() || execContext.IsExtendSelection() ||
                execContext.IsClipboard())
            {
                SetStateToEdit(execContext);
                return true;
            }

            if (execContext.MenuGroupGuid == CommandSet.MenuGroupGuid)
            {
                switch (execContext.CommandId)
                {
                    case CommandSet.Ids.AddSelectionToNextFindMatch:
                        AddSelectionToNextFindMatch();
                        break;
                    case CommandSet.Ids.MoveSelectionToNextFindMatch:
                        MoveSelectionToNextFindMatch();
                        break;
                }

                return true;
            }

            return false;
        }

        public override void OnCaretPositionChanged()
        {
            if (!InFindNextSelected)
            {
                SetStateToNormal();
            }
        }

        private void AddSelectionToNextFindMatch()
        {
            AddTrackingSpanFromSelectedSpan();
            ExecFindNextSelected();
        }

        private void MoveSelectionToNextFindMatch()
        {
            RemoveTrackingSpanIfSelectedSpan();
            ExecFindNextSelected();
        }

        private void ExecFindNextSelected()
        {
            try
            {
                InFindNextSelected = true;

                var curPoint = TextView.Selection.Start.Position;
                var curIndex = MatchedSpans.FindIndex(span => span.Contains(curPoint));
                var nextIndex = curIndex + 1 < MatchedSpans.Count
                    ? curIndex + 1
                    : 0;

                var nextMatchedSpan = MatchedSpans[nextIndex];

                StateContext.OutliningManager
                    .GetCollapsedRegions(nextMatchedSpan)
                    .ToArray()
                    .ForEach(collapsed => StateContext.OutliningManager.Expand(collapsed));

                TextView.Selection.Select(nextMatchedSpan, false);
                TextView.Caret.MoveTo(nextMatchedSpan.End);
                TextView.Caret.EnsureVisible();

                if (StartSpan != null && StartSpan == nextMatchedSpan)
                {
                    // TODO: 一周しました
                }
            }
            finally
            {
                InFindNextSelected = false;
            }
        }

        private SnapshotSpan GetCurrentMatchedSpan()
        {
            var curPoint = TextView.Selection.Start.Position;
            return MatchedSpans.FirstOrDefault(span => span.Contains(curPoint));
        }

        private void AddTrackingSpanFromSelectedSpan()
        {
            var curSpan = GetCurrentMatchedSpan();

            if (curSpan == default(SnapshotSpan))
            {
                SetStateToNormal();
                return;
            }

            if (!CaretPositions.Contains(curSpan.Start))
            {
                CaretPositions.Add(curSpan.Start);
                StateContext.TrackingSelections.Add(new TrackingSelection(TextView));
            }
        }

        private void RemoveTrackingSpanIfSelectedSpan()
        {
            var curSpan = GetCurrentMatchedSpan();

            if (curSpan == default(SnapshotSpan))
            {
                SetStateToNormal();
                return;
            }

            if (CaretPositions.Contains(curSpan.Start))
            {
                CaretPositions.Remove(curSpan.Start);
                var removeItem = StateContext.TrackingSelections
                    .First(ts => ts.Span.GetSpan(TextView.TextSnapshot).Contains(curSpan.Start));
                StateContext.TrackingSelections.Remove(removeItem);
            }
        }

        private void SetStateToNormal()
        {
            StateContext.ResetState();

            if (StartSpan != null)
            {
                TextView.Selection.Select(StartSpan.Value, false);
                TextView.Caret.MoveTo(StartSpan.Value.End);
                TextView.Caret.EnsureVisible();
            }
        }

        private void SetStateToEdit(ICommandFilterExecContext execContext)
        {
            AddTrackingSpanFromSelectedSpan();
            StateContext.SetStateToEdit(execContext);
        }
    }
}