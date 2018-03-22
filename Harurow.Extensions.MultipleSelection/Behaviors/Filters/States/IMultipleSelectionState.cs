using Harurow.Extensions.Commands;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters.States
{
    internal interface IMultipleSelectionState
    {
        string StateName { get; }
        bool Exec(ICommandFilterExecContext execContext);
        void OnCaretPositionChanged();
    }
}