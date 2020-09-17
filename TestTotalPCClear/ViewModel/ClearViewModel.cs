using System.ComponentModel;
using TestTotalPCClear.Model;
using TestTotalPCClear.CommandImplementation;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using TestTotalPCClear.Themes;

namespace TestTotalPCClear.ViewModel
{
    class ClearViewModel : BaseVM
    {
        #region Fields

        private ClearModel clearModel;
        private ResourceLoader resourceLoader;
        private SplitView mySplitView;
        private List<string> languagesList;
        private ThemeManager themeManager;

        private string selectedLanhuage;
        private string scannOrCleanText;

        private int stateOfScanning;
        private int systemCacheCount = 0;
        private int sizeSystemCache;

        private bool isScann = true;
        private bool isShowScannOrClean = true;
        private bool isShowSelectOrDeselectButton = false;

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
            this.ScannOrCleanText = ScannText;
            this.themeManager = new ThemeManager();

            #region Event Subscription

            this.CacheButton = new DelegateCommand(CacheButton_Click);
            this.LargeFilesButton = new DelegateCommand(LargeFilesButton_Click);
            this.DuplicateButton = new DelegateCommand(DuplicateButton_Click);
            this.AutoClearingButton = new DelegateCommand(AutoClearingButton_Click);
            this.DeselectAllButton = new DelegateCommand(DeselectAllButton_Click);
            this.SelectAllButton = new DelegateCommand(SelectAllButton_Click);
            this.ScannAndCleanButton = new DelegateCommand(ScannAndCleanButton_Click);
            this.SettingButton = new DelegateCommand(SettingButton_Click);
            this.MoreButton = new DelegateCommand(MoreButton_Click);
            this.ThemeDarkRadioButton = new DelegateCommand(ThemeDarkRadioButton_Click);
            this.ThemeLightRadioButton = new DelegateCommand(ThemeLightRadioButton_Click);

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
        public string ScannOrCleanText 
        { 
            get=>this.scannOrCleanText;
            set 
            {
                this.scannOrCleanText = value;
                OnPropertyChanged(nameof(ScannOrCleanText));
            }
        }
        public string PurityText { get=>this.resourceLoader.GetString("PurityText"); }
        public string SuccessfulCleanText { get=>this.resourceLoader.GetString("SuccessfulCleanText"); }
        public string ComeToUsMoreOftenText { get=>this.resourceLoader.GetString("ComeToUsMoreOftenText"); }

        #endregion

        #region Command

        public ICommand CacheButton { get; set; }
        public ICommand LargeFilesButton { get; set; }
        public ICommand DuplicateButton { get; set; }
        public ICommand AutoClearingButton { get; set; }
        public ICommand DeselectAllButton { get; set; }
        public ICommand SelectAllButton { get; set; }
        public ICommand ScannAndCleanButton { get; set; }
        public ICommand SettingButton { get; set; }
        public ICommand MoreButton { get; set; }
        public ICommand ThemeDarkRadioButton { get; set; }
        public ICommand ThemeLightRadioButton { get; set; }

        #endregion

        #region bool Propertys

        public bool IsShowScannOrClean 
        { 
            get=>this.isShowScannOrClean;
            set
            {
                this.isShowScannOrClean = value;
                OnPropertyChanged(nameof(IsShowScannOrClean));
            } 
        }

        public bool IsShowSelectOrDeselectButton 
        { 
            get=>this.isShowSelectOrDeselectButton;
            set
            {
                this.isShowSelectOrDeselectButton = value;
                OnPropertyChanged(nameof(IsShowSelectOrDeselectButton));
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

            OnPropertyChanged(nameof(ComeToUsMoreOftenText));
        }

        private async void CacheButton_Click(object obj)
        {
            if (this.StateOfScanning != 1)
                this.StateOfScanning = 1;

            if(this.isScann == false)
            {
                this.isScann = true;
                this.ScannOrCleanText = ScannText;
            }
            if (this.IsShowScannOrClean == false)
                this.IsShowScannOrClean = true;
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

        private void SelectAllButton_Click(object obj)
        {
            clearModel.IsSystemCacheSelect = true;
            clearModel.IsApplicationCacheSelect = true;
            clearModel.IsMailCacheSelect = true;
            clearModel.IsOfficeCacheSelect = true;
            clearModel.IsBrowserCacheSelect = true;
        }

        private void DeselectAllButton_Click(object obj)
        {
            clearModel.IsSystemCacheSelect = false;
            clearModel.IsApplicationCacheSelect = false;
            clearModel.IsMailCacheSelect = false;
            clearModel.IsOfficeCacheSelect = false;
            clearModel.IsBrowserCacheSelect = false;
        }

        private async void ScannAndCleanButton_Click(object obj)
        {
            bool isAnd = false; // For test

            if (this.isScann == true)
            {
                this.IsShowScannOrClean = false;

                this.isScann = false;

                //this.clearModel.IsActiveScannOrClean = true;

                isAnd = await this.clearModel.ScaningSystemCacheAsync();

                this.ScannOrCleanText = CleanText;
            }
            else
            {
                this.isScann = true;

                isAnd = await this.clearModel.DeleteFileAsync();

                this.StateOfScanning = 2;

                this.ScannOrCleanText = ScannText;
            }

            if (isAnd)
            {
                this.IsShowScannOrClean = true;
            }
        }

        private void SettingButton_Click(object obj)
        {
            this.StateOfScanning = 3;
        }

        private void MoreButton_Click(object obj)
        {
            if (IsShowSelectOrDeselectButton == false)
                IsShowSelectOrDeselectButton = true;
            else
                IsShowSelectOrDeselectButton = false;
        }

        private void ThemeLightRadioButton_Click(object obj)
        {
            App.ThemeManager.LoadTheme(ThemeManager.LightThemePath);
        }

        private void ThemeDarkRadioButton_Click(object obj)
        {
            App.ThemeManager.LoadTheme(ThemeManager.DarkThemePath);
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
