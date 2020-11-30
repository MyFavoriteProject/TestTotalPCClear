using Microsoft.Toolkit.Uwp.Helpers;
using PCCleaner.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using static PCCleaner.Model.Constants;

namespace PCCleaner.Core
{
    public class DuplicateProvider : FolderProvider
    {
        public async Task<ObservableCollection<DuplicatType>> ScannFoldersAsync(ObservableCollection<StorageFolderType<StorageFolder>> folderCollection)
        {
            ObservableCollection<DuplicatType> fileCollection = new ObservableCollection<DuplicatType>();

            var storageFolders = folderCollection.Where(c => c.IsChecked == true).ToList();

            if (storageFolders != null && storageFolders.Any())
            {
                foreach (var storageFolder in storageFolders)
                {
                    List<StorageFile> fileList = await base.PullFilesFromFolderAsync(storageFolder).ConfigureAwait(true);

                    List<int> indexList = new List<int>();

                    for (int i = 0; i < fileList.Count; i++)
                    {
                        //var a = fileList[i].GetHashCode();

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


                                        if (IndexI == null)
                                        {
                                            fileCollection.Add(new DuplicatType(fileList[i].DisplayName) { Icone = FileTypeGlyph.GetGlyph() });

                                            fileCollection[fileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[i], basicIFile));
                                            fileCollection[fileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[j], basicJFile));

                                            indexList.Add(i);
                                            indexList.Add(j);
                                        }
                                        else if (IndexJ == null)
                                        {
                                            fileCollection[fileCollection.Count - 1].FileCollection.Add(new StorageFileType<StorageFile>(fileList[j], basicJFile));
                                            indexList.Add(j);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return fileCollection;
        }

        public async void CleanFilesAsync(ObservableCollection<DuplicatType> fileCollection)
        {
            var largeFileList = fileCollection.Where(c => c.IsChecked == true).ToList();

            foreach (var file in largeFileList)
            {
                for (int i = file.FileCollection.Count - 1; i >= 0; i--)
                {
                    await file.FileCollection[i].File.DeleteAsync();
                }
            }



            fileCollection.Clear();
        }
    }
}
