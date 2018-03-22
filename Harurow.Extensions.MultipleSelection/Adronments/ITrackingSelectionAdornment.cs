using Harurow.Extensions.Adornments;
using Harurow.Extensions.MultipleSelection.Behaviors.Filters;

namespace Harurow.Extensions.MultipleSelection.Adronments
{
    internal interface ITrackingSelectionAdornment : IAdornment
    {
        void OnTrackingSelectionsChanged(object sender, TrackingSelectionsEventArgs e);
    }
}