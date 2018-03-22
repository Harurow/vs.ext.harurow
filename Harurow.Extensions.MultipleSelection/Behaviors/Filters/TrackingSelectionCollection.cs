using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.Shell.Interop;

namespace Harurow.Extensions.MultipleSelection.Behaviors.Filters
{
    internal sealed class TrackingSelectionCollection : IEnumerable<TrackingSelection>
    {
        private ImmutableArray<TrackingSelection> Items { get; set; }

        public int Count => Items.Length;

        public event EventHandler<TrackingSelectionsEventArgs> Changed;

        private void OnChanged(TrackingSelectionsEventArgs e)
            => Changed?.Invoke(this, e);

        public TrackingSelectionCollection()
        {
            Items = ImmutableArray<TrackingSelection>.Empty;
        }

        public void Set(IEnumerable<TrackingSelection> selections)
        {
            if (selections != null)
            {
                Items = selections.ToImmutableArray();
            }
            else if (!Items.IsEmpty)
            {
                Items = ImmutableArray<TrackingSelection>.Empty;
            }
            else
            {
                return;
            }

            OnChanged(new TrackingSelectionsEventArgs(Items));
        }

        public void Add(TrackingSelection selection)
        {
            Items = Items.Add(selection);
            OnChanged(new TrackingSelectionsEventArgs(Items));
        }

        public void Remove(TrackingSelection selection)
        {
            Items = Items.Remove(selection);
            OnChanged(new TrackingSelectionsEventArgs(Items));
        }

        public void Clear()
        {
            if (!Items.IsEmpty)
            {
                Items = ImmutableArray<TrackingSelection>.Empty;
                OnChanged(new TrackingSelectionsEventArgs(Items));
            }
        }

        public IEnumerator<TrackingSelection> GetEnumerator()
            => Items.OfType<TrackingSelection>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}