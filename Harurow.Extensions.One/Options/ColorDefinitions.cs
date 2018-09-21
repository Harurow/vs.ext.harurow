using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One.Options
{
    // HACK: 色の定義を増やす
    internal sealed class ColorDefinitions
    {
        internal sealed class Defaults
        {
            internal sealed class RightMargin
            {
                internal static readonly Color Background = Colors.Black;
            }

            internal sealed class RedundantWhiteSpaces
            {
                internal static readonly Color Foreground = Colors.DarkOrange;
                internal static readonly Color Background = Colors.DarkOrange;
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

        [Export(typeof(EditorFormatDefinition))]
        [UserVisible(true)]
        [Name(BaseName + nameof(RedundantWhiteSpacesColor))]
        internal class RedundantWhiteSpacesColor : ColorDefinition
        {
            public RedundantWhiteSpacesColor()
            {
                DisplayName = "Harurow.RedundantWhitespaces - 改行前の連続した空白文字";
                ForegroundColor = Defaults.RedundantWhiteSpaces.Foreground;
                BackgroundColor = Defaults.RedundantWhiteSpaces.Background;
            }
        }
    }
}
