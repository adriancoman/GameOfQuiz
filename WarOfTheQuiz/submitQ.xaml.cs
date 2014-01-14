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
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace WarOfTheQuiz
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class submitQ : Page
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


        public submitQ()
        {
            this.InitializeComponent();
            pageTitle.Text = "Ne bucurăm că vrei să ne ajuți, " + App.username;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

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
        private MobileServiceCollection<newQ, newQ> quest;
        private IMobileServiceTable<newQ> quest_tb = App.MobileService.GetTable<newQ>();

        private void sendInfo(object sender, TappedRoutedEventArgs e)
        {
            string cat;
            if (ist.IsChecked == true) cat = "istorie";
            else
                if (geo.IsChecked == true) cat = "geografie";
                else
                    if (mate.IsChecked == true) cat = "matematica";
                    else
                        if (sp.IsChecked == true) cat = "sport";
                        else
                            if (it.IsChecked == true) cat = "it";
                            else
                                cat = "other";
            try
            {
                newQ que = new newQ
                {
                    PlayerName=App.username,
                    Questin=qTxt.Text,
                    Corect=v1Txt.Text,
                    Var1=v2Txt.Text,
                    Var2=v3Txt.Text,
                    Var3=v4Txt.Text,
                    Categorie=cat
                    


                };

                SendInfo(que);
                MessageDialog md = new MessageDialog("Mulțumim pentru ajutor! Întrebarea va fi adăugată în următorul update!");
                md.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog msg = new MessageDialog("Se pare că serverul are o problemă, revenim!");
                msg.ShowAsync();
            }
        }

        private async void SendInfo(newQ que)
        {
            try
            {
                await quest_tb.InsertAsync(que);
                quest.Add(que);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
