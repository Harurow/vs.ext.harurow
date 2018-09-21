using System;
using Harurow.Extensions.One.Adornments;
using Harurow.Extensions.One.Adornments.LineAdornments;
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
        private RedundantWhiteSpaceAdornment RedundantWhiteSpaceAdornment { get; set; }

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
            RedundantWhiteSpaceAdornment?.OnLayoutChanged(sender, e);
        }

        private void OnTextViewOptionChanged(object sender, EditorOptionChangedEventArgs e)
        {
            if (e.OptionId == new UseVisibleWhitespace().Key.Name)
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
            RightMarginAdornment?.CleanUp();
            RedundantWhiteSpaceAdornment?.CleanUp();
        }

        private void CreateAdornment()
        {
            CreateRightMarginAdornment();
            CreateRedundantWhiteSpacesAdornment();
        }

        private void CreateRightMarginAdornment()
        {
            var layer = TextView.GetBeforeDifferenceChangesAdornmentLayer();
            RightMarginAdornment = new RightMarginAdornment(TextView, layer, Values.RightMargin, Resources.RightMarginBrush);
            RightMarginAdornment.OnInitialized();
        }

        private void CreateRedundantWhiteSpacesAdornment()
        {
            RedundantWhiteSpacePainter CreatePainter()
            {
                var layer = TextView.GetAfterSelectionAdornmentLayer();
                return new RedundantWhiteSpacePainter(TextView, layer,
                    Resources.RedundantWhiteSpacesBrush, Resources.RedundantWhiteSpacesPen);
            }

            bool IsEnabled(RedundantWhiteSpaceMode mode, bool useVisibleWhitespace)
            {
                switch (mode)
                {
                    case RedundantWhiteSpaceMode.True:
                        return true;
                    case RedundantWhiteSpaceMode.UseVisibleWhiteSpace:
                        return useVisibleWhitespace;
                    case RedundantWhiteSpaceMode.False:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }

            var useWhitespace = TextView.Options.GetOptionValue(new UseVisibleWhitespace().Key);

            if (IsEnabled(Values.RedundantWhiteSpaceMode, useWhitespace))
            {
                var lineAdornment = new RedundantWhiteSpacesLineAdornment(TextView, CreatePainter());
                RedundantWhiteSpaceAdornment = new RedundantWhiteSpaceAdornment(TextView, lineAdornment);
                RedundantWhiteSpaceAdornment.OnInitialized();
            }
            else
            {
                RedundantWhiteSpaceAdornment = null;
            }
        }
    }
}
