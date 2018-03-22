using System.Reactive.Disposables;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.RightMargin.AdornmentLayers;
using Harurow.Extensions.RightMargin.Adornments;
using Harurow.Extensions.RightMargin.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.RightMargin.Services
{
    internal sealed class RightMarginService
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private bool Initialized { get; set; }
        private IOptionValues Options { get; set; }
        private OptionResources Resources { get; set; }

        private IAdornment RightMargin { get; set; }

        public RightMarginService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            AdornmentLayer = RightMarginAdornmentLayer.GetAdornmentLayer(textView);

            var editorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);

            Options = OptionValues.ReadFromStore();
            Resources = new OptionResources(editorFormatMap);
            CreateAdornment();

            TextView.Bind(OnLayoutChanged);
            TextView.Bind(new CompositeDisposable(
                OptionValues.Subscribe(OptionValuesOnNext),
                OptionResources.Subscribe(editorFormatMap, OptionResourcesOnNext)));
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!Initialized)
            {
                RightMargin.OnInitialized();
                Initialized = true;
            }

            RightMargin.OnLayoutChanged(sender, e);
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
            RightMargin?.Dispose();
            RightMargin = 0 < Options.RightMargin && Options.RightMargin < 256
                ? new RightMarginAdornment(TextView, AdornmentLayer, Options.RightMargin, Resources.RightMarginBrush)
                : Adornment.Empty;

            if (Initialized)
            {
                RightMargin.OnInitialized();
            }
        }
    }
}