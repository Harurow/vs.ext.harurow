using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.One.Options
{
    // HACK: 8. ブラシペンなどのリソースを増やす
    internal sealed class OptionResources
    {
        #region define brush, pen

        // HACK: 8.1. ブラシ、ペンの定義
        public Brush RightMarginBrush { get; set; }
        public Brush RedundantWhiteSpacesBrush { get; set; }
        public Pen RedundantWhiteSpacesPen { get; set; }

        public Brush LineBreakWarningBrush { get; set; }
        public Pen LineBreakWarningPen { get; set; }
        public Brush VisibleLineBreakBrush { get; set; }
        public Pen VisibleLineBreakPen { get; set; }

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

            // HACK: 8.2. ブラシ、ペンの作成
            var rightMargin = new ColorDefinitions.RightMarginColor();
            RightMarginBrush = EditorFormatMap.GetBackgroundBrush(rightMargin, 0x20);

            var redundantWhiteSpace = new ColorDefinitions.RedundantWhiteSpacesColor();
            RedundantWhiteSpacesBrush = EditorFormatMap.GetBackgroundBrush(redundantWhiteSpace, 0x40);
            RedundantWhiteSpacesPen = EditorFormatMap.GetForegroundPen(redundantWhiteSpace, 0.5);


            var lineBreakWarning = new ColorDefinitions.LineBreakWarningColor();
            LineBreakWarningBrush = EditorFormatMap.GetBackgroundBrush(lineBreakWarning, 0x40);
            LineBreakWarningPen = EditorFormatMap.GetForegroundPen(lineBreakWarning, 0.5);

            var visibleLineBreak = new ColorDefinitions.VisibleLineBreakColor();
            VisibleLineBreakBrush = EditorFormatMap.GetForegroundBrush(visibleLineBreak);
            VisibleLineBreakPen = EditorFormatMap.GetForegroundPen(visibleLineBreak, 0.5);

            #endregion
        }
    }
}