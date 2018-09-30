using System.Linq;
using Microsoft.CodeAnalysis;

namespace Harurow.Extensions.One.Analyzer.Commons
{
    public static class SyntaxTriviaListExtensions
    {
        public static SyntaxTriviaList WithoutTrivia(this SyntaxTriviaList self, SyntaxTrivia trivia)
        {
            var triviaSpanStart = trivia.SpanStart;
            return new SyntaxTriviaList(self.Where(t => t.SpanStart != triviaSpanStart));
        }
    }
}
