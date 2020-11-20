using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PCCleaner.Model.Collections
{
    public class BaseStorageType: INotifyPropertyChanged 
    {
        protected bool isChecked;
        protected string icone;

        public bool IsChecked
        {
            get=>this.isChecked;
            set
            {
                this.isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public string Icone 
        { 
            get=>this.icone;
            set
            {
                this.icone = value;
                OnPropertyChanged(nameof(Icone));
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
