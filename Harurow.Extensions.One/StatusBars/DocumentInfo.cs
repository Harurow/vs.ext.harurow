using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Windows.Media;
using Harurow.Extensions.One.Adornments.LineBreaks;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars
{
    internal class DocumentInfo
    {

        public IReactiveProperty<string> EncodingName { get; }
        public IReactiveProperty<Brush> EncodingBackground { get; }
        public IReactiveProperty<string> LineBreakName { get; }
        public IReactiveProperty<Brush> LineBreakBackground { get; }

        private DocumentInfoViewModel ViewModel { get; }
        private IWpfTextView TextView { get; }
        private ITextDocument Document { get; }
        private CompositeDisposable Disposable { get; }

        public DocumentInfo(IWpfTextView textView)
        {
            ViewModel = DocumentInfoViewModel.Instance;
            TextView = textView;
            Document = textView.GetTextDocument();
            Disposable = new CompositeDisposable();

            EncodingName = new ReactiveProperty<string>("").AddTo(Disposable);
            EncodingBackground = new ReactiveProperty<Brush>().AddTo(Disposable);

            LineBreakName = new ReactiveProperty<string>("").AddTo(Disposable);
            LineBreakBackground = new ReactiveProperty<Brush>().AddTo(Disposable);

            TextView.Properties.AddProperty(typeof(DirectoryInfo), this);

            UpdateEncodingInfo();
            UpdateLineBreakInfo();

            TextView.Closed += OnClosed;
            TextView.GotAggregateFocus += OnGotAggregateFocus;
            TextView.LostAggregateFocus += OnLostAggregateFocus;
            Document.FileActionOccurred += OnFileActionOccurred;
            Document.EncodingChanged += OnEncodingChanged;
        }

        public void RepairEncoding()
        {
            if (Document.Encoding.IsUtf8WithBom()) return;

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var result = VsShellUtilities.ShowMessageBox(
                    ServiceProvider.GlobalProvider,
                    $"ファイル: \"{Path.GetFileName(Document.FilePath)}\" の エンコードを \r\n" +
                    $"\"{Document.Encoding.GetEncodingName()}\" から \"UTF-8 with BOM\" へ 変換します。",
                    "エンコーディングを UTF-8 with BOMへ変換しますか？",
                    OLEMSGICON.OLEMSGICON_QUERY,
                    OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND);

                if (result == 6) // Yes
                {
                    Document.Encoding = new UTF8Encoding(true);
                    Document.UpdateDirtyState(true, DateTime.Now);
                }
            });
        }

        public void RepairLineBreak()
        {
            if (TextView.TextViewLines == null ||
                LineBreakName.Value == "CR/LF" ||
                LineBreakName.Value == "")
            {
                return;
            }

            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var result = VsShellUtilities.ShowMessageBox(
                    ServiceProvider.GlobalProvider,
                    $"ファイル: \"{Path.GetFileName(Document.FilePath)}\" の 改行コードを \"CR/LF\" へ 変換します。",
                    "改行コードを \"CR/LF\" へ変換しますか？",
                    OLEMSGICON.OLEMSGICON_QUERY,
                    OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND);

                if (result == 6) // Yes
                {
                }
            });
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.Closed -= OnClosed;
            TextView.GotAggregateFocus -= OnGotAggregateFocus;
            TextView.LostAggregateFocus -= OnLostAggregateFocus;
            Document.FileActionOccurred -= OnFileActionOccurred;
            Document.EncodingChanged -= OnEncodingChanged;

            Disposable.Dispose();
        }

        private void OnFileActionOccurred(object o, TextDocumentFileActionEventArgs e)
        {
            if ((e.FileActionType & FileActionTypes.ContentLoadedFromDisk) != 0 ||
                (e.FileActionType & FileActionTypes.ContentSavedToDisk) != 0)
            {
                UpdateLineBreakInfo();
            }
        }

        private void OnGotAggregateFocus(object sender, EventArgs e)
        {
            ViewModel.SetTo(this);

            if (LineBreakName.Value == "")
            {
                UpdateLineBreakInfo();
            }
        }

        private void OnLostAggregateFocus(object sender, EventArgs e)
        {
            ViewModel.Clear();
        }

        private void OnEncodingChanged(object sender, EncodingChangedEventArgs e)
        {
            UpdateEncodingInfo();
        }

        private void UpdateEncodingInfo()
        {
            EncodingName.Value = Document.Encoding.GetEncodingName();
            EncodingBackground.Value = Document.Encoding.GetBackground();
        }

        private void UpdateLineBreakInfo()
        {
            if (TextView.TextViewLines == null)
            {
                LineBreakName.Value = "";
                LineBreakBackground.Value = null;
                return;
            }

            var lineBreakGroups = TextView.TextViewLines
                .Where(line => line.LineBreakLength > 0)
                .Select(line => line.GetLineBreakKind())
                .GroupBy(lb => lb)
                .ToArray();

            if (lineBreakGroups.Length == 0)
            {
                LineBreakName.Value = "";
                LineBreakBackground.Value = null;
                return;
            }

            var lineBreak = lineBreakGroups.Length == 1
                ? lineBreakGroups[0].Key
                : LineBreakKind.Mixture;

            LineBreakName.Value = lineBreak.GetName();
            switch (lineBreak)
            {
                case LineBreakKind.CrLf:
                    LineBreakBackground.Value = null;
                    break;
                case LineBreakKind.Cr:
                case LineBreakKind.Lf:
                case LineBreakKind.Nel:
                case LineBreakKind.Ls:
                case LineBreakKind.Ps:
                    LineBreakBackground.Value = Brushes.ForestGreen;
                    break;
                case LineBreakKind.Mixture:
                case LineBreakKind.Unknown:
                default:
                    LineBreakBackground.Value = Brushes.DarkOrange;
                    break;
            }
        }
    }
}
