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

        public async Task<bool> OpenFolderAsync()
        {
            bool result = false;

            if (this.fileCollection != null && this.folderCollection.Any())
            {
                FileCollection.Clear();
            }
            if (this.folderCollection != null && this.folderCollection.Any())
            {
                FolderCollection.Clear();
            }

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                result = true;

                //IReadOnlyList<StorageFolder> fileList = await base.folderProvider.PullFoldersFromFolderAsync(storageFolder).ConfigureAwait(true);
                //foreach (StorageFolder folder in fileList)
                //{
                //    FolderCollection.Add(new StorageFolderType<StorageFolder>(folder) { Icone = FileTypeGlyph.FolderGlyph });
                //}
            }

            return result;
        }

        public bool IsSelectedDeleteFolder()
        {
            //bool result = base.folderProvider.IsSelectedDeleteFolder(FolderCollection);

            //return result;

            return false;
        }

        public void DeleteFolder()
        {
            //ObservableCollection<StorageFolderType<StorageFolder>> checkedLists = base.folderProvider.DeleteFolder(this.FolderCollection);

            //if (!checkedLists.Equals(this.FolderCollection))
            //{
            //    this.FolderCollection = checkedLists;
            //}
        }

        public async Task<bool> ScannFoldersAsync()
        {
            bool result = false;

            var storageFolders = FolderCollection.Where(c => c.IsChecked == true).ToList();

            if(storageFolders!=null && storageFolders.Any())
            {
                result = true;

                foreach(var storageFolder in storageFolders)
                {
                    //List<StorageFile> storageFiles = await base.folderProvider.PullFilesFromFolderAsync(storageFolder).ConfigureAwait(true);

                    //foreach (StorageFile storageFile in storageFiles)
                    //{
                    //    BasicProperties properties = await storageFile.GetBasicPropertiesAsync();

                    //    this.FileCollection.Add(new StorageFileType<StorageFile>(storageFile, properties));
                    //}
                }
            }

            

            return result;
        }

        public void CleanFilesAsync()
        {
            List<StorageFile> largeFileList = this.FileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            for(int i = largeFileList.Count-1; i >= 0; i--)
            {
                largeFileList[i].DeleteAsync();
            }

            FileCollection.Clear();
        }

        #endregion

        #region private Methods

        #endregion
    }
}
