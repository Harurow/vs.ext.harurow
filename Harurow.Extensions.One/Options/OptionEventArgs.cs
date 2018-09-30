using System;
using System.Collections.ObjectModel;

namespace Harurow.Extensions.One.Options
{
    internal sealed class OptionEventArgs : EventArgs
    {
        public OptionValues NewValues { get; }
        public ReadOnlyCollection<string> ChangedItems { get; }

        /// <inheritdoc />
        public OptionEventArgs(OptionValues newValues, ReadOnlyCollection<string> changedItems)
        {
            NewValues = newValues;
            ChangedItems = changedItems;
        }
    }
}