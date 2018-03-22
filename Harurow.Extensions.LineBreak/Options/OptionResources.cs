using System;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.LineBreak.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.LineBreak.Options
{
    internal class OptionResources
    {
        public Pen LineBreakWarningPen { get; private set; }
        public Brush LineBreakWarningBrush { get; private set; }
        public Pen VisibleLineBreakPen { get; private set; }
        public Brush VisibleLineBreakBrush { get; private set; }

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

            var lineBreakWarning = new LineBreakWarningColorDefinition();
            LineBreakWarningPen = map.GetForegroundPen(lineBreakWarning, thicknss, penAlpha);
            LineBreakWarningBrush = map.GetBackgoundBrush(lineBreakWarning, brushAlpha);

            var visibleLineBreak = new VisibleLineBreakColorDefinition();
            VisibleLineBreakPen = map.GetForegroundPen(visibleLineBreak, thicknss, penAlpha);
            VisibleLineBreakBrush = map.GetForegroundBrush(visibleLineBreak, penAlpha);
        }

        public static IDisposable Subscribe(IEditorFormatMap map, Action<OptionResources> onNext)
            => map.Subscribe(new[]
            {
                LineBreakWarningColorDefinition.Name,
                VisibleLineBreakColorDefinition.Name,
            }, _ => onNext(new OptionResources(map)));
    }
}
