using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars.Models
{
    internal class GoThereInfo : IStatusBarInfoItem
    {
        public IReactiveProperty<string> Text { get; }
        public IReactiveProperty<Brush> Foreground { get; }
        public IReactiveProperty<Brush> Background { get; }
        public IReactiveProperty<Visibility> Visibility { get; }

        private ITextDocument Document { get; }

        public GoThereInfo(IWpfTextView textView, CompositeDisposable disposable)
        {
            Document = textView.GetTextDocument();

            Text = new ReactiveProperty<string>("").AddTo(disposable);
            Foreground = new ReactiveProperty<Brush>().AddTo(disposable);
            Background = new ReactiveProperty<Brush>().AddTo(disposable);
            Visibility = new ReactiveProperty<Visibility>().AddTo(disposable);
        }

        /// <inheritdoc />
        public void Click()
        {
        }
    }
}