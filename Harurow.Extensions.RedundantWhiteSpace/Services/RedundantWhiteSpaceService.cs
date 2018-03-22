using System;
using System.Reactive.Disposables;
using Harurow.Extensions.Adornments;
using Harurow.Extensions.Adornments.TextViewLines;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.RedundantWhiteSpace.AdornmentLayers;
using Harurow.Extensions.RedundantWhiteSpace.Adornments;
using Harurow.Extensions.RedundantWhiteSpace.Adornments.TextViewLines;
using Harurow.Extensions.RedundantWhiteSpace.Options;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.RedundantWhiteSpace.Services
{
    internal sealed class RedundantWhiteSpaceService
    {
        private IWpfTextView TextView { get; }
        private IAdornmentLayer AdornmentLayer { get; }

        private bool Initialized { get; set; }
        private IOptionValues Options { get; set; }
        private OptionResources Resources { get; set; }

        private IAdornment RedundantWhiteSpace { get; set; }

        public RedundantWhiteSpaceService(IWpfTextView textView, IEditorFormatMapService editorFormatMapService)
        {
            TextView = textView;
            AdornmentLayer = RedundantWhiteSpaceAdornmentLayer.GetAdornmentLayer(textView);

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
                RedundantWhiteSpace.OnInitialized();
                Initialized = true;
            }

            RedundantWhiteSpace.OnLayoutChanged(sender, e);
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

        private RedundantWhiteSpacePainter CreateRedundantWhiteSpacesPainter()
            => new RedundantWhiteSpacePainter(TextView, AdornmentLayer, Resources.RedundantWhiteSpacesHighlightBrush,
                Resources.RedundantWhiteSpacesHighlightPen);

        private static bool IsEnabled(RedundantWhiteSpaceMode mode, bool useVisibleWhitespace)
        {
            switch (mode)
            {
                case RedundantWhiteSpaceMode.True:
                    return true;
                case RedundantWhiteSpaceMode.False:
                    return false;
                case RedundantWhiteSpaceMode.UseVisibleWhiteSpace:
                    return useVisibleWhitespace;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void CreateAdornment()
        {
            AdornmentLayer.RemoveAllAdornments();
            var whitespace = TextView.Options.GetOptionValue(new UseVisibleWhitespace().Key);

            var redundantWhiteSpace = IsEnabled(Options.RedundantWhiteSpacesHighlightMode, whitespace)
                ? new RedundantWhiteSpacesTextViewLineAdronment(TextView, CreateRedundantWhiteSpacesPainter())
                : TextViewLineAdornment.Empty;

            RedundantWhiteSpace?.Dispose();
            RedundantWhiteSpace = new RedundantWhiteSpaceAdornment(TextView, redundantWhiteSpace);

            if (Initialized)
            {
                RedundantWhiteSpace.OnInitialized();
            }
        }
    }
}