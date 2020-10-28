using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;

namespace PCCleaner.Model
{
    public class LargeFileModel :BaseModel
    {
        #region Fiedls

        ObservableCollection<StorageFileObservableCollection<StorageFile>> largeFileCollection;
        ObservableCollection<StorageFolderObservableCollection<StorageFolder>> largeFolderCollection;

        #endregion

        #region Constructors

        public LargeFileModel() 
        {
            this.largeFileCollection = new ObservableCollection<StorageFileObservableCollection<StorageFile>>();
            this.largeFolderCollection = new ObservableCollection<StorageFolderObservableCollection<StorageFolder>>();
        }

        #endregion

        #region public Propertys

        public ObservableCollection<StorageFileObservableCollection<StorageFile>> LargeFileCollection  
        {
            get => this.largeFileCollection;
            set
            {
                if (this.largeFileCollection == null || !this.largeFileCollection.Equals(value))
                {
                    this.largeFileCollection = value;
                    OnPropertyChanged(nameof(LargeFileCollection));
                }
            }
        }
        public ObservableCollection<StorageFolderObservableCollection<StorageFolder>> LargeFolderCollection
        {
            get => this.largeFolderCollection;
            set
            {
                if (this.largeFolderCollection == null || !this.largeFolderCollection.Equals(value))
                {
                    this.largeFolderCollection = value;
                    OnPropertyChanged(nameof(LargeFolderCollection));
                }
            }
        }
        

        #endregion

        #region puplic Methods

        public async Task OpenLargeFolderAsync()
        {
            if (this.largeFileCollection != null && this.largeFolderCollection.Any())
            {
                LargeFileCollection.Clear();
            }
            if (this.largeFolderCollection != null && this.largeFolderCollection.Any())
            {
                LargeFolderCollection.Clear();
            }

            ObservableCollection<StorageFolderObservableCollection<StorageFolder>> largeFolders = new ObservableCollection<StorageFolderObservableCollection<StorageFolder>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {

                IReadOnlyList<StorageFolder> fileList = await PullFoldersFromFolderAsync(storageFolder).ConfigureAwait(true);
                foreach (StorageFolder folder in fileList)
                {
                    largeFolders.Add(new StorageFolderObservableCollection<StorageFolder>(folder));
                }
            }

            this.LargeFolderCollection = largeFolders;
        }

        public bool IsSelectedLargeFileDeleteFolder()
        {
            bool result = IsSelectedDeleteFolderAsync(LargeFolderCollection);

            return result;
        }

        public void LargeFileDeleteFolder()
        {
            ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedLists = DeleteFolder(this.LargeFolderCollection);

            if (!checkedLists.Equals(this.LargeFolderCollection))
            {
                this.LargeFolderCollection = checkedLists;
            }
        }

        public async Task ScanningLargeFolderAsync()
        {
            List<StorageFile> storageFiles = await PullFilesFromFolderAsync(this.LargeFolderCollection).ConfigureAwait(true);

            ObservableCollection<StorageFileObservableCollection<StorageFile>> storageFileColl = new ObservableCollection<StorageFileObservableCollection<StorageFile>>();

            foreach (StorageFile storageFile in storageFiles)
            {
                BasicProperties properties = await storageFile.GetBasicPropertiesAsync();

                storageFileColl.Add(new StorageFileObservableCollection<StorageFile>(storageFile, properties));
            }

            this.LargeFileCollection = storageFileColl;
        }

        public void CleanLargeFiles()
        {
            List<StorageFile> largeFileList = this.LargeFileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            LargeFileCollection.Clear();
        }

        #endregion

        #region private Methods

        private async Task<List<StorageFolder>> PullFoldersFromFolderAsync(StorageFolder storageFolder)
        {
            List<StorageFolder> storageFoldersList = new List<StorageFolder>();
            List<IReadOnlyList<StorageFolder>> storageNewFoldersList = new List<IReadOnlyList<StorageFolder>>();

            storageFoldersList.Add(storageFolder);

            IReadOnlyList<StorageFolder> folderList = await storageFolder.GetFoldersAsync();

            storageNewFoldersList.Add(folderList);

            for (int i = 0; i < storageNewFoldersList.Count; i++)
            {
                foreach (StorageFolder folder in storageNewFoldersList[i])
                {
                    IReadOnlyList<StorageFolder> storageFolders = await folder.GetFoldersAsync();

                    if (storageFolders.Count > 0)
                    {
                        storageNewFoldersList.Add(storageFolders);
                    }

                    GC.Collect(2);
                }
            }

            foreach (IReadOnlyList<StorageFolder> folders in storageNewFoldersList)
            {
                foreach (StorageFolder folder in folders)
                {
                    storageFoldersList.Add(folder);
                }
                GC.Collect(2);
            }

            return storageFoldersList;
        }

        private async Task<List<StorageFile>> PullFilesFromFolderAsync(ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedListItems)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            foreach (StorageFolderObservableCollection<StorageFolder> folder in this.LargeFolderCollection)
            {
                IReadOnlyList<StorageFile> fileList = await folder.Folder.GetFilesAsync();

                foreach (StorageFile storageFile in fileList)
                {
                    storageFileList.Add(storageFile);
                }

                GC.Collect(2);
            }

            return storageFileList;
        }

        private bool IsSelectedDeleteFolderAsync(ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedListItems)
        {
            bool isFounded = checkedListItems.Any(c => c.IsChecked == true);

            return isFounded;
        }

        private ObservableCollection<StorageFolderObservableCollection<StorageFolder>> DeleteFolder(ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedListItems)
        {
            ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedLists = new ObservableCollection<StorageFolderObservableCollection<StorageFolder>>();

            foreach (StorageFolderObservableCollection<StorageFolder> listItem in checkedListItems)
            {
                if (listItem.IsChecked != true)
                {
                    checkedLists.Add(listItem);
                }

                GC.Collect(2);
            }

            return checkedLists;
        }

        #endregion
    }
}
