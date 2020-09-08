using System.ComponentModel;
using TestTotalPCClear.Model;
using TestTotalPCClear.CommandImplementation;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System;

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
        private List<string> foundFiles;
        private bool canExecute;
        private int stateOfScanning;

        #endregion

        #region Constructors

        public ClearViewModel()
        {
            this.clearModel = new ClearModel();
            this.resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            this.languagesList = new List<string>() { "English", "Русский"};
            ResourceContext.SetGlobalQualifierValue("Language", "en-US");
            this.canExecute = false;
            this.StateOfScanning = 1;
            this.StateOfScanning = 0;

            this.HamburgerButton = new DelegateCommand(HamburgerButton_Click);
            this.DeleteCacheButtom = new DelegateCommand(DeleteCacheButtom_Click);
            this.ScanFilesButtom = new DelegateCommand(ScanFilesButtom_Click);

            this.CacheButton = new DelegateCommand(CacheButton_Click);
            this.LargeFilesButton = new DelegateCommand(LargeFilesButton_Click);
            this.DuplicateButton = new DelegateCommand(DuplicateButton_Click);
            this.AutoClearingButton = new DelegateCommand(AutoClearingButton_Click);
            this.ButtonOne = new DelegateCommand(ButtonOne_Click);
            this.ButtonTwo = new DelegateCommand(ButtonTwo_Click);
            this.ButtonTree = new DelegateCommand(ButtonTree_Click);
        }

        #endregion

        #region public Propertys

        public SplitView MySplitView 
        { 
            get=>this.mySplitView;
            set
            {
                this.mySplitView = value;
                OnPropertyChanged(nameof(this.MySplitView));
            } 
        }

        public ClearModel ClearModel 
        { 
            get=>this.clearModel;
            set
            {
                this.clearModel = value;
                OnPropertyChanged(nameof(this.ClearModel));
            } 
        }

        public List<string> LanguagesList 
        { 
            get=>this.languagesList;
            set
            {
                this.languagesList = value;
                OnPropertyChanged(nameof(this.LanguagesList));
            } 
        }

        
        public List<string> FoundFiles 
        { 
            get => this.foundFiles;
            set
            {
                this.foundFiles = value;
                OnPropertyChanged(nameof(this.FoundFiles));
            }
        }

        public bool IsButtonEnabled
        { 
            get=>this.canExecute;
        }

        public int StateOfScanning 
        { 
            get=>this.stateOfScanning;
            set
            {
                this.stateOfScanning = value;
                OnPropertyChanged(nameof(StateOfScanning));
            } 
        }

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

        #endregion

        #region Command

        public ICommand HamburgerButton { get; set; }
        public ICommand DeleteCacheButtom { get; set; }
        public ICommand ScanFilesButtom { get; set; }
        public ICommand CacheButton { get; set; }
        public ICommand LargeFilesButton { get; set; }
        public ICommand DuplicateButton { get; set; }
        public ICommand AutoClearingButton { get; set; }
        public ICommand ButtonOne { get; set; }
        public ICommand ButtonTwo { get; set; }
        public ICommand ButtonTree { get; set; }

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

        private void DeleteCacheButtom_Click(object obj)
        {
            this.clearModel.DeleteFileAsync();

            this.canExecute = false;
            OnPropertyChanged(nameof(IsButtonEnabled));
        }

        private void HamburgerButton_Click(object obj)
        {
            this.mySplitView.IsPaneOpen = !this.mySplitView.IsPaneOpen;
            OnPropertyChanged(nameof(this.MySplitView));
        }

        private async void ScanFilesButtom_Click(object obj)
        {
            this.foundFiles = await this.clearModel.SearchCacheFilesAsync();
            
            OnPropertyChanged(nameof(this.FoundFiles));

            if (this.foundFiles.Count > 0)
            {
                this.canExecute = true;
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        private async void CacheButton_Click(object obj)
        {
            this.StateOfScanning = 1;
        }

        private void ButtonTree_Click(object obj)
        {
            this.StateOfScanning = 0;
        }

        private void ButtonTwo_Click(object obj)
        {
            this.StateOfScanning = 3;
        }

        private void ButtonOne_Click(object obj)
        {
            this.StateOfScanning = 2;
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
