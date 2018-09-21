using System.ComponentModel.Composition;

namespace Harurow.Extensions.One.Options
{
    internal interface IOptionValues
    {
        #region Option (define)

        // HACK: 1. オプションを定義する
        int RightMargin { get; set; }
        RedundantWhiteSpaceMode RedundantWhiteSpaceMode { get; set; }
        LineBreakMode VisibleLineBreakMode { get; set; }
        LineBreakMode LineBreakWarningMode { get; set; }

        #endregion

        void LoadSettingsFromStorage();
        void SaveSettingsToStorage();
    }

    [Export(typeof(IOptionValues))]
    internal sealed class OptionValues : IOptionValues
    {
        internal sealed class Defaults
        {
            #region Defaults

            // HACK: 2. デフォルト値を定義する
            public const int RightMargin = 120;

            public const RedundantWhiteSpaceMode RedundantWhiteSpaceMode =
                Options.RedundantWhiteSpaceMode.UseVisibleWhiteSpace;

            public const LineBreakMode VisibleLineBreakMode = LineBreakMode.UseVisibleWhiteSpace;
            public const LineBreakMode LineBreakWarningMode = LineBreakMode.True;

            #endregion
        }

        #region Option

        // HACK: 3. オプションを宣言する
        public int RightMargin { get; set; }
        public RedundantWhiteSpaceMode RedundantWhiteSpaceMode { get; set; }
        public LineBreakMode VisibleLineBreakMode { get; set; }
        public LineBreakMode LineBreakWarningMode { get; set; }

        #endregion

        public void LoadSettingsFromStorage()
        {
            var store = new StoreManager();

            #region Load

            // HACK: 4. オプションをロードする
            RightMargin = store.GetPropertyValue(
                nameof(RightMargin), Defaults.RightMargin);

            RedundantWhiteSpaceMode = store.GetPropertyValue(
                nameof(RedundantWhiteSpaceMode), Defaults.RedundantWhiteSpaceMode);

            VisibleLineBreakMode = store.GetPropertyValue(
                nameof(VisibleLineBreakMode), Defaults.VisibleLineBreakMode);
            LineBreakWarningMode = store.GetPropertyValue(
                nameof(LineBreakWarningMode), Defaults.LineBreakWarningMode);

            #endregion
        }

        public void SaveSettingsToStorage()
        {
            var store = new StoreManager();

            #region Save

            // HACK: 5. オプションをセーブする
            store.SetPropertyValue(nameof(RightMargin), RightMargin);

            store.SetPropertyValue(nameof(RedundantWhiteSpaceMode), RedundantWhiteSpaceMode);

            store.SetPropertyValue(nameof(VisibleLineBreakMode), VisibleLineBreakMode);
            store.SetPropertyValue(nameof(LineBreakWarningMode), LineBreakWarningMode);

            #endregion
        }
    }
}