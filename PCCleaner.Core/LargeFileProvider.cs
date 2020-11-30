using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using static PCCleaner.Model.Constants;

namespace PCCleaner.Core
{
    public class LargeFileProvider : FolderProvider
    {
        public async Task<ObservableCollection<StorageFileType<StorageFile>>> ScannFoldersAsync(ObservableCollection<StorageFolderType<StorageFolder>> folderCollection)
        {
            ObservableCollection<StorageFileType<StorageFile>> fileCollection = new ObservableCollection<StorageFileType<StorageFile>>();

            var storageFolders = folderCollection.Where(c => c.IsChecked == true).ToList();

            if (storageFolders != null && storageFolders.Any())
            {

                foreach (var storageFolder in storageFolders)
                {
                    List<StorageFile> storageFiles = await base.PullFilesFromFolderAsync(storageFolder).ConfigureAwait(true);

                    foreach (StorageFile storageFile in storageFiles)
                    {
                        BasicProperties properties = await storageFile.GetBasicPropertiesAsync();

                        fileCollection.Add(new StorageFileType<StorageFile>(storageFile, properties));
                    }
                }
            }
            return fileCollection;
        }

        public void CleanFilesAsync(ObservableCollection<StorageFileType<StorageFile>> fileCollection)
        {
            List<StorageFile> largeFileList = fileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            for (int i = largeFileList.Count - 1; i >= 0; i--)
            {
                largeFileList[i].DeleteAsync();
            }

            fileCollection.Clear();
        }
    }
}
