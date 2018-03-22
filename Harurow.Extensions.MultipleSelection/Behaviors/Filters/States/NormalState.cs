using System.Diagnostics;
using Harurow.Extensions.Commands;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters.States
{
    internal sealed class NormalState : AbstractState
    {
        public NormalState(IMultipleSelectionStateManager stateContext)
            : base(stateContext, "Normal")
        {
            stateContext.TrackingSelections.Clear();
        }

        [Conditional("DEBUG")]
        private void DebugLogCommands(ICommandFilterExecContext execContext)
        {
            if (execContext.MenuGroupGuid == VSConstants.GUID_VSStandardCommandSet97)
            {
                Debug.WriteLine($"Cmd97 : {(VSConstants.VSStd97CmdID)execContext.CommandId}");
            }
            else if (execContext.MenuGroupGuid == VSConstants.VSStd2K)
            {
                Debug.WriteLine($"Cmd2K : {(VSConstants.VSStd2KCmdID)execContext.CommandId}");
            }
            else if (execContext.MenuGroupGuid == VSConstants.VsStd2010)
            {
                Debug.WriteLine($"Cmd2010 : {(VSConstants.VSStd2010CmdID)execContext.CommandId}");
            }
            else if (execContext.MenuGroupGuid == VSConstants.VsStd11)
            {
                Debug.WriteLine($"Cmd11 : {(VSConstants.VSStd11CmdID)execContext.CommandId}");
            }
            else if (execContext.MenuGroupGuid == VSConstants.VsStd12)
            {
                Debug.WriteLine($"Cmd12 : {(VSConstants.VSStd12CmdID)execContext.CommandId}");
            }
            else if (execContext.MenuGroupGuid == VSConstants.VsStd14)
            {
                Debug.WriteLine($"Cmd14 : {(VSConstants.VSStd14CmdID)execContext.CommandId}");
            }
            else if (execContext.MenuGroupGuid == VSConstants.VsStd15)
            {
                Debug.WriteLine($"Cmd15 : {(VSConstants.VSStd15CmdID)execContext.CommandId}");
            }
            else
            {
                Debug.WriteLine($"Cmd? : {execContext.MenuGroupGuid} - 0x{execContext.CommandId:x4}");
            }
        }

        public override bool Exec(ICommandFilterExecContext execContext)
        {
            DebugLogCommands(execContext);

            if (execContext.MenuGroupGuid != CommandSet.MenuGroupGuid)
            {
                return false;
            }

            if (execContext.CommandId == CommandSet.Ids.AddSelectionToNextFindMatch)
            {
                AddSelectionToNextFindMatch(execContext);
                return true;
            }

            if (execContext.CommandId == CommandSet.Ids.MoveSelectionToNextFindMatch)
            {
                MoveSelectionToNextFindMatch(execContext);
                return true;
            }

            return false;
        }

        public override void OnCaretPositionChanged()
        {
        }

        private void AddSelectionToNextFindMatch(ICommandFilterExecContext execContext)
        {
            if (TextView.Selection.Mode != TextSelectionMode.Stream)
            {
                return;
            }

            if (TextView.Selection.IsEmpty)
            {
                Dte.ExecuteCommand(VsCommandNames.ExpandSelection);
                TextView.Caret.MoveTo(TextView.Selection.End);
                return;
            }

            StateContext.SetStateToSelect(execContext, true);
        }

        private void MoveSelectionToNextFindMatch(ICommandFilterExecContext execContext)
        {
            if (TextView.Selection.Mode != TextSelectionMode.Stream)
            {
                return;
            }

            if (TextView.Selection.IsEmpty)
            {
                Dte.ExecuteCommand(VsCommandNames.ExpandSelection);
                TextView.Caret.MoveTo(TextView.Selection.End);
                return;
            }

            StateContext.SetStateToSelect(execContext, false);
        }
    }
}