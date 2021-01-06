using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoBook.ViewModel.Dialogs
{
    public class DialogYesNoViewModel : DialogViewModelBase
    {
        public DialogYesNoViewModel(string message = "")
        {
            Message = message;
        }

        public DialogResult Result { get; private set; } = DialogResult.No;
        public string Message { get; private set; }

        private RelayCommand _yesCommand = null;
        public RelayCommand YesCommand
        {
            get
            {
                if (_yesCommand == null)
                {
                    _yesCommand = new RelayCommand(
                        () => CloseDialogWithResult(
                            DialogResult.Yes
                        )
                   );
                }
                return _yesCommand;
            }
        }

        private RelayCommand _noCommand = null;
        public RelayCommand NoCommand
        {
            get
            {
                if (_noCommand == null)
                {
                    _noCommand = new RelayCommand(
                        () => CloseDialogWithResult(
                            DialogResult.No
                        )
                   );
                }
                return _noCommand;
            }
        }

        private void CloseDialogWithResult(DialogResult result)
        {
            Result = result;
            Close();
        }
    }
}
