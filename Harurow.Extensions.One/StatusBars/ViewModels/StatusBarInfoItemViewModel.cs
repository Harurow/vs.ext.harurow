using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Media;
using Harurow.Extensions.One.StatusBars.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars.ViewModels
{
    internal class StatusBarInfoItemViewModel
    {
        public IReactiveProperty<string> Text { get; }
        public IReactiveProperty<Brush> Foreground { get; }
        public IReactiveProperty<Brush> Background { get; }
        public IReactiveProperty<Visibility> Visibility { get; }
        public ReactiveCommand Command { get; }

        public StatusBarInfoItemViewModel(CompositeDisposable disposable)
        {
            Text = new ReactiveProperty<string>("").AddTo(disposable);
            Foreground = new ReactiveProperty<Brush>().AddTo(disposable);
            Background = new ReactiveProperty<Brush>().AddTo(disposable);
            Visibility = new ReactiveProperty<Visibility>().AddTo(disposable);
            Command = new ReactiveCommand().AddTo(disposable);
        }

        public void SetTo(IStatusBarInfoItem info, CompositeDisposable disposable)
        {
            info.Text.Subscribe(x => Text.Value = x).AddTo(disposable);
            info.Foreground.Subscribe(x => Foreground.Value = x).AddTo(disposable);
            info.Background.Subscribe(x => Background.Value = x).AddTo(disposable);
            info.Visibility.Subscribe(x => Visibility.Value = x).AddTo(disposable);
            Command.Subscribe(info.Click).AddTo(disposable);
        }
    }
}