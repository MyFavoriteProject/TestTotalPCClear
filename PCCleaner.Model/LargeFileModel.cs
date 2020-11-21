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

        ObservableCollection<StorageFileType<StorageFile>> fileCollection;
        ObservableCollection<StorageFolderType<StorageFolder>> folderCollection;

        #endregion

        #region Constructors

        public LargeFileModel() 
        {
            this.fileCollection = new ObservableCollection<StorageFileType<StorageFile>>();
            this.folderCollection = new ObservableCollection<StorageFolderType<StorageFolder>>();
        }

        #endregion

        #region public Propertys

        public ObservableCollection<StorageFileType<StorageFile>> FileCollection  
        {
            get => this.fileCollection;
            set
            {
                if (this.fileCollection == null || !this.fileCollection.Equals(value))
                {
                    this.fileCollection = value;
                    OnPropertyChanged(nameof(FileCollection));
                }
            }
        }
        public ObservableCollection<StorageFolderType<StorageFolder>> FolderCollection
        {
            get => this.folderCollection;
            set
            {
                if (this.folderCollection == null || !this.folderCollection.Equals(value))
                {
                    this.folderCollection = value;
                    OnPropertyChanged(nameof(FolderCollection));
                }
            }
        }
        

        #endregion

        #region puplic Methods

        public async Task OpenLargeFolderAsync()
        {
            if (this.fileCollection != null && this.folderCollection.Any())
            {
                FileCollection.Clear();
            }
            if (this.folderCollection != null && this.folderCollection.Any())
            {
                FolderCollection.Clear();
            }

            ObservableCollection<StorageFolderType<StorageFolder>> largeFolders = new ObservableCollection<StorageFolderType<StorageFolder>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {

                IReadOnlyList<StorageFolder> fileList = await PullFoldersFromFolderAsync(storageFolder).ConfigureAwait(true);
                foreach (StorageFolder folder in fileList)
                {
                    largeFolders.Add(new StorageFolderType<StorageFolder>(folder));
                }
            }

            this.FolderCollection = largeFolders;
        }

        public bool IsSelectedLargeFileDeleteFolder()
        {
            bool result = IsSelectedDeleteFolderAsync(FolderCollection);

            return result;
        }

        public void LargeFileDeleteFolder()
        {
            ObservableCollection<StorageFolderType<StorageFolder>> checkedLists = DeleteFolder(this.FolderCollection);

            if (!checkedLists.Equals(this.FolderCollection))
            {
                this.FolderCollection = checkedLists;
            }
        }

        public async Task ScanningLargeFolderAsync()
        {
            List<StorageFile> storageFiles = await PullFilesFromFolderAsync(this.FolderCollection).ConfigureAwait(true);

            ObservableCollection<StorageFileType<StorageFile>> storageFileColl = new ObservableCollection<StorageFileType<StorageFile>>();

            foreach (StorageFile storageFile in storageFiles)
            {
                BasicProperties properties = await storageFile.GetBasicPropertiesAsync();

                storageFileColl.Add(new StorageFileType<StorageFile>(storageFile, properties));
            }

            this.FileCollection = storageFileColl;
        }

        public void CleanLargeFiles()
        {
            List<StorageFile> largeFileList = this.FileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            FileCollection.Clear();
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

        private async Task<List<StorageFile>> PullFilesFromFolderAsync(ObservableCollection<StorageFolderType<StorageFolder>> checkedListItems)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            foreach (StorageFolderType<StorageFolder> folder in this.FolderCollection)
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

        private bool IsSelectedDeleteFolderAsync(ObservableCollection<StorageFolderType<StorageFolder>> checkedListItems)
        {
            bool isFounded = checkedListItems.Any(c => c.IsChecked == true);

            return isFounded;
        }

        private ObservableCollection<StorageFolderType<StorageFolder>> DeleteFolder(ObservableCollection<StorageFolderType<StorageFolder>> checkedListItems)
        {
            ObservableCollection<StorageFolderType<StorageFolder>> checkedLists = new ObservableCollection<StorageFolderType<StorageFolder>>();

            foreach (StorageFolderType<StorageFolder> listItem in checkedListItems)
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
