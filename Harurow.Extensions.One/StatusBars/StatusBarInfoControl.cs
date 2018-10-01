using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Harurow.Extensions.One.StatusBars.ViewModels;
using Harurow.Extensions.One.Utilities;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Brush = System.Windows.Media.Brush;

namespace Harurow.Extensions.One.StatusBars
{
    internal static class StatusBarInfoControl
    {
        public static void AddTo(StatusBarProvider statusBar)
        {
            statusBar.AddControl(CreateGoThereInfo());
            statusBar.AddControl(CreateEncodingInfo());
            statusBar.AddControl(CreateLineBreakInfo());
        }

        public static Brush GetUiContextTextBrush()
        {
            var app = Application.Current;
            var key = EnvironmentColors.StatusBarDebuggingTextBrushKey;

            if (KnownUIContexts.DebuggingContext.IsActive) key = EnvironmentColors.StatusBarDebuggingTextBrushKey;
            if (KnownUIContexts.SolutionBuildingContext.IsActive) key = EnvironmentColors.StatusBarBuildingTextBrushKey;

            return (Brush) app.FindResource(key);
        }

        private static FrameworkElement CreateGoThereInfo()
        {
            var item = new StatusBarItem
            {
                Name = "GoThereInfo",
                Margin = new Thickness(3, 0, 3, 0),
                Padding = new Thickness(5, 0, 5, 0),
                DataContext = StatusBarInfoViewModel.Instance,
            };

            item.SetBinding(ContentControl.ContentProperty, new Binding("GoThereInfo.Text.Value"));
            item.SetBinding(Control.ForegroundProperty, new Binding("GoThereInfo.Foreground.Value"));
            item.SetBinding(Control.BackgroundProperty, new Binding("GoThereInfo.Background.Value"));
            item.SetBinding(UIElement.VisibilityProperty, new Binding("GoThereInfo.Visibility.Value"));
            item.MouseDoubleClick += (o, e)
                => StatusBarInfoViewModel.Instance.EncodingInfo.Command.Execute();

            return item;
        }

        private static FrameworkElement CreateEncodingInfo()
        {
            var item = new StatusBarItem
            {
                Name = "EncodingInfo",
                Margin = new Thickness(3, 0, 3, 0),
                Padding = new Thickness(5, 0, 5, 0),
                DataContext = StatusBarInfoViewModel.Instance,
            };

            item.SetBinding(ContentControl.ContentProperty, new Binding("EncodingInfo.Text.Value"));
            item.SetBinding(Control.ForegroundProperty, new Binding("EncodingInfo.Foreground.Value"));
            item.SetBinding(Control.BackgroundProperty, new Binding("EncodingInfo.Background.Value"));
            item.SetBinding(UIElement.VisibilityProperty, new Binding("EncodingInfo.Visibility.Value"));
            item.MouseDoubleClick += (o, e)
                => StatusBarInfoViewModel.Instance.EncodingInfo.Command.Execute();

            return item;
        }

        private static FrameworkElement CreateLineBreakInfo()
        {
            var item = new StatusBarItem
            {
                Name = "LineBreakInfo",
                Margin = new Thickness(3, 0, 3, 0),
                Padding = new Thickness(5, 0, 5, 0),
                DataContext = StatusBarInfoViewModel.Instance,
            };

            item.SetBinding(ContentControl.ContentProperty, new Binding("LineBreakInfo.Text.Value"));
            item.SetBinding(Control.ForegroundProperty, new Binding("LineBreakInfo.Foreground.Value"));
            item.SetBinding(Control.BackgroundProperty, new Binding("LineBreakInfo.Background.Value"));
            item.SetBinding(UIElement.VisibilityProperty, new Binding("LineBreakInfo.Visibility.Value"));
            item.MouseDoubleClick += (o, e) =>
                StatusBarInfoViewModel.Instance.LineBreakInfo.Command.Execute();

            return item;
        }
    }
}