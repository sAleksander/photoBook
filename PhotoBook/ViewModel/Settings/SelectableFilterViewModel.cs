﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel.Settings
{
    public class SelectableFilterViewModel : ViewModelBase
    {
        private Model.Pages.ContentPage contentPage;
        private int imageIndex;
        //private Model.Graphics.Image image;
        private Model.Graphics.Filter.Type filterType;

        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set => Set(nameof(IsChecked), ref isChecked, value);
        }

        public string ImageName { get => contentPage.GetImage(imageIndex).DisplayedPath; }

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
                        //Debug.WriteLine(image.DisplayedAbsolutePath);
                        contentPage.GetImage(imageIndex).SetFilter(filterType);
                    }));
            }
        }

        public SelectableFilterViewModel(
            Model.Pages.ContentPage contentPage,
            int imageIndex,
            Model.Graphics.Filter.Type filterType,
            string name)
        {
            this.contentPage = contentPage;
            this.imageIndex = imageIndex;
            this.filterType = filterType;

            //this.image = contentPage.GetImage(imageIndex);

            Name = name;
        }
    }
}
