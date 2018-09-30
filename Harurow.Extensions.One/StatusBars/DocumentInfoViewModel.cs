using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars
{
    internal class DocumentInfoViewModel
    {
        private static readonly Lazy<DocumentInfoViewModel> LazyInstance = new Lazy<DocumentInfoViewModel>(()=> new DocumentInfoViewModel());
        public static DocumentInfoViewModel Instance => LazyInstance.Value;

        public IReactiveProperty<Visibility> DocumentInfoVisibility { get; }

        public IReactiveProperty<string> EncodingName { get; }
        public IReactiveProperty<Brush> EncodingBackground { get; }
        public ReactiveCommand EncodingCommand { get; }

        public IReactiveProperty<string> LineBreakName { get; }
        public IReactiveProperty<Brush> LineBreakBackground { get; }
        public ReactiveCommand LineBreakCommand { get; }

        public CompositeDisposable Disposable { get; }
        private CompositeDisposable ModelDisposable { get; set; }

        private DocumentInfoViewModel()
        {
            Disposable = new CompositeDisposable();
            ModelDisposable = new CompositeDisposable();

            DocumentInfoVisibility = new ReactiveProperty<Visibility>(Visibility.Visible);

            EncodingName = new ReactiveProperty<string>("").AddTo(Disposable);
            EncodingBackground = new ReactiveProperty<Brush>().AddTo(Disposable);
            EncodingCommand = new ReactiveCommand().AddTo(Disposable);

            LineBreakName = new ReactiveProperty<string>("").AddTo(Disposable);
            LineBreakBackground = new ReactiveProperty<Brush>().AddTo(Disposable);
            LineBreakCommand = new ReactiveCommand().AddTo(Disposable);
        }

        public void Clear()
        {
            DocumentInfoVisibility.Value = Visibility.Collapsed;

            ModelDisposable.Dispose();
            ModelDisposable = new CompositeDisposable();
        }

        public void SetTo(DocumentInfo docInfo)
        {
            docInfo.EncodingName.Subscribe(x => EncodingName.Value = x).AddTo(ModelDisposable);
            docInfo.EncodingBackground.Subscribe(x => EncodingBackground.Value = x).AddTo(ModelDisposable);
            EncodingCommand.Subscribe(docInfo.RepairEncoding).AddTo(ModelDisposable);

            docInfo.LineBreakName.Subscribe(x => LineBreakName.Value = x).AddTo(ModelDisposable);
            docInfo.LineBreakBackground.Subscribe(x => LineBreakBackground.Value = x).AddTo(ModelDisposable);
            LineBreakCommand.Subscribe(docInfo.RepairLineBreak).AddTo(ModelDisposable);

            DocumentInfoVisibility.Value = Visibility.Visible;
        }
    }
}