using System;
using System.Windows.Input;

namespace Harurow.Extensions.One.UserInterfaces
{
    internal class KeyEvents
    {
        public DateTime RaiseDateTime { get; }
        public KeyEventArgs Args { get; }

        public KeyEvents(DateTime raiseDateTime, KeyEventArgs args)
        {
            RaiseDateTime = raiseDateTime;
            Args = args;
        }
    }
}