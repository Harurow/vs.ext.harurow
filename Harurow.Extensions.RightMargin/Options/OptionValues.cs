using System;
using System.Reactive.Subjects;

namespace Harurow.Extensions.RightMargin.Options
{
    internal sealed class OptionValues : IOptionValues
    {
        private static ISubject<IOptionValues> Subject { get; } = new Subject<IOptionValues>();

        public int RightMargin { get; }

        public OptionValues(int rightMargin)
            => RightMargin = rightMargin;

        public static OptionValues ReadFromStore()
        {
            var store = StoreManagerFactory.Create();

            return new OptionValues(
                store.GetPropertyValue(nameof(IOptionValues.RightMargin), DefaultValues.RightMargin));
        }

        public static IDisposable Subscribe(Action<IOptionValues> onNext)
            => Subject.Subscribe(onNext);

        public static void OnNext(IOptionValues optionValues)
            => Subject.OnNext(optionValues);
    }
}