using Microsoft.CodeAnalysis;

namespace Harurow.Extensions.One.Analyzer.Commons
{
    public static class SyntaxTokenExtensions
    {
        public static SyntaxToken RemoveTrivia(this SyntaxToken self, SyntaxTrivia trivia)
            => trivia.SpanStart < self.SpanStart
                ? self.WithLeadingTrivia(self.LeadingTrivia.WithoutTrivia(trivia))
                : self.WithTrailingTrivia(self.TrailingTrivia.WithoutTrivia(trivia));
    }
}