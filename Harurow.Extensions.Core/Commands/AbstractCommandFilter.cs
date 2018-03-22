using System;
using System.Reactive.Disposables;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Harurow.Extensions.Commands
{
    public abstract class AbstractCommandFilter : IOleCommandTarget, IDisposable
    {
        private Guid MenuGroupGuid { get; }

        private IOleCommandTarget NextTarget { get; set; }

        protected CompositeDisposable Disposer { get; }

        protected AbstractCommandFilter(Guid menuGroupGuid, IVsTextView viewAdapter)
        {
            MenuGroupGuid = menuGroupGuid;
            Disposer = new CompositeDisposable(Bind(viewAdapter));
        }

        public void Dispose()
        {
            Disposer.Dispose();
        }

        private void ThrowComExceptionIfFaild(int hresult)
        {
            if (hresult != VSConstants.S_OK)
            {
                throw new COMException("Failed AddCommandFilter", hresult);
            }
        }

        private IDisposable Bind(IVsTextView viewAdapter)
        {
            ThrowComExceptionIfFaild(viewAdapter.AddCommandFilter(this, out var nextTarget));

            NextTarget = nextTarget;

            return Disposable.Create(() =>
            {
                viewAdapter.RemoveCommandFilter(this);
                NextTarget = null;
            });
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pguidCmdGroup != MenuGroupGuid)
            {
                return NextTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            }

            for (var i = 0; i < cCmds; i++)
            {
                var isEnabled = false;
                var cmdId = prgCmds[i].cmdID;

                if (QueryStatus(pguidCmdGroup, cmdId, ref isEnabled))
                {
                    var cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED;
                    if (isEnabled)
                    {
                        cmdf |= (uint)OLECMDF.OLECMDF_ENABLED;
                    }

                    prgCmds[i].cmdf = cmdf;
                    break;
                }
            }

            return VSConstants.S_OK;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdId, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var context = new CommandFilterExecContext(pguidCmdGroup, nCmdId, nCmdexecopt, pvaIn, pvaOut, NextTarget);

            var hr = VSConstants.S_OK;

            if (Exec(context, ref hr))
            {
                return hr;
            }

            return NextTarget.Exec(ref pguidCmdGroup, nCmdId, nCmdexecopt, pvaIn, pvaOut);
        }

        protected abstract bool QueryStatus(Guid groupId, uint cmdId, ref bool isEnabled);

        protected abstract bool Exec(ICommandFilterExecContext context, ref int hresult);
    }
}