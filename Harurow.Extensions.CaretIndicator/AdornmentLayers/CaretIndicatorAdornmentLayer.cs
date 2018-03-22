using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.CaretIndicator.AdornmentLayers
{
    internal sealed class CaretIndicatorAdornmentLayer
    {
        private const string LayerName = "Harurow." + nameof(CaretIndicatorAdornmentLayer);

#pragma warning disable 649, 169

        [Export(typeof(AdornmentLayerDefinition))]
        [Name(LayerName)]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
        private AdornmentLayerDefinition AdornmentLayer { get; set; }

#pragma warning restore 649, 169

        public static IAdornmentLayer GetAdornmentLayer(IWpfTextView textView)
        {
            var layer = textView.GetAdornmentLayer(LayerName);
            return layer;
        }
    }
}
