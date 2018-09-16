using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.One.Options
{
    internal sealed class OptionResources
    {
        public Brush RightMarginBackground { get; set; }

        private IEditorFormatMap EditorFormatMap { get; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            EditorFormatMap = editorFormatMap;
            CreateResource();
        }

        public void CreateResource()
        {
            const byte marginAlpha = 0x20;

            RightMarginBackground = EditorFormatMap
                .GetBackgroundBrush(new ColorDefinitions.RightMarginColor(), marginAlpha);
        }
    }
}