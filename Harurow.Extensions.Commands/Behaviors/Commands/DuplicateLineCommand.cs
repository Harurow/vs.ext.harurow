using System;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Commands.Behaviors.Commands
{
    internal sealed class DuplicateLineCommand : AbstractMenuCommand
    {
        public DuplicateLineCommand(IMenuCommandService menuCommandService)
            : base(menuCommandService, CommandSet.MenuGroupGuid, CommandSet.Ids.DuplicationLine)
        {
        }

        protected override void OnQueryStatus(object sender, EventArgs e)
        {
            MenuCommand.Enabled = GetTextView() != null;
        }

        private bool IsFirstColumn(ITextSnapshot snap, int position)
            => snap.GetLineFromPosition(position).Start.Position == position;

        protected override void OnExec(object sender, EventArgs e)
        {
            var textView = GetTextView();
            if (textView == null || textView.Selection.Mode != TextSelectionMode.Stream)
            {
                return;
            }

            var snapshot = textView.TextSnapshot;
            var selection = textView.Selection;

            var startLine = snapshot.GetLineFromPosition(selection.Start.Position);
            var endLine = snapshot.GetLineFromPosition(selection.End.Position);

            var startLineNo = startLine.LineNumber;
            var endLineNo = endLine.LineNumber;

            if (startLineNo < endLineNo && IsFirstColumn(snapshot, selection.End.Position))
            {
                endLineNo--;
            }

            var text = string.Join(string.Empty,
                Enumerable.Range(startLineNo, endLineNo - startLineNo + 1)
                    .Select(snapshot.GetLineFromLineNumber)
                    .Select(line => line.GetTextIncludingLineBreak()));

            using (var edit = snapshot.TextBuffer.CreateEdit())
            {
                edit.Insert(startLine.Start.Position, text);
                edit.Apply();
            }
        }
    }
}