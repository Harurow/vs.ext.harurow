using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Harurow.Extensions.One
{
    internal static class TextViewHelper
    {
        public static IWpfTextView GetCurrentWpfTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var textView = GetCurrentTextView();

            if (textView == null)
            {
                return null;
            }

            var adapter = ComponentModel?.GetService<IVsEditorAdaptersFactoryService>();
            return adapter?.GetWpfTextView(textView);
        }

        public static IVsTextView GetCurrentTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!(ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager)) is IVsTextManager textManager))
            {
                return null;
            }

            textManager.GetActiveView(1, null, out var activeTextView);
            return activeTextView;
        }

        private static IComponentModel ComponentModel
            => Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
    }
}