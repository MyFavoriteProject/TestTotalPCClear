using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace PCCleaner.Model
{
    public class CacheModel : BaseModel
    {
        #region Fields

        ObservableCollection<StorageFolderType<StorageFolder>> folderCollection;

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

        #region Constructor

        public CacheModel()
        {
            cacheView = new List<(string key, string icon, List<string> pathes)>
            {
                (key:cacheName[0], icon:Constants.FileTypeGlyph.SystemCacheGlyph, pathes:this.systemCacheUniversalPaths),
                (key:cacheName[1], icon:Constants.FileTypeGlyph.ApplicationCacheGlyph, pathes:this.applicationCacheUniversalPath),
                (key:cacheName[2], icon:Constants.FileTypeGlyph.MailCacheGlyph, pathes:this.mailCacheUniversalPath),
                (key:cacheName[3], icon:Constants.FileTypeGlyph.OfficeCacheGlyph, pathes:this.officeCacheUniversalPath),
                (key:cacheName[4], icon:Constants.FileTypeGlyph.BrowserCacheGlyph, pathes:this.browserCacheUniversalPath)
            };

            this.FolderCollection = new ObservableCollection<StorageFolderType<StorageFolder>>();

            foreach(var item in cacheView)
            {
                this.FolderCollection.Add(new StorageFolderType<StorageFolder> { FolderName = item.key, Icone = item.icon });
            }
        }

        #endregion

        #region public Properties

        public ObservableCollection<StorageFolderType<StorageFolder>> FolderCollection 
        { 
            get=>this.folderCollection;
            set
            {
                this.folderCollection = value;
            } 
        }

        #endregion

        #region public Methods

        public async Task ScanningSystemCacheAsync()
        {
            this.FolderCollection = new ObservableCollection<StorageFolderType<StorageFolder>>();

            try
            {
                await SearchPathesAsync().ConfigureAwait(true);
            }
            catch (UnauthorizedAccessException UAE)
            {
                throw UAE;
            }

            var folders = this.FolderCollection;

            foreach (var folder in folders)
            {
                foreach(var file in folder.FileCollection)
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

            this.FolderCollection = folders;
        }

        public async Task DeleteFileAsync()
        {
            foreach(var folder in this.FolderCollection)
            {
                if (folder.IsChecked)
                {
                    foreach (var file in folder.FileCollection)
                    {
                        try
                        {
                            file.File.DeleteAsync();
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
            }

        }

        public void SetChekValue(bool value)
        {
            foreach(var folder in FolderCollection)
            {
                folder.IsChecked = value;
            }
        }

        #endregion

        #region private Methods

        private async Task SearchPathesAsync()
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
                foreach (var item in cacheView)
                {
                    pathes = new List<string>();
                    for (int i = 0; i < drives.Count; i++)
                    {
                        foreach (string universalPath in item.pathes)
                        {
                            pathes.Add(drives[i].Name + universalPath);
                        }
                    }

                    var fileCollection = SetPathesAsync(pathes);

                    var collection = FolderCollection.FirstOrDefault(c => c.FolderName.Equals(item.key));
                    if (collection != null)
                    {
                        collection.FileCollection = fileCollection;
                    }

                    //this.FolderCollection.Add(new StorageFolderObservableCollection<StorageFolder> { FileCollection = fileCollection , FolderName  = item.key, Icone = item.icon });
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
