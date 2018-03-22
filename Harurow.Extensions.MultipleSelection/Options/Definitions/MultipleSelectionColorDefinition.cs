using System.ComponentModel.Composition;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.MultipleSelection.Options.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    public class MultipleSelectionColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.MultipleSelection.Selection";

        public override string ResourceName => Name;

        public MultipleSelectionColorDefinition()
        {
            DisplayName = "Harurow.MultipleSelection - 複数選択範囲";
            ForegroundColor = Color.FromRgb(0x26, 0x8B, 0xD2);
            BackgroundColor = Color.FromRgb(0x26, 0x8B, 0xD2);
        }
    }
}