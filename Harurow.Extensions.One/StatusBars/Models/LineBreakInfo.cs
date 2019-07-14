using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.Adornments.LineBreaks;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars.Models
{
    internal class LineBreakInfo : IStatusBarInfoItem
    {
        public IReactiveProperty<string> Text { get; }
        public IReactiveProperty<Brush> Foreground { get; }
        public IReactiveProperty<Brush> Background { get; }
        public IReactiveProperty<Visibility> Visibility { get; }

        private ITextDocument Document { get; }

        public LineBreakInfo(IWpfTextView textView, IReactiveProperty<Visibility> visibility,
            CompositeDisposable disposable)
        {
            Document = textView.GetTextDocument();

            if (Document != null)
            {
                Text = new ReactiveProperty<string>("").AddTo(disposable);
                Foreground = new ReactiveProperty<Brush>().AddTo(disposable);
                Background = new ReactiveProperty<Brush>().AddTo(disposable);
                Visibility = visibility;

                Observable.FromEventPattern<TextDocumentFileActionEventArgs>(
                        h => Document.FileActionOccurred += h,
                        h => Document.FileActionOccurred -= h)
                    .Subscribe(_ => Analyze())
                    .AddTo(disposable);

                Analyze();
            }
        }

        /// <inheritdoc />
        public void Activate()
        {
            if (Document != null)
            {
                Analyze();
            }
        }

        /// <inheritdoc />
        public void Inactivate()
        {
        }

        /// <inheritdoc />
        public void Click()
        {
            if (Document == null) return;

            Analyze();

            if (Text.Value == "" || Text.Value == "CRLF")
            {
                return;
            }

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var result = VsShellUtilities.ShowMessageBox(
                    ServiceProvider.GlobalProvider,
                    $"ファイル: \"{Path.GetFileName(Document.FilePath)}\" の 改行コードを \"CRLF\" へ 変換します。",
                    "改行コードを \"CRLF\" へ変換しますか？",
                    OLEMSGICON.OLEMSGICON_QUERY,
                    OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND);

                if (result == 6) // Yes
                {
                    var tb = Document.TextBuffer;
                    using (var edit = tb.CreateEdit())
                    {
                        Document
                            .TextBuffer
                            .CurrentSnapshot
                            .Lines
                            .Where(line => line.LineBreakLength == 1)
                            .Select(line => new Span(
                                line.EndIncludingLineBreak.Position - line.LineBreakLength,
                                line.LineBreakLength))
                            .OrderByDescending(s => s.Start)
                            .ForEach(s => edit.Replace(s, "\r\n"));

                        edit.Apply();
                    }
                }
            });
        }

        private void Analyze()
        {
            if (Document == null || Document.TextBuffer == null)
            {
                return;
            }

            var lineBreaks = Document
                .TextBuffer
                .CurrentSnapshot
                .Lines
                .Where(l => l.LineBreakLength > 0)
                .Select(l => l.GetLineBreakKind())
                .Where(lb => lb != LineBreakKind.Unknown)
                .GroupBy(lb => lb)
                .OrderByDescending(g => g.Count())
                .ToArray();

            if (lineBreaks.Length == 0)
            {
                UpdateLineBreakAnalyzedInfo(LineBreakKind.Unknown, false);
            }

            UpdateLineBreakAnalyzedInfo(lineBreaks[0].Key, lineBreaks.Length != 1);
        }

        private void UpdateLineBreakAnalyzedInfo(LineBreakKind lineBreak, bool isMixture)
        {
            if (lineBreak == LineBreakKind.Unknown)
            {
                Text.Value = "";
                Background.Value = null;
                return;
            }

            switch (lineBreak)
            {
                case LineBreakKind.CrLf:
                    Text.Value = "CRLF";
                    break;
                case LineBreakKind.Cr:
                    Text.Value = "CR";
                    break;
                case LineBreakKind.Lf:
                    Text.Value = "LF";
                    break;
                case LineBreakKind.Nel:
                    Text.Value = "NEL";
                    break;
                case LineBreakKind.Ls:
                    Text.Value = "LS";
                    break;
                case LineBreakKind.Ps:
                    Text.Value = "PS";
                    break;
            }

            if (isMixture)
            {
                Text.Value += "+";
                Foreground.Value = Brushes.White;
                Background.Value = Brushes.DarkOrange;
            }
            else if (Text.Value != "CRLF")
            {
                Foreground.Value = Brushes.White;
                Background.Value = Brushes.ForestGreen;
            }
            else
            {
                Foreground.Value = StatusBarInfoControl.GetUiContextTextBrush();
                Background.Value = null;
            }
        }
    }
}