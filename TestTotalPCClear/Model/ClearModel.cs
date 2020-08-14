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

namespace TestTotalPCClear.Model
{
    class ClearModel
    {
        #region Fields

        private ResourceLoader resourceLoader;
        private List<StorageFile> storageFiles;
        private string[] universalPaths;
        private List<string> filesPath;

        #endregion

        #region Constructors

        public ClearModel()
        {
            this.resourceLoader = ResourceLoader.GetForCurrentView("Resources");

            this.universalPaths = new string[] 
            {
                @"Users\home\AppData\Local\dbgengsqmdata00.sqm",
                @"Users\home\AppData\Local\dbgengsqmdata01.sqm",
                @"Users\home\AppData\Local\dbgengsqmdata02.sqm",
                @"Users\home\AppData\Local\dbgengsqmdata03.sqm",
                @"Users\home\AppData\Local\dbgengsqmdata04.sqm",
                @"Users\home\AppData\Local\dbgengsqmdata05.sqm",
                @"Users\home\AppData\Local\dbgengsqmdata06.sqm",
                @"Windows\Logs\MoSetup\BlueBox.log",
                @"Windows\Logs\DISM\dism.log",
                @"Windows\Logs\CBS\FilterList.log",
                @"Windows\Logs\DPX\setuperr.log"
            };
        }

        #endregion

        #region public Propertys

        public List<string> FilesInFolderList { get; private set; }

        #endregion

        #region public Methods

        public async void DeleteFileAsync()
        {
            if (this.storageFiles == null)
            {
                MessageDialog dialogError = new MessageDialog(this.resourceLoader.GetString("IsNotFoundFile"));
                await dialogError.ShowAsync();

                return;
            }
                
            MessageDialog dialog = null;
            StringBuilder stringBuilder = null;
            MessageDialog dialogIsNotDelet = null;
            StringBuilder stringBuilderIsNotDelet = null;

            foreach (StorageFile storageFile in this.storageFiles)
            {
                try
                {
                    storageFile.DeleteAsync();

                    if (stringBuilder == null)
                        stringBuilder = new StringBuilder();
                    stringBuilder.Append(this.resourceLoader.GetString("DeleteFile") + ": " + storageFile.Name + "\n");
                }
                catch (Exception)
                {
                    if (stringBuilderIsNotDelet == null)
                        stringBuilderIsNotDelet = new StringBuilder();
                    stringBuilderIsNotDelet.Append(this.resourceLoader.GetString("FileWasNotDelete") + ": " + storageFile.Name + "\n");
                    
                }
            }
            if (stringBuilderIsNotDelet != null)
            {
                dialogIsNotDelet = new MessageDialog(stringBuilderIsNotDelet.ToString());
                await dialogIsNotDelet.ShowAsync();
            }
            if (stringBuilder != null)
            {
                dialog = new MessageDialog(stringBuilder.ToString());
                await dialog.ShowAsync();
            }
            this.storageFiles.Clear();
            this.FilesInFolderList.Clear();
        }

        public async Task<List<string>> SearchCacheFilesAsync()
        {
            SearchPathes();

            StringBuilder stringBuilder =null;
            MessageDialog dialog=null;

            StorageFile storageFile = null;
            this.storageFiles = new List<StorageFile>();
            this.FilesInFolderList = new List<string>();

            foreach(string filePath in this.filesPath)
            {
                try
                {
                    storageFile = await StorageFile.GetFileFromPathAsync(filePath);
                    BasicProperties basicProperties = await storageFile.GetBasicPropertiesAsync();
                    this.storageFiles.Add(storageFile);
                    this.FilesInFolderList.Add(storageFile.Path+" "+ (double)basicProperties.Size/1000 +"kBytes" + "\n");
                }
                catch
                {
                    if (stringBuilder == null)
                        stringBuilder = new StringBuilder();

                    stringBuilder.Append(this.resourceLoader.GetString("IsNotFoundFile") + ": " + filePath + "\n");
                }
            }

            if (stringBuilder != null)
            {
                dialog = new MessageDialog(stringBuilder.ToString());
                dialog.ShowAsync();
            }

            return this.FilesInFolderList;
        }



        #endregion

        #region private Methods

        private void SearchPathes()
        {
            this.filesPath = new List<string>();
            List<string> pathes = new List<string>();

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
                for (int i = 0; i < drives.Count; i++)
                {
                    for (int j = 0; j < this.universalPaths.Length; j++)
                    {
                        pathes.Add(drives[i].Name + this.universalPaths[j]);
                    }
                }
                SetPathesAsync(pathes);
            }
            else
            {
                MessageDialog dialog = new MessageDialog("");
                dialog.ShowAsync();
            }
        }

        private void SetPathesAsync(List<string> pathes)
        {
            foreach(string path in pathes)
            {
                if (File.Exists(path)==true)
                    this.filesPath.Add(path);
            }
        }

        #endregion
    }
}
