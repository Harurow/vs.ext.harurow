using System.ComponentModel.Composition;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.LineBreak.Options.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    public class VisibleLineBreakColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.LineBreak";

        public override string ResourceName => Name;

        public VisibleLineBreakColorDefinition()
        {
            DisplayName = "Harurow.LineBreak - 改行の表示";
            ForegroundColor = Color.FromRgb(0x07, 0x36, 0x42);
            BackgroundCustomizable = false;
        }
    }
}
