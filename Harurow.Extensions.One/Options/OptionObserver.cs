using System;

namespace Harurow.Extensions.One.Options
{
    internal sealed class OptionObserver
    {
        public static event EventHandler<OptionEventArgs> OptionChanged;

        public static void OnOptionChanged(OptionEventArgs e)
        {
            OptionChanged?.Invoke(null, e);
        }
    }
}