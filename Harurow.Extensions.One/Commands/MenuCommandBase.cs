using System;
using System.ComponentModel.Design;
using Harurow.Extensions.One.Extensions;
using Harurow.Extensions.One.Utilities;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.Commands
{
    public abstract class MenuCommandBase
    {
        protected OleMenuCommand MenuCommand { get; }

        protected MenuCommandBase(IMenuCommandService menuCommandService, Guid menuGroup, int commandId)
        {
            MenuCommand = new OleMenuCommand(OnExec, OnChange, OnQueryStatus, new CommandID(menuGroup, commandId));
            menuCommandService.AddCommand(MenuCommand);
        }

        protected IWpfTextView GetTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return TextViewHelper.CurrentWpfTextView;
        }

        protected ITextDocument GetTextDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return GetTextView()?.GetTextDocument();
        }

        protected virtual void OnQueryStatus(object sender, EventArgs e)
        {
            MenuCommand.Enabled = false;
        }

        protected abstract void OnExec(object sender, EventArgs e);

        protected virtual void OnChange(object sender, EventArgs e) {}

    }
}
