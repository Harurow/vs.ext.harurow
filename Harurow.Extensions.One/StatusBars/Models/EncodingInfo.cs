using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars.Models
{
    internal class EncodingInfo : IStatusBarInfoItem
    {
        public IReactiveProperty<string> Text { get; }
        public IReactiveProperty<Brush> Foreground { get; }
        public IReactiveProperty<Brush> Background { get; }
        public IReactiveProperty<Visibility> Visibility { get; }

        private ITextDocument Document { get; }

        public EncodingInfo(IWpfTextView textView, IReactiveProperty<Visibility> visibility,
            CompositeDisposable disposable)
        {
            Document = textView.GetTextDocument();

            Text = new ReactiveProperty<string>("").AddTo(disposable);
            Foreground = new ReactiveProperty<Brush>().AddTo(disposable);
            Background = new ReactiveProperty<Brush>().AddTo(disposable);
            Visibility = visibility;

            UpdateEncodingInfo();

            Observable.FromEventPattern<EncodingChangedEventArgs>(
                    h => Document.EncodingChanged += h,
                    h => Document.EncodingChanged -= h)
                .Subscribe(_ => UpdateEncodingInfo())
                .AddTo(disposable);
        }

        /// <inheritdoc />
        public void Activate()
        {
        }

        /// <inheritdoc />
        public void Inactivate()
        {
        }

        /// <inheritdoc />
        public void Click()
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

        private void UpdateEncodingInfo()
        {
            Text.Value = Document.Encoding.GetEncodingName();
            Foreground.Value = Document.Encoding.GetForeground();
            Background.Value = Document.Encoding.GetBackground();
        }
    }
}