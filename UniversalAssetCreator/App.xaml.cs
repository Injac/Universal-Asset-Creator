using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Advertising.WinRT.UI;
using UniversalAssetCreator.Views;

namespace UniversalAssetCreator
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        private static string MyAppId = "b21931ff-0116-49bd-ae02-4e9f8cbb0632";
        private static string MyAppUnitId = "238551";

        public static InterstitialAd MyVideoAd = new InterstitialAd();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();

            DispatcherHelper.Initialize();

            var connProfile = NetworkInformation.GetInternetConnectionProfile();

            if (connProfile != null && connProfile.IsWlanConnectionProfile)
            {


                MyVideoAd.AdReady += MyVideoAd_AdReady;
                MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
                MyVideoAd.Completed += MyVideoAd_Completed;
                MyVideoAd.Cancelled += MyVideoAd_Cancelled;

                var mainViewModel = (((ViewModelLocator) App.Current.Resources["Locator"])).Main;

                if (mainViewModel != null)
                {
                    mainViewModel.Ended += MainViewModel_Ended;
                    
                }
            }
        }

        private void MainViewModel_Ended(object sender, EventArgs e)
        {
            MyVideoAd.RequestAd(AdType.Video, MyAppId, MyAppUnitId);
        }

        private void MyVideoAd_Cancelled(object sender, object e)
        {
            //throw new NotImplementedException();
        }

        private void MyVideoAd_Completed(object sender, object e)
        {
            //throw new NotImplementedException();
        }

        private void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // new NotImplementedException();
        }

        private void MyVideoAd_AdReady(object sender, object e)
        {
            MyVideoAd.Show();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
