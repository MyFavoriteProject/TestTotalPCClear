using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCleaner.Model.Collections
{
    public class BaseObservableCollection<T>: ObservableCollection<T>
    {
        protected bool isChecked;

        public bool IsChecked
        {
            get=>this.isChecked;
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                }
            }
        }

        public string Icone { get; set; }
    }
}
