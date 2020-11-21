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
    public class LargeFileViewModel:BaseViewModel
    {
        #region Fields

        private LargeFileModel largeFileModel;

        private bool showAddFolderPage = true;
        private bool showFolderPage = false;
        private bool showFilesPage = false;
        private bool isShowMore = false;

        #endregion

        #region Constructors

        public LargeFileViewModel()
        {
            this.largeFileModel = new LargeFileModel();

            this.LargeFileScannFolderButton = new DelegateCommand(LargeFileScannFolderButton_Click);
            this.LargeFileDeleteFolderButton = new DelegateCommand(LargeFileDeleteFolderButtonButton_Click);
            this.CleanLargeFileButton = new DelegateCommand(CleanLargeFileButton_Click);
            this.FileOpenFolderButton = new DelegateCommand(FileOpenFolderButton_Click);

            this.MoreButtonCommand = new DelegateCommand(MoreButtonExecute);
            this.SelectedAllCommand = new DelegateCommand(SelectedAllExecute);
            this.DeselectedAllCommand = new DelegateCommand(DeselectedAllExecute);
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

        public bool IsShowMore
        {
            get => this.isShowMore;
            set
            {
                if (value != this.isShowMore)
                {
                    this.isShowMore = value;
                    OnPropertyChanged(nameof(IsShowMore));
                }
            }
        }

        public bool ShowAddFolderPage
        {
            get => this.showAddFolderPage;
            set
            {
                if (value != this.showAddFolderPage)
                {
                    this.showAddFolderPage = value;
                    OnPropertyChanged(nameof(ShowAddFolderPage));
                }
            }
        }
        public bool ShowFolderPage
        {
            get => this.showFolderPage;
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

        public ICommand FileOpenFolderButton { get; set; }
        public ICommand LargeFileScannFolderButton { get; set; }
        public ICommand LargeFileDeleteFolderButton { get; set; }
        public ICommand CleanLargeFileButton { get; set; }

        public ICommand MoreButtonCommand { get; set; }
        public ICommand SelectedAllCommand { get; set; }
        public ICommand DeselectedAllCommand { get; set; }

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
            if (largeFileModel.FolderCollection != null && largeFileModel.FolderCollection.Any())
            {
                foreach (var folder in largeFileModel.FolderCollection)
                {
                    folder.IsChecked = true;
                }
            }
        }

        private void DeselectedAllExecute(object obj)
        {
            if (largeFileModel.FolderCollection != null && largeFileModel.FolderCollection.Any())
            {
                foreach (var folder in largeFileModel.FolderCollection)
                {
                    folder.IsChecked = false;
                }
            }
        }

        #endregion
    }
}
