using System;
using System.ComponentModel.Design;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Commands
{
    public abstract class AbstractMenuCommand
    {
        protected OleMenuCommand MenuCommand { get; }

        protected AbstractMenuCommand(IMenuCommandService menuCommandService, Guid menuGroup, int commandId)
        {
            MenuCommand = new OleMenuCommand(OnExec, OnChange, OnQueryStatus, new CommandID(menuGroup, commandId));
            menuCommandService.AddCommand(MenuCommand);
        }

        protected IWpfTextView GetTextView()
            => TextViewHelper.GetCurrentWpfTextView();

        protected ITextDocument GetTextDocument()
            => GetTextView()?.GetTextDocument();

        protected virtual void OnExec(object sender, EventArgs e)
        {
        }

        protected virtual void OnChange(object sender, EventArgs e)
        {
        }

        protected virtual void OnQueryStatus(object sender, EventArgs e)
        {
        }
    }
}
