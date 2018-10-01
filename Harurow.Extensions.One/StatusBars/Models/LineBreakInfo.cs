using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.Analyzer.CodeFixes;
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

            Text = new ReactiveProperty<string>("").AddTo(disposable);
            Foreground = new ReactiveProperty<Brush>().AddTo(disposable);
            Background = new ReactiveProperty<Brush>().AddTo(disposable);
            Visibility = visibility;

            var path = Document.FilePath.ToLower();

            LineBreakAnalyzedInfo
                .Infos.GetOrAdd(path, key => new ReactiveProperty<LineBreakAnalyzedInfo>())
                .Subscribe(UpdateLineBreakAnalyzedInfo)
                .AddTo(disposable);
        }

        public void Click()
        {
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

        private void UpdateLineBreakAnalyzedInfo(LineBreakAnalyzedInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.LineBreak))
            {
                Text.Value = "";
                Background.Value = null;
                return;
            }

            switch (info.LineBreak)
            {
                case "\r\n":
                    Text.Value = "CRLF";
                    break;
                case "\r":
                    Text.Value = "CR";
                    break;
                case "\n":
                    Text.Value = "LF";
                    break;
                case "\u0085":
                    Text.Value = "NEL";
                    break;
                case "\u2028":
                    Text.Value = "LS";
                    break;
                case "\u2029":
                    Text.Value = "PS";
                    break;
            }

            if (info.IsMixture)
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