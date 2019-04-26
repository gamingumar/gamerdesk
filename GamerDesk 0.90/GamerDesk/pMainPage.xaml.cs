using GamerDesk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.Web.Http;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GamerDesk
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
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


        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            //Loaded += MainPage_Loaded;

            //grdAllGames.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            //txtSearch.Focus(Windows.UI.Xaml.FocusState.Pointer);           
            
        }

        /*async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //using (var client = new HttpClient())
            //{
            //    string text = await client.GetStringAsync(new Uri("http://www.giantbomb.com/api/search/?api_key=6d10bfb6e754c5d8183e94476ba65a639565f333&format=json&query=metroid prime&resources=game"));

            //    var obj = JsonConvert.DeserializeObject<Rootobject>(text);
            //    //ObservableCollection<Rootobject> dataCollection = JsonConvert.DeserializeObject<Rootobject>(text);
            //    //foreach (var Rootobject in obj.)
            //    //{
                    
            //    //}
            //    txtData.Text = text;
            //}
        }*/


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
                    await cGuFuncs.GetCounterFileVal();
                    App.gvm.GamesCollection.Clear();
                    try
                    {
                        if (await cUserData.GetGames())
                        {
                            grdAllGames.ItemsSource = App.gvm.GamesCollection;
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
                await cGuFuncs.GetCounterFileVal();

                try
                {
                    if (await cUserData.GetGames())
                    {
                        grdAllGames.ItemsSource = App.gvm.GamesCollection;
                    }
                }
                catch (Exception) {}               
                
            }

            if (App.gvm.GamesCollection.Count > 0)
            {
                txtEmptyHelp.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            //did this to loss auto focus on app start
            txtSearch.IsEnabled = true;
            btnDummy.Focus(Windows.UI.Xaml.FocusState.Keyboard);
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

        private void grdAllGames_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (grdAllGames.SelectedIndex != -1)
            {
               
                Frame.Navigate(typeof(pViewGame), grdAllGames.SelectedItem as cGame);
                
                
                /*
                if (await cUserData.GameRequest(grdAllGames.SelectedItem as cGame))
                {
                    App.Current.Exit();
                }
                else
                {
                    await new MessageDialog("Unable to Launch Game").ShowAsync();
                }
                grdAllGames.SelectedIndex = -1;
                */
            }
            else
            {/*
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;
                btnRemoveGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnEditGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdAllGames.SelectedIndex = -1;                */
            }
        }

       
        

        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(classAddGame));
        }

        private void btnEditGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EditGame),grdAllGames.SelectedItem as cGame);
        }

        private async void btnRemoveGame_Click(object sender, RoutedEventArgs e)
        {
            if (grdAllGames.SelectedIndex != -1)
            {

                string name = Path.GetFileName(App.gvm.GamesCollection[grdAllGames.SelectedIndex].Image);

                //message dialog
                var confirm = new MessageDialog("Are you sure you want to delete this game from Library? All Game track time will be lost. GU recommends to Backup your Gamer Desk Data folder before removing any game.");
                confirm.Commands.Add(new UICommand("Yes", async delegate(IUICommand command)
                {
                    App.gvm.GamesCollection[grdAllGames.SelectedIndex].Image = "";
                    await cUserData.delCover(name);

                    cGame game = grdAllGames.SelectedItem as cGame;

                    if (grdAllGames.SelectedIndex != -1)
                    {
                        App.gvm.GamesCollection.RemoveAt(grdAllGames.SelectedIndex);
                        cUserData.AddNewGame();

                        cGuFuncs.CreateToast("GameR Desk", game.Name + " removed from Games Library");
                    }
                }));

                confirm.Commands.Add(new UICommand("No"));

                await confirm.ShowAsync();






                
                /*await new MessageDialog("to del: " + name ).ShowAsync();
                if (await classUserData.delCover(name))
                {
                    await new MessageDialog("img delted").ShowAsync();
                }
                else
                {
                    await new MessageDialog("img not delted").ShowAsync();
                }
                */

                //did this as image is locked previously to access
                
                

            }
            else
            {
                await new MessageDialog("unable to remove").ShowAsync();
            }
        }

        private void grdAllGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdAllGames.SelectedIndex != -1)
            {
                if (!this.BottomAppBar.IsOpen)
                {
                    this.BottomAppBar.IsOpen = true;
                    this.BottomAppBar.IsSticky = true;
                    btnRemoveGame.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnEditGame.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnPlayGame.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnHideGame.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    sep0.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    sep1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    sep2.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    btnFolderChange.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnSettings.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                    btnDownload.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    this.BottomAppBar.IsOpen = false;
                    this.BottomAppBar.IsSticky = false;
                    btnRemoveGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnEditGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnPlayGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    btnHideGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                    sep0.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    sep1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    sep2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                    btnFolderChange.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnSettings.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    btnDownload.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    grdAllGames.SelectedIndex = -1;
                }
            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;
                btnRemoveGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnEditGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnPlayGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnHideGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                sep0.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                sep1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                sep2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                btnFolderChange.Visibility = Windows.UI.Xaml.Visibility.Visible;
                btnSettings.Visibility = Windows.UI.Xaml.Visibility.Visible;

                btnDownload.Visibility = Windows.UI.Xaml.Visibility.Visible;
                grdAllGames.SelectedIndex = -1;               
            }
            if (grdAllGames.SelectedIndex != -1)
            {
                cGame game = grdAllGames.SelectedItem as cGame;
                if (game.Path == "")
                {
                    btnPlayGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            
        }

        private void grdAllGames_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            grdAllGames.SelectionMode = ListViewSelectionMode.Single;
        }

        
        private async void txtSearch_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (cGuFuncs.IsConnectedToInternet())
            {
                if (txtSearch.QueryText != "")
                {
                    txtSearch.IsEnabled = false;
                    ring.IsActive = true;
                    bar.Visibility = Visibility.Visible;
                    await guSearch.SearchInputGame(txtSearch.QueryText);

                    string result;

                    dynamic Search = new ExpandoObject();
                    if (App.ApiGamesList.Count == 0)
                    {
                        result = " (Some Error Occured or Unable to Find Game)";
                    }
                    else
                    {
                        result = " (About " + App.ApiGamesList.Count.ToString() + " results)";
                    }
                    Search.gameQuery = txtSearch.QueryText + result;


                    Frame.Navigate(typeof(pGamesSearch), Search);
                    txtSearch.IsEnabled = true;
                    ring.IsActive = false;
                    bar.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                await new MessageDialog("You are not Connected to the Internet.").ShowAsync();
            }
            
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await cUserData.GetGames();
            grdAllGames.ItemsSource = App.gvm.GamesCollection;

            cGuFuncs.CreateToast("GameR Desk", "Games Library Refreshed Successfully.");


            if (App.gvm.GamesCollection.Count > 0)
            {
                txtEmptyHelp.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void btnSettings_Click(object sender, RoutedEventArgs e)
        {   
            await cUserData.GetFolder();
        }

        private async void btnPlayGame_Click(object sender, RoutedEventArgs e)
        {
            if (grdAllGames.SelectedIndex != -1)
            {
                if (await cUserData.GameRequest(grdAllGames.SelectedItem as cGame))
                {
                    App.Current.Exit();
                }
                else
                {
                    await new MessageDialog("Unable to Launch Game").ShowAsync();
                }
                grdAllGames.SelectedIndex = -1;

            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;
                btnRemoveGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                btnEditGame.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                grdAllGames.SelectedIndex = -1;
            }
        }

        private async void btnFolderChange_Click(object sender, RoutedEventArgs e)
        {

            //message dialog
            //var confirm = new MessageDialog("Are you sure you want to change your Gamer Desk Data Folder Location? For all features to work select C Drive.");
            //confirm.Commands.Add(new UICommand("Yes", async delegate(IUICommand command)
            //{
                if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
                {
                    StorageApplicationPermissions.FutureAccessList.Remove("AccessToken");
                    App.gvm.GamesCollection = null;
                    txtEmptyHelp.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    await cUserData.GetFolder();

                    await cUserData.GetGames();
                    //{
                    //    await new MessageDialog("True" + App.gvm.GamesCollection.Count).ShowAsync();
                    //}

                    grdAllGames.ItemsSource = App.gvm.GamesCollection;
                    // }

                    //await new MessageDialog("Refresh Games from the App Bar").ShowAsync();


                }
                else
                {
                    await new MessageDialog("You have not selected any location to save User Data Folder").ShowAsync();
                }
            //}));

            //confirm.Commands.Add(new UICommand("No"));

            //if (Window.Current.Dispatcher.HasThreadAccess)
            //{

            //    await confirm.ShowAsync();
            //}


            
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
            //same code of grid all games selection changed...

            
                //await new MessageDialog("CHK").ShowAsync();
                if (!this.BottomAppBar.IsOpen)
                {
                    this.BottomAppBar.IsOpen = true;
                    this.BottomAppBar.IsSticky = true;
                    //sep1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    //sep2.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    this.BottomAppBar.IsOpen = false;
                    this.BottomAppBar.IsSticky = false;
                    //sep1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //sep2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                    grdAllGames.SelectedIndex = -1;
                }
            
                
 
        }

        private void grdAllGames_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            //Frame.Navigate(typeof(pViewGame), e.ClickedItem as cGame);
            
        }

        private async void btnHideGame_Click(object sender, RoutedEventArgs e)
        {

            if (grdAllGames.SelectedIndex != -1)
            {
                string name = Path.GetFileName(App.gvm.GamesCollection[grdAllGames.SelectedIndex].Image);

                //message dialog
                /*var confirm = new MessageDialog("Are you sure you want to delete this game from Library? All Game track time will be lost. GU recommends to Backup your Gamer Desk Data folder before removing any game.");
                confirm.Commands.Add(new UICommand("Yes", async delegate(IUICommand command)
                {
                    App.gvm.GamesCollection[grdAllGames.SelectedIndex].Image = "";
                    await cUserData.delCover(name);
                    */
                    cGame game = grdAllGames.SelectedItem as cGame;

                    App.modelArchive.GamesCollection.Add(game);

                    cUserData.AddToArchive();
                    
                    cGuFuncs.CreateToast("GameR Desk", game.Name + " is now in Archived Games Library");
                    
               // }));

               // confirm.Commands.Add(new UICommand("No"));

               // await confirm.ShowAsync();


                //Now removing Game from Main Grid

                //App.gvm.GamesCollection[grdAllGames.SelectedIndex].Image = "";
                //await cUserData.delCover(name);

                if (grdAllGames.SelectedIndex != -1)
                {
                    App.gvm.GamesCollection.RemoveAt(grdAllGames.SelectedIndex);
                    cUserData.AddNewGame();

                    //cGuFuncs.CreateToast("GameR Desk", game.Name + " removed from Games Library");
                }

            }
            else
            {
                await new MessageDialog("Unable to add to Archive").ShowAsync();
            }

        }

        private void btnArchivedGames_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(pArchiveGames));
        }

        

       
    }
}
