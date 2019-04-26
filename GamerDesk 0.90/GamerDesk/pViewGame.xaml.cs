using GamerDesk.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GamerDesk
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class pViewGame : Page
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


        public pViewGame()
        {
            this.InitializeComponent();
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
        cGame Game;
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Game = e.NavigationParameter as cGame;
            pageTitle.Text = Game.Name;
            
            imgCover.Source = new BitmapImage(new Uri(Game.Image));
            if (Game.ReleaseDate != "")
            {
                txtDate.Text = "Release Date: " + Game.ReleaseDate;
            }
            if (Game.Description != "")
            {
                txtDesc.Text = "Description: " + Game.Description;
            }

            if (Game.Path == "")
            {
                txtLastPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                txtTotalPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                txtPath.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnPlay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
            else
            {
                txtLastPlay.Text = "Last Play Time:   " + Game.lastTimeS;
                txtTotalPlay.Text = "Total Play Time:  " + Game.totalTimeS;
                txtPath.Text = "Game Path Location: " + Game.Path;
                txtMsg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
           
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

        private async void btnPlay_Click(object sender, RoutedEventArgs e)
        {

            if (Game.Path == "")
            {
                await new MessageDialog("You must Provide game EXE File Path to Play this game. Goto Edit Section.").ShowAsync();
                return;
            }
            
            if (await cUserData.GameRequest(Game))
            {
                App.Current.Exit();
            }
            else
            {
                await new MessageDialog("Unable to Launch Game").ShowAsync();
            }
        }

        private void btnEditGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditGame), Game);
        }

        private void btnRemoveGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(App.gdLauncherLink));
        }

        private async void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(App.gdHelpLink));
        }

        private void imgOptions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.BottomAppBar.IsOpen)
            {
                this.BottomAppBar.IsOpen = true;
                this.BottomAppBar.IsSticky = true;
               
            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;
               
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.BottomAppBar.IsOpen = false;
            this.BottomAppBar.IsSticky = false;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
