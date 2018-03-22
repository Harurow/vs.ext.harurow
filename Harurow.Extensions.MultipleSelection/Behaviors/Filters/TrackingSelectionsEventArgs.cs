using System;
using System.Collections.Immutable;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal class TrackingSelectionsEventArgs : EventArgs
    {
        public ImmutableArray<TrackingSelection> TrackingSelections { get; }

        public TrackingSelectionsEventArgs(ImmutableArray<TrackingSelection> trackingSelections)
        {
            TrackingSelections = trackingSelections;
        }
    }
}