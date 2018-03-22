using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.RedundantWhiteSpace.Options;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.RedundantWhiteSpace
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionDialogPage), "Harurow", "RedundantWhiteSpace", 0, 0, true)]
    public sealed class RedundantWhiteSpacePackage : Package
    {
        public const string PackageGuidString = "6541cced-93fa-4ef4-824d-ba3f503493c6";
    }
}
