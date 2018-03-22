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
    public class LineBreakWarningColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.LineBreak.Warning";

        public override string ResourceName => Name;

        public LineBreakWarningColorDefinition()
        {
            DisplayName = "Harurow.LineBreak - 異なる改行コード";
            ForegroundColor = Colors.Red;
            BackgroundColor = Colors.Red;
        }
    }
}