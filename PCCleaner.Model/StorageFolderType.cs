using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;

namespace PCCleaner.Model
{
    public class StorageFolderType<T> : BaseStorageType where T : IStorageFolder
    {
        private T folder;
        private ObservableCollection<StorageFileType<StorageFile>> fileCollection;
        private string path;
        private string folderName;

        public static Action OnItemChecked;

        public StorageFolderType() { }

        public StorageFolderType(string folderName, string icone, List<string> pathes)
        {
            this.FolderName = folderName;
            base.Icone = icone;

            if (pathes.Any())
            {
                FileCollection = new ObservableCollection<StorageFileType<StorageFile>>();

                foreach(var path in pathes)
                {
                    FileCollection.Add(new StorageFileType<StorageFile>(path));
                }
            }
        }

        public StorageFolderType(T folder, bool isChecked = false)
        {
            this.folder = folder;
            base.isChecked = isChecked;

            this.Path = folder.Path;
            this.FolderName = folder.Name;
        }

        public string FolderName 
        { 
            get=>this.folderName;
            set
            {
                folderName = value;
                OnPropertyChanged(nameof(FolderName));
            } 
        }

        public T Folder 
        { 
            get=>this.folder;
            set
            {
                this.folder = value;

                this.path = this.folder.Path;

                OnPropertyChanged(nameof(Folder));
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

        public string Path 
        { 
            get => this.path;
            set
            {
                this.path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public int FolderSize { get; private set; }

        private void SetAllSizeFolder()
        {
            FolderSize = 0;

            foreach (var file in FileCollection)
            {
                FolderSize += (int)file.BasicProperties.Size;
            }
        }
    }
}
