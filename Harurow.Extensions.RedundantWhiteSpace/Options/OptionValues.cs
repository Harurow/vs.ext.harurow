using System;
using System.Reactive.Subjects;

namespace Harurow.Extensions.RedundantWhiteSpace.Options
{
    internal sealed class OptionValues : IOptionValues
    {
        private static ISubject<IOptionValues> Subject { get; } = new Subject<IOptionValues>();

        public RedundantWhiteSpaceMode RedundantWhiteSpacesHighlightMode { get; }

        public OptionValues(RedundantWhiteSpaceMode redundantWhiteSpacesHighlightMode)
        {
            RedundantWhiteSpacesHighlightMode = redundantWhiteSpacesHighlightMode;
        }

        public static OptionValues ReadFromStore()
        {
            var store = StoreManagerFactory.Create();

            return new OptionValues(
                (RedundantWhiteSpaceMode) store.GetPropertyValue(
                    nameof(IOptionValues.RedundantWhiteSpacesHighlightMode),
                    (int) DefaultValues.RedundantWhiteSpacesHighlightMode));
        }

        public static IDisposable Subscribe(Action<IOptionValues> onNext)
            => Subject.Subscribe(onNext);

        public static void OnNext(IOptionValues optionValues)
            => Subject.OnNext(optionValues);
    }
}