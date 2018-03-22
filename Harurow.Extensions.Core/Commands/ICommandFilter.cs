using System;

namespace Harurow.Extensions.Commands
{
    public interface ICommandFilter
    {
        bool QueryStatus(Guid groupId, uint cmdId, ref bool isEnabled);

        bool Exec(ICommandFilterExecContext context, ref int hresult);
    }
}