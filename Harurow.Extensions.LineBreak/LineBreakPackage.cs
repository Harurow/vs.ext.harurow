using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Harurow.Extensions.LineBreak.Options;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.LineBreak
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionDialogPage), "Harurow", "LineBreak", 0, 0, true)]
    public sealed class LineBreakPackage : Package
    {
        public const string PackageGuidString = "0b088061-c269-45d5-9908-484aede9e2f0";
    }
}
