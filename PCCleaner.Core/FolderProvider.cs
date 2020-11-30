using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using static PCCleaner.Model.Constants;

namespace PCCleaner.Core
{
    public class FolderProvider
    {

        #region public Methods
        public async Task<ObservableCollection<StorageFolderType<StorageFolder>>> OpenFolderAsync()
        {
            ObservableCollection<StorageFolderType<StorageFolder>> folderCollection = new ObservableCollection<StorageFolderType<StorageFolder>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                IReadOnlyList<StorageFolder> fileList = await PullFoldersFromFolderAsync(storageFolder).ConfigureAwait(true);
                foreach (StorageFolder folder in fileList)
                {
                    folderCollection.Add(new StorageFolderType<StorageFolder>(folder) { Icone = FileTypeGlyph.FolderGlyph });
                }
            }

            return folderCollection;
        }

        public List<StorageFolderType<StorageFolder>> IsSelectedDeleteFolder(ObservableCollection<StorageFolderType<StorageFolder>> folderCollection)
        {
            var result = folderCollection.Where(c => c.IsChecked == true).ToList();

            return result;
        }

        public ObservableCollection<StorageFolderType<StorageFolder>> DeleteFolder(List<StorageFolderType<StorageFolder>> folderCollection)
        {
            ObservableCollection<StorageFolderType<StorageFolder>> checkedLists = new ObservableCollection<StorageFolderType<StorageFolder>>();

            foreach (StorageFolderType<StorageFolder> listItem in folderCollection)
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

        #region protected Methods

        protected async Task<List<StorageFolder>> PullFoldersFromFolderAsync(StorageFolder storageFolder)
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
            }

            GC.Collect(2);

            return storageFoldersList;
        }

        protected async Task<List<StorageFile>> PullFilesFromFolderAsync(StorageFolderType<StorageFolder> listItem)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            IReadOnlyList<StorageFile> fileList = await listItem.Folder.GetFilesAsync();

            foreach (StorageFile storageFile in fileList)
            {
                storageFileList.Add(storageFile);
            }

            GC.Collect(2);

            return storageFileList;
        }

        

        

        #endregion

        #region private Methods

        //private bool IsSelectedDeleteFolder(ObservableCollection<StorageFolderType<StorageFolder>> checkedListItems)
        //{
        //    bool isFounded = checkedListItems.Any(c => c.IsChecked == true);

        //    return isFounded;
        //}

        //private ObservableCollection<StorageFolderType<StorageFolder>> DeleteFolder(ObservableCollection<StorageFolderType<StorageFolder>> checkedListItems)
        //{
        //    ObservableCollection<StorageFolderType<StorageFolder>> checkedLists = new ObservableCollection<StorageFolderType<StorageFolder>>();

        //    foreach (StorageFolderType<StorageFolder> listItem in checkedListItems)
        //    {
        //        if (listItem.IsChecked != true)
        //        {
        //            checkedLists.Add(listItem);
        //        }

        //        GC.Collect(2);
        //    }

        //    return checkedLists;
        //}

        #endregion
    }
}
