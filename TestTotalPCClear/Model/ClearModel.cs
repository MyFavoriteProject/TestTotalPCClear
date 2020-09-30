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

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
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
        ObservableCollection<CheckedListFile<StorageFile>> largeFileCollection;
        ObservableCollection<CheckedListItem<StorageFolder>> largeFolderCollection;
        ObservableCollection<CheckedListFile<StorageFile>> duplicateFileCollection;
        ObservableCollection<CheckedListItem<StorageFolder>> duplicateFolderCollection;

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
        public ObservableCollection<CheckedListFile<StorageFile>> LargeFileCollection
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
        public ObservableCollection<CheckedListItem<StorageFolder>> LargeFolderCollection
        {
            get => this.largeFolderCollection;
            set
            {
                if (this.largeFolderCollection == null || !this.largeFolderCollection.Equals(value))
                {
                    this.largeFolderCollection = value;
                    OnPropertyChanged(nameof(LargeFolderCollection));
                }
            }
        }
        public ObservableCollection<CheckedListFile<StorageFile>> DuplicateFileCollection 
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
        public ObservableCollection<CheckedListItem<StorageFolder>> DuplicateFolderCollection
        {
            get => this.duplicateFolderCollection;
            set
            {
                if (this.duplicateFolderCollection == null || !this.duplicateFolderCollection.Equals(value))
                {
                    this.duplicateFolderCollection = value;
                    OnPropertyChanged(nameof(DuplicateFolderCollection));
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

        #region CacheMethods

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

        public async Task ScanningSystemCacheAsync()
        {
            this.IsActiveScannOrClean = true;

            try
            {
                await SearchPathesAsync().ConfigureAwait(true);
            }
            catch(UnauthorizedAccessException UAE)
            {
                throw UAE;
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

        #endregion

        #region LargeFolderMatheds

        public async Task<bool> OpenLargeFolderAsync()
        {
            if (this.LargeFileCollection != null)
            {
                LargeFileCollection.Clear();
            }
            if (this.LargeFolderCollection != null)
            {
                LargeFolderCollection.Clear();
            }

            bool isOpenFolder = false;

            ObservableCollection<CheckedListItem<StorageFolder>> largeFolders = new ObservableCollection<CheckedListItem<StorageFolder>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                isOpenFolder = true;

                IReadOnlyList<StorageFolder> fileList = await PullFoldersFromFolderAsync(storageFolder).ConfigureAwait(true);
                foreach(StorageFolder folder in fileList)
                {
                    largeFolders.Add(new CheckedListItem<StorageFolder>(folder));
                }
            }

            this.LargeFolderCollection = largeFolders;

            return isOpenFolder;
        }

        public bool IsSelectedLargeFileDeleteFolder()
        {
            bool result = IsSelectedDeleteFolderAsync(LargeFolderCollection);

            return result;
        }

        public void LargeFileDeleteFolder()
        {
            ObservableCollection<CheckedListItem<StorageFolder>> checkedLists = DeleteFolder(this.LargeFolderCollection);

            if (!checkedLists.Equals(this.LargeFolderCollection))
            {
                this.LargeFolderCollection = checkedLists;
            }
        }

        public async Task ScanningLargeFolderAsync()
        {
            List<StorageFile> storageFiles = await PullFilesFromFolderAsync(this.LargeFolderCollection).ConfigureAwait(true);

            ObservableCollection<CheckedListFile<StorageFile>> storageFileColl = new ObservableCollection<CheckedListFile<StorageFile>>();

            foreach (StorageFile storageFile in storageFiles)
            {
                BasicProperties properties = await storageFile.GetBasicPropertiesAsync();

                storageFileColl.Add(new CheckedListFile<StorageFile>(storageFile, properties));
            }

            this.LargeFileCollection = storageFileColl;
        }

        public async Task CleanLargeFilesAsync()
        {
            List<StorageFile> largeFileList = this.LargeFileCollection.Where(c => c.IsChecked == true).Select(c=>c.Item).ToList();

            LargeFileCollection.Clear();
        }

        #endregion

        #region DuplicateMEthods

        public async Task<bool> OpenDuplicateFolderAsync()
        {
            this.DuplicateFolderCollection.Clear();
            this.DuplicateFileCollection.Clear();

            bool isOpenFolder = false;

            ObservableCollection<CheckedListItem<StorageFolder>> folderCollection = new ObservableCollection<CheckedListItem<StorageFolder>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                isOpenFolder = true;
                List<StorageFolder> storageFolders = await PullFoldersFromFolderAsync(storageFolder);

                foreach(StorageFolder folder in storageFolders)
                {
                    folderCollection.Add(new CheckedListItem<StorageFolder>(folder));
                }

            }

            this.DuplicateFolderCollection = folderCollection;

            return isOpenFolder;
        }

        public bool IsSelectedDuplicateDeleteFolder()
        {
            bool result = IsSelectedDeleteFolderAsync(DuplicateFolderCollection);

            return result;
        }

        public void DuplicateDeleteFolder()
        {
            ObservableCollection<CheckedListItem<StorageFolder>> checkedLists = DeleteFolder(this.DuplicateFolderCollection);

            if (!checkedLists.Equals(this.DuplicateFolderCollection))
            {
                this.DuplicateFolderCollection = checkedLists;
            }
        }

        public async Task ScannDuplicateFilesAsync()
        {
            //Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
            ObservableCollection<CheckedListFile<StorageFile>> fileCollection = new ObservableCollection<CheckedListFile<StorageFile>>();

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                List<StorageFile> fileList = await PullFilesFromFolderAsync(DuplicateFolderCollection).ConfigureAwait(true);

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
                                    fileCollection.Add(new CheckedListFile<StorageFile>(fileList[j], basicJFile));
                                }
                            }
                        }
                    }
                }

                this.DuplicateFileCollection = fileCollection;
            }
        }

        public async Task CleanDuplicateFilesAsync()
        {
            List<StorageFile> duplicateFileList = this.DuplicateFileCollection.Where(c => c.IsChecked == true).Select(c => c.Item).ToList();

            DuplicateFileCollection.Clear();
        }

        #endregion

        #endregion

        #region private Methods

        private async Task SearchPathesAsync()
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

        private async Task<List<StorageFile>> PullFilesFromFolderAsync(ObservableCollection<CheckedListItem<StorageFolder>> checkedListItems)
        {
            List<StorageFile> storageFileList = new List<StorageFile>();

            foreach (CheckedListItem<StorageFolder> folder in this.LargeFolderCollection)
            {
                IReadOnlyList<StorageFile> fileList = await folder.Item.GetFilesAsync();

                foreach(StorageFile storageFile in fileList)
                {
                    storageFileList.Add(storageFile);
                }

                GC.Collect(2);
            }

            return storageFileList;
        }

        private bool IsSelectedDeleteFolderAsync(ObservableCollection<CheckedListItem<StorageFolder>> checkedListItems)
        {
            bool isFounded = checkedListItems.Any(c => c.IsChecked == true);

            return isFounded;
        }

        private ObservableCollection<CheckedListItem<StorageFolder>> DeleteFolder(ObservableCollection<CheckedListItem<StorageFolder>> checkedListItems)
        {
            ObservableCollection<CheckedListItem<StorageFolder>> checkedLists = new ObservableCollection<CheckedListItem<StorageFolder>>();


            foreach (CheckedListItem<StorageFolder> listItem in checkedListItems)
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