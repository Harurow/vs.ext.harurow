using System;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.Commands
{
    internal class CommandFilterExecContext : ICommandFilterExecContext
    {
        public Guid MenuGroupGuid { get; }
        public uint CommandId { get; }

        private IOleCommandTarget NextTarget { get; }
        private uint CommadnExecOption { get; }
        private IntPtr VariantArgIn { get; }
        private IntPtr VariantArgOut { get; }

        public CommandFilterExecContext(Guid menuGroupGuid, uint commandId, uint commadnExecOption,
            IntPtr variantArgIn, IntPtr variantArgOut, IOleCommandTarget nextTarget)
        {
            MenuGroupGuid = menuGroupGuid;
            CommandId = commandId;
            NextTarget = nextTarget;

            CommadnExecOption = commadnExecOption;
            VariantArgIn = variantArgIn;
            VariantArgOut = variantArgOut;
        }

        public int ExecNextTarget()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var guid = MenuGroupGuid;
            return NextTarget.Exec(ref guid, CommandId, CommadnExecOption, VariantArgIn, VariantArgOut);
        }
    }
}