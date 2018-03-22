using System;
using System.Reactive.Subjects;

namespace Harurow.Extensions.CaretIndicator.Options
{
    internal sealed class OptionValues : IOptionValues
    {
        private static ISubject<IOptionValues> Subject { get; } = new Subject<IOptionValues>();

        public bool IsEnabledLineIndicator { get; }
        public bool IsEnabledColumnIndicator { get; }

        public OptionValues(bool isEnabledLineIndicator, bool isEnabledColumnIndicator)
        {
            IsEnabledLineIndicator = isEnabledLineIndicator;
            IsEnabledColumnIndicator = isEnabledColumnIndicator;
        }

        public static OptionValues ReadFromStore()
        {
            var store = StoreManagerFactory.Create();

            return new OptionValues(
                store.GetPropertyValue(nameof(IOptionValues.IsEnabledLineIndicator),
                    DefaultValues.IsEnabledLineIndicator),
                store.GetPropertyValue(nameof(IOptionValues.IsEnabledColumnIndicator),
                    DefaultValues.IsEnabledColumnIndicator));
        }

        public static IDisposable Subscribe(Action<IOptionValues> onNext)
            => Subject.Subscribe(onNext);

        public static void OnNext(IOptionValues optionValues)
            => Subject.OnNext(optionValues);
    }
}