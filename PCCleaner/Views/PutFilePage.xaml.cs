﻿using PCCleaner.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PCCleaner.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PutFilePage : Page
    {
        public PutFilePage()
        {
            this.InitializeComponent();
        }

        public ICommand OpenFolderCommand 
        { 
            get=>(ICommand)GetValue(OpenFolderCommandProperty); 
            set => SetValue(OpenFolderCommandProperty, value);
        }
        public static readonly DependencyProperty OpenFolderCommandProperty =
            DependencyProperty.Register("SeeAllCommand", typeof(ICommand), typeof(PutFilePage), new PropertyMetadata(null, OpenFolderCommand_Click));

        private static void OpenFolderCommand_Click(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}