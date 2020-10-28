using Windows.Storage;
using Windows.Storage.FileProperties;

namespace PCCleaner.Model.Collections
{
    public class StorageFileObservableCollection<T> : BaseObservableCollection<T> where T : IStorageFile
    {
        T fileCO;
        BasicProperties basicProperties;
        private string path;

        public StorageFileObservableCollection() { }

        public StorageFileObservableCollection(T file, BasicProperties basicProperties, bool isChecked = false)
        {
            this.fileCO = file;
            base.isChecked = isChecked;
            this.basicProperties = basicProperties;
        }

        public T File
        { 
            get=>this.fileCO;
            set
            {
                if(this.fileCO == null || !this.fileCO.Equals(value))
                {
                    this.fileCO = value;
                    this.path = this.fileCO.Path;
                }
            } 
        }

        public BasicProperties BasicProperties 
        {
            get => this.basicProperties;
            set
            {
                if (this.fileCO == null || !this.basicProperties.Equals(value))
                {
                    this.basicProperties = value;
                }
            }
        }

        public string Path
        {
            get => this.path;
            set
            {
                if (!value.Equals(this.path)&&File==null)
                {
                    this.path = value;
                }
            }
        }

    }
}