using WarOfTheQuiz.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.Data.Json;
using System.Net.Http;
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace WarOfTheQuiz
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class welcome : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public welcome()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        #region login
        private void login(object sender, TappedRoutedEventArgs e)
        {
            switch ((string)((Button)sender).Name)
            {
                case "fb":
                    AuthenticateFB();
                    break;
                case "tw":
                    AuthenticateTwitter();
                    break;
            }
        }
        private MobileServiceUser user;
        private async System.Threading.Tasks.Task AuthenticateFB()
        {
            while (user == null)
            {
                string message;
                try
                {
                    user = await App.MobileService
                        .LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                    message =
                        string.Format("Te-ai logat cu succes - {0}", user.UserId);


                    var userId = App.MobileService.CurrentUser.UserId;
                    var facebookId = userId.Substring(userId.IndexOf(':') + 1);
                    var client = new HttpClient();
                    var fbUser = await client.GetAsync("https://graph.facebook.com/" + facebookId);
                    var response = await fbUser.Content.ReadAsStringAsync();
                    var jo = JsonObject.Parse(response);
                    var username = jo["name"].GetString();
                    App.username = username.ToString();
                    fb.Visibility = Visibility.Collapsed;
                    tw.Visibility = Visibility.Collapsed;
                    this.Frame.Navigate(typeof(ItemsPage));
                }
                catch (InvalidOperationException)
                {
                    message = "Sugereăm să te loghezi. ";
                }


                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
                break;
            }


        }

        private async System.Threading.Tasks.Task AuthenticateTwitter()
        {
            while (user == null)
            {
                string message;
                try
                {
                    user = await App.MobileService
                        .LoginAsync(MobileServiceAuthenticationProvider.Twitter);
                    message =
                        string.Format("Te-ai logat cu succes - {0}", user.UserId);


                    fb.Visibility = Visibility.Collapsed;
                    tw.Visibility = Visibility.Collapsed;
                    App.username = "Unknown";
                    this.Frame.Navigate(typeof(ItemsPage));
                }
                catch (InvalidOperationException)
                {
                    message = "Sugereăm să te loghezi.";
                }


                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
                break;
            }


        }
        #endregion 
        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void ItemPageGo(object sender, TappedRoutedEventArgs e)
        {
            if (userTxt.Text == "" || userTxt.Text == " ")
            {
                var messageDialog = new MessageDialog("Va rugam să vă introduceți numele");
                messageDialog.Commands.Add(new UICommand("Ok"));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }

            else
            {
                App.username = userTxt.Text;
                this.Frame.Navigate(typeof(ItemsPage));
            }
           
        }
    }
}
