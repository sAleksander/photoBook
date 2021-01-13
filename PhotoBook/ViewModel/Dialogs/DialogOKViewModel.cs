using GalaSoft.MvvmLight.Command;

namespace PhotoBook.ViewModel.Dialogs
{
    public class DialogOKViewModel : DialogViewModelBase
    {
        public string Message { get; private set; }

        public DialogOKViewModel(string message = "")
        {
            Message = message;
        }

        private RelayCommand okCommand = null;
        public RelayCommand OKCommand => okCommand ?? (okCommand = new RelayCommand(Close));
    }
}
