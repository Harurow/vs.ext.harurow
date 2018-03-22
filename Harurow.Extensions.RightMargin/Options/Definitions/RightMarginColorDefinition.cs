using System.ComponentModel.Composition;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.RightMargin.Options.Definitions
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    internal class RightMarginColorDefinition : ResourceDefinition
    {
        internal const string Name = "Harurow.Extensions.RightMargin";

        public override string ResourceName => Name;

        public RightMarginColorDefinition()
        {
            DisplayName = "Harurow.RightMargin - 有効桁数を超えた領域";
            ForegroundCustomizable = false;
            BackgroundColor = Colors.Black;
        }
    }
}
