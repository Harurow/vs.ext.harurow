using System.ComponentModel.Composition;
using Harurow.Extensions.LineBreak.Services;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.LineBreak
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
    internal sealed class LineBreakServiceProvider
        : IWpfTextViewCreationListener
    {
#pragma warning disable 649, 169

        [Import]
        private IEditorFormatMapService EditorFormatMapService { get; set; }

#pragma warning restore 649, 169

        // ReSharper disable once ObjectCreationAsStatement
        public void TextViewCreated(IWpfTextView textView)
            => new LineBreakService(textView, EditorFormatMapService);
    }
}