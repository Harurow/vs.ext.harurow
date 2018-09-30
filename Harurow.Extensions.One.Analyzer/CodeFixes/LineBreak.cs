using System.Collections.Concurrent;
using System.Collections.Generic;
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
using Reactive.Bindings;

namespace Harurow.Extensions.One.Analyzer.CodeFixes
{
    using LineBreakAnalyzedInfoRxProp = IReactiveProperty<LineBreakAnalyzedInfo>;
    using LineBreakAnalyzedInfoDic = ConcurrentDictionary<string, IReactiveProperty<LineBreakAnalyzedInfo>>;

    public class LineBreakAnalyzedInfo
    {
        public static readonly LineBreakAnalyzedInfoDic Infos;

        static LineBreakAnalyzedInfo()
        {
            Infos = new LineBreakAnalyzedInfoDic();
        }

        public string LineBreak { get; }
        public bool IsMixture { get; }

        public LineBreakAnalyzedInfo(string lineBreak, bool isMixture)
        {
            LineBreak = lineBreak;
            IsMixture = isMixture;
        }
    }

    public static class LineBreak
    {
        #region meta

        private const string Id = "HEOCF002";

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
                Report(context, root);
            }

            private void Report(SyntaxTreeAnalysisContext context, SyntaxNode root)
            {
                var ballotBox = new Dictionary<string, List<SyntaxTrivia>>
                {
                    ["\r\n"] = new List<SyntaxTrivia>(),
                    ["\r"] = new List<SyntaxTrivia>(),
                    ["\n"] = new List<SyntaxTrivia>(),
                    ["\u0085"] = new List<SyntaxTrivia>(),
                    ["\u2028"] = new List<SyntaxTrivia>(),
                    ["\u2029"] = new List<SyntaxTrivia>(),
                };

                var text = root.GetText();

                // コメントの改行コードが判断できないがよしとする
                // 文字列の改行コードはそもそも判定外とする想定
                root.DescendantTrivia(null, true)
                    .Where(t => t.IsKind(SyntaxKind.EndOfLineTrivia))
                    .ForEach(t =>
                    {
                        var lineBreak = text.GetSubText(t.Span).ToString();
                        if (ballotBox.TryGetValue(lineBreak, out var list))
                        {
                            list.Add(t);
                        }
                    });

                var validBallotBox = ballotBox
                    .Where(kv => kv.Value.Count > 0)
                    .OrderByDescending(kv => kv.Value.Count)
                    .ToArray();

                var path = context.Tree.FilePath.ToLower();
                var info = LineBreakAnalyzedInfo.Infos.GetOrAdd(path, key => new ReactiveProperty<LineBreakAnalyzedInfo>());

                if (validBallotBox.Length == 0)
                {
                    info.Value = new LineBreakAnalyzedInfo("", false);
                    return;
                }

                var majority = validBallotBox[0];
                if (validBallotBox.Length == 1)
                {
                    info.Value = new LineBreakAnalyzedInfo(majority.Key, false);
                    return;
                }
                info.Value = new LineBreakAnalyzedInfo(majority.Key, true);

                var majorityLineBreak = majority.Key;
                var props = CreateProperties(majorityLineBreak);

                validBallotBox
                    .Where(kv => kv.Key != majorityLineBreak)
                    .SelectMany(kv => kv.Value)
                    .ForEach(t => Report(context, t, props));
            }

            private static void Report(SyntaxTreeAnalysisContext context, SyntaxTrivia trivia, ImmutableDictionary<string, string> props)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, trivia.GetLocation(), props));
            }

            private static ImmutableDictionary<string, string> CreateProperties(string lineBreak)
            {
                var builder = ImmutableDictionary.CreateBuilder<string, string>();
                builder.Add("LineBreak", lineBreak);
                return builder.ToImmutable();
            }
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

                var lineBreak = diagnostic.Properties["LineBreak"];
                var newTrivia = SyntaxFactory.ParseTrailingTrivia(lineBreak)[0];

                var newToken = token.ReplaceTrivia(trivia, newTrivia);
                var newRoot = root.ReplaceToken(token, newToken);
                var newDoc = doc.WithSyntaxRoot(newRoot);

                return Task.FromResult(newDoc);
            }
        }
    }
}