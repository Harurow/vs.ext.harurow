using Microsoft.CodeAnalysis;

namespace Harurow.Extensions.One.Analyzer.Commons
{
    public sealed class LocalizableResourceStrings
    {
        private const string TitleSuffix = "Title";
        private const string MessageFormatSuffix = "MessageFormat";
        private const string DescriptionSuffix = "Description";
        private const string CodeFixSuffix = "CodeFix";

        public string Prefix { get; }
        public LocalizableResourceString Title { get; }
        public LocalizableResourceString MessageFormat { get; }
        public LocalizableResourceString Description { get; }
        public string CodeFix { get; }

        public LocalizableResourceStrings(string prefix)
        {
            Prefix = prefix;
            Title = GetString(TitleSuffix);
            MessageFormat = GetString(MessageFormatSuffix);
            Description = GetString(DescriptionSuffix);
            CodeFix = GetString(CodeFixSuffix).ToString();
        }

        private LocalizableResourceString GetString(string suffix)
            =>  LocalizableResourceStringUtil.Get(Prefix + suffix);
    }
}