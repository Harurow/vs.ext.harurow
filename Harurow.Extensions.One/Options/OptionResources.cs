using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.One.Options
{
    // HACK: 4. ブラシペンなどのリソースを増やす
    internal sealed class OptionResources
    {
        #region define brush, pen

        // HACK: 4.1. ブラシ、ペンの定義
        public Brush RightMarginBrush { get; set; }
        public Brush VisibleLineBreakBrush { get; set; }
        public Pen VisibleLineBreakPen { get; set; }
        public Pen LineIndicatorPen { get; set; }
        public Pen ColumnIndicatorPen { get; set; }

        #endregion

        private IEditorFormatMap EditorFormatMap { get; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            EditorFormatMap = editorFormatMap;
            CreateResource();
        }

        public void CreateResource()
        {
            #region create resouces

            // HACK: 4.2. ブラシ、ペンの作成
            var rightMargin = new ColorDefinitions.RightMarginColor();
            RightMarginBrush = EditorFormatMap.GetBackgroundBrush(rightMargin, 0x20);

            var visibleLineBreak = new ColorDefinitions.VisibleLineBreakColor();
            VisibleLineBreakBrush = EditorFormatMap.GetForegroundBrush(visibleLineBreak);
            VisibleLineBreakPen = EditorFormatMap.GetForegroundPen(visibleLineBreak, 0.5);

            var lineIndicator =  new ColorDefinitions.LineIndicatorColor();
            LineIndicatorPen = EditorFormatMap.GetForegroundPen(lineIndicator, 0.5);

            var columnIndicator =  new ColorDefinitions.ColumnIndicatorColor();
            ColumnIndicatorPen = EditorFormatMap.GetForegroundPen(columnIndicator, 0.5);

            #endregion
        }
    }
}