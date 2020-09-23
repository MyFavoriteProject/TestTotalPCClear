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
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Helpers;

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

    public class ClearModel: BaseModel
    {
        #region Fields

        private ResourceLoader resourceLoader;
        private bool isActiveScannOrClean = false;

        private string driveLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        ObservableCollection<CheckedListItem<StorageFile>> largeFileCollection;
        ObservableCollection<CheckedListItem<StorageFile>> duplicateFileCollection;

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
                if (!this.systemCacheSize.Equals(value))
                {
                    this.systemCacheSize = value;
                    OnPropertyChanged(nameof(SystemCacheSize));
                }
            }
        }
        public int ApplicationCacheSize
        {
            get => this.applicationCacheSize;
            set
            {
                if (!this.applicationCacheSize.Equals(value))
                {
                    this.applicationCacheSize = value;
                    OnPropertyChanged(nameof(ApplicationCacheSize));
                }
            }
        }
        public int MailCacheSize
        {
            get => this.mailCacheSize;
            set
            {
                if (!this.mailCacheSize.Equals(value))
                {
                    this.mailCacheSize = value;
                    OnPropertyChanged(nameof(MailCacheSize));
                }
            }
        }
        public int OfficeCacheSize
        {
            get => this.officeCacheSize;
            set
            {
                if (!this.officeCacheSize.Equals(value))
                {
                    this.officeCacheSize = value;
                    OnPropertyChanged(nameof(OfficeCacheSize));
                }
            }
        }
        public int BrowserCacheSize
        {
            get => this.browserCacheSize;
            set
            {
                if (!this.browserCacheSize.Equals(value))
                {
                    this.browserCacheSize = value;
                    OnPropertyChanged(nameof(BrowserCacheSize));
                }
            }
        }
        public int DeleteCacheSize 
        { 
            get=>this.deleteCacheSize;
            set
            {
                if(!this.deleteCacheSize.Equals(value))
                this.deleteCacheSize = value;
                OnPropertyChanged(nameof(DeleteCacheSize));
            } 
        }
        public ObservableCollection<CheckedListItem<StorageFile>> LargeFileCollection
        {
            get => this.largeFileCollection;
            set
            {
                if (this.largeFileCollection == null || !this.largeFileCollection.Equals(value))
                {
                    this.largeFileCollection = value;
                    OnPropertyChanged(nameof(LargeFileCollection));
                }
            }
        }

        public ObservableCollection<CheckedListItem<StorageFile>> DuplicateFileCollection 
        { 
            get=>this.duplicateFileCollection; 
            set
            {
                if (this.duplicateFileCollection == null || !this.duplicateFileCollection.Equals(value))
                {
                    this.duplicateFileCollection = value;
                    OnPropertyChanged(nameof(DuplicateFileCollection));
                }
            }
        }

        #region ChechBoxes

        public bool IsSystemCacheSelect
        {
            get => this.isSystemCacheSelect;
            set
            {
                if (!this.isActiveScannOrClean.Equals(value))
                {
                    this.isSystemCacheSelect = value;
                    OnPropertyChanged(nameof(IsSystemCacheSelect));
                }
            }
        }
        public bool IsApplicationCacheSelect
        {
            get => this.isApplicationCacheSelect;
            set
            {
                if (!this.isApplicationCacheSelect.Equals(value))
                {
                    this.isApplicationCacheSelect = value;
                    OnPropertyChanged(nameof(IsApplicationCacheSelect));
                }
            }
        }
        public bool IsMailCacheSelect
        {
            get => this.isMailCacheSelect;
            set
            {
                if (!this.isMailCacheSelect.Equals(value))
                {
                    this.isMailCacheSelect = value;
                    OnPropertyChanged(nameof(IsMailCacheSelect));
                }
            }
        }
        public bool IsOfficeCacheSelect
        {
            get => this.isOfficeCacheSelect;
            set
            {
                if (!this.isOfficeCacheSelect.Equals(value))
                {
                    this.isOfficeCacheSelect = value;
                    OnPropertyChanged(nameof(IsOfficeCacheSelect));
                }
            }
        }
        public bool IsBrowserCacheSelect
        {
            get => this.isBrowserCacheSelect;
            set
            {
                if (!this.isBrowserCacheSelect.Equals(value))
                {
                    this.isBrowserCacheSelect = value;
                    OnPropertyChanged(nameof(IsBrowserCacheSelect));
                }
            }
        }

        #endregion

        public bool IsActiveScannOrClean 
        { 
            get=>this.isActiveScannOrClean;
            set
            {
                if (!this.isActiveScannOrClean.Equals(value))
                {
                    this.isActiveScannOrClean = value;
                    OnPropertyChanged(nameof(IsActiveScannOrClean));
                }
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
            BrowserCacheSize = 0;
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

        public async Task DeleteFileAsync()
        {
            if(DeleteCacheSize != 0)
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

            //this.Default();
        }

        public async Task ScaningSystemCacheAsync()
        {
            this.IsActiveScannOrClean = true;

            try
            {
                await SearchPathes().ConfigureAwait(true);
            }
            catch(Exception e)
            {
                throw new Exception();
            }


            bool isExeption = false;

            typeCacheAndStorageFiles = new Dictionary<string, List<StorageFile>>();
            List<StorageFile> storageFiles = new List<StorageFile>();
            int fileSize;

            foreach (string key in this.typeCacheAndPathes.Keys)
            {
                fileSize = 0;
                foreach (string filePath in this.typeCacheAndPathes[key])
                {
                    try
                    {
                        StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filePath);
                        BasicProperties basicProperties = await storageFile.GetBasicPropertiesAsync();
                        storageFiles.Add(storageFile);
                        fileSize += (int)basicProperties.Size;
                    }
                    catch (Exception) 
                    {
                        isExeption = true;
                    }

                    if(isExeption == false)
                    {
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
        }

        public async Task OpenLargeFolder()
        {
            ObservableCollection<CheckedListItem<StorageFile>> largeFileCollection = new ObservableCollection<CheckedListItem<StorageFile>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                IReadOnlyList<StorageFile> fileList = await PullFilesFromFolder(storageFolder).ConfigureAwait(true);
                foreach(StorageFile storageFile in fileList)
                {
                    largeFileCollection.Add(new CheckedListItem<StorageFile>(storageFile));
                }
            }

            this.LargeFileCollection = largeFileCollection;
        }

        public async Task CleanLargeFiles()
        {
            List<StorageFile> largeFileList = this.LargeFileCollection.Where(c => c.IsChecked == true).Select(c=>c.Item).ToList();

            LargeFileCollection.Clear();
        }

        public async Task ScannDuplicateFiles()
        {
            //Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
            ObservableCollection<CheckedListItem<StorageFile>> fileCollection = new ObservableCollection<CheckedListItem<StorageFile>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                List<StorageFile> fileList = await PullFilesFromFolder(storageFolder).ConfigureAwait(true);

                for(int i = 0; i< fileList.Count; i++)
                {
                    var a = fileList[i].GetHashCode();

                    for (int j = i+1; j < fileList.Count; j++)
                    {
                        if(fileList[i].FileType == fileList[j].FileType && i!=j)
                        {
                            BasicProperties basicIFile = await fileList[i].GetBasicPropertiesAsync();
                            BasicProperties basicJFile = await fileList[j].GetBasicPropertiesAsync();

                            if (basicIFile.Size == basicJFile.Size)
                            {
                                var byteI = await StorageFileHelper.ReadBytesAsync(fileList[i]).ConfigureAwait(true);
                                var byteJ = await StorageFileHelper.ReadBytesAsync(fileList[j]).ConfigureAwait(true);

                                if (byteI.SequenceEqual(byteJ))
                                {
                                    fileCollection.Add(new CheckedListItem<StorageFile>(fileList[j]));
                                }
                            }
                        }
                    }
                }

                this.DuplicateFileCollection = fileCollection;
            }
        }

        public async Task CleanDuplicateFiles()
        {
            List<StorageFile> duplicateFileList = this.DuplicateFileCollection.Where(c => c.IsChecked == true).Select(c => c.Item).ToList();

            DuplicateFileCollection.Clear();
        }

        #endregion

        #region private Methods

        private async Task SearchPathes()
        {
            this.Default();

            List<string> pathes = new List<string>();
            this.typeCacheAndPathes = new Dictionary<string, List<string>>();
            this.typeCacheAndSize = new Dictionary<string, int>();

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
                    removableDriveLetters += driveLetter;
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
                foreach(string key in typeCacheAndUniversalPaths.Keys)
                {
                    pathes = new List<string>();
                    for (int i = 0; i < drives.Count; i++)
                    {
                        foreach(string universalPath in typeCacheAndUniversalPaths[key])
                        {
                            pathes.Add(drives[i].Name + universalPath);
                        }
                    }

                    SetPathesAsync(key, pathes);
                }
            }
            else
            {
                //MessageDialog dialog = new MessageDialog("");
                //dialog.ShowAsync();
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

        private async Task<IReadOnlyList<StorageFolder>> PullFoldersFromFolder(StorageFolder storageFolder)
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

                    if (storageFolders.Count>0)
                    {
                        storageNewFoldersList.Add(storageFolders);
                    }
                }
            }

            foreach (IReadOnlyList<StorageFolder> folders in storageNewFoldersList)
            {
                foreach(StorageFolder folder in folders)
                {
                    storageFoldersList.Add(folder);
                }
            }

            return storageFoldersList;
        }

        private async Task<List<StorageFile>> PullFilesFromFolder(StorageFolder storageFolder)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            IReadOnlyList<StorageFolder> folderList = await PullFoldersFromFolder(storageFolder).ConfigureAwait(true);

            foreach (StorageFolder folder in folderList)
            {
                IReadOnlyList<StorageFile> fileList = await folder.GetFilesAsync();

                foreach(StorageFile storageFile in fileList)
                {
                    storageFileList.Add(storageFile);
                }
            }

            return storageFileList;
        }

        #endregion
    }
}