using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media;
using Harurow.Extensions.Options.Definitions;
using Microsoft.VisualStudio.Text.Classification;

namespace Harurow.Extensions.Extensions
{
    public static class EditorFormatMapExtensions
    {
        public static IDisposable Subscribe(this IEditorFormatMap self, IEnumerable<string> itemNames,
            Action<IEditorFormatMap> onNext)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (itemNames == null) throw new ArgumentNullException(nameof(itemNames));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            return Observable.FromEventPattern<FormatItemsEventArgs>(
                    h => self.FormatMappingChanged += h,
                    h => self.FormatMappingChanged -= h)
                .Subscribe(e =>
                {
                    if (e.EventArgs.ChangedItems.Any(itemNames.Contains))
                    {
                        onNext(self);
                    }
                });
        }

        public static Brush GetForegroundBrush(this IEditorFormatMap self, ResourceDefinition definition,
            byte alpha = 0xff)
        {
            var prop = self.GetProperties(definition.ResourceName);
            var color = (prop?[EditorFormatDefinition.ForegroundColorId] as Color?
                         ?? definition.ForegroundColor
                         ?? Colors.Transparent).SetAlpha(alpha);
            return new SolidColorBrush(color).FreezeAnd();
        }

        public static Pen GetForegroundPen(this IEditorFormatMap self, ResourceDefinition definition, double thickness,
            byte alpha = 0xff)
            => new Pen(self.GetForegroundBrush(definition, alpha), thickness);

        public static Brush GetBackgoundBrush(this IEditorFormatMap self, ResourceDefinition definition,
            byte alpha = 0xff)
        {
            var prop = self.GetProperties(definition.ResourceName);
            var color = (prop?[EditorFormatDefinition.BackgroundColorId] as Color?
                         ?? definition.BackgroundColor
                         ?? Colors.Transparent).SetAlpha(alpha);
            return new SolidColorBrush(color).FreezeAnd();
        }
    }
}