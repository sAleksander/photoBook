using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
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

namespace PhotoBook.View.SettingsView
{
    /// <summary>
    /// Interaction logic for FilePickerButton.xaml
    /// </summary>
    public partial class FilePickerButton : UserControl
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(
            nameof(ButtonContent),
            typeof(string), 
            typeof(FilePickerButton),
            new PropertyMetadata("")
        );

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            nameof(Filter),
            typeof(string),
            typeof(FilePickerButton),
            new PropertyMetadata("")
        );

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(
            nameof(InitialDirectory),
            typeof(string),
            typeof(FilePickerButton),
            new PropertyMetadata("")
        );

        public string InitialDirectory
        {
            get { return (string)GetValue(InitialDirectoryProperty); }
            set { SetValue(InitialDirectoryProperty, value); }
        }

        public static readonly DependencyProperty FileChosenCommandProperty = DependencyProperty.Register(
            nameof(FileChosenCommand),
            typeof(RelayCommand),
            typeof(FilePickerButton),
            new PropertyMetadata(null)
        );

        public RelayCommand FileChosenCommand
        {
            get { return (RelayCommand)GetValue(FileChosenCommandProperty); }
            set { SetValue(FileChosenCommandProperty, value); }
        }

        public static readonly DependencyProperty ChosenFileProperty = DependencyProperty.Register(
            nameof(ChosenFile),
            typeof(string),
            typeof(FilePickerButton),
            new PropertyMetadata("")
        );

        public string ChosenFile
        {
            get { return (string)GetValue(ChosenFileProperty); }
            set { SetValue(ChosenFileProperty, value); }
        }

        public FilePickerButton()
        {
            InitializeComponent();
        }

        private void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = Filter;
            fileDialog.InitialDirectory = InitialDirectory;

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                ChosenFile = fileDialog.FileName;
                FileChosenCommand.Execute(null);
            }
        }
    }
}
