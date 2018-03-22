using System;
using System.Reactive.Subjects;

namespace Harurow.Extensions.EncodingInfo.Options
{
    internal sealed class OptionValues : IOptionValues
    {
        private static ISubject<IOptionValues> Subject { get; } = new Subject<IOptionValues>();

        public bool IsEnabledRecommendUtf8Bom { get; }
        public bool IsEnabledWarningOtherEncoding { get; }
        public bool IsEnabledAutoHide { get; }

        public OptionValues(bool isEnabledRecommendUtf8Bom, bool isEnabledWarningOtherEncoding, bool isEnabledAutoHide)
        {
            IsEnabledRecommendUtf8Bom = isEnabledRecommendUtf8Bom;
            IsEnabledWarningOtherEncoding = isEnabledWarningOtherEncoding;
            IsEnabledAutoHide = isEnabledAutoHide;
        }

        public static OptionValues ReadFromStore()
        {
            var store = StoreManagerFactory.Create();

            return new OptionValues(
                store.GetPropertyValue(nameof(IOptionValues.IsEnabledRecommendUtf8Bom),
                    DefaultValues.IsEnabledRecommendUtf8Bom),
                store.GetPropertyValue(nameof(IOptionValues.IsEnabledWarningOtherEncoding),
                    DefaultValues.IsEnabledWarningOtherEncoding),
                store.GetPropertyValue(nameof(IOptionValues.IsEnabledAutoHide),
                    DefaultValues.IsEnabledAutoHide));
        }

        public static IDisposable Subscribe(Action<IOptionValues> onNext)
            => Subject.Subscribe(onNext);

        public static void OnNext(IOptionValues optionValues)
            => Subject.OnNext(optionValues);
    }
}