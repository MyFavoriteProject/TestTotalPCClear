using Microsoft.Toolkit.Uwp.Helpers;
using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;

namespace PCCleaner.Model
{
    public class DuplicateModel:BaseModel
    {
        #region Fields

        ObservableCollection<StorageFileObservableCollection<StorageFile>> duplicateFileCollection;
        ObservableCollection<StorageFolderObservableCollection<StorageFolder>> duplicateFolderCollection;

        #endregion

        #region Constructors

        public DuplicateModel() 
        {
            this.duplicateFileCollection = new ObservableCollection<StorageFileObservableCollection<StorageFile>>();
            this.duplicateFolderCollection = new ObservableCollection<StorageFolderObservableCollection<StorageFolder>>();
        }

        #endregion

        #region public Propertys

        public ObservableCollection<StorageFileObservableCollection<StorageFile>> DuplicateFileCollection
        {
            get => this.duplicateFileCollection;
            set
            {
                if (this.duplicateFileCollection == null || !this.duplicateFileCollection.Equals(value))
                {
                    this.duplicateFileCollection = value;
                    OnPropertyChanged(nameof(DuplicateFileCollection));
                }
            }
        }
        public ObservableCollection<StorageFolderObservableCollection<StorageFolder>> DuplicateFolderCollection
        {
            get => this.duplicateFolderCollection;
            set
            {
                if (this.duplicateFolderCollection == null || !this.duplicateFolderCollection.Equals(value))
                {
                    this.duplicateFolderCollection = value;
                    OnPropertyChanged(nameof(DuplicateFolderCollection));
                }
            }
        }

        #endregion

        #region public Methods

        public async Task<bool> OpenDuplicateFolderAsync()
        {
            this.DuplicateFolderCollection.Clear();
            this.DuplicateFileCollection.Clear();

            bool isOpenFolder = false;

            ObservableCollection<StorageFolderObservableCollection<StorageFolder>> folderCollection = new ObservableCollection<StorageFolderObservableCollection<StorageFolder>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                isOpenFolder = true;
                List<StorageFolder> storageFolders = await PullFoldersFromFolderAsync(storageFolder);

                foreach (StorageFolder folder in storageFolders)
                {
                    folderCollection.Add(new StorageFolderObservableCollection<StorageFolder>(folder));
                }

            }

            this.DuplicateFolderCollection = folderCollection;

            return isOpenFolder;
        }

        public bool IsSelectedDuplicateDeleteFolder()
        {
            bool result = IsSelectedDeleteFolderAsync(DuplicateFolderCollection);

            return result;
        }

        public void DuplicateDeleteFolder()
        {
            ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedLists = DeleteFolder(this.DuplicateFolderCollection);

            if (!checkedLists.Equals(this.DuplicateFolderCollection))
            {
                this.DuplicateFolderCollection = checkedLists;
            }
        }
        
        public async Task ScannDuplicateFilesAsync()
        {
            ObservableCollection<StorageFileObservableCollection<StorageFile>> fileCollection = new ObservableCollection<StorageFileObservableCollection<StorageFile>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                List<StorageFile> fileList = await PullFilesFromFolderAsync(DuplicateFolderCollection).ConfigureAwait(true);

                for (int i = 0; i < fileList.Count; i++)
                {
                    var a = fileList[i].GetHashCode();

                    for (int j = i + 1; j < fileList.Count; j++)
                    {
                        if (fileList[i].FileType == fileList[j].FileType && i != j)
                        {
                            BasicProperties basicIFile = await fileList[i].GetBasicPropertiesAsync();
                            BasicProperties basicJFile = await fileList[j].GetBasicPropertiesAsync();

                            if (basicIFile.Size == basicJFile.Size)
                            {
                                var byteI = await StorageFileHelper.ReadBytesAsync(fileList[i]).ConfigureAwait(true);
                                var byteJ = await StorageFileHelper.ReadBytesAsync(fileList[j]).ConfigureAwait(true);

                                if (byteI.SequenceEqual(byteJ))
                                {
                                    fileCollection.Add(new StorageFileObservableCollection<StorageFile>(fileList[j], basicJFile));
                                }
                            }
                        }
                    }
                }

                this.DuplicateFileCollection = fileCollection;
            }
        }

        public async Task CleanDuplicateFilesAsync()
        {
            List<StorageFile> duplicateFileList = this.DuplicateFileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            DuplicateFileCollection.Clear();
        }

        #endregion

        #region private Methods

        private async Task<List<StorageFolder>> PullFoldersFromFolderAsync(StorageFolder storageFolder)
        {
            bool isHaveFolder = true;

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
                }
            }

            foreach (IReadOnlyList<StorageFolder> folders in storageNewFoldersList)
            {
                foreach (StorageFolder folder in folders)
                {
                    storageFoldersList.Add(folder);
                }
            }

            return storageFoldersList;
        }

        private async Task<List<StorageFile>> PullFilesFromFolderAsync(ObservableCollection<StorageFolderObservableCollection<StorageFolder>> checkedListItems)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            foreach (StorageFolderObservableCollection<StorageFolder> folder in this.DuplicateFolderCollection)
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
