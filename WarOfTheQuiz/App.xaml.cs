using WarOfTheQuiz.Common;

using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using Windows.UI.Popups;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.ApplicationSettings;

// The Split App template is documented at http://go.microsoft.com/fwlink/?LinkId=234228

namespace WarOfTheQuiz
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static int appColor=0;
        public static MobileServiceClient MobileService = new MobileServiceClient(
    "https://smartapps.azure-mobile.net/",
    "fishFingersAndCustard-getYourOwnKey"
);
        public static MobileServiceClient mobileService
        {
            get { return MobileService; }
        }

        public static string username="Anonim";

        private async void InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();


            var hub = new NotificationHub("warofthequiz", "Endpoint=sb://warofthequiz-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=PuXus8cJ+6RNsOyxWhpcbXrRWi8EmtACEXfz/pvFI5Q=");
            var result = await hub.RegisterNativeAsync(channel.Uri);

        }



        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitNotificationsAsync();
            SettingsPane.GetForCurrentView().CommandsRequested += DisplaySettingsFlyout;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            

            if (rootFrame == null)
            {
                
                rootFrame = new Frame();                           
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                    }
                }

                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(welcome), e.Arguments);
            }
            Window.Current.Activate();
        }

        private void DisplaySettingsFlyout(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("Credits", "Credits", (x) =>
            {
                Users flyout = new Users();
                flyout.Title = "Aplicație realizată de";
                flyout.Show();
            });
            args.Request.ApplicationCommands.Add(cmd);

            SettingsCommand cmd2 = new SettingsCommand("Privacy Settings", "Privacy Settings", (x) =>
            {
                Permission flyout = new Permission();
                flyout.Title = "Privacy Settings";
                flyout.Show();
            });
            args.Request.ApplicationCommands.Add(cmd2);

            //SettingsCommand cmd3 = new SettingsCommand("Setări", "Setări", (x) =>
            //{
            //    SettingsFlyout1 set = new SettingsFlyout1();
            //    set.Title = "Setări background";
            //    set.Show();
            //});
            //args.Request.ApplicationCommands.Add(cmd3);

        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
