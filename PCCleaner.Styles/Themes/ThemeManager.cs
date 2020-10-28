using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace PCCleaner.Styles.Themes
{
    public class BaseThemeManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ThemeManager : BaseThemeManager
    {

        #region Fields

        public const string DarkThemePath = "ms-appx:///Themes/Dark.xaml";
        public const string LightThemePath = "ms-appx:///Themes/Light.xaml";

        private ResourceDictionary currentThemeDictionary;

        #endregion

        #region Сonstructor

        public ThemeManager()
        {
            LoadTheme(DarkThemePath);
        }

        #endregion

        #region Propertys 

        public string CurrentTheme { get; private set; }

        public Brush NavigationViewMenuBackground => currentThemeDictionary[nameof(NavigationViewMenuBackground)] as Brush;
        public Brush NavigationViewDefaultPaneBackground => currentThemeDictionary[nameof(NavigationViewDefaultPaneBackground)] as Brush;
        public Brush NavigationViewExpandedPaneBackground => currentThemeDictionary[nameof(NavigationViewExpandedPaneBackground)] as Brush;
        public Brush ContextAcrylicBrush => currentThemeDictionary[nameof(ContextAcrylicBrush)] as Brush;
        public Brush CheckBlockLightAcrylicBrush => currentThemeDictionary[nameof(CheckBlockLightAcrylicBrush)] as Brush;
        public Brush CheckBlockDarkAcrylicBrush => currentThemeDictionary[nameof(CheckBlockDarkAcrylicBrush)] as Brush;
        public Brush IsCheckBlockDarkAcrylicBrush => currentThemeDictionary[nameof(IsCheckBlockDarkAcrylicBrush)] as Brush;
        public Brush ForegroundBrush => currentThemeDictionary[nameof(ForegroundBrush)] as Brush;
        public Brush ColorMenu => currentThemeDictionary[nameof(ColorMenu)] as Brush;

        #endregion

        #region puplic Methods

        public void LoadTheme(string path)
        {
            currentThemeDictionary = new ResourceDictionary();
            App.LoadComponent(currentThemeDictionary, new Uri(path));
            CurrentTheme = Path.GetFileNameWithoutExtension(path);

            RaisePropertyChanged();
        }

        public async Task LoadThemeFromFile(StorageFile file)
        {
            string xaml = await FileIO.ReadTextAsync(file);
            currentThemeDictionary = XamlReader.Load(xaml) as ResourceDictionary;
            CurrentTheme = Path.GetFileNameWithoutExtension(file.Path);

            RaisePropertyChanged();
        }

        #endregion

        #region private Methods

        private void RaisePropertyChanged()
        {
            OnPropertyChanged(nameof(NavigationViewMenuBackground));
            OnPropertyChanged(nameof(NavigationViewDefaultPaneBackground));
            OnPropertyChanged(nameof(NavigationViewExpandedPaneBackground));
            OnPropertyChanged(nameof(ContextAcrylicBrush));
            OnPropertyChanged(nameof(CheckBlockLightAcrylicBrush));
            OnPropertyChanged(nameof(CheckBlockDarkAcrylicBrush));
            OnPropertyChanged(nameof(IsCheckBlockDarkAcrylicBrush));
            OnPropertyChanged(nameof(ForegroundBrush));
            OnPropertyChanged(nameof(ColorMenu));
            OnPropertyChanged(nameof(CurrentTheme));
        }

        #endregion
    }
}
