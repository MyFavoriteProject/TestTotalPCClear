using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PCCleaner.Core;
using PCCleaner.Model;
using PCCleaner.Model;
using PCCleaner.ViewModels.Command;
using Windows.Storage;

namespace PCCleaner.ViewModels
{
    public class LargeFileViewModel:BaseViewModel
    {
        #region Fields

        private LargeFileProvider largeFileProvider;

        ObservableCollection<StorageFileType<StorageFile>> fileCollection;
        ObservableCollection<StorageFolderType<StorageFolder>> folderCollection;

        private bool showAddFolderPage = true;
        private bool showFolderPage = false;
        private bool showFilesPage = false;
        private bool isShowMore = false;

        #endregion

        #region Constructors

        public LargeFileViewModel()
        {
            this.largeFileProvider = new LargeFileProvider();

            this.ScannCommand = new DelegateCommand(ScannExecute);
            this.FolderDeleteCommand = new DelegateCommand(FolderDeleteExecute);
            this.CleanCommand = new DelegateCommand(CleanExecute);
            this.OpenCommand = new DelegateCommand(OpenExecute);

            this.MoreButtonCommand = new DelegateCommand(MoreButtonExecute);
            this.SelectedAllCommand = new DelegateCommand(SelectedAllExecute);
            this.DeselectedAllCommand = new DelegateCommand(DeselectedAllExecute);
        }

        #endregion

        #region public Propertys

        public LargeFileProvider LargeFileProvider 
        { 
            get=>this.largeFileProvider;
            set
            {
                if (value != this.largeFileProvider)
                {
                    this.largeFileProvider = value;
                    OnPropertyChanged(nameof(LargeFileProvider));
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


        public ObservableCollection<StorageFileType<StorageFile>> FileCollection
        {
            get => this.fileCollection;
            set
            {
                if (this.fileCollection == null || !this.fileCollection.Equals(value))
                {
                    this.fileCollection = value;
                    OnPropertyChanged(nameof(FileCollection));
                }
            }
        }
        public ObservableCollection<StorageFolderType<StorageFolder>> FolderCollection
        {
            get => this.folderCollection;
            set
            {
                if (this.folderCollection == null || !this.folderCollection.Equals(value))
                {
                    this.folderCollection = value;
                    OnPropertyChanged(nameof(FolderCollection));
                }
            }
        }


        public ICommand OpenCommand { get; set; }
        public ICommand ScannCommand { get; set; }
        public ICommand FolderDeleteCommand { get; set; }
        public ICommand CleanCommand { get; set; }

        public ICommand MoreButtonCommand { get; set; }
        public ICommand SelectedAllCommand { get; set; }
        public ICommand DeselectedAllCommand { get; set; }

        #endregion

        #region private Methods

        private async void OpenExecute(object obj)
        {
            base.BusyCount++;

            var result = await this.largeFileProvider.OpenFolderAsync().ConfigureAwait(true);

            if (result.Any())
            {
                this.FolderCollection = result;
                ShowAddFolderPage = false;
                ShowFolderPage = true;
            }

            base.BusyCount--;
        }

        private async void ScannExecute(object obj)
        {
            base.BusyCount++;

            var result = await this.largeFileProvider.ScannFoldersAsync(folderCollection).ConfigureAwait(true);

            if(result.Any())
            {
                this.FileCollection = result;
                this.ShowFolderPage = false;
                this.ShowFilesPage = true;
            }

            base.BusyCount--;
        }

        private void FolderDeleteExecute(object obj)
        {
            base.BusyCount++;

            var result = this.largeFileProvider.IsSelectedDeleteFolder(folderCollection);

            if (result != null&&result.Any())
            {
                FolderCollection = this.largeFileProvider.DeleteFolder(result);
                //await this.deleteDialog.ShowAsync();
            }

            base.BusyCount--;
        }

        private void CleanExecute(object obj)
        {
            base.BusyCount++;

            this.largeFileProvider.CleanFilesAsync(fileCollection);

            base.BusyCount--;
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
            if (ShowFolderPage)
            {
                if (this.FolderCollection != null && this.FolderCollection.Any())
                {
                    foreach (var folder in this.FolderCollection)
                    {
                        folder.IsChecked = true;
                    }
                }
            }
            if (ShowFilesPage)
            {
                if (this.FileCollection != null && this.FileCollection.Any())
                {
                    foreach (var folder in this.FileCollection)
                    {
                        folder.IsChecked = true;
                    }
                }
            }
        }

        private void DeselectedAllExecute(object obj)
        {
            if (ShowFolderPage)
            {
                if (this.FolderCollection != null && this.FolderCollection.Any())
                {
                    foreach (var folder in this.FolderCollection)
                    {
                        folder.IsChecked = false;
                    }
                }
            }
            if (ShowFilesPage)
            {
                if (this.FileCollection != null && this.FileCollection.Any())
                {
                    foreach (var folder in this.FileCollection)
                    {
                        folder.IsChecked = false;
                    }
                }
            }
        }

        #endregion
    }
}
