using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Harurow.Extensions.One.Commands;
using Harurow.Extensions.One.Options;
using Harurow.Extensions.One.StatusBars;
using Harurow.Extensions.One.Utilities;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Harurow.Extensions.One
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(PackageGuidString)]
    [ProvideOptionPage(typeof(OptionPage), "Harurow", "One", 0, 0, true)]
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class Package : AsyncPackage
    {
        public const string PackageGuidString = "9b10b519-5596-4de8-985a-765d8cf94f79";

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            var menuCommandService = (IMenuCommandService)await GetServiceAsync(typeof(IMenuCommandService));

            // ReSharper disable ObjectCreationAsStatement
            new SetUtf8WithBomCommand(menuCommandService);

            var statusBar = new StatusBarProvider(Application.Current.MainWindow);
            StatusBarInfoControl.AddTo(statusBar);
            Debug.WriteLine("* Init Harurow.One *");
        }

        #endregion
    }
}
