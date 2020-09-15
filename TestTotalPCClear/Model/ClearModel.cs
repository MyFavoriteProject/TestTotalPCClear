using System;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using System.Text;
using System.Collections.Generic;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using System.Linq;
using System.IO;
using System.ComponentModel;

namespace TestTotalPCClear.Model
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class ClearModel: BaseModel
    {
        #region Fields

        private ResourceLoader resourceLoader;
        bool isActiveScannOrClean = false;

        private bool isSystemCacheSelect = false,
            isApplicationCacheSelect = false,
            isMailCacheSelect = false,
            isOfficeCacheSelect = false,
            isBrowserCacheSelect = false;

        private int systemCacheSize = 0;
        private int applicationCacheSize=0;
        private int mailCacheSize=0;
        private int officeCacheSize=0;
        private int browserCacheSize=0;

        private int deleteCacheSize = 0;

        private string[] cacheName = new string[]
        { 
            "System Cache", 
            "Application Cache", 
            "Mail Cache", 
            "Office Cache", 
            "Browser Cache" 
        }; //Type Cache

        Dictionary<string, List<string>> typeCacheAndPathes; // Pathes For Cache

        Dictionary<string, int> typeCacheAndSize; // Size For Cache

        Dictionary<string, List<string>> typeCacheAndUniversalPaths; // Size For Cache

        Dictionary<string, List<StorageFile>> typeCacheAndStorageFiles;

        #region Universal Paths

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

        #endregion

        #region Constructors

        public ClearModel()
        {
            this.typeCacheAndPathes = new Dictionary<string, List<string>>();

            this.typeCacheAndSize = new Dictionary<string, int>();

            this.resourceLoader = ResourceLoader.GetForCurrentView("Resources");

            this.typeCacheAndUniversalPaths = new Dictionary<string, List<string>>();
            
            this.typeCacheAndUniversalPaths.Add(cacheName[0], this.systemCacheUniversalPaths);
            this.typeCacheAndUniversalPaths.Add(cacheName[1], this.applicationCacheUniversalPath);
            this.typeCacheAndUniversalPaths.Add(cacheName[2], this.mailCacheUniversalPath);
            this.typeCacheAndUniversalPaths.Add(cacheName[3], this.officeCacheUniversalPath);
            this.typeCacheAndUniversalPaths.Add(cacheName[4], this.browserCacheUniversalPath);
            
            
        }

        #endregion

        #region public Propertys

        public int SystemCacheSize 
        { 
            get=>this.systemCacheSize;
            set
            {
                this.systemCacheSize = value;
                OnPropertyChanged(nameof(SystemCacheSize));
            }
        }
        public int ApplicationCacheSize
        {
            get => this.applicationCacheSize;
            set
            {
                this.applicationCacheSize = value;
                OnPropertyChanged(nameof(ApplicationCacheSize));
            }
        }
        public int MailCacheSize
        {
            get => this.mailCacheSize;
            set
            {
                this.mailCacheSize = value;
                OnPropertyChanged(nameof(MailCacheSize));
            }
        }
        public int OfficeCacheSize
        {
            get => this.officeCacheSize;
            set
            {
                this.officeCacheSize = value;
                OnPropertyChanged(nameof(OfficeCacheSize));
            }
        }
        public int BrowserCacheSize
        {
            get => this.browserCacheSize;
            set
            {
                this.browserCacheSize = value;
                OnPropertyChanged(nameof(BrowserCacheSize));
            }
        }

        public int DeleteCacheSize 
        { 
            get=>this.deleteCacheSize;
            set
            {
                this.deleteCacheSize = value;
                OnPropertyChanged(nameof(DeleteCacheSize));
            } 
        }

        #region ChechBoxes

        public bool IsSystemCacheSelect
        {
            get => this.isSystemCacheSelect;
            set
            {
                this.isSystemCacheSelect = value;
                OnPropertyChanged(nameof(IsSystemCacheSelect));
            }
        }
        public bool IsApplicationCacheSelect
        {
            get => this.isApplicationCacheSelect;
            set
            {
                this.isApplicationCacheSelect = value;
                OnPropertyChanged(nameof(IsApplicationCacheSelect));
            }
        }
        public bool IsMailCacheSelect
        {
            get => this.isMailCacheSelect;
            set
            {
                this.isMailCacheSelect = value;
                OnPropertyChanged(nameof(IsMailCacheSelect));
            }
        }
        public bool IsOfficeCacheSelect
        {
            get => this.isOfficeCacheSelect;
            set
            {
                this.isOfficeCacheSelect = value;
                OnPropertyChanged(nameof(IsOfficeCacheSelect));
            }
        }
        public bool IsBrowserCacheSelect
        {
            get => this.isBrowserCacheSelect;
            set
            {
                this.isBrowserCacheSelect = value;
                OnPropertyChanged(nameof(IsBrowserCacheSelect));
            }
        }

        #endregion

        public bool IsActiveScannOrClean 
        { 
            get=>this.isActiveScannOrClean;
            set
            {
                this.isActiveScannOrClean = value;
                OnPropertyChanged(nameof(IsActiveScannOrClean));
            } 
        }

        #endregion

        #region public Methods

        private void Default()
        {
            SystemCacheSize = 0;
            ApplicationCacheSize = 0;
            MailCacheSize = 0;
            OfficeCacheSize = 0;
            DeleteCacheSize = 0;

            IsSystemCacheSelect = false;
            IsApplicationCacheSelect = false;
            IsMailCacheSelect = false;
            IsOfficeCacheSelect = false;
            IsBrowserCacheSelect = false;

            typeCacheAndSize = new Dictionary<string, int>();
            //typeCacheAndUniversalPaths = new Dictionary<string, List<string>>();
            typeCacheAndStorageFiles = new Dictionary<string, List<StorageFile>>();
        }

        public async Task<bool> DeleteFileAsync()
        {
            DeleteCacheSize = 0;

            this.IsActiveScannOrClean = true;

            if (IsSystemCacheSelect == false)
                typeCacheAndStorageFiles.Remove(cacheName[0]);

            if (IsApplicationCacheSelect == false)
                typeCacheAndStorageFiles.Remove(cacheName[1]);

            if (IsMailCacheSelect == false)
                typeCacheAndStorageFiles.Remove(cacheName[2]);

            if (IsOfficeCacheSelect == false)
                typeCacheAndStorageFiles.Remove(cacheName[3]);

            if (IsBrowserCacheSelect == false)
                typeCacheAndStorageFiles.Remove(cacheName[4]);

            foreach (string key in this.typeCacheAndStorageFiles.Keys)
            {
                if (typeCacheAndStorageFiles[key].Any())
                {
                    foreach(StorageFile storageFile in this.typeCacheAndStorageFiles[key])
                    {
                        try
                        {
                            storageFile.DeleteAsync();
                        }
                        catch (Exception)
                        {
                            throw new Exception("Can't delete"+storageFile.DisplayName);
                        }
                    }

                    DeleteCacheSize += typeCacheAndSize[key];
                }
            }

            this.typeCacheAndStorageFiles.Clear();
            this.typeCacheAndSize.Clear();

            this.IsActiveScannOrClean = false;

            this.Default();

            return true;
        }

        public async Task<bool> ScaningSystemCacheAsync()
        {
            //this.IsActiveScannOrClean = true;
            SearchPathes();

            bool isExeption = false;

            typeCacheAndStorageFiles = new Dictionary<string, List<StorageFile>>();
            StorageFile storageFile = null;
            BasicProperties basicProperties = null;
            List<StorageFile> storageFiles = new List<StorageFile>();
            int fileSize;

            foreach (string key in this.typeCacheAndPathes.Keys)
            {
                fileSize = 0;
                foreach (string filePath in this.typeCacheAndPathes[key])
                {
                    try
                    {
                        storageFile = await StorageFile.GetFileFromPathAsync(filePath);
                        basicProperties = await storageFile.GetBasicPropertiesAsync();
                    }
                    catch (Exception) 
                    {
                        isExeption = true;
                    }

                    if(isExeption == false)
                    {
                        storageFiles.Add(storageFile);
                        fileSize += (int)basicProperties.Size;

                        if (this.cacheName[0].Equals(key))
                        {
                            SystemCacheSize = fileSize;
                        }
                        if (this.cacheName[1].Equals(key))
                        {
                            ApplicationCacheSize = fileSize;
                        }
                        if (this.cacheName[2].Equals(key))
                        {
                            MailCacheSize = fileSize;
                        }
                        if (this.cacheName[3].Equals(key))
                        {
                            OfficeCacheSize = fileSize;
                        }
                        if (this.cacheName[4].Equals(key))
                        {
                            BrowserCacheSize = fileSize;
                        }
                    }

                }

                this.typeCacheAndSize.Add(key,fileSize);
                this.typeCacheAndStorageFiles.Add(key, storageFiles);
            }

            this.IsActiveScannOrClean = false;

            return true;
        }



        #endregion

        #region private Methods

        private void SearchPathes()
        {
            this.Default();

            List<string> pathes = new List<string>();
            this.typeCacheAndPathes = new Dictionary<string, List<string>>();
            this.typeCacheAndSize = new Dictionary<string, int>();

            string driveLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int driveLettersLen = driveLetters.Length;
            string removableDriveLetters = "";
            string driveLetter;

            List<StorageFolder> drives = new List<StorageFolder>();
            StorageFolder removableDevices = KnownFolders.RemovableDevices;
            IReadOnlyList<StorageFolder> folders = Task.Run<IReadOnlyList<StorageFolder>>(async () => await removableDevices.GetFoldersAsync()).Result;

            foreach (StorageFolder removableDevice in folders)
            {
                if (string.IsNullOrEmpty(removableDevice.Path))
                    continue;

                driveLetter = removableDevice.Path.Substring(0, 1).ToUpper();

                if (driveLetters.IndexOf(driveLetter) > -1)
                    removableDriveLetters += driveLetter;
            }

            for (int curDrive = 0; curDrive < driveLettersLen; curDrive++)
            {
                driveLetter = driveLetters.Substring(curDrive, 1);

                if (removableDriveLetters.IndexOf(driveLetter) > -1)
                    continue;

                try
                {
                    StorageFolder drive = Task.Run<StorageFolder>(
                        async () => await StorageFolder.GetFolderFromPathAsync(driveLetter + ":")).Result;
                    drives.Add(drive);
                }
                catch (System.AggregateException) { }

            }

            if (drives.Any() == true)
            {

                foreach(string key in typeCacheAndUniversalPaths.Keys)
                {
                    pathes = new List<string>();
                    for (int i = 0; i < drives.Count; i++)
                    {
                        foreach(string universalPath in typeCacheAndUniversalPaths[key])
                        {
                            pathes.Add(drives[i].Name + universalPath);
                        }

                        //for (int j = 0; j < this.universalPaths.Length; j++)
                        //{
                        //    pathes.Add(drives[i].Name + this.universalPaths[j]);
                        //}
                    }

                    SetPathesAsync(key, pathes);
                }
                
            }
            else
            {
                MessageDialog dialog = new MessageDialog("");
                dialog.ShowAsync();
            }
        }

        private void SetPathesAsync(string key,List<string> pathes)
        {
            List<string> realPathes = new List<string>();

            foreach(string path in pathes)
            {
                if (File.Exists(path)==true)
                    realPathes.Add(path);
            }

            this.typeCacheAndPathes.Add(key, realPathes);
        }

        #endregion
    }
}