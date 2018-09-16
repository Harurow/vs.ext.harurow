using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One
{
    internal sealed class TextAdornmentLayerDefinitions
    {
        public sealed class LayerNames
        {
            public const string BeforeDifferenceChanges = "Harurow.BeforeDifferenceChanges";
            public const string AfterSelection = "Harurow.AfterSelection";
            public const string AfterText = "Harurow.AfterText";
            public const string AfterCaret = "Harurow.AfterCaret";
        }

#pragma warning disable 649, 169

        [Export(typeof(AdornmentLayerDefinition))]
        [Order(Before = PredefinedAdornmentLayers.DifferenceChanges)]
        [Name(LayerNames.BeforeDifferenceChanges)]
        private AdornmentLayerDefinition BeforeDifferenceChangesAdornmentLayer { get; set; }

        [Export(typeof(AdornmentLayerDefinition))]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [Name(LayerNames.AfterSelection)]
        private AdornmentLayerDefinition AfterSelectionAdornmentLayer { get; set; }

        [Export(typeof(AdornmentLayerDefinition))]
        [Order(After = PredefinedAdornmentLayers.Text, Before = PredefinedAdornmentLayers.Caret)]
        [Name(LayerNames.AfterText)]
        private AdornmentLayerDefinition AdornmentLayer { get; set; }

        [Export(typeof(AdornmentLayerDefinition))]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [Name(LayerNames.AfterCaret)]
        private AdornmentLayerDefinition AfterCaretAdornmentLayer { get; set; }

#pragma warning restore 649, 169
    }

    internal static class TextAdornmentLayerExtensions
    {
        public static IAdornmentLayer GetBeforeDifferenceChangesAdornmentLayer(this IWpfTextView self)
            => self.GetAdornmentLayer(TextAdornmentLayerDefinitions.LayerNames.BeforeDifferenceChanges);

        public static IAdornmentLayer GetAfterSelectionAdornmentLayer(this IWpfTextView self)
            => self.GetAdornmentLayer(TextAdornmentLayerDefinitions.LayerNames.AfterSelection);

        public static IAdornmentLayer GetAfterTextAdornmentLayer(this IWpfTextView self)
            => self.GetAdornmentLayer(TextAdornmentLayerDefinitions.LayerNames.AfterText);

        public static IAdornmentLayer GetAfterCaretAdornmentLayer(this IWpfTextView self)
            => self.GetAdornmentLayer(TextAdornmentLayerDefinitions.LayerNames.AfterCaret);
    }
}