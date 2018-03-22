using System;
using System.Reactive.Disposables;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.Adornments.TextViewLines;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.LineBreak.AdornmentLayers;
using Harurow.Extensions.LineBreak.Adornments;
using Harurow.Extensions.LineBreak.Adornments.TextViewLines;
using Harurow.Extensions.LineBreak.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.LineBreak.Services
{
    internal sealed class LineBreakService
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private bool Initialized { get; set; }
        private IOptionValues Options { get; set; }
        private OptionResources Resources { get; set; }

        private IAdornment LineBreak { get; set; }

        public LineBreakService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            AdornmentLayer = LineBreakAdornmentLayer.GetAdornmentLayer(textView);

            var editorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);

            Options = OptionValues.ReadFromStore();
            Resources = new OptionResources(editorFormatMap);
            CreateAdornment();

            TextView.Bind(OnLayoutChanged);
            TextView.Bind(new CompositeDisposable(
                OptionValues.Subscribe(OptionValuesOnNext),
                OptionResources.Subscribe(editorFormatMap, OptionResourcesOnNext),
                TextView.Options.Subscribe(new[] {new UseVisibleWhitespace().Key.Name}, EditorOptionsOnNext)));
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!Initialized)
            {
                LineBreak.OnInitialized();
                Initialized = true;
            }

            LineBreak.OnLayoutChanged(sender, e);
        }

        private void OptionValuesOnNext(IOptionValues optionValues)
        {
            Options = optionValues;
            CreateAdornment();
        }

        private void OptionResourcesOnNext(OptionResources resources)
        {
            Resources = resources;
            CreateAdornment();
        }

        private void EditorOptionsOnNext(string name)
        {
            CreateAdornment();
        }

        private VisibleLineBreakPainter CreateVisibleLineBreakPainter()
            => new VisibleLineBreakPainter(AdornmentLayer, Resources.VisibleLineBreakBrush,
                Resources.VisibleLineBreakPen);

        private HighlightLineBreakWarningPainter CreateHighlightLineBreakWarningPainter()
            => new HighlightLineBreakWarningPainter(TextView, AdornmentLayer, Resources.LineBreakWarningBrush,
                Resources.LineBreakWarningPen);

        private static bool IsEnabled(LineBreakMode mode, bool useVisibleWhitespace)
        {
            switch (mode)
            {
                case LineBreakMode.True:
                    return true;
                case LineBreakMode.False:
                    return false;
                case LineBreakMode.UseVisibleWhiteSpace:
                    return useVisibleWhitespace;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void CreateAdornment()
        {
            AdornmentLayer.RemoveAllAdornments();
            var whitespace = TextView.Options.GetOptionValue(new UseVisibleWhitespace().Key);

            var visibleLineBreak = IsEnabled(Options.VisibleLineBreakMode, whitespace)
                ? new VisibleLineBreakTextViewLineAdornment(TextView, CreateVisibleLineBreakPainter())
                : TextViewLineAdornment.Empty;

            var highlightLineBreak = IsEnabled(Options.LineBreakWarningMode, whitespace)
                ? new HighlightLineBreakWarningTextViewLineAdornment(TextView, CreateHighlightLineBreakWarningPainter())
                : HighlightLineBreakWarningTextViewLineAdornment.Empty;

            LineBreak?.Dispose();
            LineBreak = new LineBreakAdornment(TextView, visibleLineBreak, highlightLineBreak);

            if (Initialized)
            {
                LineBreak.OnInitialized();
            }
        }
    }
}