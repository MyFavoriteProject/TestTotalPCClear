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
using Windows.UI.Xaml;

namespace PCCleaner.ViewModels
{
    public class DuplicateViewModel:BaseViewModel, IPutFile
    {
        #region Fields

        private DuplicateModel duplicateModel;
        private bool showAddFolderPage = true;
        private bool showFolderPage = false;
        private bool showFilesPage = false;
        private bool isShowMore = false;

        #endregion

        #region Constuctoes

        public DuplicateViewModel()
        {
            this.duplicateModel = new DuplicateModel();

            this.FileOpenFolderButton = new DelegateCommand(FileOpenFolderButton_Click);
            this.DuplicateFileDeleteFolderButton = new DelegateCommand(DuplicateFileDeleteFolderButton_click);
            this.DuplicateScannFolderButton = new DelegateCommand(DuplicateScannFolderButton_Click);
            this.CleanDuplicateFileButton = new DelegateCommand(CleanDuplicateFileButton_Click);
            this.MoreButtonCommand = new DelegateCommand(MoreButtonExecute);
            this.SelectedAllCommand = new DelegateCommand(SelectedAllExecute);
            this.DeselectedAllCommand = new DelegateCommand(DeselectedAllExecute);
        }

        #endregion

        #region Properties

        public DuplicateModel DuplicateModel 
        { 
            get=>this.duplicateModel;
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

        public bool ShowAddFolderPage 
        { 
            get => this.showAddFolderPage;
            set
            {
                if(value!= this.showAddFolderPage)
                {
                    this.showAddFolderPage = value;
                    OnPropertyChanged(nameof(ShowAddFolderPage));
                }
            } 
        }
        public bool ShowFolderPage 
        { 
            get=>this.showFolderPage;
            set
            {
                if (value != this.showFolderPage)
                {
                    this.showFolderPage = value;
                    OnPropertyChanged(nameof(ShowFolderPage));
                }
            } 
        }

        public bool ShowFilesPage
        {
            get => this.showFilesPage;
            set
            {
                if (value != this.showFilesPage)
                {
                    this.showFilesPage = value;
                    OnPropertyChanged(nameof(ShowFilesPage));
                }
            }
        }

        public bool IsShowMore 
        { 
            get=>this.isShowMore;
            set
            {
                if (value != this.isShowMore)
                {
                    this.isShowMore = value;
                    OnPropertyChanged(nameof(IsShowMore));
                }
            } 
        }

        public ICommand FileOpenFolderButton { get; set; }
        public ICommand DuplicateFileDeleteFolderButton { get; set; }
        public ICommand DuplicateScannFolderButton { get; set; }
        public ICommand CleanDuplicateFileButton { get; set; }
        public ICommand MoreButtonCommand { get; set; }
        public ICommand SelectedAllCommand { get; set; }
        public ICommand DeselectedAllCommand { get; set; }

        #endregion

        #region puplic Methods

        #endregion

        #region private Methods

        private async void FileOpenFolderButton_Click(object obj)
        {
            bool result = await this.duplicateModel.OpenDuplicateFolderAsync();

            if (result)
            {
                ShowAddFolderPage = false;
                ShowFolderPage = true;
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
            var result = await this.duplicateModel.ScannDuplicateFilesAsync().ConfigureAwait(true);

            if (result)
            {
                this.ShowFolderPage = false;
                this.ShowFilesPage = true;
            }
        }

        private async void CleanDuplicateFileButton_Click(object obj)
        {
            await this.duplicateModel.CleanDuplicateFilesAsync().ConfigureAwait(true);
        }

        private void MoreButtonExecute(object obj)
        {
            if (IsShowMore)
            {
                IsShowMore = false;
            }
            else
            {
                IsShowMore = true;
            }
        }

        private void SelectedAllExecute(object obj)
        {
            if(duplicateModel.DuplicateFolderCollection!=null&& duplicateModel.DuplicateFolderCollection.Any())
            {
                foreach(var folder in duplicateModel.DuplicateFolderCollection)
                {
                    folder.IsChecked = true;
                }
            }
        }

        private void DeselectedAllExecute(object obj)
        {
            if (duplicateModel.DuplicateFolderCollection != null && duplicateModel.DuplicateFolderCollection.Any())
            {
                foreach (var folder in duplicateModel.DuplicateFolderCollection)
                {
                    folder.IsChecked = false;
                }
            }
        }

        #endregion
    }
}
