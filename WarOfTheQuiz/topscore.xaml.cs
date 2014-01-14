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
using System.Diagnostics;
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace WarOfTheQuiz
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class topscore : Page
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


        public topscore()
        {
            this.InitializeComponent(); 
            
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }
        private async void RefreshLeaderboard(string resultsId)
        {
            var sw = new Stopwatch();
            sw.Start();

            var leaderboardUpdated = false;
            while (!leaderboardUpdated && sw.ElapsedMilliseconds < 5000)
            {
                var aux = await App.MobileService.GetTable<Result>().Where(r => r.Id == resultsId.ToString()).ToEnumerableAsync();

                var resultsItem = aux.Single();
                leaderboardUpdated = resultsItem.LeaderboardUpdated;
            }

            sw.Stop();

            if (leaderboardUpdated)
            {
                var leaderboardItems = await App.MobileService.GetTable<Leaderboard>().ToEnumerableAsync();
                leaderboardItems = leaderboardItems.OrderBy(item => item.Position).Take(5);

                var model = new LeaderboardModel();
                foreach (var item in leaderboardItems)
                {
                    model.Items.Add(new leaderboardItemModel
                    {
                        Player = item.PlayerName,
                        Position = item.Position,
                        Score = item.Score
                    });
                }

                this.DataContext = model;
                progressRing.Visibility = Visibility.Collapsed;
                LeaderboardGridView.Visibility = Visibility.Visible;
                //this.LoadingLeaderboardLegend.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //this.LoadingLeaderboardProgressRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //this.LeaderboardGridView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                //this.LoadingLeaderboardLegend.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //this.LoadingLeaderboardProgressRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                var leaderboardItems = await App.MobileService.GetTable<Leaderboard>().ToEnumerableAsync();
                leaderboardItems = leaderboardItems.OrderBy(item => item.Position).Take(5);

                var model = new LeaderboardModel();
                foreach (var item in leaderboardItems)
                {
                    model.Items.Add(new leaderboardItemModel
                    {
                        Player = item.PlayerName,
                        Position = item.Position,
                        Score = item.Score
                    });
                }

                this.DataContext = model;
            }
        }
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }


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
            //navigationHelper.OnNavigatedTo(e);
            string a = e.Parameter as string;
            //pageTitle.Text = a;
            //var resultsEntity = new Result { PlayerName = App.username, Scor=666 };
            RefreshLeaderboard(a);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void BackTap(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemsPage));
        }
    }
}
