using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harurow.Extensions.One.Analyzer.CodeFixes.Commons;
using Harurow.Extensions.One.Analyzer.Commons;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Harurow.Extensions.One.Analyzer.CodeFixes
{
    public static class VerboseWhitespace
    {
        #region meta

        private const string Id = "HEOCF001";

        private static readonly CodeDiagnosticMetaInfo MetaInfo =
            new CodeDiagnosticMetaInfo(Id, CodeDiagnosticCategory.CodeFormat);

        private static readonly DiagnosticDescriptor Rule = MetaInfo.ToDiagnosticDescriptor();

        #endregion

        [DiagnosticAnalyzer(LanguageNames.CSharp)]
        public class Analyzer : DiagnosticAnalyzer
        {
            public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
                => ImmutableArray.Create(Rule);

            public override void Initialize(AnalysisContext context)
            {
                context.RegisterSyntaxTreeAction(Analyze);
            }

            private void Analyze(SyntaxTreeAnalysisContext context)
            {
                var root = context.Tree.GetRoot(context.CancellationToken);

                root.DescendantTokens()
                    .ForEach(token => Report(context, token));
            }

            private static void Report(SyntaxTreeAnalysisContext context, SyntaxToken token)
            {
                var isEof = token.IsKind(SyntaxKind.EndOfFileToken);

                if (token.HasLeadingTrivia)
                {
                    Report(context, token.LeadingTrivia, isEof);
                }

                if (token.HasTrailingTrivia)
                {
                    Report(context, token.TrailingTrivia, isEof);
                }
            }

            private static void Report(SyntaxTreeAnalysisContext context, SyntaxTriviaList list, bool isEof)
            {
                list.Pairs(TriviaFilter)
                    .ForEach(p => context.ReportDiagnostic(Diagnostic.Create(Rule, p.Item1.GetLocation())));

                if (isEof)
                {
                    var lastTravis = list[list.Count - 1];
                    if (lastTravis.IsKind(SyntaxKind.WhitespaceTrivia))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, lastTravis.GetLocation()));
                    }
                }
            }

            private static bool TriviaFilter(SyntaxTrivia left, SyntaxTrivia right)
                => left.IsKind(SyntaxKind.WhitespaceTrivia) &&
                   right.IsKind(SyntaxKind.EndOfLineTrivia);
        }

        [ExportCodeFixProvider(LanguageNames.CSharp)]
        [Shared]
        public class CodeFixer : CodeFixProvider
        {
            public sealed override ImmutableArray<string> FixableDiagnosticIds
                => ImmutableArray.Create(Id);

            public sealed override FixAllProvider GetFixAllProvider()
                => WellKnownFixAllProviders.BatchFixer;

            public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
            {
                var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
                var title = MetaInfo.ToLocalizableResourceStrings().CodeFix;

                context.Diagnostics
                    .Where(d => d.Id == Id)
                    .ForEach(d => RegisterCodeFix(title, context, root, d));
            }

            private void RegisterCodeFix(string title, CodeFixContext context, SyntaxNode root, Diagnostic diagnostic)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title,
                        c => DoActionAsync(context, root, diagnostic, c),
                        title),
                    diagnostic);
            }

            // ReSharper disable once UnusedParameter.Local
            private static Task<Document> DoActionAsync(CodeFixContext context, SyntaxNode root,
                Diagnostic diagnostic, CancellationToken cancellationToken)
            {
                var span = diagnostic.Location.SourceSpan;
                var doc = context.Document;
                var trivia = root.FindTrivia(span.Start);
                var token = trivia.Token;

                var newToken = token.RemoveTrivia(trivia);
                var newRoot = root.ReplaceToken(token, newToken);
                var newDoc = doc.WithSyntaxRoot(newRoot);

                return Task.FromResult(newDoc);
            }
        }
    }
}