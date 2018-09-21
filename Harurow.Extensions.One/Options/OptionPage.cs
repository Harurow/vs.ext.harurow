using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.One.Options
{
    // HACK: オプションを増やす
    internal sealed class OptionPage : DialogPage
    {
        [Category("RightMargin")]
        [DisplayName("推奨桁数")]
        [Description("推奨桁数を指定します。0-1024までの範囲を指定します。0を指定すると無効化されます。")]
        [DefaultValue(OptionValues.Defaults.RightMargin)]
        public int RightMargin { get; set; } = OptionValues.Defaults.RightMargin;

        [Category("RedundantWhiteSpace")]
        [DisplayName("改行前の連続した空白文字")]
        [Description("改行の前の連続した空白文字を強調表示します")]
        [DefaultValue(OptionValues.Defaults.RedundantWhiteSpaceMode)]
        public RedundantWhiteSpaceMode RedundantWhiteSpaceMode { get; set; } =
            OptionValues.Defaults.RedundantWhiteSpaceMode;


        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = new OptionValues();
            opt.LoadSettingsFromStorage();

            RightMargin = opt.RightMargin;
            RedundantWhiteSpaceMode = opt.RedundantWhiteSpaceMode;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var oldOpt = new OptionValues();
            oldOpt.LoadSettingsFromStorage();

            var newOpt = new OptionValues();
            newOpt.RightMargin = Math.Max(0, Math.Min(RightMargin, 1024));
            newOpt.RedundantWhiteSpaceMode = RedundantWhiteSpaceMode;

            newOpt.SaveSettingsToStorage();

            var changedOptions = UpdateNames(oldOpt, newOpt)
                .ToArray();

            if (!changedOptions.IsEmpty())
            {
                var e = new OptionEventArgs(newOpt, new ReadOnlyCollection<string>(changedOptions));
                OptionObserver.OnOptionChanged(e);
            }
        }

        private IEnumerable<string> UpdateNames(OptionValues oldOpt, OptionValues newOpt)
        {
            if (oldOpt.RightMargin != newOpt.RightMargin)
                yield return nameof(RightMargin);
            if (oldOpt.RedundantWhiteSpaceMode != newOpt.RedundantWhiteSpaceMode)
                yield return nameof(RedundantWhiteSpaceMode);
        }
    }
}