using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PCCleaner.Model
{
    public class DuplicatType: BaseStorageType
    {
        private ObservableCollection<StorageFileType<StorageFile>> fileCollection;
        private string fileName;
        private int size = 0;

        public DuplicatType(string name, bool isChecked = false)
        {
            fileName = name;

            FileCollection = new ObservableCollection<StorageFileType<StorageFile>>();
            base.isChecked = isChecked;

            FileCollection.CollectionChanged += FileCollection_CollectionChanged;
        }

        private void FileCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                // Следующая строка сработает если в коллекцию был добавлен элемент.
                case NotifyCollectionChangedAction.Add:
                    StorageFileType<StorageFile> newDrink = e.NewItems[e.NewItems.Count-1] as StorageFileType<StorageFile>;
                    newDrink.Icone = this.Icone;
                    Size += (int)newDrink.BasicProperties.Size;
                    break;
                // Следующая строка если элемент был удален из коллекции.
                case NotifyCollectionChangedAction.Remove:
                    StorageFileType<StorageFile> oldDrink = e.OldItems[e.OldItems.Count - 1] as StorageFileType<StorageFile>;
                    Size -= (int)oldDrink.BasicProperties.Size;
                    break;
                // Следующая строка сработает если элемент был перемещен.
                case NotifyCollectionChangedAction.Replace:
                    StorageFileType<StorageFile> replacedDrink = e.OldItems[e.OldItems.Count - 1] as StorageFileType<StorageFile>;
                    Size -= (int)replacedDrink.BasicProperties.Size;
                    StorageFileType<StorageFile> replacingDrink = e.NewItems[e.NewItems.Count - 1] as StorageFileType<StorageFile>;
                    replacingDrink.Icone = this.Icone;
                    Size += (int)replacingDrink.BasicProperties.Size;
                    break;
            }
        }

        public string FileName 
        { 
            get=>this.fileName;
            set
            {
                this.fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public ObservableCollection<StorageFileType<StorageFile>> FileCollection
        {
            get => this.fileCollection;
            set
            {
                this.fileCollection = value;
                OnPropertyChanged(nameof(FileCollection));
            }
        }

        public int Size 
        { 
            get => this.size;
            set
            {
                this.size = value;
                OnPropertyChanged(nameof(Size));
            } 
        }
    }
}
