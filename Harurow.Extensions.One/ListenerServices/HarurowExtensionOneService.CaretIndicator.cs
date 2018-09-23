using Harurow.Extensions.One.Adornments.CaretIndicators;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.ListenerServices
{
    partial class HarurowExtensionOneService
    {
        private LineIndicatorAdornment LineIndicator { get; set; }
        private ColumnIndicatorAdornment ColumnIndicator { get; set; }

        private void CreateCaretIndicatorAdornment()
        {
            var layer = TextView.GetAfterCaretAdornmentLayer();
            var id = DefaultWpfViewOptions.EnableHighlightCurrentLineId;

            if (Values.IsEnabledLineIndicator)
            {
                TextView.Options.SetOptionValue(id, false);
                LineIndicator = new LineIndicatorAdornment(TextView, layer, Resources.LineIndicatorPen);
                LineIndicator.OnInitialized();
            }
            else
            {
                TextView.Options.SetOptionValue(id, TextView.Options.Parent.GetOptionValue(id));
            }

            if (Values.IsEnabledColumnIndicator)
            {
                ColumnIndicator = new ColumnIndicatorAdornment(TextView, layer, Resources.ColumnIndicatorPen);
                ColumnIndicator.OnInitialized();
            }
        }
    }
}
