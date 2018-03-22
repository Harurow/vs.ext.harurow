using System.ComponentModel.Composition;
using Harurow.Extensions.Extensions;
using Harurow.Extensions.MultipleSelection.Services;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.MultipleSelection
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
    internal sealed class MultipleSelectionServiceProvider
        : IWpfTextViewCreationListener
    {
#pragma warning disable 649, 169

        [Import]
        private IEditorFormatMapService EditorFormatMapService { get; set; }

        [Import]
        private IOutliningManagerService OutliningManagerService { get; set; }

#pragma warning restore 649, 169

        public void TextViewCreated(IWpfTextView textView)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new MultipleSelectionService(textView, textView.GetVsTextView(), EditorFormatMapService,
                OutliningManagerService);
        }
    }
}