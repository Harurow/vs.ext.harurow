using System;
using EnvDTE80;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal sealed class MultipleSelectionCommandFilter : AbstractCommandFilter
    {
        public IWpfTextView TextView { get; }
        public DTE2 Dte { get; }

        private MultipleSelectionStateManager MultipleSelectionStateManager { get; }

        public TrackingSelectionCollection TrackingSelections
            => MultipleSelectionStateManager.TrackingSelections;

        public MultipleSelectionCommandFilter(IWpfTextView textView, IVsTextView viewAdapter, DTE2 dte,
            IOutliningManager outliningManager)
            : base(CommandSet.MenuGroupGuid, viewAdapter)
        {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            Dte = dte ?? throw new ArgumentNullException(nameof(dte));

            MultipleSelectionStateManager = new MultipleSelectionStateManager(textView, dte, outliningManager);
        }

        protected override bool QueryStatus(Guid groupId, uint cmdId, ref bool isEnabled)
            => false;

        protected override bool Exec(ICommandFilterExecContext context, ref int hresult)
            => MultipleSelectionStateManager.Exec(context);
    }
}