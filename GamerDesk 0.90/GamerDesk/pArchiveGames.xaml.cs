using GamerDesk.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.AccessCache;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GamerDesk
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class pArchiveGames : Page
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


        public pArchiveGames()
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
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            //***IF APPLICATION IS STARTED FOR THE FIRST TIME****
            if (!StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                await new MessageDialog("Select a Location to Save Games Data").ShowAsync();
                if (await cUserData.GetFolder())
                {
                    //create game counter file
                //    await cGuFuncs.UpdateCounterFile();

                    //get games counter value
                    //await cGuFuncs.GetCounterFileVal();
                    App.modelArchive.GamesCollection.Clear();
                    try
                    {
                        if (await cUserData.GetArchiveGames())
                        {
                            lstAllGames.ItemsSource = App.modelArchive.GamesCollection;
                        }
                    }
                    catch (Exception) { }           

                }


                //string data = await GetWebData();

                //txtData.Text = data;

            }
            else
            {
                //get games counter value
                //await cGuFuncs.GetCounterFileVal();

                try
                {
                    if (await cUserData.GetArchiveGames())
                    {
                        lstAllGames.ItemsSource = App.modelArchive.GamesCollection;
                    }
                }
                catch (Exception) {}               
                
            }

            if (App.modelArchive.GamesCollection.Count > 0)
            {
                txtEmptyHelp.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            //did this to loss auto focus on app start
            //txtSearch.IsEnabled = true;
            //btnDummy.Focus(Windows.UI.Xaml.FocusState.Keyboard);

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

        private void lstAllGames_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lstAllGames.SelectedIndex != -1)
            {
                Frame.Navigate(typeof(pViewGame), lstAllGames.SelectedItem as cGame);
            }
        }

        private void lstAllGames_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void lstAllGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lstAllGames.SelectedIndex != -1)
            {
                if (!this.BottomAppBar.IsOpen)
                {
                    this.BottomAppBar.IsOpen = true;
                    this.BottomAppBar.IsSticky = true;
                    
                    btnAddGame.Visibility = Windows.UI.Xaml.Visibility.Visible;

                }
                else
                {
                    this.BottomAppBar.IsOpen = false;
                    this.BottomAppBar.IsSticky = false;
                    
                    btnAddGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                    lstAllGames.SelectedIndex = -1;
                }
            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;
                btnAddGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                
                lstAllGames.SelectedIndex = -1;               
            }
            
        }

        private void lstAllGames_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {

        }

        private void btnAddToLib_Click(object sender, RoutedEventArgs e)
        {

            string name = Path.GetFileName(App.modelArchive.GamesCollection[lstAllGames.SelectedIndex].Image);
            
            cGame game = lstAllGames.SelectedItem as cGame;

            ///App.modelArchive.GamesCollection.Add(game);
            App.gvm.GamesCollection.Add(game);

            cUserData.AddNewGame();
            //cUserData.AddToArchive();

            cGuFuncs.CreateToast("GameR Desk", game.Name + " is now in Games Library");



            //Now removing Game from Main Grid

            //App.gvm.GamesCollection[grdAllGames.SelectedIndex].Image = "";
            //await cUserData.delCover(name);

            //App.modelArchive.GamesCollection[lstAllGames.SelectedIndex].Image = "";
            //await cUserData.delCover(name);


            if (lstAllGames.SelectedIndex != -1)
            {
                App.modelArchive.GamesCollection.RemoveAt(lstAllGames.SelectedIndex);
                cUserData.AddToArchive();

                //cGuFuncs.CreateToast("GameR Desk", game.Name + " removed from Games Library");
            }

        }

        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(App.gdHelpLink));
        }

        
    }
}
