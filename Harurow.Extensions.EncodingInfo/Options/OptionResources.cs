using System;
using System.Windows.Media;
using Harurow.Extensions.EncodingInfo.Options.Definitions;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.EncodingInfo.Options
{
    internal class OptionResources
    {
        public Brush InfoForeBrush { get; private set; }
        public Brush InfoBackBrush { get; private set; }
        public Brush HintForeBrush { get; private set; }
        public Brush HintBackBrush { get; private set; }
        public Brush WarningForeBrush { get; private set; }
        public Brush WarningBackBrush { get; private set; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            if (editorFormatMap == null) throw new ArgumentNullException(nameof(editorFormatMap));
            Create(editorFormatMap);
        }

        private void Create(IEditorFormatMap map)
        {
            InfoForeBrush = map.GetForegroundBrush(new InfoColorDefinition());
            InfoBackBrush = map.GetBackgoundBrush(new InfoColorDefinition());
            HintForeBrush = map.GetForegroundBrush(new HintColorDefinition());
            HintBackBrush = map.GetBackgoundBrush(new HintColorDefinition());
            WarningForeBrush = map.GetForegroundBrush(new WarningColorDefinition());
            WarningBackBrush = map.GetBackgoundBrush(new WarningColorDefinition());
        }

        public static IDisposable Subscribe(IEditorFormatMap map, Action<OptionResources> onNext)
            => map.Subscribe(new[]
            {
                InfoColorDefinition.Name,
                HintColorDefinition.Name,
                WarningColorDefinition.Name,
            }, _ => onNext(new OptionResources(map)));
    }
}
