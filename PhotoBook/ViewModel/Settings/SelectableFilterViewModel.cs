using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel.Settings
{
    public class SelectableFilterViewModel : ViewModelBase
    {
        private Model.Graphics.Image image;
        Model.Graphics.Filter.Type filterType;

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
                        image.SetFilter(filterType);
                    }));
            }
        }

        public SelectableFilterViewModel(
            Model.Graphics.Image image,
            Model.Graphics.Filter.Type filterType,
            string name)
        {
            this.image = image;
            this.filterType = filterType;

            Name = name;
        }
    }
}
