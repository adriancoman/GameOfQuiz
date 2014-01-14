using WarOfTheQuiz.Common;
using WarOfTheQuiz.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NotificationsExtensions.ToastContent;
using Windows.UI.Notifications;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.Data.Json;
using System.Net.Http;


namespace WarOfTheQuiz
{

    public sealed partial class ItemsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();


        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public ItemsPage()
        {
            this.InitializeComponent();
            pageTitle.Text = "Salutare, " + App.username + ", alege categoria!";
            //MakeToast();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        private void MakeToast()
        {
            Int16 dueTimeInHours = 1;
            DateTime dueTime = DateTime.Now.AddHours(dueTimeInHours);
            string updatedString = "Haide să ne mai jucăm puțin!";
            int id = 0;

            SaveToast(updatedString, dueTime, id);
        }

        private void SaveToast(string updatedString, DateTime dueTime, int id)
        {
            IToastText02 toastContent = ToastContentFactory.CreateToastText02();
            toastContent.TextHeading.Text = updatedString;
            toastContent.TextBodyWrap.Text = "Notificare de la World Of Quizz!";

            ScheduledToastNotification toast;
            toast = new ScheduledToastNotification(toastContent.GetXml(), dueTime);
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
        }

             private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
                 
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Items"] = sampleDataGroups;
                 
        }

       
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
           
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(NewGame), groupId);
        }

        #region NavigationHelper registration

     

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

     

        private void sendQ(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(submitQ));
        }

        private void reportProblem(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(reportProblem));
        }
    }
}
