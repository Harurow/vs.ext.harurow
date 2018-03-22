using System;
using System.Reactive.Subjects;

namespace Harurow.Extensions.Commands.Options
{
    internal sealed class OptionValues : IOptionValues
    {
        private static ISubject<IOptionValues> Subject { get; } = new Subject<IOptionValues>();

        public bool IsLockedMouseWheelZoom { get; }

        public OptionValues(bool isLockedMouseWheelZoom)
        {
            IsLockedMouseWheelZoom = isLockedMouseWheelZoom;
        }

        public static OptionValues ReadFromStore()
        {
            var store = StoreManagerFactory.Create();

            return new OptionValues(
                store.GetPropertyValue(nameof(IOptionValues.IsLockedMouseWheelZoom),
                    DefaultValues.IsLockedMouseWheelZoom));
        }

        public static IDisposable Subscribe(Action<IOptionValues> onNext)
            => Subject.Subscribe(onNext);

        public static void OnNext(IOptionValues optionValues)
            => Subject.OnNext(optionValues);
    }
}