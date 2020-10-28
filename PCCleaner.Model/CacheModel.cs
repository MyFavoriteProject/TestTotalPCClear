using PCCleaner.Model.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        ObservableCollection<StorageFolderObservableCollection<StorageFolder>> folderCollection;

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
                (key:cacheName[0], icon:"&#xE770;", pathes:this.systemCacheUniversalPaths),
                (key:cacheName[1], icon:"&#xE74C;", pathes:this.applicationCacheUniversalPath),
                (key:cacheName[2], icon:"&#xE715;", pathes:this.mailCacheUniversalPath),
                (key:cacheName[3], icon:"&#xF56E;", pathes:this.officeCacheUniversalPath),
                (key:cacheName[4], icon:"&#xE774;", pathes:this.browserCacheUniversalPath)
            };
        }

        #endregion

        #region public Properties

        public ObservableCollection<StorageFolderObservableCollection<StorageFolder>> FolderCollection 
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
            this.FolderCollection = new ObservableCollection<StorageFolderObservableCollection<StorageFolder>>();

            try
            {
                await SearchPathesAsync().ConfigureAwait(true);
            }
            catch (UnauthorizedAccessException UAE)
            {
                throw UAE;
            }

            foreach(var folder in this.FolderCollection)
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

                    this.FolderCollection.Add(new StorageFolderObservableCollection<StorageFolder> { FileCollection = fileCollection , FolderName  = item.key, Icone = item.icon });
                }
            }
            else
            {
                
            }
        }

        private ObservableCollection<StorageFileObservableCollection<StorageFile>> SetPathesAsync(List<string> pathes)
        {
            ObservableCollection<StorageFileObservableCollection<StorageFile>> fileCollection = new ObservableCollection<StorageFileObservableCollection<StorageFile>>();

            foreach (string path in pathes)
            {
                if (File.Exists(path) == true)
                {
                    fileCollection.Add(new StorageFileObservableCollection<StorageFile> { Path = path });
                }
            }

            return fileCollection;
        }

        #endregion
    }
}
