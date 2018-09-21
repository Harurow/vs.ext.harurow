using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.TextViewCreationListeners
{
    partial class HarurowExtensionOneService
    {
        private void UpdateIsLockedWheelZoom()
        {
            var id = DefaultWpfViewOptions.EnableMouseWheelZoomId;

            TextView.Options.SetOptionValue(id,
                !Values.IsLockedWheelZoom &&
                TextView.Options.Parent.GetOptionValue(id));
        }
    }
}
