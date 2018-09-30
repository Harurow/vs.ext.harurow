using System.ComponentModel.Composition;
using Harurow.Extensions.One.ListenerServices;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class TextViewCreationListener
        : IWpfTextViewCreationListener
    {
#pragma warning disable 649, 169

        [Import]
        private IEditorFormatMapService EditorFormatMapService { get; set; }

#pragma warning restore 649, 169

        /// <inheritdoc />
        public void TextViewCreated(IWpfTextView textView)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new HarurowExtensionOneService(textView, EditorFormatMapService);
        }
    }
}
