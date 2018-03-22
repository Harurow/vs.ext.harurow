using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.MultipleSelection.AdornmentLayers
{
    internal sealed class MultipleSelectionAdornmentLayer
    {
        private const string LayerName = "Harurow." + nameof(MultipleSelectionAdornmentLayer);

#pragma warning disable 649, 169

        [Export(typeof(AdornmentLayerDefinition))]
        [Name(LayerName)]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
        private AdornmentLayerDefinition AdornmentLayer { get; set; }

#pragma warning restore 649, 169

        public static IAdornmentLayer GetAdornmentLayer(IWpfTextView textView)
            => textView.GetAdornmentLayer(LayerName);
    }
}
