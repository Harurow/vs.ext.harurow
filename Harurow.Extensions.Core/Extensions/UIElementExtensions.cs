﻿using System.Windows;
using System.Windows.Controls;

namespace Harurow.Extensions.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class UIElementExtensions
    {
        public static T SetTopLeft<T>(this T self, Point location)
            where T : UIElement
        {
            Canvas.SetTop(self, location.Y);
            Canvas.SetLeft(self, location.X);
            return self;
        }
    }
}