using System;
using System.Reactive.Disposables;
using Harurow.Extensions.CaretIndicator.AdornmentLayers;
using Harurow.Extensions.CaretIndicator.Adronments;
using Harurow.Extensions.CaretIndicator.Options;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.CaretIndicator.Services
{
    internal sealed class CaretIndicatorService : IDisposable
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private bool Initialized { get; set; }
        private IOptionValues Options { get; set; }
        private OptionResources Resources { get; set; }

        private ICaretIndicatorAdornment LineIndicator { get; set; }
        private ICaretIndicatorAdornment ColumnIndicator { get; set; }
        public CaretIndicatorService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            AdornmentLayer = CaretIndicatorAdornmentLayer.GetAdornmentLayer(textView);

            var editorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);

            Options = OptionValues.ReadFromStore();
            Resources = new OptionResources(editorFormatMap);
            CreateAdornment();

            TextView.Bind(OnLayoutChanged);
            TextView.Bind(OnPositionChanged);
            TextView.Bind(new CompositeDisposable(
                OptionValues.Subscribe(OptionValuesOnNext),
                OptionResources.Subscribe(editorFormatMap, OptionResourcesOnNext)));
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!Initialized)
            {
                LineIndicator.OnInitialized();
                ColumnIndicator.OnInitialized();
                Initialized = true;
            }

            LineIndicator.OnLayoutChanged(sender, e);
            ColumnIndicator.OnLayoutChanged(sender, e);
        }

        private void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            LineIndicator.OnPositionChanged(sender, e);
            ColumnIndicator.OnPositionChanged(sender, e);
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

        private void CreateAdornment()
        {
            ColumnIndicator?.Dispose();
            ColumnIndicator = Options.IsEnabledColumnIndicator
                ? new ColumnIndicatorAdornment(TextView, AdornmentLayer, Resources.ColumnIndicatorPen)
                : CaretIndicatorAdornment.Empty;

            LineIndicator?.Dispose();
            LineIndicator = Options.IsEnabledLineIndicator
                ? new LineIndicatorAdornment(TextView, AdornmentLayer, Resources.LineIndicatorPen)
                : CaretIndicatorAdornment.Empty;

            if (Options.IsEnabledLineIndicator)
            {
                TextView.Options.SetOptionValue(DefaultWpfViewOptions.EnableHighlightCurrentLineId, false);
            }
            else
            {
                TextView.Options.RestoreOption(DefaultWpfViewOptions.EnableHighlightCurrentLineId);
            }

            if (Initialized)
            {
                LineIndicator.OnInitialized();
                ColumnIndicator.OnInitialized();
            }
        }

        public void Dispose()
        {
            LineIndicator?.Dispose();
            ColumnIndicator?.Dispose();
        }
    }
}