using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Harurow.Extensions.One.Utilities;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.One.StatusBars
{
    internal class DocumentInfoControl
    {
        public static void AddTo(StatusBarProvider statusBar)
        {
            statusBar.AddControl(CreateEncodingInfo());
            statusBar.AddControl(CreateLineBreakInfo());
        }

        private static FrameworkElement CreateEncodingInfo()
        {
            var item = new StatusBarItem
            {
                Name = "EncodingInfo",
                Margin = new Thickness(3, 0, 3, 0),
                Padding = new Thickness(8, 0, 8, 0),
                Foreground = Application.Current.FindResource(VsBrushes.StatusBarTextKey) as Brush,
                DataContext = DocumentInfoViewModel.Instance,
            };
            item.SetBinding(Control.BackgroundProperty, new Binding("EncodingBackground.Value"));
            item.SetBinding(ContentControl.ContentProperty, new Binding("EncodingName.Value"));
            item.MouseLeftButtonDown += (o, e) =>
            {
                var vm =DocumentInfoViewModel.Instance;
                vm.EncodingCommand.Execute();
            };

            return item;
        }

        private static FrameworkElement CreateLineBreakInfo()
        {
            var item = new StatusBarItem
            {
                Name = "LineBreakInfo",
                Margin = new Thickness(3, 0, 3, 0),
                Padding = new Thickness(8, 0, 8, 0),
                Foreground = Application.Current.FindResource(VsBrushes.StatusBarTextKey) as Brush,
                DataContext = DocumentInfoViewModel.Instance,
            };

            item.SetBinding(ContentControl.ContentProperty, new Binding("LineBreakName.Value"));
            item.SetBinding(Control.BackgroundProperty, new Binding("LineBreakBackground.Value"));

            return item;
        }
    }
}