﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Harurow.Extensions.One.UserInterfaces
{
    [Export(typeof(IMouseProcessorProvider))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [Name("Mouse Processor")]
    internal sealed class MouseProcessorProvider
        : IMouseProcessorProvider
    {
        /// <inheritdoc />
        public IMouseProcessor GetAssociatedProcessor(IWpfTextView textView)
            => new MouseProcessor(textView);
    }
}