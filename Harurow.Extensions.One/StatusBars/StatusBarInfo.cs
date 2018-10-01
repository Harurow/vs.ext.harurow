using System;
using System.IO;
using System.Reactive.Disposables;
using Harurow.Extensions.One.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.StatusBars
{
    internal class StatusBarInfo
    {
        public EncodingInfo EncodingInfo { get; }
        public LineBreakInfo LineBreakInfo { get; }

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

            EncodingInfo = new EncodingInfo(TextView, Disposable);
            LineBreakInfo = new LineBreakInfo(TextView, Disposable);

            TextView.Properties.AddProperty(typeof(DirectoryInfo), this);

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
            ViewModel.SetTo(this);
        }

        private void OnLostAggregateFocus(object sender, EventArgs e)
        {
            ViewModel.Clear();
        }
    }
}
