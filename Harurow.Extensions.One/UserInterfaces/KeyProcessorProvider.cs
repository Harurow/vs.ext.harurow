using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One.UserInterfaces
{
    [Export(typeof(IKeyProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [Name("Key Processor")]
    internal sealed class KeyProcessorProvider
        : IKeyProcessorProvider
    {
        /// <inheritdoc />
        public Microsoft.VisualStudio.Text.Editor.KeyProcessor GetAssociatedProcessor(IWpfTextView textView)
            => new KeyProcessor(textView);
    }
}