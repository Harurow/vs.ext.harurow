using System.Windows;
using System.Windows.Media;
using Reactive.Bindings;

namespace Harurow.Extensions.One.StatusBars
{
    internal interface IStatusBarInfoItem
    {
        IReactiveProperty<string> Text { get; }
        IReactiveProperty<Brush> Foreground { get; }
        IReactiveProperty<Brush> Background { get; }
        IReactiveProperty<Visibility> Visibility { get; }

        void Click();
    }
}