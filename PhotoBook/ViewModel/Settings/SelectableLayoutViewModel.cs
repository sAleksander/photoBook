using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Arrangement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel.Settings
{
    public class SelectableLayoutViewModel : ViewModelBase
    {
        private Layout layout;
        private Layout.Type layoutType;

        public event Action<Layout.Type> Selected;

        public Layout Layout
        {
            get => layout;
        }

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set => Set(nameof(IsChecked), ref isChecked, value);
        }

        private string name;
        public string Name
        {
            get => name;
            private set => Set(nameof(Name), ref name, value);
        }

        private RelayCommand check;
        public RelayCommand Check
        {
            get
            {
                return check ?? (check = new RelayCommand(
                    () =>
                    {
                        Selected?.Invoke(layoutType);
                    }));
            }
        }

        public SelectableLayoutViewModel(Layout layout, Layout.Type layoutType)
        {
            this.layout = layout;
            this.layoutType = layoutType;

            IsChecked = false;
            Name = layout.Name;
        }
    }
}
