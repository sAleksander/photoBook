using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoBook.View.SettingsView
{
    /// <summary>
    /// Interaction logic for FolderPickerButton.xaml
    /// </summary>
    public partial class FolderPickerButton : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(
            nameof(ButtonContent),
            typeof(string), 
            typeof(FolderPickerButton),
            new PropertyMetadata("")
        );

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(
            nameof(ButtonStyle),
            typeof(Style),
            typeof(FolderPickerButton),
            new PropertyMetadata(null)
        );

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty DirChosenCommandProperty = DependencyProperty.Register(
            nameof(DirChosenCommand),
            typeof(RelayCommand),
            typeof(FolderPickerButton),
            new PropertyMetadata(null)
        );

        public RelayCommand DirChosenCommand
        {
            get { return (RelayCommand)GetValue(DirChosenCommandProperty); }
            set { SetValue(DirChosenCommandProperty, value); }
        }

        public static readonly DependencyProperty ChosenDirProperty = DependencyProperty.Register(
            nameof(ChosenDir),
            typeof(string),
            typeof(FolderPickerButton),
            new PropertyMetadata("")
        );

        public string ChosenDir
        {
            get { return (string)GetValue(ChosenDirProperty); }
            set { SetValue(ChosenDirProperty, value); }
        }

        public FolderPickerButton()
        {
            InitializeComponent();
        }

        private void PickFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dirDialog = new FolderBrowserDialog();

            var result = dirDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ChosenDir = dirDialog.SelectedPath;
                DirChosenCommand.Execute(null);
            }
        }
    }
}
