using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PCCleaner.Model;
using PCCleaner.ViewModels.Command;
using PCCleaner.ViewModels.Interfaces;

namespace PCCleaner.ViewModels
{
    public class LargeFileViewModel:BaseViewModel, IPutFile
    {
        #region Fields

        private LargeFileModel largeFileModel;

        #endregion

        #region Constructors

        public LargeFileViewModel()
        {
            this.largeFileModel = new LargeFileModel();

            this.LargeFileScannFolderButton = new DelegateCommand(LargeFileScannFolderButton_Click);
            this.LargeFileDeleteFolderButton = new DelegateCommand(LargeFileDeleteFolderButtonButton_Click);
            this.CleanLargeFileButton = new DelegateCommand(CleanLargeFileButton_Click);
            this.FileOpenFolderButton = new DelegateCommand(FileOpenFolderButton_Click);
        }

        #endregion

        #region public Propertys

        public LargeFileModel LargeFileModel 
        { 
            get=>this.largeFileModel;
            set
            {
                if (value != this.largeFileModel)
                {
                    this.largeFileModel = value;
                    OnPropertyChanged(nameof(LargeFileModel));
                }
            } 
        }

        public string HelloText { get; set; }
        public string LetsPutThingsInOrderText { get; set; }
        public ICommand FileOpenFolderButton { get; set; }
        public string AddFoldersText { get; set; }
        public ICommand LargeFileScannFolderButton { get; set; }
        public ICommand LargeFileDeleteFolderButton { get; set; }
        public ICommand CleanLargeFileButton { get; set; }

        #endregion

        #region private Methods

        private async void FileOpenFolderButton_Click(object obj)
        {
            await this.largeFileModel.OpenLargeFolderAsync().ConfigureAwait(true);
        }

        private async void LargeFileScannFolderButton_Click(object obj)
        {
            await this.largeFileModel.ScanningLargeFolderAsync().ConfigureAwait(true);
        }

        private void LargeFileDeleteFolderButtonButton_Click(object obj)
        {
            bool isSelected = this.largeFileModel.IsSelectedLargeFileDeleteFolder();

            if (isSelected == true)
            {
                this.largeFileModel.LargeFileDeleteFolder();
                //await this.deleteDialog.ShowAsync();
            }
        }

        private void CleanLargeFileButton_Click(object obj)
        {
            this.largeFileModel.CleanLargeFiles();
        }

        

        #endregion
    }
}
