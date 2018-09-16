using System;
using Harurow.Extensions.One.Adornments;
using Harurow.Extensions.One.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.AdornmentServices
{
    public class HarurowExtensionOneService
    {
        private IWpfTextView TextView { get; }
        private IEditorFormatMap EditorFormatMap { get; }
        private OptionValues Values { get; set; }
        private OptionResources Resources { get; }

        private bool IsInitialized { get; set; }
        private RightMarginAdornment RightMarginAdornment { get; set; }

        public HarurowExtensionOneService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            EditorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);

            Values = new OptionValues();
            Values.LoadSettingsFromStorage();

            Resources = new OptionResources(EditorFormatMap);

            TextView.LayoutChanged += OnLayoutChanged;
            TextView.Closed += OnClosed;

            OptionObserver.OptionChanged += OnOptionChanged;
            EditorFormatMap.FormatMappingChanged += OnFormatMappingChanged;
        }

        private void OnFormatMappingChanged(object sender, FormatItemsEventArgs e)
        {
            Resources.CreateResource();
            ReBuild();
        }

        private void OnOptionChanged(object sender, OptionEventArgs e)
        {
            Values = e.NewValues;
            ReBuild();
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!IsInitialized)
            {
                CleanUp();
                CreateAdornment();
                IsInitialized = true;
            }

            RightMarginAdornment.OnLayoutChanged(sender, e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.Closed -= OnClosed;
            TextView.LayoutChanged -= OnLayoutChanged;
            OptionObserver.OptionChanged -= OnOptionChanged;
        }

        private void ReBuild()
        {
            if (IsInitialized)
            {
                CleanUp();
                CreateAdornment();
            }
        }

        private void CleanUp()
        {
            RightMarginAdornment?.CleanUp();
        }

        private void CreateAdornment()
        {
            // ReSharper disable ObjectCreationAsStatement
            var layer = TextView.GetBeforeDifferenceChangesAdornmentLayer();
            RightMarginAdornment = new RightMarginAdornment(TextView, layer, Values.RightMargin, Resources.RightMarginBackground);
            RightMarginAdornment.OnInitialized();
        }
    }
}
