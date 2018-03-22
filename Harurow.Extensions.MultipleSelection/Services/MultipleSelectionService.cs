using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using EnvDTE;
using EnvDTE80;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.MultipleSelection.AdornmentLayers;
using Harurow.Extensions.MultipleSelection.Adronments;
using Harurow.Extensions.MultipleSelection.Behaviors.Filters;
using Harurow.Extensions.MultipleSelection.Options;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Harurow.Extensions.MultipleSelection.Services
{
    internal sealed class MultipleSelectionService
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private bool Initialized { get; set; }
        private OptionResources Resources { get; set; }

        private MultipleSelectionCommandFilter MultipleSelectionCommandFilter { get; }
        private MultipleSelectionAdornment MultipleSelectionsAdornment { get; set; }

        public MultipleSelectionService(IWpfTextView textView, IVsTextView viewAdapter,
            IEditorFormatMapService editorFormatMapService, IOutliningManagerService outliningManagerService)
        {
            TextView = textView;
            AdornmentLayer = MultipleSelectionAdornmentLayer.GetAdornmentLayer(TextView);

            var editorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);
            var outliningManager = outliningManagerService.GetOutliningManager(textView);

            Resources = new OptionResources(editorFormatMap);

            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE2;

            MultipleSelectionCommandFilter =
                new MultipleSelectionCommandFilter(TextView, viewAdapter, dte, outliningManager);

            CreateAdornment();

            TextView.Bind(OnLayoutChanged);
            TextView.Bind(new CompositeDisposable(
                MultipleSelectionCommandFilter,
                outliningManager,
                OptionResources.Subscribe(editorFormatMap, OptionResourcesOnNext),
                Observable.FromEventPattern<TrackingSelectionsEventArgs>(
                        h => MultipleSelectionCommandFilter.TrackingSelections.Changed += h,
                        h => MultipleSelectionCommandFilter.TrackingSelections.Changed -= h)
                    .Subscribe(e => OnTrackingSelectionsChanged(e.Sender, e.EventArgs))));
        }

        private void OptionResourcesOnNext(OptionResources resources)
        {
            Resources = resources;
            CreateAdornment();
            if (Initialized)
            {
                MultipleSelectionsAdornment.OnInitialized();
            }
        }

        private void OnTrackingSelectionsChanged(object sender, TrackingSelectionsEventArgs e)
            => MultipleSelectionsAdornment.OnTrackingSelectionsChanged(sender, e);

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!Initialized)
            {
                MultipleSelectionsAdornment.OnInitialized();
                Initialized = true;
            }
            MultipleSelectionsAdornment.OnLayoutChanged(sender, e);
        }

        private void CreateAdornment()
        {
            MultipleSelectionsAdornment?.Dispose();
            MultipleSelectionsAdornment =
                new MultipleSelectionAdornment(TextView, AdornmentLayer, MultipleSelectionCommandFilter.TrackingSelections,
                    Resources.CaretsBrush, Resources.CaretsSelectionsBrush, Resources.CaretsSelectionsPen);
        }
    }
}