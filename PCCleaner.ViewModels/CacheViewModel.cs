using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PCCleaner.Model;
using PCCleaner.ViewModels.Command;

namespace PCCleaner.ViewModels
{
    public class CacheViewModel:BaseViewModel
    {
        #region Fields

        private CacheModel cacheModel;
        private bool isShowSelectOrDeselectButton;

        #endregion

        #region Constructors

        public CacheViewModel()
        {
            this.cacheModel = new CacheModel();

            //this.cacheModel.SetFolderCollectionAsynk();

            this.ScannCacheButton = new DelegateCommand(ScannCacheButton_Click);
            this.CleanCacheButton = new DelegateCommand(CleanCacheButton_Click);
            this.SelectAllButton = new DelegateCommand(SelectAllButton_Click);
            this.DeselectAllButton = new DelegateCommand(DeselectAllButton_Click);
            this.MoreButton = new DelegateCommand(MoreButton_Click);
        }

        #endregion

        #region public Properys

        public CacheModel CacheModel 
        { 
            get =>this.cacheModel;
            set
            {
                if (value != this.cacheModel)
                {
                    this.cacheModel = value;
                    OnPropertyChanged(nameof(CacheModel));
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
                await this.cacheModel.ScanningSystemCacheAsync().ConfigureAwait(true);
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
                await this.cacheModel.DeleteFileAsync().ConfigureAwait(true);
            }
            catch (UnauthorizedAccessException)
            {
                //await AccessFileSystem().ConfigureAwait(true);
            }
        }

        private void SelectAllButton_Click(object obj)
        {
            this.cacheModel.SetChekValue(true);
        }

        private void DeselectAllButton_Click(object obj)
        {
            this.cacheModel.SetChekValue(false);
        }

        private void MoreButton_Click(object obj)
        {
            if (IsShowSelectOrDeselectButton)
                IsShowSelectOrDeselectButton = false;
            if (!IsShowSelectOrDeselectButton)
                IsShowSelectOrDeselectButton = true;
        }

        private async Task AccessFileSystem()
        {
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-broadfilesystemaccess"));
        }

        #endregion
    }
}
