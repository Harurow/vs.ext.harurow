using System.ComponentModel.Composition;
using Harurow.Extensions.Commands.Services;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.Commands
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.PrimaryDocument)]
    internal sealed class CommandsServiceProvider
        : IWpfTextViewCreationListener
    {
        // ReSharper disable once ObjectCreationAsStatement
        public void TextViewCreated(IWpfTextView textView)
        {
            new MouseWheelZoomLockService(textView);
        }
    }
}