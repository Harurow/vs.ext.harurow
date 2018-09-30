using System;
using System.Collections.Generic;

namespace Harurow.Extensions.One.Analyzer.Commons
{
    public static class EnumerableExtensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            foreach (var item in source)
            {
                onNext(item);
            }
        }

        public static IEnumerable<(TSource, TSource)> Pairs<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            TSource lastItem = default;
            foreach (var item in source)
            {
                yield return (lastItem, item);
                lastItem = item;
            }

            yield return (lastItem, default);
        }

        public static IEnumerable<(TSource, TSource)> Pairs<TSource>(this IEnumerable<TSource> source,
                                                                     Func<TSource, TSource, bool> filter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            TSource lastItem = default;
            foreach (var item in source)
            {
                if (filter(lastItem, item))
                {
                    yield return (lastItem, item);
                }

                lastItem = item;
            }

            if (filter(lastItem, default))
            {
                yield return (lastItem, default);
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> onNext)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (onNext == null) throw new ArgumentNullException(nameof(onNext));

            var i = 0;
            foreach (var item in source)
            {
                onNext(item, i++);
            }
        }
    }
}