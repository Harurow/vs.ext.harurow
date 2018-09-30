using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Harurow.Extensions.One.Utilities
{
    internal static class TextViewHelper
    {
        public static IVsTextView CurrentVsTextView
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var textManager = VsTextManager;
                if (textManager != null)
                {
                    textManager.GetActiveView(1, null, out var activeTextView);
                    return activeTextView;
                }

                return null;
            }
        }

        public static IWpfTextView CurrentWpfTextView
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var textView = CurrentVsTextView;
                if (textView != null)
                {
                    return VsEditorAdaptersFactoryService?.GetWpfTextView(textView);
                }

                return null;
            }
        }

        private static IVsTextManager VsTextManager
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                return ServiceProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;
            }
        }

        private static IVsEditorAdaptersFactoryService VsEditorAdaptersFactoryService
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return GetComponentModelService<IVsEditorAdaptersFactoryService>();
            }
        }

        private static ServiceProvider ServiceProvider
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return ServiceProvider.GlobalProvider;
            }
        }

        private static IComponentModel ComponentModel
            => Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;

        private static T GetComponentModelService<T>()
            where T : class
            => ComponentModel?.GetService<T>();
    }
}