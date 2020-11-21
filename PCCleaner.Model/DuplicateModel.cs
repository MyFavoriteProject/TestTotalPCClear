using Microsoft.Toolkit.Uwp.Helpers;
using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using static PCCleaner.Model.Constants;

namespace PCCleaner.Model
{
    public class DuplicateModel:BaseModel
    {
        #region Fields

        ObservableCollection<DuplicatType> fileCollection;
        ObservableCollection<StorageFolderType<StorageFolder>> folderCollection;

        Random random;

        #endregion

        #region Constructors

        public DuplicateModel() 
        {
            this.fileCollection = new ObservableCollection<DuplicatType>();
            this.folderCollection = new ObservableCollection<StorageFolderType<StorageFolder>>();

            random = new Random();
        }

        #endregion

        #region public Propertys

        public ObservableCollection<DuplicatType> FileCollection
        {
            get => this.fileCollection;
            set
            {
                this.fileCollection = value;
                OnPropertyChanged(nameof(FileCollection));
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

        #region public Methods

        public async Task<bool> OpenDuplicateFolderAsync()
        {
            this.FolderCollection.Clear();
            this.FileCollection.Clear();

            bool isOpenFolder = false;

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
                    this.FolderCollection.Add(new StorageFolderType<StorageFolder>(folder) { Path = folder.Path, Icone = FileTypeGlyph.FolderGlyph });
                }

            }


            return isOpenFolder;
        }

        public bool IsSelectedDuplicateDeleteFolder()
        {
            bool result = IsSelectedDeleteFolderAsync(FolderCollection);

            return result;
        }

        public void DuplicateDeleteFolder()
        {
            ObservableCollection<StorageFolderType<StorageFolder>> checkedLists = DeleteFolder(this.FolderCollection);

            if (!checkedLists.Equals(this.FolderCollection))
            {
                this.FolderCollection = checkedLists;
            }
        }
        
        public async Task<bool> ScannDuplicateFilesAsync()
        {
            bool result = false;

            //ObservableCollection<DuplicatType> duplicatType = new ObservableCollection<DuplicatType>();

            var storageFolders = FolderCollection.Where(c => c.IsChecked == true).ToList();

            if (storageFolders != null && FolderCollection.Any())
            {
                result = true;

                foreach(var storageFolder in storageFolders)
                {
                    List<StorageFile> fileList = await PullFilesFromFolderAsync(storageFolder).ConfigureAwait(true);

                    List<int> indexList = new List<int>();

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
                                        int? IndexI = indexList.Cast<int?>().FirstOrDefault(c => c == i);
                                        int? IndexJ = indexList.Cast<int?>().FirstOrDefault(c => c == j);


                                        if (IndexI==null)
                                        {
                                            FileCollection.Add(new DuplicatType(fileList[i].DisplayName) { Icone = FileTypeGlyph.GetGlyph() });

                                            FileCollection[FileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[i], basicIFile));
                                            FileCollection[FileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[j], basicJFile));

                                            indexList.Add(i);
                                            indexList.Add(j);
                                        }
                                        else if(IndexJ==null)
                                        {
                                            FileCollection[FileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[j], basicJFile));
                                            indexList.Add(j);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task CleanDuplicateFilesAsync()
        {
            return;

            //List<StorageFile> duplicateFileList = this.DuplicateFileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            FileCollection.Clear();
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

            return storageFoldersList;
        }

        private async Task<List<StorageFile>> PullFilesFromFolderAsync(StorageFolderType<StorageFolder> listItem)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            IReadOnlyList<StorageFile> fileList = await listItem.Folder.GetFilesAsync();

            foreach (StorageFile storageFile in fileList)
            {
                storageFileList.Add(storageFile);
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
