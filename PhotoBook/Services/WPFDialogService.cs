using PhotoBook.ViewModel.Dialogs;
using PhotoBook.View.Dialogs;

namespace PhotoBook.Services
{
    public class WPFDialogService : IDialogService
    {
        public void OpenDialog(DialogViewModelBase viewModel)
        {
            DialogWindow window = new DialogWindow(viewModel);

            window.ShowDialog();
        }
    }
}
