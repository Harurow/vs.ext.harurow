using System;
using EnvDTE80;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters.States
{
    internal abstract class AbstractState : IMultipleSelectionState
    {
        protected IMultipleSelectionStateManager StateContext { get; }
        protected IWpfTextView TextView => StateContext.TextView;
        protected DTE2 Dte => StateContext.Dte;

        public string StateName { get; }

        protected AbstractState(IMultipleSelectionStateManager stateContext, string stateName)
        {
            StateContext = stateContext;
            StateName = stateName;
        }

        public abstract bool Exec(ICommandFilterExecContext execContext);
        public abstract void OnCaretPositionChanged();

        protected bool Edit(string undoName, Action transaction)
        {
            Dte.UndoContext.Open($"Carets - {undoName}");

            try
            {
                transaction();
                Dte.UndoContext.Close();
                return true;
            }
            catch (Exception)
            {
                Dte.UndoContext.SetAborted();
                StateContext.ResetState();
                return false;
            }
        }
    }
}