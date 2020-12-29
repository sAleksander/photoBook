using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoBook.View
{
    /// <summary>
    /// The purpose of this class is to handle viewing view models using data templates.
    /// Instead of using Content property of Frame, it recreates a ContentControl UI Element
    /// every time ViewModel changes. It solves a subtle bug when new ViewModel object
    /// is of the same type as the previous one (say, PagesViewModel).
    /// </summary>
    public partial class ViewModelViewer : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(ViewModelBase),
            typeof(ViewModelViewer),
            new PropertyMetadata(null, new PropertyChangedCallback(OnViewModelChanged))
        );

        public ViewModelBase ViewModel
        {
            get { return (ViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private ContentControl child;

        public ViewModelViewer()
        {
            InitializeComponent();
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = d as ViewModelViewer;
            var newVM = e.NewValue;

            if (viewer.child != null)
                viewer.MainGrid.Children.Remove(viewer.child);

            viewer.child = new ContentControl() { Content = newVM };
            viewer.MainGrid.Children.Add(viewer.child);
        }
    }
}
