using System.ComponentModel.Composition;

namespace Harurow.Extensions.One.Options
{
    // HACK: 1. オプションを追加する
    internal interface IOptionValues
    {
        #region Option (define)

        // HACK: 1.1. オプションを定義する
        int RightMargin { get; set; }
        LineBreakMode VisibleLineBreakMode { get; set; }
        bool IsEnabledLineIndicator { get; set; }
        bool IsEnabledColumnIndicator { get; set; }
        bool IsLockedWheelZoom { get; set; }
        bool IsEnableGoThere { get; set; }

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

            // HACK: 1.2. デフォルト値を定義する
            public const int RightMargin = 120;
            public const LineBreakMode VisibleLineBreakMode = LineBreakMode.UseVisibleWhiteSpace;
            public const bool IsEnabledLineIndicator = true;
            public const bool IsEnabledColumnIndicator = false;
            public const bool IsLockedWheelZoom = true;
            public const bool IsEnableGoThere = false;

            #endregion
        }

        #region Option

        // HACK: 1.3. オプションを宣言する
        public int RightMargin { get; set; }
        public LineBreakMode VisibleLineBreakMode { get; set; }
        public bool IsEnabledLineIndicator { get; set; }
        public bool IsEnabledColumnIndicator { get; set; }
        public bool IsLockedWheelZoom { get; set; }
        public bool IsEnableGoThere { get; set; }

        #endregion

        public void LoadSettingsFromStorage()
        {
            var store = new StoreManager();

            #region Load

            // HACK: 1.4. オプションをロードする
            RightMargin = store.GetPropertyValue(nameof(RightMargin), Defaults.RightMargin);
            VisibleLineBreakMode = store.GetPropertyValue(nameof(VisibleLineBreakMode), Defaults.VisibleLineBreakMode);
            IsEnabledLineIndicator = store.GetPropertyValue(nameof(IsEnabledLineIndicator),
                Defaults.IsEnabledLineIndicator);
            IsEnabledColumnIndicator = store.GetPropertyValue(nameof(IsEnabledColumnIndicator),
                Defaults.IsEnabledColumnIndicator);
            IsLockedWheelZoom = store.GetPropertyValue(nameof(IsLockedWheelZoom), Defaults.IsLockedWheelZoom);
            IsEnableGoThere = store.GetPropertyValue(nameof(IsEnableGoThere), Defaults.IsEnableGoThere);

            #endregion
        }

        public void SaveSettingsToStorage()
        {
            var store = new StoreManager();

            #region Save

            // HACK: 1.5. オプションをセーブする
            store.SetPropertyValue(nameof(RightMargin), RightMargin);
            store.SetPropertyValue(nameof(VisibleLineBreakMode), VisibleLineBreakMode);
            store.SetPropertyValue(nameof(IsEnabledLineIndicator), IsEnabledLineIndicator);
            store.SetPropertyValue(nameof(IsEnabledColumnIndicator), IsEnabledColumnIndicator);
            store.SetPropertyValue(nameof(IsLockedWheelZoom), IsLockedWheelZoom);
            store.SetPropertyValue(nameof(IsEnableGoThere), IsEnableGoThere);

            #endregion
        }
    }
}