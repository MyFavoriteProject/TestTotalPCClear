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

        public CacheModel cacheModel;

        #endregion

        #region Constructors

        public CacheViewModel()
        {
            this.cacheModel = new CacheModel();

            this.ScannCacheButton = new DelegateCommand(ScannCacheButton_Click);
            this.CleanCacheButton = new DelegateCommand(CleanCacheButton_Click);
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

        public ICommand ScannCacheButton { get; set; }
        public ICommand CleanCacheButton { get; set; }

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
               //await AccessFileSystem().ConfigureAwait(true);
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

        #endregion
    }
}
