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

        public async Task<bool> OpenFolderAsync()
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
                //List<StorageFolder> storageFolders = await base.folderProvider.PullFoldersFromFolderAsync(storageFolder);

                //foreach (StorageFolder folder in storageFolders)
                //{
                //    this.FolderCollection.Add(new StorageFolderType<StorageFolder>(folder) { Icone = FileTypeGlyph.FolderGlyph });
                //}

            }


            return isOpenFolder;
        }

        public bool IsSelectedDeleteFolder()
        {
            //bool result = base.folderProvider.IsSelectedDeleteFolder(FolderCollection);

            return true;
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

            //ObservableCollection<DuplicatType> duplicatType = new ObservableCollection<DuplicatType>();

            var storageFolders = FolderCollection.Where(c => c.IsChecked == true).ToList();

            if (storageFolders != null && storageFolders.Any())
            {
                result = true;

                foreach(var storageFolder in storageFolders)
                {
                    //List<StorageFile> fileList = await base.folderProvider.PullFilesFromFolderAsync(storageFolder).ConfigureAwait(true);

                    //List<int> indexList = new List<int>();

                    //for (int i = 0; i < fileList.Count; i++)
                    //{
                    //    //var a = fileList[i].GetHashCode();

                    //    for (int j = i + 1; j < fileList.Count; j++)
                    //    {
                    //        if (fileList[i].FileType == fileList[j].FileType && i != j)
                    //        {
                    //            BasicProperties basicIFile = await fileList[i].GetBasicPropertiesAsync();
                    //            BasicProperties basicJFile = await fileList[j].GetBasicPropertiesAsync();

                    //            if (basicIFile.Size == basicJFile.Size)
                    //            {
                    //                var byteI = await StorageFileHelper.ReadBytesAsync(fileList[i]).ConfigureAwait(true);
                    //                var byteJ = await StorageFileHelper.ReadBytesAsync(fileList[j]).ConfigureAwait(true);

                    //                if (byteI.SequenceEqual(byteJ))
                    //                {
                    //                    int? IndexI = indexList.Cast<int?>().FirstOrDefault(c => c == i);
                    //                    int? IndexJ = indexList.Cast<int?>().FirstOrDefault(c => c == j);


                    //                    if (IndexI==null)
                    //                    {
                    //                        FileCollection.Add(new DuplicatType(fileList[i].DisplayName) { Icone = FileTypeGlyph.GetGlyph() });

                    //                        FileCollection[FileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[i], basicIFile));
                    //                        FileCollection[FileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[j], basicJFile));

                    //                        indexList.Add(i);
                    //                        indexList.Add(j);
                    //                    }
                    //                    else if(IndexJ==null)
                    //                    {
                    //                        FileCollection[FileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[j], basicJFile));
                    //                        indexList.Add(j);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            return result;
        }

        public async Task CleanFilesAsync()
        {
            return;

            //List<StorageFile> duplicateFileList = this.DuplicateFileCollection.Where(c => c.IsChecked == true).Select(c => c.File).ToList();

            FileCollection.Clear();
        }

        #endregion

        #region private Methods

        #endregion
    }
}
