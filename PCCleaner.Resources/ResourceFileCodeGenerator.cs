
namespace AppleMusicPlayer.Resources
{
	using PCCleaner.Resources.Core;
    using System;
	using System.ComponentModel;
	using System.Globalization;
    
	

	/// <summary>
    /// Provides access to resources from CommonResources.resw file.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "Yet it will be unstatic.")]
	public class CommonResources
	{
		/// <summary>
        /// Contains logic for accessing contsnt of resource file.
        /// </summary>
		private static readonly ResourceProvider resourceProvider = new ResourceProvider("AppleMusicPlayer.Resources/CommonResources");

		/// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        public static CultureInfo Culture
        {
            get
            {
                return resourceProvider.OverridedCultureInfo;
            }
            set
            {
                resourceProvider.OverridedCultureInfo = value;
            }
        }



		/// <summary>
        /// Gets a localized string similar to Acent Color
        /// </summary>
		public static string AcentColorText => resourceProvider.GetString("AcentColorText");

		/// <summary>
        /// Gets a localized string similar to Add folders
        /// </summary>
		public static string AddFoldersText => resourceProvider.GetString("AddFoldersText");

		/// <summary>
        /// Gets a localized string similar to Application cache
        /// </summary>
		public static string ApplicationCacheText => resourceProvider.GetString("ApplicationCacheText");

		/// <summary>
        /// Gets a localized string similar to Auto cleaning at device startup
        /// </summary>
		public static string AutoCleaningAtDeviceStartupText => resourceProvider.GetString("AutoCleaningAtDeviceStartupText");

		/// <summary>
        /// Gets a localized string similar to Auto Clearing
        /// </summary>
		public static string AutoClearingText => resourceProvider.GetString("AutoClearingText");

		/// <summary>
        /// Gets a localized string similar to Auto Start
        /// </summary>
		public static string AutoStart => resourceProvider.GetString("AutoStart");

		/// <summary>
        /// Gets a localized string similar to Browser cache
        /// </summary>
		public static string BrowserCacheText => resourceProvider.GetString("BrowserCacheText");

		/// <summary>
        /// Gets a localized string similar to Cache
        /// </summary>
		public static string CacheText => resourceProvider.GetString("CacheText");

		/// <summary>
        /// Gets a localized string similar to Clean
        /// </summary>
		public static string CleanText => resourceProvider.GetString("CleanText");

		/// <summary>
        /// Gets a localized string similar to Come to us more often ! See you !
        /// </summary>
		public static string ComeToUsMoreOftenText => resourceProvider.GetString("ComeToUsMoreOftenText");

		/// <summary>
        /// Gets a localized string similar to Dark
        /// </summary>
		public static string DarkText => resourceProvider.GetString("DarkText");

		/// <summary>
        /// Gets a localized string similar to Delete cache
        /// </summary>
		public static string DeleteCacheText => resourceProvider.GetString("DeleteCacheText");

		/// <summary>
        /// Gets a localized string similar to Deselect all
        /// </summary>
		public static string DeselectAllText => resourceProvider.GetString("DeselectAllText");

		/// <summary>
        /// Gets a localized string similar to Design theme
        /// </summary>
		public static string DesignThemeText => resourceProvider.GetString("DesignThemeText");

		/// <summary>
        /// Gets a localized string similar to Duplicate file
        /// </summary>
		public static string DuplicateFileText => resourceProvider.GetString("DuplicateFileText");

		/// <summary>
        /// Gets a localized string similar to Duplicate
        /// </summary>
		public static string DuplicateText => resourceProvider.GetString("DuplicateText");

		/// <summary>
        /// Gets a localized string similar to Hello !
        /// </summary>
		public static string HelloText => resourceProvider.GetString("HelloText");

		/// <summary>
        /// Gets a localized string similar to Large Files
        /// </summary>
		public static string LargeFilesText => resourceProvider.GetString("LargeFilesText");

		/// <summary>
        /// Gets a localized string similar to Large file
        /// </summary>
		public static string LargeFileText => resourceProvider.GetString("LargeFileText");

		/// <summary>
        /// Gets a localized string similar to Lenguage
        /// </summary>
		public static string LenguageText => resourceProvider.GetString("LenguageText");

		/// <summary>
        /// Gets a localized string similar to Let's put things in order?
        /// </summary>
		public static string LetsPutThingsInOrderText => resourceProvider.GetString("LetsPutThingsInOrderText");

		/// <summary>
        /// Gets a localized string similar to Light
        /// </summary>
		public static string LightText => resourceProvider.GetString("LightText");

		/// <summary>
        /// Gets a localized string similar to Mail cache
        /// </summary>
		public static string MailCacheText => resourceProvider.GetString("MailCacheText");

		/// <summary>
        /// Gets a localized string similar to Office cache
        /// </summary>
		public static string OfficeCacheText => resourceProvider.GetString("OfficeCacheText");

		/// <summary>
        /// Gets a localized string similar to Off
        /// </summary>
		public static string OffText => resourceProvider.GetString("OffText");

		/// <summary>
        /// Gets a localized string similar to On
        /// </summary>
		public static string OnText => resourceProvider.GetString("OnText");

		/// <summary>
        /// Gets a localized string similar to Property
        /// </summary>
		public static string PropertyText => resourceProvider.GetString("PropertyText");

		/// <summary>
        /// Gets a localized string similar to Purity
        /// </summary>
		public static string PurityText => resourceProvider.GetString("PurityText");

		/// <summary>
        /// Gets a localized string similar to Scan files
        /// </summary>
		public static string ScanFilesText => resourceProvider.GetString("ScanFilesText");

		/// <summary>
        /// Gets a localized string similar to Scann
        /// </summary>
		public static string ScannText => resourceProvider.GetString("ScannText");

		/// <summary>
        /// Gets a localized string similar to Select all
        /// </summary>
		public static string SelectAllText => resourceProvider.GetString("SelectAllText");

		/// <summary>
        /// Gets a localized string similar to Setting
        /// </summary>
		public static string SettingText => resourceProvider.GetString("SettingText");

		/// <summary>
        /// Gets a localized string similar to Cleaning Was Successfully !
        /// </summary>
		public static string SuccessfulCleanText => resourceProvider.GetString("SuccessfulCleanText");

		/// <summary>
        /// Gets a localized string similar to System cache
        /// </summary>
		public static string SystemCacheText => resourceProvider.GetString("SystemCacheText");

		/// <summary>
        /// Gets a localized string similar to User system setting
        /// </summary>
		public static string UserSystemSettingText => resourceProvider.GetString("UserSystemSettingText");

	}

	public sealed class LocalizedStrings : INotifyPropertyChanged
    {
        public LocalizedStrings()
        {
            this.RefreshLanguageSettings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static event EventHandler<EventArgs> LanguageChanged;

        public static event EventHandler LocalizedStringsRefreshedEvent;

        public void OnLocalizedStringsRefreshedEvent()
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler handler = LocalizedStringsRefreshedEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, new EventArgs());
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

		
		/// <summary>
		/// Gets resources that are common across application.
		/// </summary>
		public CommonResources CommonResources { get; private set; }
	

        public void RefreshLanguageSettings()
        {
			
			this.CommonResources = new CommonResources();
			this.RaisePropertyChanged("CommonResources");
		
			LanguageChanged?.Invoke(this, null);
		}
	}
}