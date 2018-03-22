using System;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.RightMargin.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.RightMargin.Options
{
    internal class OptionResources
    {
        public Brush RightMarginBrush { get; private set; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            if (editorFormatMap == null) throw new ArgumentNullException(nameof(editorFormatMap));
            Create(editorFormatMap);
        }

        private void Create(IEditorFormatMap map)
        {
            const byte marginAlpha = 0x20;
            RightMarginBrush = map.GetBackgoundBrush(new RightMarginColorDefinition(), marginAlpha);
        }

        public static IDisposable Subscribe(IEditorFormatMap map, Action<OptionResources> onNext)
            => map.Subscribe(new[]
            {
                RightMarginColorDefinition.Name
            }, _ => onNext(new OptionResources(map)));
    }
}
