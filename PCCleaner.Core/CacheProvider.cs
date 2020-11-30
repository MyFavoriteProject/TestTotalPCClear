using PCCleaner.Model;
using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace PCCleaner.Core
{
    public class CacheProvider
    {
        #region Fields

        //ObservableCollection<StorageFolderType<StorageFolder>> folderCollection;

        private const string driveLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string[] cacheName = new string[]
        {
            "System Cache",
            "Application Cache",
            "Mail Cache",
            "Office Cache",
            "Browser Cache"
        };

        #region Universal Pathes

        private List<string> systemCacheUniversalPaths = new List<string>
        {
            @"Users\home\AppData\Local\dbgengsqmdata00.sqm",
            @"Users\home\AppData\Local\dbgengsqmdata01.sqm",
            @"Users\home\AppData\Local\dbgengsqmdata02.sqm",
            @"Users\home\AppData\Local\dbgengsqmdata03.sqm",
            @"Users\home\AppData\Local\dbgengsqmdata04.sqm",
            @"Users\home\AppData\Local\dbgengsqmdata05.sqm",
            @"Users\home\AppData\Local\dbgengsqmdata06.sqm"
        };

        private List<string> applicationCacheUniversalPath = new List<string>
        {
            @"Windows\Logs\MoSetup\BlueBox.log"
        };

        private List<string> mailCacheUniversalPath = new List<string>
        {
            @"Windows\Logs\DISM\dism.log"
        };

        private List<string> officeCacheUniversalPath = new List<string>
        {
            @"Windows\Logs\CBS\FilterList.log"
        };

        private List<string> browserCacheUniversalPath = new List<string>
        {
            @"Windows\Logs\CBS\FilterList.log"
        };

        #endregion

        private List<(string key, string icon, List<string> pathes)> cacheView;

        #endregion

        public CacheProvider()
        {

        }

        #region puplic Methods

        public ObservableCollection<StorageFolderType<StorageFolder>> GetStorageFolderType()
        {
            var folderCollection = new ObservableCollection<StorageFolderType<StorageFolder>>();

            folderCollection.Add(new StorageFolderType<StorageFolder> ( cacheName[0], Constants.FileTypeGlyph.SystemCacheGlyph, systemCacheUniversalPaths));
            folderCollection.Add(new StorageFolderType<StorageFolder> ( cacheName[1], Constants.FileTypeGlyph.ApplicationCacheGlyph, applicationCacheUniversalPath));
            folderCollection.Add(new StorageFolderType<StorageFolder> ( cacheName[2], Constants.FileTypeGlyph.MailCacheGlyph, mailCacheUniversalPath));
            folderCollection.Add(new StorageFolderType<StorageFolder> ( cacheName[3], Constants.FileTypeGlyph.OfficeCacheGlyph, officeCacheUniversalPath));
            folderCollection.Add(new StorageFolderType<StorageFolder> ( cacheName[4], Constants.FileTypeGlyph.BrowserCacheGlyph, browserCacheUniversalPath));

            return folderCollection;
        }

        public async Task<ObservableCollection<StorageFolderType<StorageFolder>>> ScanningSystemCacheAsync(ObservableCollection<StorageFolderType<StorageFolder>> folderCollection)
        {
            try
            {
                await SearchPathesAsync(folderCollection).ConfigureAwait(true);
            }
            catch (UnauthorizedAccessException UAE)
            {
                throw UAE;
            }

            foreach (var folder in folderCollection)
            {
                foreach (var file in folder.FileCollection)
                {
                    try
                    {
                        StorageFile storageFile = await StorageFile.GetFileFromPathAsync(file.Path);
                        BasicProperties basicProperties = await storageFile.GetBasicPropertiesAsync();

                        file.File = storageFile;
                        file.BasicProperties = basicProperties;
                    }
                    catch (Exception) { }
                }
            }

            return folderCollection;
        }

        public async Task DeleteFileAsync(ObservableCollection<StorageFolderType<StorageFolder>> folderCollection)
        {
            foreach (var folder in folderCollection)
            {
                if (folder.IsChecked)
                {
                    foreach (var file in folder.FileCollection)
                    {
                        try
                        {
                            file.File.DeleteAsync();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }

        }

        #endregion

        #region private Methods

        private async Task SearchPathesAsync(ObservableCollection<StorageFolderType<StorageFolder>> folderCollection)
        {
            List<string> pathes = new List<string>();

            int driveLettersLen = driveLetters.Length;
            string removableDriveLetters = "";
            string driveLetter;

            List<StorageFolder> drives = new List<StorageFolder>();

            StorageFolder removableDevices = KnownFolders.RemovableDevices;

            IReadOnlyList<StorageFolder> folders = Task.Run(async () => await removableDevices.GetFoldersAsync()).Result;

            foreach (StorageFolder removableDevice in folders)
            {
                driveLetter = removableDevice.Path.Substring(0, 1).ToUpper();

                if (driveLetters.IndexOf(driveLetter) > -1)
                {
                    removableDriveLetters += driveLetter;
                }
            }

            for (int curDrive = 0; curDrive < driveLettersLen; curDrive++)
            {
                driveLetter = driveLetters.Substring(curDrive, 1);

                try
                {
                    StorageFolder drive = Task.Run(async () => await StorageFolder.GetFolderFromPathAsync(driveLetter + ":")).Result;
                    drives.Add(drive);
                }
                catch (AggregateException) { }

            }

            if (drives.Any())
            {
                foreach (var folder in folderCollection)
                {
                    pathes = new List<string>();
                    for (int i = 0; i < drives.Count; i++)
                    {
                        foreach (var File in folder.FileCollection)
                        {
                            pathes.Add(drives[i].Name + File.Path);
                        }
                    }

                    var fileCollection = SetPathesAsync(pathes);

                    var collection = folderCollection.FirstOrDefault(c => c.FolderName.Equals(folder.FolderName));
                    if (collection != null)
                    {
                        collection.FileCollection = fileCollection;
                    }
                }
            }
            else
            {

            }
        }

        private ObservableCollection<StorageFileType<StorageFile>> SetPathesAsync(List<string> pathes)
        {
            ObservableCollection<StorageFileType<StorageFile>> fileCollection = new ObservableCollection<StorageFileType<StorageFile>>();

            foreach (string path in pathes)
            {
                if (File.Exists(path) == true)
                {
                    fileCollection.Add(new StorageFileType<StorageFile> { Path = path });
                }
            }

            return fileCollection;
        }

        #endregion
    }
}
