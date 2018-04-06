using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.Commands.Behaviors;
using Harurow.Extensions.Commands.Options;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.Commands
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad("{F1536EF8-92EC-443C-9ED7-FDADF150DA82}")]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionDialogPage), "Harurow", "Commands", 0, 0, true)]
    public sealed class CommandsPackage : Package
    {
        public const string PackageGuidString = "465d2000-2106-4cec-a423-fdc054883419";

        #region Package Members

        protected override void Initialize()
        {
            base.Initialize();

            ThreadHelper.ThrowIfNotOnUIThread();
            var service = (IMenuCommandService)GetService(typeof(IMenuCommandService));

            Bootstrap.Initialize(service);
        }

        #endregion
    }
}
