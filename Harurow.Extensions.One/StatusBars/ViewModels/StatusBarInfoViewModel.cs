using System;
using System.Reactive.Disposables;
using System.Windows;
using Harurow.Extensions.One.StatusBars.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Harurow.Extensions.One.StatusBars.ViewModels
{
    internal class StatusBarInfoViewModel
    {
        private static readonly Lazy<StatusBarInfoViewModel> LazyInstance
            = new Lazy<StatusBarInfoViewModel>(()=> new StatusBarInfoViewModel());
        public static StatusBarInfoViewModel Instance => LazyInstance.Value;

        public IReactiveProperty<Visibility> StatusBarInfoVisibility { get; }
        public StatusBarInfoItemViewModel GoThereInfo { get; }
        public StatusBarInfoItemViewModel EncodingInfo { get; }
        public StatusBarInfoItemViewModel LineBreakInfo { get; }

        private CompositeDisposable Disposable { get; }
        private CompositeDisposable ModelDisposable { get; set; }

        private StatusBarInfoModel ActiveInfo { get; set; }

        private StatusBarInfoViewModel()
        {
            Disposable = new CompositeDisposable();
            ModelDisposable = new CompositeDisposable();

            StatusBarInfoVisibility = new ReactiveProperty<Visibility>(Visibility.Visible).AddTo(Disposable);

            GoThereInfo = new StatusBarInfoItemViewModel(Disposable);
            EncodingInfo = new StatusBarInfoItemViewModel(Disposable);
            LineBreakInfo = new StatusBarInfoItemViewModel(Disposable);
        }

        public void Clear()
        {
            ActiveInfo?.Inactive();
            ActiveInfo = null;

            StatusBarInfoVisibility.Value = Visibility.Collapsed;
            GoThereInfo.Visibility.Value = Visibility.Collapsed;
            EncodingInfo.Visibility.Value = Visibility.Collapsed;
            LineBreakInfo.Visibility.Value = Visibility.Collapsed;

            ModelDisposable.Dispose();
            ModelDisposable = new CompositeDisposable();
        }

        public void SetTo(StatusBarInfoModel info)
        {
            GoThereInfo.SetTo(info.GoThereInfo, ModelDisposable);
            EncodingInfo.SetTo(info.EncodingInfo, ModelDisposable);
            LineBreakInfo.SetTo(info.LineBreakInfo, ModelDisposable);
            StatusBarInfoVisibility.Value = Visibility.Visible;

            ActiveInfo?.Inactive();

            ActiveInfo = info;
            ActiveInfo.Active();
        }
    }
}