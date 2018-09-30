using Harurow.Extensions.One.Analyzer.Commons;
using Microsoft.CodeAnalysis;

namespace Harurow.Extensions.One.Analyzer.CodeFixes.Commons
{
    public static class CodeDiagnosticMetaInfoExtensions
    {
        public static DiagnosticDescriptor ToDiagnosticDescriptor(this CodeDiagnosticMetaInfo self)
        {
            var res = new LocalizableResourceStrings(self.Id);
            var res2 = new DiagnosticDescriptor(self.Id, res.Title, res.MessageFormat, self.Category.ToString("g"),
                self.DefaultSeverity, self.IsEnabledByDefault, res.Description, self.HelpLinkUri, self.Tags);
            return res2;
        }

        public static LocalizableResourceStrings ToLocalizableResourceStrings(this CodeDiagnosticMetaInfo self)
            => new LocalizableResourceStrings(self.Id);
    }
}