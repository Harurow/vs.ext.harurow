using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Shell;

namespace Harurow.Extensions.One.Utilities
{
    internal class StatusBarProvider
    {
        private Window MainWindow { get; }

        private FrameworkElement StatusBar { get; set; }
        private Panel Panel { get; set; }

        public StatusBarProvider(Window mainWindow)
        {
            MainWindow = mainWindow;
            FindStatusBar();
        }

        public FrameworkElement AddControl(FrameworkElement control)
            => ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                FindStatusBar();
                if (Panel != null)
                {
                    control.SetValue(DockPanel.DockProperty, Dock.Right);
                    var index = 1;
                    if (FindChild(Panel, "PART_SccStatusBarHost") is FrameworkElement scc)
                    {
                        index = Panel.Children.IndexOf(scc) + 1;
                    }
                    Panel.Children.Insert(index, control);
                }
                return control;
            });

        public bool Contains(FrameworkElement control)
            => ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                FindStatusBar();
                if (Panel != null)
                {
                    return Panel.Children.Contains(control);
                }

                return false;
            });

        public void RemoveControl(FrameworkElement control)
            => ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                FindStatusBar();
                Panel?.Children.Remove(control);
            });

        #region helpers

        private void FindStatusBar()
        {
            if (Panel == null)
            {
                ThreadHelper.JoinableTaskFactory.Run(async delegate {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    StatusBar = FindChild(MainWindow, "StatusBarContainer") as FrameworkElement;

                    if (StatusBar != null)
                    {
                        Panel = StatusBar.Parent as DockPanel;
                    }
                });
            }
        }

        private static DependencyObject FindChild(DependencyObject parent, string childName)
        {
            if (parent == null)
            {
                return null;
            }

            var cnt = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < cnt; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement frameworkElement &&
                    frameworkElement.Name == childName)
                {
                    return frameworkElement;
                }

                child = FindChild(child, childName);

                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        #endregion
    }
}
