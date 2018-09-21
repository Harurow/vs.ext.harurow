using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.One.Options
{
    // HACK: ペン・ブラシなどのリソースを増やす
    internal sealed class OptionResources
    {
        public Brush RightMarginBrush { get; set; }
        public Pen RedundantWhiteSpacesPen { get; set; }
        public Brush RedundantWhiteSpacesBrush { get; set; }

        private IEditorFormatMap EditorFormatMap { get; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            EditorFormatMap = editorFormatMap;
            CreateResource();
        }

        public void CreateResource()
        {
            RightMarginBrush = EditorFormatMap
                .GetBackgroundBrush(new ColorDefinitions.RightMarginColor(), 0x20);

            RedundantWhiteSpacesPen = EditorFormatMap
                .GetForegroundPen(new ColorDefinitions.RedundantWhiteSpacesColor(), 0.5);
            RedundantWhiteSpacesBrush = EditorFormatMap
                .GetBackgroundBrush(new ColorDefinitions.RedundantWhiteSpacesColor(), 0x40);
        }
    }
}