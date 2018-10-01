using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars
{
    internal class StatusBarInfoViewModel
    {
        private static readonly Lazy<StatusBarInfoViewModel> LazyInstance
            = new Lazy<StatusBarInfoViewModel>(()=> new StatusBarInfoViewModel());
        public static StatusBarInfoViewModel Instance => LazyInstance.Value;

        public IReactiveProperty<Visibility> DocumentInfoVisibility { get; }

        public IReactiveProperty<string> EncodingName { get; }
        public IReactiveProperty<Brush> EncodingBackground { get; }
        public ReactiveCommand EncodingCommand { get; }

        public IReactiveProperty<string> LineBreakName { get; }
        public IReactiveProperty<Brush> LineBreakBackground { get; }
        public ReactiveCommand LineBreakCommand { get; }

        public CompositeDisposable Disposable { get; }
        private CompositeDisposable ModelDisposable { get; set; }

        private StatusBarInfoViewModel()
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

        public void SetTo(StatusBarInfo info)
        {
            info.EncodingInfo.Text.Subscribe(x => EncodingName.Value = x).AddTo(ModelDisposable);
            info.EncodingInfo.Background.Subscribe(x => EncodingBackground.Value = x).AddTo(ModelDisposable);
            EncodingCommand.Subscribe(info.EncodingInfo.Repair).AddTo(ModelDisposable);

            info.LineBreakInfo.Text.Subscribe(x => LineBreakName.Value = x).AddTo(ModelDisposable);
            info.LineBreakInfo.Background.Subscribe(x => LineBreakBackground.Value = x).AddTo(ModelDisposable);
            LineBreakCommand.Subscribe(info.LineBreakInfo.Repair).AddTo(ModelDisposable);

            DocumentInfoVisibility.Value = Visibility.Visible;
        }
    }
}