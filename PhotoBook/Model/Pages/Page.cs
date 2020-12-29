using PhotoBook.Model.Backgrounds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    public abstract class Page : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Background background;
        public Background Background
        {
            get => background;
            set
            {
                background = value;
                InvokePropertyChanged(nameof(Background));
            }
        }

        public virtual void setBackground(int R, int G, int B, string path, int X, int Y, int Width, int Height) { }

        protected void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
