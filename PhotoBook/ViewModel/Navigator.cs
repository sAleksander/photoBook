using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel
{
    public class Navigator : INotifyPropertyChanged
    {
        private ViewModelBase currentVM;
        public ViewModelBase CurrentVM
        {
            get
            {
                return currentVM;
            }
            private set
            {
                currentVM = value;
                OnPropertyChange(nameof(CurrentVM));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ChangeCurrentVM<T>() where T : ViewModelBase, new()
        {
            CurrentVM = new T();
        }

        protected void OnPropertyChange(params string[] propertsName)
        {
            if (PropertyChanged != null)
            {
                foreach (var propertyName in propertsName)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
