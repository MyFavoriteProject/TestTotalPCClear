using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace TestTotalPCClear.Model
{
    public class CheckedListItem<T> : BaseModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isChecked;
        private T item;

        public CheckedListItem()
        { }

        public CheckedListItem(T item, bool isChecked = false)
        {
            this.item = item;
            this.isChecked = isChecked;
        }

        public T Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
    }
}
