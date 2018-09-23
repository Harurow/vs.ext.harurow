using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.One.Options
{
    // HACK: 2. オプションをページに追加する
    internal sealed class OptionPage : DialogPage
    {
        #region option page rows

        // HACK: 2.1. オプションページの行を追加する

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

        [Category("VisibleLineBreak")]
        [DisplayName("改行の表示")]
        [Description("改行を表示します")]
        [DefaultValue(OptionValues.Defaults.VisibleLineBreakMode)]
        public LineBreakMode VisibleLineBreakMode { get; set; } = OptionValues.Defaults.VisibleLineBreakMode;

        [Category("VisibleLineBreak")]
        [DisplayName("異なる改行コードを強調")]
        [Description("改行コードが異なる場合に強調表示します")]
        [DefaultValue(OptionValues.Defaults.LineBreakWarningMode)]
        public LineBreakMode LineBreakWarningMode { get; set; } = OptionValues.Defaults.LineBreakWarningMode;

        [Category("CaretIndicator")]
        [DisplayName("現在行の水平線")]
        [Description("現在の行の下に水平線を引く")]
        [DefaultValue(OptionValues.Defaults.IsEnabledLineIndicator)]
        public bool IsEnabledLineIndicator { get; set; } = OptionValues.Defaults.IsEnabledLineIndicator;

        [Category("CaretIndicator")]
        [DisplayName("現在列の垂直線")]
        [Description("現在の列の左に垂直線を引く")]
        [DefaultValue(OptionValues.Defaults.IsEnabledColumnIndicator)]
        public bool IsEnabledColumnIndicator { get; set; } = OptionValues.Defaults.IsEnabledColumnIndicator;

        [Category("MouseBehavior")]
        [DisplayName("マウス・ホイールのズームを抑制")]
        [Description("有効にするとマウスホイールで拡大・縮小ができないようになります")]
        [DefaultValue(OptionValues.Defaults.IsLockedWheelZoom)]
        public bool IsLockedWheelZoom { get; set; } = OptionValues.Defaults.IsLockedWheelZoom;

        #endregion

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var opt = new OptionValues();
            opt.LoadSettingsFromStorage();

            #region load options

            // HACK: 2.2. ロードしたオプション値をダイアログへ設定する
            RightMargin = opt.RightMargin;
            RedundantWhiteSpaceMode = opt.RedundantWhiteSpaceMode;
            VisibleLineBreakMode = opt.VisibleLineBreakMode;
            LineBreakWarningMode = opt.LineBreakWarningMode;
            IsEnabledLineIndicator = opt.IsEnabledLineIndicator;
            IsEnabledColumnIndicator = opt.IsEnabledColumnIndicator;
            IsLockedWheelZoom = opt.IsLockedWheelZoom;

            #endregion
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var oldOpt = new OptionValues();
            oldOpt.LoadSettingsFromStorage();

            #region save options

            // HACK: 2.3. ダイアログからオプションへ値を設定する
            var newOpt = new OptionValues
            {
                RightMargin = Math.Max(0, Math.Min(RightMargin, 1024)),
                RedundantWhiteSpaceMode = RedundantWhiteSpaceMode,
                VisibleLineBreakMode = VisibleLineBreakMode,
                LineBreakWarningMode = LineBreakWarningMode,
                IsEnabledLineIndicator = IsEnabledLineIndicator,
                IsEnabledColumnIndicator = IsEnabledColumnIndicator,
                IsLockedWheelZoom = IsLockedWheelZoom,
            };

            #endregion

            newOpt.SaveSettingsToStorage();

            var changedOptions = UpdateNames(oldOpt, newOpt)
                .ToArray();

            if (!changedOptions.IsEmpty())
            {
                var e = new OptionEventArgs(newOpt, new ReadOnlyCollection<string>(changedOptions));
                OptionObserver.OnOptionChanged(e);
            }
        }

        private IEnumerable<string> UpdateNames(IOptionValues oldOpt, IOptionValues newOpt)
        {
            #region notify changed options

            // HACK: 2.4. 変更のあったオプションを通知する
            if (oldOpt.RightMargin != newOpt.RightMargin)
                yield return nameof(RightMargin);

            if (oldOpt.RedundantWhiteSpaceMode != newOpt.RedundantWhiteSpaceMode)
                yield return nameof(RedundantWhiteSpaceMode);

            if (oldOpt.VisibleLineBreakMode != newOpt.VisibleLineBreakMode)
                yield return nameof(VisibleLineBreakMode);

            if (oldOpt.LineBreakWarningMode != newOpt.LineBreakWarningMode)
                yield return nameof(LineBreakWarningMode);

            if (oldOpt.IsEnabledLineIndicator != newOpt.IsEnabledLineIndicator)
                yield return nameof(IsEnabledLineIndicator);

            if (oldOpt.IsEnabledColumnIndicator != newOpt.IsEnabledColumnIndicator)
                yield return nameof(IsEnabledColumnIndicator);

            if (oldOpt.IsLockedWheelZoom != newOpt.IsLockedWheelZoom)
                yield return nameof(IsLockedWheelZoom);

            #endregion
        }
    }
}