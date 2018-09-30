using System;
using System.ComponentModel.Design;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;

namespace Harurow.Extensions.One.Commands
{
    internal sealed class SetUtf8WithBomCommand : MenuCommandBase
    {
        public SetUtf8WithBomCommand(IMenuCommandService menuCommandService)
            : base(menuCommandService, CommandSets.MenuGroupGuid, CommandSets.Ids.SetEncodingToUtf8WithBom)
        {
        }

        protected override void OnQueryStatus(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var doc = GetTextDocument();
            if (doc == null)
            {
                MenuCommand.Enabled = false;
                return;
            }

            MenuCommand.Enabled = !IsUtf8WithBom(doc);
        }

        protected override void OnExec(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var doc = GetTextDocument();
            if (doc == null)
            {
                return;
            }

            doc.Encoding = new UTF8Encoding(true);
            doc.UpdateDirtyState(true, DateTime.Now);
        }

        private bool IsUtf8WithBom(ITextDocument doc)
            => doc.Encoding is UTF8Encoding u8 && u8.GetPreamble().Length == 3;
    }
}