using EnvDTE80;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal interface IMultipleSelectionStateManager
    {
        IWpfTextView TextView { get; }
        DTE2 Dte { get; }
        IOutliningManager OutliningManager { get; }

        TrackingSelectionCollection TrackingSelections { get; }

        void ResetState();
        void SetStateToSelect(ICommandFilterExecContext execContext, bool addSeletion);
        void SetStateToEdit(ICommandFilterExecContext execContext);
    }
}