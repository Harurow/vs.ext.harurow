using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.MultipleSelection.Behaviors;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.MultipleSelection
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideAutoLoad("{F1536EF8-92EC-443C-9ED7-FDADF150DA82}")]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class MultipleSelectionPackage : Package
    {
        public const string PackageGuidString = "b6e02d17-0526-4566-b46e-f3208d94d22e";

        #region Package Members

        protected override void Initialize()
        {
            base.Initialize();

            ThreadHelper.ThrowIfNotOnUIThread();

            var service = (IMenuCommandService) GetService(typeof(IMenuCommandService));

            Bootstrap.Initialize(service);
        }

        #endregion
    }
}