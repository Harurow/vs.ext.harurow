using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.Extensions
{
    public static class EditorOptionsExtensions
    {
        public static void RestoreOption<T>(this IEditorOptions self, EditorOptionKey<T> optionkey)
        {
            self.SetOptionValue(optionkey, self.Parent.GetOptionValue(optionkey));
        }

        public static IDisposable Subscribe(this IEditorOptions self, IEnumerable<string> optionsIds,
            Action<string> onNext)
            => Observable.FromEventPattern<EditorOptionChangedEventArgs>(
                    h => self.OptionChanged += h,
                    h => self.OptionChanged -= h)
                .Subscribe(e =>
                {
                    if (optionsIds.Any(id => id == e.EventArgs.OptionId))
                    {
                        onNext(e.EventArgs.OptionId);
                    }
                });
    }
}