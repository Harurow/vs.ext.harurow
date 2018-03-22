using System;
using System.ComponentModel.Design;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal class EmptyMenuCommand : MenuCommand
    {
        public EmptyMenuCommand(int commandId)
            : base(OnExec, new CommandID(CommandSet.MenuGroupGuid, commandId))
        {
        }

        private static void OnExec(object sender, EventArgs e)
        {
        }
    }
}