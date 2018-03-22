using System;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.RedundantWhiteSpace.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.RedundantWhiteSpace.Options
{
    internal class OptionResources
    {
        public Pen RedundantWhiteSpacesHighlightPen { get; private set; }
        public Brush RedundantWhiteSpacesHighlightBrush { get; private set; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            if (editorFormatMap == null) throw new ArgumentNullException(nameof(editorFormatMap));
            Create(editorFormatMap);
        }

        private void Create(IEditorFormatMap map)
        {
            const double thicknss = 0.5;
            const byte penAlpha = 0xff;
            const byte brushAlpha = 0x40;

            var redundantWhiteSpacesHighlight = new RedundantWhiteSpacesHighlightColorDefinition();
            RedundantWhiteSpacesHighlightPen =
                map.GetForegroundPen(redundantWhiteSpacesHighlight, thicknss, penAlpha);
            RedundantWhiteSpacesHighlightBrush = map.GetBackgoundBrush(redundantWhiteSpacesHighlight, brushAlpha);
        }

        public static IDisposable Subscribe(IEditorFormatMap map, Action<OptionResources> onNext)
            => map.Subscribe(new[]
            {
                RedundantWhiteSpacesHighlightColorDefinition.Name,
            }, _ => onNext(new OptionResources(map)));
    }
}
