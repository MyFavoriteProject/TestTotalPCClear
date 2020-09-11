using System.ComponentModel;
using TestTotalPCClear.Model;
using TestTotalPCClear.CommandImplementation;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Storage;

namespace TestTotalPCClear.ViewModel
{
    class ClearViewModel : BaseVM
    {
        #region Fields

        private ClearModel clearModel;
        private ResourceLoader resourceLoader;
        private List<string> languagesList;
        private string selectedLanhuage;
        private SplitView mySplitView;
        private List<int> fileSizeList;
        private int stateOfScanning;
        private bool isSystemCacheSelect = false,
            isApplicationCacheSelect = false,
            isMailCacheSelect = false,
            isOfficeCacheSelect = false,
            isBrowserCacheSelect = false;
        private int systemCacheCount = 0;
        private int sizeSystemCache;

        #endregion

        #region Constructors

        public ClearViewModel()
        {
            this.clearModel = new ClearModel();
            this.resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            this.languagesList = new List<string>() { "English", "Русский"};
            ResourceContext.SetGlobalQualifierValue("Language", "en-US");
            this.StateOfScanning = 1;
            this.StateOfScanning = 0;

            //this.HamburgerButton = new DelegateCommand(HamburgerButton_Click);
            //this.DeleteCacheButtom = new DelegateCommand(DeleteCacheButtom_Click);
            //this.ScanFilesButtom = new DelegateCommand(ScanFilesButtom_Click);

            #region Event Subscription

            this.CacheButton = new DelegateCommand(CacheButton_Click);
            this.LargeFilesButton = new DelegateCommand(LargeFilesButton_Click);
            this.DuplicateButton = new DelegateCommand(DuplicateButton_Click);
            this.AutoClearingButton = new DelegateCommand(AutoClearingButton_Click);
            this.ScannButton = new DelegateCommand(ScannButton_Click);
            this.CleanButton = new DelegateCommand(CleanButton_Click);
            this.DeselectAllButton = new DelegateCommand(DeselectAllButton_Click);
            this.SelectAllTextButton = new DelegateCommand(SelectAllTextButton_Click);

            #endregion
        }

        #endregion

        #region public Propertys

        #region Not Group Propertys

        public SplitView MySplitView
        {
            get => this.mySplitView;
            set
            {
                this.mySplitView = value;
                OnPropertyChanged(nameof(this.MySplitView));
            }
        }

        public ClearModel ClearModel
        {
            get => this.clearModel;
            set
            {
                this.clearModel = value;
                OnPropertyChanged(nameof(this.ClearModel));
            }
        }

        public List<string> LanguagesList
        {
            get => this.languagesList;
            set
            {
                this.languagesList = value;
                OnPropertyChanged(nameof(this.LanguagesList));
            }
        }

        public int SizeSystemCache 
        { 
            get=> this.sizeSystemCache;
            set
            {
                this.sizeSystemCache = value;
                OnPropertyChanged(nameof(SizeSystemCache));
            } 
        }

        public int StateOfScanning
        {
            get => this.stateOfScanning;
            set
            {
                this.stateOfScanning = value;
                OnPropertyChanged(nameof(StateOfScanning));
            }
        }

        public int SystemCacheCount 
        { 
            get=>this.systemCacheCount;
            set
            {
                this.systemCacheCount = value;
                OnPropertyChanged(nameof(SystemCacheCount));
            } 
        }

        #endregion

        #region TextBinding

        public string SelectedLanguage
        {
            get => this.selectedLanhuage;
            set
            {
                if (SelectedLanguage == null)
                {
                    this.selectedLanhuage = value;

                    ChangeLanguage();

                    OnPropertyChanged(nameof(SelectedLanguage));
                }

                if (this.selectedLanhuage != value)
                {
                    this.selectedLanhuage = value;

                    ChangeLanguage();

                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }


        public string DeleteCacheText
        {
            get => this.resourceLoader.GetString("DeleteCacheText");
        }

        public string CacheText
        {
            get => this.resourceLoader.GetString("CacheText");
        }

        public string LargeFilesText
        {
            get => this.resourceLoader.GetString("LargeFilesText");
        }

        public string DuplicateText
        {
            get => this.resourceLoader.GetString("DuplicateText");
        }

        public string AutoClearingText
        {
            get => this.resourceLoader.GetString("AutoClearingText");
        }

        public string PropertyText
        {
            get => this.resourceLoader.GetString("PropertyText");
        }

        public string SelectAllText { get => this.resourceLoader.GetString("SelectAllText"); }

        public string DeselectAllText { get=>this.resourceLoader.GetString("DeselectAllText"); }

        public string SystemCacheText { get=>this.resourceLoader.GetString("SystemCacheText"); }
        public string ApplicationCacheText { get=>this.resourceLoader.GetString("ApplicationCacheText"); }
        public string MailCacheText { get=>this.resourceLoader.GetString("MailCacheText"); }
        public string OfficeCacheText { get => this.resourceLoader.GetString("OfficeCacheText"); }
        public string BrowserCacheText { get=>this.resourceLoader.GetString("BrowserCacheText"); }
        public string ScannText { get=>this.resourceLoader.GetString("ScannText"); }
        public string CleanText { get=>this.resourceLoader.GetString("CleanText"); }

        #endregion

        #region Command

        //public ICommand HamburgerButton { get; set; }
        //public ICommand DeleteCacheButtom { get; set; }
        //public ICommand ScanFilesButtom { get; set; }
        public ICommand CacheButton { get; set; }
        public ICommand LargeFilesButton { get; set; }
        public ICommand DuplicateButton { get; set; }
        public ICommand AutoClearingButton { get; set; }
        public ICommand ScannButton { get; set; }
        public ICommand CleanButton { get; set; }
        public ICommand DeselectAllButton { get; set; }
        public ICommand SelectAllTextButton { get; set; }

        #endregion

        #region ChechBoxes

        public bool IsSystemCacheSelect 
        { 
            get=>this.isSystemCacheSelect;
            set
            {
                this.isSystemCacheSelect = value;
                OnPropertyChanged(nameof(IsSystemCacheSelect));
            }
        }

        public bool IsApplicationCacheSelect 
        { 
            get=>this.isApplicationCacheSelect;
            set
            {
                this.isApplicationCacheSelect = value;
                OnPropertyChanged(nameof(IsApplicationCacheSelect));
            } 
        }
        public bool IsMailCacheSelect 
        {
            get => this.isMailCacheSelect;
            set
            {
                this.isMailCacheSelect = value;
                OnPropertyChanged(nameof(IsMailCacheSelect));
            }
        }
        public bool IsOfficeCacheSelect 
        {
            get => this.isOfficeCacheSelect;
            set
            {
                this.isOfficeCacheSelect = value;
                OnPropertyChanged(nameof(IsOfficeCacheSelect));
            }
        }
        public bool IsBrowserCacheSelect 
        {
            get => this.isBrowserCacheSelect;
            set
            {
                this.isBrowserCacheSelect = value;
                OnPropertyChanged(nameof(IsBrowserCacheSelect));
            }
        }

        #endregion

        #endregion

        #region public Methods

        #endregion

        #region privet Methods

        private void ChangeLanguage()
        {
            if (SelectedLanguage == null)
                return;

            if (this.languagesList[0].Equals(SelectedLanguage))
            {
                ResourceContext.SetGlobalQualifierValue("Language", "en-US");
            }

            if (this.languagesList[1].Equals(SelectedLanguage))
            {
                ResourceContext.SetGlobalQualifierValue("Language", "ru-RU");
            }

        }

        private async void CacheButton_Click(object obj)
        {
            this.StateOfScanning = 1;
        }

        private void LargeFilesButton_Click(object obj)
        {

        }

        private void DuplicateButton_Click(object obj)
        {

        }

        private void AutoClearingButton_Click(object obj)
        {

        }

        private async void ScannButton_Click(object obj)
        {
            this.StateOfScanning = 2;

            this.sizeSystemCache = await this.clearModel.ScaningSystemCacheAsync();

            OnPropertyChanged(nameof(this.SizeSystemCache));

            this.StateOfScanning = 3;
        }

        private void CleanButton_Click(object obj)
        {
            this.StateOfScanning = 4;

            this.clearModel.DeleteFileAsync();

            this.StateOfScanning = 5;
        }

        private void SelectAllTextButton_Click(object obj)
        {
            this.IsSystemCacheSelect = true;
            this.IsApplicationCacheSelect = true;
            this.IsMailCacheSelect = true;
            this.IsOfficeCacheSelect = true;
            this.IsBrowserCacheSelect = true;

            OnPropertyChanged(nameof(IsSystemCacheSelect));
            OnPropertyChanged(nameof(IsApplicationCacheSelect));
            OnPropertyChanged(nameof(IsMailCacheSelect));
            OnPropertyChanged(nameof(IsOfficeCacheSelect));
            OnPropertyChanged(nameof(IsBrowserCacheSelect));
        }

        private void DeselectAllButton_Click(object obj)
        {
            this.IsSystemCacheSelect = false;
            this.IsApplicationCacheSelect = false;
            this.IsMailCacheSelect = false;
            this.IsOfficeCacheSelect = false;
            this.IsBrowserCacheSelect = false;

            OnPropertyChanged(nameof(IsSystemCacheSelect));
            OnPropertyChanged(nameof(IsApplicationCacheSelect));
            OnPropertyChanged(nameof(IsMailCacheSelect));
            OnPropertyChanged(nameof(IsOfficeCacheSelect));
            OnPropertyChanged(nameof(IsBrowserCacheSelect));
        }

        #endregion
    }

    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
