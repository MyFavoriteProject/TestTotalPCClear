using System.Collections.ObjectModel;
using Windows.Storage;

namespace PCCleaner.Model.Collections
{
    public class StorageFolderTypes<T> : BaseStorageType<T> 
    {
        private T folder;
        private ObservableCollection<StorageFileType<StorageFile>> fileCollection;
        private string path;
        private string folderName;

        public StorageFolderTypes() { }

        public StorageFolderTypes(T folder, bool isChecked = false)
        {
            this.folder = folder;
            base.isChecked = isChecked;
        }

        public string FolderName 
        { 
            get=>this.folderName;
            set
            {
                if (value != this.folderName)
                {
                    folderName = value;
                }
            } 
        }

        public T Folder 
        { 
            get=>this.folder;
            set
            {
                if (this.folder == null || !this.folder.Equals(value))
                {
                    this.folder = value;

                    this.path = this.folder.Path;
                }
            } 
        }

        public ObservableCollection<StorageFileType<StorageFile>> FileCollection
        {
            get => this.fileCollection;
            set
            {
                if (this.fileCollection == null || !this.fileCollection.Equals(value))
                {
                    this.fileCollection = value;
                }
            }
        }

        public string Path 
        { 
            get => this.path;
            set
            {
                if (!value.Equals(this.path) /*&& Folder == null*/)
                {
                    this.path = value;
                }
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
