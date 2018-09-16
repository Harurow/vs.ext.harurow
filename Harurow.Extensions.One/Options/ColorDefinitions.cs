using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One.Options
{
    internal sealed class ColorDefinitions
    {
        internal sealed class Defaults
        {
            internal sealed class RightMargin
            {
                internal static readonly Color Background = Colors.Black;
            }
        }

        private const string BaseName = "Harurow.Extensions.One.";

        [Export(typeof(EditorFormatDefinition))]
        [UserVisible(true)]
        [Name(BaseName + nameof(RightMarginColor))]
        internal class RightMarginColor : ColorDefinition
        {
            public RightMarginColor()
            {
                DisplayName = "Harurow.RightMargin - 有効桁数を超えた領域";
                ForegroundCustomizable = false;
                BackgroundColor = Defaults.RightMargin.Background;
            }
        }
    }
}
