using Harurow.Extensions.Commands.Options;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Commands.Services
{
    internal sealed class MouseWheelZoomLockService
    {
        private IWpfTextView TextView { get; }

        public MouseWheelZoomLockService(IWpfTextView textView)
        {
            TextView = textView;

            OptionValuesOnNext(OptionValues.ReadFromStore());
            textView.Bind(OptionValues.Subscribe(OptionValuesOnNext));
        }

        private void OptionValuesOnNext(IOptionValues options)
        {
            if (options.IsLockedMouseWheelZoom)
            {
                TextView.Options.SetOptionValue(DefaultWpfViewOptions.EnableMouseWheelZoomId, false);
            }
            else
            {
                TextView.Options.RestoreOption(DefaultWpfViewOptions.EnableMouseWheelZoomId);
            }
        }
    }
}