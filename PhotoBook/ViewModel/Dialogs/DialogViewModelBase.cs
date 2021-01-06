using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel.Dialogs
{
    public class DialogViewModelBase : ViewModelBase
    {
        public event Action RequestClose;

        protected void Close()
        {
            RequestClose?.Invoke();
        }
    }
}
