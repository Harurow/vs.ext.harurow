using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.EncodingInfo.Options;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.EncodingInfo
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionDialogPage), "Harurow", "EncodingInfo", 0, 0, true)]
    public sealed class EncodingInfoPackage : Package
    {
        public const string PackageGuidString = "1abd4ba7-4dad-46ed-be49-8a478fd5714d";
    }
}
