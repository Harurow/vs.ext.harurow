using EnvDTE80;
using Harurow.Extensions.Commands;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.MultipleSelection.Behaviors.Filters.States;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal sealed class MultipleSelectionStateManager : IMultipleSelectionStateManager
    {
        public IWpfTextView TextView { get; }
        public DTE2 Dte { get; }
        public IOutliningManager OutliningManager { get; }

        public TrackingSelectionCollection TrackingSelections { get; }

        private IMultipleSelectionState State { get; set; }

        public MultipleSelectionStateManager(IWpfTextView textView, DTE2 dte, IOutliningManager outliningManager)
        {
            TextView = textView;
            Dte = dte;
            OutliningManager = outliningManager;

            TextView.Bind(OnCaretPositionChanged);

            TrackingSelections = new TrackingSelectionCollection();
            State = new NormalState(this);
        }

        public bool Exec(ICommandFilterExecContext execContext)
            => State.Exec(execContext);

        public void ResetState()
        {
            State = new NormalState(this);
            TrackingSelections.Clear();
        }

        public void SetStateToSelect(ICommandFilterExecContext execContext, bool addSelection)
            => State = new SelectState(this, addSelection);

        public void SetStateToEdit(ICommandFilterExecContext execContext)
            => State = new EditState(this, execContext);

        private void OnCaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
            => State.OnCaretPositionChanged();
    }
}