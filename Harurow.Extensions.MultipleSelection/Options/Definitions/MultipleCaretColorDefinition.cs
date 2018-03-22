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
    public class MultipleCaretColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.MultipleSelection.Caret";

        public override string ResourceName => Name;

        public MultipleCaretColorDefinition()
        {
            DisplayName = "Harurow.MultipleSelection - 複数選択カレット";
            ForegroundCustomizable = false;
            BackgroundColor = Color.FromRgb(0x8D, 0x4B, 0xE2);
        }
    }
}