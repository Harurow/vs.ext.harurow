using System;
using System.Windows.Media;
using Harurow.Extensions.CaretIndicator.Options.Definitions;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.CaretIndicator.Options
{
    internal class OptionResources
    {
        public Pen LineIndicatorPen { get; private set; }
        public Pen ColumnIndicatorPen { get; private set; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            if (editorFormatMap == null) throw new ArgumentNullException(nameof(editorFormatMap));
            Create(editorFormatMap);
        }

        private void Create(IEditorFormatMap map)
        {
            const double thicknss = 0.5;
            const byte penAlpha = 0xff;

            LineIndicatorPen = map.GetForegroundPen(new LineIndicatorColorDefinition(), thicknss, penAlpha);
            ColumnIndicatorPen = map.GetForegroundPen(new ColumnIndicatorColorDefinition(), thicknss, penAlpha);
        }

        public static IDisposable Subscribe(IEditorFormatMap map, Action<OptionResources> onNext)
            => map.Subscribe(new[]
            {
                LineIndicatorColorDefinition.Name,
                ColumnIndicatorColorDefinition.Name,
            }, _ => onNext(new OptionResources(map)));
    }
}
