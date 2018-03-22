using System;
using System.ComponentModel.Design;
using System.Text;

namespace Harurow.Extensions.Commands.Behaviors.Commands
{
    internal sealed class SetUtf8WithBomCommand : AbstractMenuCommand
    {
        public SetUtf8WithBomCommand(IMenuCommandService menuCommandService)
            : base(menuCommandService, CommandSet.MenuGroupGuid, CommandSet.Ids.SetEncodingToUtf8WithBom)
        {
        }

        protected override void OnQueryStatus(object sender, EventArgs e)
        {
            var doc = GetTextDocument();
            if (doc == null)
            {
                MenuCommand.Enabled = false;
                return;
            }

            MenuCommand.Enabled = !(doc.Encoding is UTF8Encoding u8 &&
                                    u8.GetPreamble().Length == 3);
        }

        protected override void OnExec(object sender, EventArgs e)
        {
            var doc = GetTextDocument();
            if (doc == null)
            {
                return;
            }

            doc.Encoding = new UTF8Encoding(true);
            doc.UpdateDirtyState(true, DateTime.Now);
        }
    }
}