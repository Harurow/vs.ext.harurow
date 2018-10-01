using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using Harurow.Extensions.One.Extensions;
using Harurow.Extensions.One.StatusBars.ViewModels;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.StatusBars.Models
{
    internal class StatusBarInfo
    {
        public IStatusBarInfoItem GoThereInfo { get; }
        public IStatusBarInfoItem EncodingInfo { get; }
        public IStatusBarInfoItem LineBreakInfo { get; }

        private StatusBarInfoViewModel ViewModel { get; }
        private IWpfTextView TextView { get; }
        private ITextDocument Document { get; }
        private CompositeDisposable Disposable { get; }

        public StatusBarInfo(IWpfTextView textView)
        {
            Disposable = new CompositeDisposable();
            ViewModel = StatusBarInfoViewModel.Instance;
            TextView = textView;
            Document = textView.GetTextDocument();

            GoThereInfo = new GoThereInfo(TextView, Disposable);
            EncodingInfo = new EncodingInfo(TextView, ViewModel.StatusBarInfoVisibility, Disposable);
            LineBreakInfo = new LineBreakInfo(TextView, ViewModel.StatusBarInfoVisibility, Disposable);

            TextView.Properties.AddProperty(typeof(StatusBarInfo), this);

            TextView.Closed += OnClosed;
            TextView.GotAggregateFocus += OnGotAggregateFocus;
            TextView.LostAggregateFocus += OnLostAggregateFocus;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            TextView.Closed -= OnClosed;
            TextView.GotAggregateFocus -= OnGotAggregateFocus;
            TextView.LostAggregateFocus -= OnLostAggregateFocus;

            Disposable.Dispose();
        }

        private void OnGotAggregateFocus(object sender, EventArgs e)
        {
            Debug.WriteLine($"* OnGotAggregateFocus : {Path.GetFileName(Document.FilePath)}");
            ViewModel.SetTo(this);
        }

        private void OnLostAggregateFocus(object sender, EventArgs e)
        {
            Debug.WriteLine($"* OnLostAggregateFocus : {Path.GetFileName(Document.FilePath)}");
            ViewModel.Clear();
        }
    }
}
