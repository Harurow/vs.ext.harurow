using System;
using Harurow.Extensions.One.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.TextViewCreationListeners
{
    // HACK: 5. イベントと紐づける
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

            OnAttached(this, EventArgs.Empty);
        }

        private void OnAttached(object sender, EventArgs e)
        {
            // HACK: 5.1. OnAttached. 初回イベント
            UpdateIsLockedWheelZoom();
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!IsInitialized)
            {
                CleanUp();
                CreateAdornment();
                IsInitialized = true;
            }

            // HACK: 5.2. OnLayoutChanged. レイアウトの変更イベント
            RightMarginAdornment.OnLayoutChanged(sender, e);
            RedundantWhiteSpaceAdornment?.OnLayoutChanged(sender, e);
            LineBreaksAdornment?.OnLayoutChanged(sender, e);
        }

        private void OnTextViewOptionChanged(object sender, EditorOptionChangedEventArgs e)
        {
            // HACK: 5.3. OnTextViewOptionChanged. オプション(Visual Studio)の変更イベント
            if (e.OptionId == UseVisibleWhitespace.Key.Name)
            {
                if (IsInitialized)
                {
                    RedundantWhiteSpaceAdornment?.CleanUp();
                    LineBreaksAdornment?.CleanUp();
                    CreateRedundantWhiteSpacesAdornment();
                    CreateLineBreaksAdornment();
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
            // HACK: 5.4. OnOptionChanged. オプション(Custom)の変更イベント
            Values = e.NewValues;
            ReBuild();
            UpdateIsLockedWheelZoom();
        }

        private void OnFormatMappingChanged(object sender, FormatItemsEventArgs e)
        {
            // HACK: 5.5. OnFormatMappingChanged. オプション(色)の変更イベント
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
            // HACK: 5.6. CleanUp
            RightMarginAdornment?.CleanUp();
            RedundantWhiteSpaceAdornment?.CleanUp();
            LineBreaksAdornment?.CleanUp();
        }

        private void CreateAdornment()
        {
            // HACK: 5.7. CreateAdornment
            CreateRightMarginAdornment();
            CreateRedundantWhiteSpacesAdornment();
            CreateLineBreaksAdornment();
        }
    }
}
