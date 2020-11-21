using Windows.Storage;
using Windows.Storage.FileProperties;

namespace PCCleaner.Model.Collections
{
    public class StorageFileType<T> : BaseStorageType where T : IStorageFile
    {
        T fileCO;
        BasicProperties basicProperties;
        private string path;

        public StorageFileType() { }

        public StorageFileType(T file, BasicProperties basicProperties, bool isChecked = false)
        {
            this.fileCO = file;
            base.isChecked = isChecked;
            this.basicProperties = basicProperties;
            this.Path = file.Path;
        }

        public T File
        { 
            get=>this.fileCO;
            set
            {
                this.fileCO = value;
                this.path = this.fileCO.Path;
                OnPropertyChanged(nameof(File));
            } 
        }

        public BasicProperties BasicProperties 
        {
            get => this.basicProperties;
            set
            {
                this.basicProperties = value;
                OnPropertyChanged(nameof(BasicProperties));
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

    }
}