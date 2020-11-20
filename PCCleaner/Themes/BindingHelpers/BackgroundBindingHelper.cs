﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PCCleaner.Themes.BindingHelpers
{
    public static class BackgroundBindingHelper
    {
        public static string GetBackground(DependencyObject obj)
            => (string)obj.GetValue(BackgroundProperty);

        public static void SetBackground(DependencyObject obj, string value)
            => obj.SetValue(BackgroundProperty, value);

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background", typeof(string), typeof(BackgroundBindingHelper),
                new PropertyMetadata(null, BackgroundPathPropertyChanged));

        private static void BackgroundPathPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var propertyPath = e.NewValue as string;
            if (propertyPath != null)
            {
                var backgroundproperty = Control.BackgroundProperty;

                BindingOperations.SetBinding(obj, backgroundproperty, new Binding
                {
                    Path = new PropertyPath(propertyPath),
                    //Source = App.ThemeManager
                });
            }
        }
    }
}
