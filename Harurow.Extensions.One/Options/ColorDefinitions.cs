using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One.Options
{
    // HACK: 3. 色の定義を増やす
    internal sealed class ColorDefinitions
    {
        internal sealed class Defaults
        {
            #region default colors

            // HACK: 3.1. デフォルトの色を定義
            internal sealed class RightMargin
            {
                internal static readonly Color Background = Colors.Black;
            }

            internal sealed class VisibleLineBreak
            {
                internal static readonly Color Foreground = Color.FromRgb(0x07, 0x36, 0x42);
            }

            internal sealed class LineIndicator
            {
                internal static readonly Color Foreground = Color.FromRgb(0x26, 0x8b, 0xD2);
            }

            internal sealed class ColumnIndicator
            {
                internal static readonly Color Foreground = Color.FromRgb(0x26, 0x8b, 0xD2);
            }

            #endregion
        }

        private const string BaseName = "Harurow.Extensions.One.";

        #region define colors

        // HACK: 3.2. 色を宣言する
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
        [Name(BaseName + nameof(LineIndicatorColor))]
        public class LineIndicatorColor : ColorDefinition
        {
            public LineIndicatorColor()
            {
                DisplayName = "Harurow.CaretIndicator - 現在の行 水平線";
                ForegroundColor = Defaults.LineIndicator.Foreground;
                BackgroundCustomizable = false;
            }
        }
        [Export(typeof(EditorFormatDefinition))]
        [UserVisible(true)]
        [Name(BaseName + nameof(ColumnIndicatorColor))]
        public class ColumnIndicatorColor : ColorDefinition
        {
            public ColumnIndicatorColor()
            {
                DisplayName = "Harurow.CaretIndicator - 現在の列 垂直線";
                ForegroundColor = Defaults.ColumnIndicator.Foreground;
                BackgroundCustomizable = false;
            }
        }

        #endregion
    }
}
