using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace TestTotalPCClear.Model
{
    public class CheckedListFile<T>: CheckedListItem<T>
    {
        private BasicProperties basicProperties;

        Dictionary<string, string> fileIcon = new Dictionary<string, string>();

        string[] typeFiles =
        {
            ".exe",
            ".doc",
            ".gif",
            ".mp3",
            ".zip",
            ".avi"
        };

        string[] iconFiles =
        {
            "&#xE7C3;",
            "&#xE8A5;",
            "&#xE7C3;",
            "&#xE7C3;",
            "&#xE81E;",
            "&#xE786;",
        };
        

        string date;

        DateTime dateTime;

        public CheckedListFile() { }

        public CheckedListFile(T item, BasicProperties basicProperties, bool isChecked = false) : base(item)
        {
            this.basicProperties = basicProperties;

            StorageFile storageFile = item as StorageFile;

            if (storageFile != null)
            {
                DateTime dateTime = storageFile.DateCreated.DateTime;

                date = dateTime.ToShortDateString();
            }
        }

        public BasicProperties BasicProperties
        {
            get => this.basicProperties;
            set
            {
                if (this.basicProperties == null || !this.basicProperties.Equals(value))
                {
                    this.basicProperties = value;
                    OnPropertyChanged(nameof(BasicProperties));
                }
            }
        }

        public string Date 
        { 
            get=>this.date;
            set
            {
                this.date = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        private void SetFileIcon()
        {

        }

    }
}
