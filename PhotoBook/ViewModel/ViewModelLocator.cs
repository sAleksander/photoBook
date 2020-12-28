using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using PhotoBook.ViewModel.Settings;

namespace PhotoBook.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<EditorViewModel>();

            SimpleIoc.Default.Register<FrontCoverSettingsViewModel>();
            SimpleIoc.Default.Register<BackCoverSettingsViewModel>();
            SimpleIoc.Default.Register<PagesSettingsViewModel>();

            SimpleIoc.Default.Register<BookViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public EditorViewModel Editor => ServiceLocator.Current.GetInstance<EditorViewModel>();

        public FrontCoverSettingsViewModel FrontCoverSettings => (FrontCoverSettingsViewModel)
            ServiceLocator.Current.GetInstance<EditorViewModel>().SettingsViewModel;
        public BackCoverSettingsViewModel BackCoverSettings => (BackCoverSettingsViewModel)
            ServiceLocator.Current.GetInstance<EditorViewModel>().SettingsViewModel;
        public PagesSettingsViewModel PagesSettings => (PagesSettingsViewModel)
            ServiceLocator.Current.GetInstance<EditorViewModel>().SettingsViewModel;

        public BookViewModel Book => ServiceLocator.Current.GetInstance<EditorViewModel>().BookViewModel;

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
