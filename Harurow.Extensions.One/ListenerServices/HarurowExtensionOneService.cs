using System;
using Harurow.Extensions.One.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.ListenerServices
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
            TextView.Caret.PositionChanged += OnPositionChanged;
            TextView.Options.OptionChanged += OnTextViewOptionChanged;
            TextView.Selection.SelectionChanged += OnSelectionChanged;
            OptionObserver.OptionChanged += OnOptionChanged;
            EditorFormatMap.FormatMappingChanged += OnFormatMappingChanged;
            TextView.Closed += OnClosed;

            UseVisibleWhitespace = new UseVisibleWhitespace();

            OnAttached(this, EventArgs.Empty);
        }

        private void OnAttached(object sender, EventArgs e)
        {
            // HACK: 5.1. OnAttached. 初回イベント
            AttachIsLockedWheelZoom();
            AttachEncodingInfo();
        }

        private void OnPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            // HACK: 5.2. OnPositionChanged. ポジションの変更イベント
            LineIndicator?.OnPositionChanged(sender, e);
            ColumnIndicator?.OnPositionChanged(sender, e);
        }

        private void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (!IsInitialized)
            {
                CleanUp();
                CreateAdornment();
                IsInitialized = true;
            }

            // HACK: 5.3. OnLayoutChanged. レイアウトの変更イベント
            RightMarginAdornment?.OnLayoutChanged(sender, e);
            LineBreaksAdornment?.OnLayoutChanged(sender, e);
            LineIndicator?.OnLayoutChanged(sender, e);
            ColumnIndicator?.OnLayoutChanged(sender, e);
        }

        private void OnTextViewOptionChanged(object sender, EditorOptionChangedEventArgs e)
        {
            // HACK: 5.4. OnTextViewOptionChanged. オプション(Visual Studio)の変更イベント
            if (e.OptionId == UseVisibleWhitespace.Key.Name)
            {
                if (IsInitialized)
                {
                    LineBreaksAdornment?.CleanUp();
                    CreateLineBreaksAdornment();
                }
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            // HACK: 5.5. OnSelectionChanged. 選択変更のイベント
            LineIndicator?.OnSelectionChanged(sender, e);
            ColumnIndicator?.OnSelectionChanged(sender, e);
        }

        private void OnOptionChanged(object sender, OptionEventArgs e)
        {
            // HACK: 5.6. OnOptionChanged. オプション(Custom)の変更イベント
            Values = e.NewValues;
            ReBuild();
            AttachIsLockedWheelZoom();
        }

        private void OnFormatMappingChanged(object sender, FormatItemsEventArgs e)
        {
            // HACK: 5.7. OnFormatMappingChanged. オプション(色)の変更イベント
            Resources.CreateResource();
            ReBuild();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.LayoutChanged -= OnLayoutChanged;
            TextView.Caret.PositionChanged -= OnPositionChanged;
            TextView.Options.OptionChanged -= OnTextViewOptionChanged;
            TextView.Selection.SelectionChanged -= OnSelectionChanged;
            OptionObserver.OptionChanged -= OnOptionChanged;
            EditorFormatMap.FormatMappingChanged -= OnFormatMappingChanged;
            TextView.Closed -= OnClosed;
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
            // HACK: 5.7. CleanUp
            RightMarginAdornment?.CleanUp();
            LineBreaksAdornment?.CleanUp();
            LineIndicator?.CleanUp();
            ColumnIndicator?.CleanUp();

            RightMarginAdornment = null;
            LineBreaksAdornment = null;
            LineIndicator = null;
            ColumnIndicator = null;
        }

        private void CreateAdornment()
        {
            // HACK: 5.8. CreateAdornment
            CreateRightMarginAdornment();
            CreateLineBreaksAdornment();
            CreateCaretIndicatorAdornment();
        }
    }
}
