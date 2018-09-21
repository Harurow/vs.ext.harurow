using System;
using Harurow.Extensions.One.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.AdornmentServices
{
    public partial class HarurowExtensionOneService
    {
        private IWpfTextView TextView { get; }
        private IEditorFormatMap EditorFormatMap { get; }
        private OptionValues Values { get; set; }
        private OptionResources Resources { get; }

        private bool IsInitialized { get; set; }

        private UseVisibleWhitespace UseVisibleWhitespace { get; }

        public HarurowExtensionOneService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            EditorFormatMap = editorFormatMapService.GetEditorFormatMap(textView);

            Values = new OptionValues();
            Values.LoadSettingsFromStorage();

            Resources = new OptionResources(EditorFormatMap);

            TextView.LayoutChanged += OnLayoutChanged;
            TextView.Options.OptionChanged += OnTextViewOptionChanged;
            TextView.Closed += OnClosed;

            OptionObserver.OptionChanged += OnOptionChanged;
            EditorFormatMap.FormatMappingChanged += OnFormatMappingChanged;

            UseVisibleWhitespace = new UseVisibleWhitespace();
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!IsInitialized)
            {
                CleanUp();
                CreateAdornment();
                IsInitialized = true;
            }

            // HACK: 9.1. OnLayoutChanged
            RightMarginAdornment.OnLayoutChanged(sender, e);
            RedundantWhiteSpaceAdornment?.OnLayoutChanged(sender, e);
        }

        private void OnTextViewOptionChanged(object sender, EditorOptionChangedEventArgs e)
        {
            // HACK: 9.4. OnTextViewOptionChanged. If need it.
            if (e.OptionId == UseVisibleWhitespace.Key.Name)
            {
                if (IsInitialized)
                {
                    RedundantWhiteSpaceAdornment?.CleanUp();
                    CreateRedundantWhiteSpacesAdornment();
                }
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.Closed -= OnClosed;
            TextView.LayoutChanged -= OnLayoutChanged;
            OptionObserver.OptionChanged -= OnOptionChanged;
        }

        private void OnOptionChanged(object sender, OptionEventArgs e)
        {
            Values = e.NewValues;
            ReBuild();
        }

        private void OnFormatMappingChanged(object sender, FormatItemsEventArgs e)
        {
            Resources.CreateResource();
            ReBuild();
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
            // HACK: 9.2. CleanUp
            RightMarginAdornment?.CleanUp();
            RedundantWhiteSpaceAdornment?.CleanUp();
        }

        private void CreateAdornment()
        {
            // HACK: 9.3. CreateAdornment
            CreateRightMarginAdornment();
            CreateRedundantWhiteSpacesAdornment();
        }
    }
}
