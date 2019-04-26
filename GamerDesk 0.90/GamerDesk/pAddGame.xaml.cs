using GamerDesk.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Html;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
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
    public sealed partial class classAddGame : Page
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


        public classAddGame()
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

        private void grdSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void imgSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private async void btnImgSelect_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage imgCover = await BrowseImage();

            if (imgCover == null)
            {
                await new MessageDialog("Please Select Game Cover").ShowAsync();
            }
            else
            {
                imgGameCover.Source = imgCover;
            }
            
        }

        //gu function
        public static StorageFile GameCover { get; set; }

        public static async Task<BitmapImage> BrowseImage()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.ViewMode = PickerViewMode.Thumbnail;

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");

            GameCover = await picker.PickSingleFileAsync();

            BitmapImage img = new BitmapImage();
            if (GameCover != null)
            {
                var imgStream = await GameCover.OpenReadAsync();

                img.SetSource(imgStream);
            }
            return img;
        }

        //ADD NEW GAME BUTTON CLICK
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private async void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            //if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            //{
            //    StorageFolder userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
            //    StorageFolder f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);

            ringAddGame.IsActive = true;
            btnAddGame.IsEnabled = false;

            if (txtName.Text == "")
            {
                await new MessageDialog("You must provide Game Name.").ShowAsync();

                ringAddGame.IsActive = false;
                btnAddGame.IsEnabled = true;

                return;
            }
            if (txtDate.Text == "" && txtDesc.Text == "")
            {
                txtDate.Text = "No Release Date Available";
                txtDesc.Text = "No Description Available";
            }

            
            string imgName;

            //for downloaded image
            if (bImgDown != null && GameCover == null)
            {
                imgName = Path.GetFileName(imgLink);//txtName.Text + Path.GetExtension(imgLink);

#region Dowload Image
                //download image code start
                var httpClient = new HttpClient();
                var data = await httpClient.GetByteArrayAsync(new Uri(imgLink));
                GameCover = await localFolder.CreateFileAsync(imgName, CreationCollisionOption.ReplaceExisting);

                var targetStream = await GameCover.OpenAsync(FileAccessMode.ReadWrite);
                await targetStream.AsStreamForWrite().WriteAsync(data, 0, data.Length);
                await targetStream.FlushAsync();
                targetStream.Dispose();
                //download image code end
#endregion

                 
            }
            else
            {
                if (GameCover == null)
                {
                    await new MessageDialog("Please Select Game Cover Image.").ShowAsync();

                    ringAddGame.IsActive = false;
                    btnAddGame.IsEnabled = true;

                    return;
                }
                //imgName = txtName.Text + Path.GetExtension(GameCover.Name);
                imgName = Path.GetFileName(GameCover.Name);

                try
                {
                    StorageFile file = await localFolder.CreateFileAsync(imgName, CreationCollisionOption.ReplaceExisting);
                    await GameCover.CopyAndReplaceAsync(file);
                }
                catch (Exception) { }
            }
            
            

            if (GameCover != null)
            {
                //increment games static id counter
                cGame.GamesCount++;
                await cGuFuncs.UpdateCounterFile();

                cGame Game = new cGame(txtName.Text, txtPath.Text, "ms-appdata:///local/" +imgName,
                    txtDate.Text,txtDesc.Text);

                App.gvm.AddGame(Game);

                cUserData.AddNewGame();
                await cUserData.GetGames();
                
                if (this.Frame.CanGoBack)
                {                    
                    this.Frame.GoBack();                        
                }

                cGuFuncs.CreateToast("GameR Desk ", txtName.Text + " Added to Library.");
                    
            }
            else
            {
                await new MessageDialog("Please select Game Cover Image").ShowAsync();
            }

            ringAddGame.IsActive = false;
            btnAddGame.IsEnabled = true;
        }

        private async void btnPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.ViewMode = PickerViewMode.List;
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".exe");
            //await new MessageDialog("Select the File").ShowAsync();
            StorageFile filepath = await picker.PickSingleFileAsync();

            if (filepath != null)
            {
                txtPath.Text = filepath.Path;
            }
            else
            {
                await new MessageDialog("Please Provide Game EXE File Location").ShowAsync();
            }
            
        }

        private async void btnSearchGame_Click(object sender, RoutedEventArgs e)
        {
            if (cGuFuncs.IsConnectedToInternet())
            {
                if (txtName.Text != "")
                {
                    ring.IsActive = true;
                    btnSearchGame.IsEnabled = false;

                    await guSearch.SearchInputGame(txtName.Text);
                    lstFoundGames.ItemsSource = App.ApiGamesList;

                    btnSearchGame.IsEnabled = true;
                    ring.IsActive = false;
                }
                else
                {
                    await new MessageDialog("Enter Game Name First").ShowAsync();
                }
            }
            else
            {
                await new MessageDialog("You are not Connected to the Internet.").ShowAsync();
            }
        }

        BitmapImage bImgDown = null;
        
        string imgLink;
        private void lstFoundGames_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ringImgLoad.IsActive = true;
            btnImgSelect.IsEnabled = false;

            ApiSearchResult game = lstFoundGames.SelectedItem as ApiSearchResult;

            imgLink = game.image.super_url;

            //await new MessageDialog(imgLink).ShowAsync();
            bImgDown =  new BitmapImage(new Uri(imgLink));

            imgGameCover.Source = bImgDown;
            GameCover = null;

            txtDate.Text = game.original_release_date;
            txtDesc.Text = game.deck + " \n " + HtmlUtilities.ConvertToText(game.description);

            ringImgLoad.IsActive = false;
            btnImgSelect.IsEnabled = true;

        }
        //}
    }
}
