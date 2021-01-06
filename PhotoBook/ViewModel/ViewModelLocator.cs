using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using PhotoBook.Services;
using PhotoBook.ViewModel.Pages;
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

            SimpleIoc.Default.Register<IDialogService>(() => new WPFDialogService());

            // Used to pass ViewModelLocator to view models so that they can navigate
            // to other VMs.
            SimpleIoc.Default.Register(() => this);
        }

        public void DestroyViewModel<T>() where T : ViewModelBase
        {
            SimpleIoc.Default.Unregister<T>();
            SimpleIoc.Default.Register<T>();
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

        public FrontCoverViewModel FrontCover => (FrontCoverViewModel)
            ServiceLocator.Current.GetInstance<EditorViewModel>().BookViewModel;
        public BackCoverViewModel BackCover => (BackCoverViewModel)
            ServiceLocator.Current.GetInstance<EditorViewModel>().BookViewModel;
        public PagesViewModel Pages => (PagesViewModel)
            ServiceLocator.Current.GetInstance<EditorViewModel>().BookViewModel;

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
