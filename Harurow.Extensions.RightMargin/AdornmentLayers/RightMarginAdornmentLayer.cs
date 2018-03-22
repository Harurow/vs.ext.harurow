using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.RightMargin.AdornmentLayers
{
    internal sealed class RightMarginAdornmentLayer
    {
        private const string LayerName = "Harurow." + nameof(RightMarginAdornmentLayer);

#pragma warning disable 649, 169

        [Export(typeof(AdornmentLayerDefinition))]
        [Name(LayerName)]
        [Order(Before = PredefinedAdornmentLayers.DifferenceChanges)]
        private AdornmentLayerDefinition AdornmentLayer { get; set; }

#pragma warning restore 649, 169

        public static IAdornmentLayer GetAdornmentLayer(IWpfTextView textView)
            => textView.GetAdornmentLayer(LayerName);
    }
}
