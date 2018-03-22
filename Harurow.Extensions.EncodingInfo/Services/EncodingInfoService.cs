using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.EncodingInfo.AdornmentLayers;
using Harurow.Extensions.EncodingInfo.Adronments;
using Harurow.Extensions.EncodingInfo.Options;
using Harurow.Extensions.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.EncodingInfo.Services
{
    internal sealed class EncodingInfoService
    {
        private TimeSpan EncodeNameCloseDelay { get; } = TimeSpan.FromSeconds(15);

        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private bool Initialized { get; set; }
        private IOptionValues Options { get; set; }
        private OptionResources Resources { get; set; }

        private IAdornment EncodingNameLabel { get; set; }
        private IDisposable EncodingDelayDisposer { get; set; }

        public EncodingInfoService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            AdornmentLayer = EncodingInfoAdornmentLayer.GetAdornmentLayer(textView);

            var editorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);

            Options = OptionValues.ReadFromStore();
            Resources = new OptionResources(editorFormatMap);
            CreateAdornment();

            TextView.Bind(OnLayoutChanged);
            TextView.Bind(OnEncodingChanged);
            TextView.Bind(new CompositeDisposable(
                OptionValues.Subscribe(OptionValuesOnNext),
                OptionResources.Subscribe(editorFormatMap, OptionResourcesOnNext)));
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!Initialized)
            {
                EncodingNameLabel.OnInitialized();
                Initialized = true;
            }

            EncodingNameLabel.OnLayoutChanged(sender, e);
        }

        private void OnEncodingChanged(object sender, EncodingChangedEventArgs e)
        {
            CreateAdornment();
            if (Initialized)
            {
                EncodingNameLabel.OnInitialized();
            }
        }

        private void OptionValuesOnNext(IOptionValues optionValues)
        {
            Options = optionValues;
        }

        private void OptionResourcesOnNext(OptionResources resources)
        {
            Resources = resources;
        }

        private void CreateAdornment()
        {
            EncodingDelayDisposer?.Dispose();
            EncodingNameLabel?.Dispose();
            EncodingNameLabel = new EncodingNameLabelAdornment(TextView, AdornmentLayer, Options, Resources);

            if (Options.IsEnabledAutoHide)
            {
                EncodingDelayDisposer = Observable.Return(EncodingNameLabel)
                    .Delay(EncodeNameCloseDelay)
                    .ObserveOnDispatcher()
                    .Subscribe(_ =>
                    {
                        EncodingNameLabel?.Dispose();
                        EncodingNameLabel = Adornment.Empty;
                        EncodingDelayDisposer?.Dispose();
                        EncodingDelayDisposer = null;
                    });
            }
        }
    }
}