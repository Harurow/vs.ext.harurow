using System;
using System.Windows.Media;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.MultipleSelection.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.MultipleSelection.Options
{
    internal class OptionResources
    {
        public Brush CaretsBrush { get; private set; }
        public Brush CaretsSelectionsBrush { get; private set; }
        public Pen CaretsSelectionsPen { get; private set; }

        public OptionResources(IEditorFormatMap editorFormatMap)
        {
            if (editorFormatMap == null) throw new ArgumentNullException(nameof(editorFormatMap));
            Create(editorFormatMap);
        }

        private void Create(IEditorFormatMap map)
        {
            const byte marginAlpha = 0x80;

            CaretsBrush = map.GetBackgoundBrush(new MultipleCaretColorDefinition());
            CaretsSelectionsBrush = map.GetBackgoundBrush(new MultipleSelectionColorDefinition(), marginAlpha);
            CaretsSelectionsPen = map.GetForegroundPen(new MultipleSelectionColorDefinition(), 1.0);
        }

        public static IDisposable Subscribe(IEditorFormatMap map, Action<OptionResources> onNext)
            => map.Subscribe(new[]
            {
                MultipleCaretColorDefinition.Name,
                MultipleSelectionColorDefinition.Name,
            }, _ => onNext(new OptionResources(map)));
    }
}
