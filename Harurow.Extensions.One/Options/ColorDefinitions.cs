using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One.Options
{
    // HACK: 7. 色の定義を増やす
    internal sealed class ColorDefinitions
    {
        internal sealed class Defaults
        {
            #region default colors

            // HACK: 7.1. デフォルトの色を定義
            internal sealed class RightMargin
            {
                internal static readonly Color Background = Colors.Black;
            }

            internal sealed class RedundantWhiteSpaces
            {
                internal static readonly Color Foreground = Colors.DarkOrange;
                internal static readonly Color Background = Colors.DarkOrange;
            }

            internal sealed class VisibleLineBreak
            {
                internal static readonly Color Foreground = Color.FromRgb(0x07, 0x36, 0x42);
            }

            internal sealed class LineBreakWarning
            {
                internal static readonly Color Foreground = Colors.Red;
                internal static readonly Color Background = Colors.Red;
            }

            #endregion
        }

        private const string BaseName = "Harurow.Extensions.One.";

        #region define colors

        // HACK: 7.2. 色を宣言する
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

        [Export(typeof(EditorFormatDefinition))]
        [UserVisible(true)]
        [Name(BaseName + nameof(VisibleLineBreakColor))]
        public class VisibleLineBreakColor : ColorDefinition
        {
            public VisibleLineBreakColor()
            {
                DisplayName = "Harurow.LineBreak - 改行";
                ForegroundColor = Defaults.VisibleLineBreak.Foreground;
                BackgroundCustomizable = false;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [UserVisible(true)]
        [Name(BaseName + nameof(LineBreakWarningColor))]
        public class LineBreakWarningColor : ColorDefinition
        {
            public LineBreakWarningColor()
            {
                DisplayName = "Harurow.LineBreak - 改行文字の警告";
                ForegroundColor = Defaults.LineBreakWarning.Foreground;
                BackgroundColor = Defaults.LineBreakWarning.Background;
            }
        }

        #endregion
    }
}
