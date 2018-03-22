using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.RightMargin.Options;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.RightMargin
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionDialogPage), "Harurow", "RightMargin", 0, 0, true)]
    public sealed class RightMarginPackage : Package
    {
        public const string PackageGuidString = "8aa8e288-1c67-4494-b9a3-984b71538f11";
    }
}