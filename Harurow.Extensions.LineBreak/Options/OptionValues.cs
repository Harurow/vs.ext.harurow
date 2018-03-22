using System;
using System.Reactive.Subjects;

namespace Harurow.Extensions.LineBreak.Options
{
    internal sealed class OptionValues : IOptionValues
    {
        private static ISubject<IOptionValues> Subject { get; } = new Subject<IOptionValues>();

        public LineBreakMode VisibleLineBreakMode { get; }
        public LineBreakMode LineBreakWarningMode { get; }

        public OptionValues(LineBreakMode visibleLineBreakMode, LineBreakMode lineBreakWarningMode)
        {
            VisibleLineBreakMode = visibleLineBreakMode;
            LineBreakWarningMode = lineBreakWarningMode;
        }

        public static OptionValues ReadFromStore()
        {
            var store = StoreManagerFactory.Create();

            return new OptionValues(
                (LineBreakMode) store.GetPropertyValue(nameof(IOptionValues.VisibleLineBreakMode),
                    (int) DefaultValues.VisibleLineBreakMode),
                (LineBreakMode) store.GetPropertyValue(nameof(IOptionValues.LineBreakWarningMode),
                    (int) DefaultValues.LineBreakWarningMode));
        }

        public static IDisposable Subscribe(Action<IOptionValues> onNext)
            => Subject.Subscribe(onNext);

        public static void OnNext(IOptionValues optionValues)
            => Subject.OnNext(optionValues);
    }
}