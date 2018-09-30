using Microsoft.CodeAnalysis;

namespace Harurow.Extensions.One.Analyzer.CodeFixes.Commons
{
    public struct CodeDiagnosticMetaInfo
    {
        public readonly string Id;
        public readonly CodeDiagnosticCategory Category;
        public readonly DiagnosticSeverity DefaultSeverity;
        public readonly bool IsEnabledByDefault;
        public readonly string HelpLinkUri;
        public readonly string[] Tags;

        public CodeDiagnosticMetaInfo(string id, CodeDiagnosticCategory category,
            DiagnosticSeverity defaultSeverity = DiagnosticSeverity.Warning, bool isEnabledByDefault = true,
            string helpLinkUri = null, params string[] tags)
        {
            Id = id;
            Category = category;
            DefaultSeverity = defaultSeverity;
            IsEnabledByDefault = isEnabledByDefault;
            HelpLinkUri = helpLinkUri;
            Tags = tags;
        }
    }
}