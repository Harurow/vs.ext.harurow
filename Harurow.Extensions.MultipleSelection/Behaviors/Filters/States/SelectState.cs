using System;
using System.Collections.Generic;
using System.Linq;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio.Text;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters.States
{
    internal sealed class SelectState : AbstractState
    {
        private const int BufferSize = 4 * 1024;

        private HashSet<int> CaretPositions { get; }
        private List<ITrackingSpan> MatchedSpans { get; }
        private ITrackingSpan StartSpan { get; }
        private bool InFindNextSelected { get; set; }

        public SelectState(IMultipleSelectionStateManager stateContext, bool addSelection)
            : base(stateContext, "Select")
        {
            CaretPositions = new HashSet<int>();

            MatchedSpans = new List<ITrackingSpan>();
            MatchedSpans.AddRange(EnumerateToNextFindMatch());

            var startPoint = TextView.Selection.Start.Position;
            var startSpan = MatchedSpans.First(span => span.GetSpan(TextView.TextSnapshot).Contains(startPoint));

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

        private IEnumerable<ITrackingSpan> EnumerateToNextFindMatch()
        {
            var matches = new List<ITrackingSpan>();

            if (!TextView.Selection.IsEmpty)
            {
                var snapshot = TextView.TextSnapshot;
                var curSpan = TextView.Selection.SelectedSpans[0];
                var targetText = curSpan.GetText();

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

                        matches.Add(TextView.TextSnapshot.CreateTrackingSpan(startIndex + findIndex, targetText.Length,
                            SpanTrackingMode.EdgePositive));

                        offset = findIndex + targetText.Length;
                    }

                    if (endIndex == snapshot.Length)
                    {
                        break;
                    }

                    startIndex = endIndex - targetText.Length + 1;
                }
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
                var curIndex = MatchedSpans.FindIndex(span => span.GetSpan(TextView.TextSnapshot).Contains(curPoint));
                var nextIndex = curIndex + 1 < MatchedSpans.Count
                    ? curIndex + 1
                    : 0;

                var nextMatchedSpan = MatchedSpans[nextIndex].GetSpan(TextView.TextSnapshot);

                StateContext.OutliningManager
                    .GetCollapsedRegions(nextMatchedSpan)
                    .ToArray()
                    .ForEach(collapsed => StateContext.OutliningManager.Expand(collapsed));

                TextView.Selection.Select(nextMatchedSpan, false);
                TextView.Caret.MoveTo(nextMatchedSpan.End);
                TextView.Caret.EnsureVisible();

                if (StartSpan != null && StartSpan.GetSpan(TextView.TextSnapshot) == nextMatchedSpan)
                {
                    // TODO: 一周しました
                }
            }
            finally
            {
                InFindNextSelected = false;
            }
        }

        private ITrackingSpan GetCurrentMatchedSpan()
        {
            var curPoint = TextView.Selection.Start.Position;
            return MatchedSpans.FirstOrDefault(span => span.GetSpan(TextView.TextSnapshot).Contains(curPoint));
        }

        private void AddTrackingSpanFromSelectedSpan()
        {
            var curSpan = GetCurrentMatchedSpan();

            if (curSpan == null)
            {
                SetStateToNormal();
                return;
            }

            var pos = curSpan.GetStartPoint(TextView.TextSnapshot).Position;
            if (!CaretPositions.Contains(pos))
            {
                CaretPositions.Add(pos);
                StateContext.TrackingSelections.Add(new TrackingSelection(TextView));
            }
        }

        private void RemoveTrackingSpanIfSelectedSpan()
        {
            var curSpan = GetCurrentMatchedSpan();

            if (curSpan == null)
            {
                SetStateToNormal();
                return;
            }

            var pos = curSpan.GetStartPoint(TextView.TextSnapshot).Position;
            if (CaretPositions.Contains(pos))
            {
                CaretPositions.Remove(pos);
                var removeItem = StateContext.TrackingSelections
                    .First(ts => ts.Span.GetSpan(TextView.TextSnapshot).Contains(pos));
                StateContext.TrackingSelections.Remove(removeItem);
            }
        }

        private void SetStateToNormal()
        {
            StateContext.ResetState();

            if (StartSpan != null)
            {
                TextView.Selection.Select(StartSpan.GetSpan(TextView.TextSnapshot), false);
                TextView.Caret.MoveTo(StartSpan.GetStartPoint(TextView.TextSnapshot));
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