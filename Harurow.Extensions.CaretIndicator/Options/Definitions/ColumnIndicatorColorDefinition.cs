using System.ComponentModel.Composition;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.CaretIndicator.Options.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    public class ColumnIndicatorColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.CaretIndicator.ColumnIndicator";

        public override string ResourceName => Name;

        public ColumnIndicatorColorDefinition()
        {
            DisplayName = "Harurow.CaretIndicator - 現在の列 垂直線";
            ForegroundColor = Color.FromRgb(0x26, 0x8B, 0xD2);
            BackgroundCustomizable = false;
        }
    }
}