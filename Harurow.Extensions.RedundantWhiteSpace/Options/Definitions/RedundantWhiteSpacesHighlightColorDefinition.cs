using System.ComponentModel.Composition;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.RedundantWhiteSpace.Options.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    public class RedundantWhiteSpacesHighlightColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.RedundantWhitespaces";

        public override string ResourceName => Name;

        public RedundantWhiteSpacesHighlightColorDefinition()
        {
            DisplayName = "Harurow.RedundantWhitespaces - 改行前の連続した空白文字";
            ForegroundColor = Colors.DarkOrange;
            BackgroundColor = Colors.DarkOrange;
        }
    }
}