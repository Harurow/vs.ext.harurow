using System;
using Harurow.Extensions.MultipleSelection.Behaviors.Filters;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.MultipleSelection.Adronments
{
    internal static class TrackingSelectionAdornment
    {
        private static readonly Lazy<ITrackingSelectionAdornment> LazyInstance =
            new Lazy<ITrackingSelectionAdornment>(() => new EmptyTrackingSelectionAdornment());

        private sealed class EmptyTrackingSelectionAdornment : ITrackingSelectionAdornment
        {
            public void Dispose()
            {
            }

            public void OnInitialized()
            {
            }

            public void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
            {
            }

            public void OnTrackingSelectionsChanged(object sender, TrackingSelectionsEventArgs e)
            {
            }
        }

        public static ITrackingSelectionAdornment Empty
            => LazyInstance.Value;
    }
}