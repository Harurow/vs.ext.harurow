using System;

namespace Harurow.Extensions.Commands
{
    public interface ICommandFilterExecContext
    {
        Guid MenuGroupGuid { get; }
        uint CommandId { get; }

        int ExecNextTarget();
    }
}