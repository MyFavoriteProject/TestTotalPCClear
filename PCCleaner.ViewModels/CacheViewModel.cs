using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PCCleaner.Core;
using PCCleaner.Model;
using PCCleaner.Model.Collections;
using PCCleaner.ViewModels.Command;
using Windows.Storage;

namespace PCCleaner.ViewModels
{
    public class CacheViewModel:BaseViewModel
    {
        #region Fields

        ObservableCollection<StorageFolderType<StorageFolder>> folderCollection;
        private CacheProvider cacheProvider;
        private bool isShowSelectOrDeselectButton;

        #endregion

        #region Constructors

        public CacheViewModel()
        {
            this.cacheProvider = new CacheProvider();

            folderCollection = cacheProvider.GetStorageFolderType();

            //this.cacheModel.SetFolderCollectionAsynk();

            this.ScannCacheButton = new DelegateCommand(ScannCacheButton_Click);
            this.CleanCacheButton = new DelegateCommand(CleanCacheButton_Click);
            this.SelectAllButton = new DelegateCommand(SelectedAllExecute);
            this.DeselectAllButton = new DelegateCommand(DeselectedAllExecute);
            this.MoreButton = new DelegateCommand(MoreButtonExecute);
        }

        #endregion

        #region public Properys

        public CacheProvider CacheProvider 
        { 
            get =>this.cacheProvider;
            set
            {
                if (value != this.cacheProvider)
                {
                    this.cacheProvider = value;
                    OnPropertyChanged(nameof(CacheProvider));
                }
            } 
        }

        public ObservableCollection<StorageFolderType<StorageFolder>> FolderCollection
        {
            get => this.folderCollection;
            set
            {
                if (!value.Equals(this.folderCollection))
                {
                    this.folderCollection = value;
                }
            }
        }

        public bool IsShowSelectOrDeselectButton 
        { 
            get=>this.isShowSelectOrDeselectButton;
            set
            {
                if (!value.Equals(this.isShowSelectOrDeselectButton))
                {
                    this.isShowSelectOrDeselectButton = value;
                    OnPropertyChanged(nameof(IsShowSelectOrDeselectButton));
                }
            } 
        }

        public ICommand ScannCacheButton { get; set; }
        public ICommand CleanCacheButton { get; set; }
        public ICommand SelectAllButton { get; set; }
        public ICommand DeselectAllButton { get; set; }
        public ICommand MoreButton { get; set; }

        #endregion

        #region public Methods

        #endregion

        #region private Methods

        private async void ScannCacheButton_Click(object obj)
        {
            try
            {
                await this.cacheProvider.ScanningSystemCacheAsync(folderCollection).ConfigureAwait(true);
            }
            catch (UnauthorizedAccessException)
            {
               await AccessFileSystem().ConfigureAwait(true);
            }
        }

        private async void CleanCacheButton_Click(object obj)
        {
            try
            {
                await this.cacheProvider.DeleteFileAsync(folderCollection).ConfigureAwait(true);
            }
            catch (UnauthorizedAccessException)
            {
                //await AccessFileSystem().ConfigureAwait(true);
            }
        }


        private void MoreButtonExecute(object obj)
        {
            if (IsShowSelectOrDeselectButton)
            {
                IsShowSelectOrDeselectButton = false;
            }
            else
            {
                IsShowSelectOrDeselectButton = true;
            }
        }

        private void SelectedAllExecute(object obj)
        {
            if (this.FolderCollection != null && this.FolderCollection.Any())
            {
                foreach (var folder in this.FolderCollection)
                {
                    folder.IsChecked = true;
                }
            }
        }

        private void DeselectedAllExecute(object obj)
        {
            if (this.FolderCollection != null && this.FolderCollection.Any())
            {
                foreach (var folder in this.FolderCollection)
                {
                    folder.IsChecked = false;
                }
            }
        }

        private async Task AccessFileSystem()
        {
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-broadfilesystemaccess"));
        }

        #endregion
    }
}
