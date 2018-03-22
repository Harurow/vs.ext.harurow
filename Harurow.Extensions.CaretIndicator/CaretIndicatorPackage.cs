using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.CaretIndicator.Options;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.CaretIndicator
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad("{F1536EF8-92EC-443C-9ED7-FDADF150DA82}")]
    [ProvideOptionPage(typeof(OptionDialogPage), "Harurow", "CaretIndicator", 0, 0, true)]
    public sealed class CaretIndicatorPackage : Package
    {
        public const string PackageGuidString = "bd5ce0ae-84ed-4ed2-9b46-26e8a56b141e";
    }
}