using Microsoft.Toolkit.Extensions;
using PCCleaner.Model;
using PCCleaner.ViewModels.Command;
using PCCleaner.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCleaner.ViewModels
{
    public class DuplicateViewModel:BaseViewModel, IPutFile
    {
        #region Fields

        private DuplicateModel duplicateModel;

        #endregion

        #region Constuctoes

        public DuplicateViewModel()
        {
            this.duplicateModel = new DuplicateModel();

            this.FileOpenFolderButton = new DelegateCommand(FileOpenFolderButton_Click);
            this.DuplicateFileDeleteFolderButton = new DelegateCommand(DuplicateFileDeleteFolderButton_click);
            this.DuplicateScannFolderButton = new DelegateCommand(DuplicateScannFolderButton_Click);
            this.CleanDuplicateFileButton = new DelegateCommand(CleanDuplicateFileButton_Click);
        }

        #endregion

        #region Properties

        public DuplicateModel DuplicateModel 
        { 
            get=>this.DuplicateModel;
            set
            {
                if (!value.Equals(this.duplicateModel))
                {
                    this.duplicateModel = value;
                    OnPropertyChanged(nameof(DuplicateModel));
                }
            } 
        }

        public string HelloText { get; set; }
        public string LetsPutThingsInOrderText { get; set; }
        public string AddFoldersText { get; set; }

        public ICommand FileOpenFolderButton { get; set; }
        public ICommand DuplicateFileDeleteFolderButton { get; set; }
        public ICommand DuplicateScannFolderButton { get; set; }
        public ICommand CleanDuplicateFileButton { get; set; }

        #endregion

        #region puplic Methods

        #endregion

        #region private Methods

        private async void FileOpenFolderButton_Click(object obj)
        {
            bool result = await this.duplicateModel.OpenDuplicateFolderAsync();

            if (result)
            {

            }
        }

        private void DuplicateFileDeleteFolderButton_click(object obj)
        {
            bool result = this.duplicateModel.IsSelectedDuplicateDeleteFolder();

            if (result)
            {

            }
        }

        private async void DuplicateScannFolderButton_Click(object obj)
        {
            await this.duplicateModel.ScannDuplicateFilesAsync().ConfigureAwait(true);
        }

        private async void CleanDuplicateFileButton_Click(object obj)
        {
            await this.duplicateModel.CleanDuplicateFilesAsync().ConfigureAwait(true);
        }

        #endregion
    }
}
