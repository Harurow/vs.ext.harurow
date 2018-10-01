using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Harurow.Extensions.One.Utilities;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Brush = System.Windows.Media.Brush;

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
                Foreground = Application.Current.FindResource(EnvironmentColors.StatusBarDefaultTextBrushKey) as Brush,
                DataContext = DocumentInfoViewModel.Instance,
            };

            item.SetBinding(Control.BackgroundProperty, new Binding("EncodingBackground.Value"));
            item.SetBinding(ContentControl.ContentProperty, new Binding("EncodingName.Value"));
            item.MouseLeftButtonDown += (o, e) => DocumentInfoViewModel.Instance.EncodingCommand.Execute();

            return item;
        }

        private static FrameworkElement CreateLineBreakInfo()
        {
            var item = new StatusBarItem
            {
                Name = "LineBreakInfo",
                Margin = new Thickness(3, 0, 3, 0),
                Padding = new Thickness(8, 0, 8, 0),
                Foreground = Application.Current.FindResource(EnvironmentColors.StatusBarDefaultTextBrushKey) as Brush,
                DataContext = DocumentInfoViewModel.Instance,
            };

            item.SetBinding(ContentControl.ContentProperty, new Binding("LineBreakName.Value"));
            item.SetBinding(Control.BackgroundProperty, new Binding("LineBreakBackground.Value"));
            item.MouseLeftButtonDown += (o, e) => DocumentInfoViewModel.Instance.LineBreakCommand.Execute();

            return item;
        }
    }
}