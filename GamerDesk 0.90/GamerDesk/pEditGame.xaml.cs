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
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
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
    public sealed partial class EditGame : Page
    {
        cGame Game;
        public static StorageFile GameCover { get; set; }

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


        public EditGame()
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
            Game = e.NavigationParameter as cGame;
            pageTitle.Text = Game.Name;
            txtName.Text = Game.Name;
            txtPath.Text = Game.Path;
            txtDate.Text = Game.ReleaseDate;
            txtDesc.Text = Game.Description;

            if (Game.Image != null || Game.Image != "")
            {
                imgGameCover.Source = new BitmapImage(new Uri(Game.Image));
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


        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private async void btnEditGame_Click(object sender, RoutedEventArgs e)
        {

            btnEditGame.IsEnabled = false;
            ringEditGame.IsActive = true;
            
            if (imgGameCover.Source != null)
            {

                int i = 0;
                foreach (cGame game in App.gvm.GamesCollection)
                {
                    //await new MessageDialog("chk " + game.Name).ShowAsync();
                    if (game.Name == Game.Name)
                    {
                        //await new MessageDialog("chk").ShowAsync();
                        App.gvm.GamesCollection[i].Name = txtName.Text;
                        
                        App.gvm.GamesCollection[i].Path = txtPath.Text;

                        App.gvm.GamesCollection[i].Description = txtDesc.Text;

                        App.gvm.GamesCollection[i].ReleaseDate = txtDate.Text;  
                        //await new MessageDialog("chk " + App.gvm.GamesCollection[i].Path).ShowAsync();

                        try
                        {
                            string imgName;

                            //for downloaded image
                            if (bImgDown != null && GameCover == null)
                            {
                                imgName = Path.GetFileName(imgLink);// txtName.Text + Path.GetExtension(imgLink);

                                #region Dowload Image
                                //download image code start
                                var httpClient = new HttpClient();
                                //httpClient.MaxResponseContentBufferSize = 256;
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
                                imgName = Path.GetFileName(GameCover.Name);// txtName.Text + Path.GetExtension(GameCover.Name);

                                try
                                {
                                    StorageFile file = await localFolder.CreateFileAsync(imgName, CreationCollisionOption.ReplaceExisting);
                                    await GameCover.CopyAndReplaceAsync(file);
                                }
                                catch (Exception) { }
                            }


                            App.gvm.GamesCollection[i].Image = "ms-appdata:///local/" + imgName;
                     
                        }
                        catch (Exception)
                        {
                            
                        }

                        
                        
                        break;
                    }
                    i++;
                }


                cUserData.AddNewGame();
                await cUserData.GetGames();
                GameCover = null;
                if (this.Frame.CanGoBack)
                {                    
                    this.Frame.GoBack();
                }

                cGuFuncs.CreateToast("GameR Desk", txtName.Text + " Details Updated in Library.");

            }
            else
            {
                await new MessageDialog("Please Select Game Cover").ShowAsync();
            }

            btnEditGame.IsEnabled = true;
            ringEditGame.IsActive = false;

        }


        //public async Task<bool> delCover()
        //{
        //    string name = Path.GetFileName(Game.Image);
            
        //    StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(name);
        //    await new MessageDialog("file name " + Game.Image).ShowAsync();
        //    if (file != null)
        //    {
        //        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

        //        return true;
        //    }

        //    return false;
            
        //}



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
                using (var imgStream = await GameCover.OpenReadAsync())
                {
                    img.SetSource(imgStream);                    
                }
               
            }
            
            return img;
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

        BitmapImage bImgDown;
        //StorageFile downFile;
        string imgLink;
        private void lstFoundGames_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ringDownloadImage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ringDownloadImage.IsActive = true;

            ApiSearchResult game = lstFoundGames.SelectedItem as ApiSearchResult;

            imgLink = game.image.super_url;

            //await new MessageDialog(imgLink).ShowAsync();
            bImgDown = new BitmapImage(new Uri(imgLink));

            imgGameCover.Source = bImgDown;
            GameCover = null;

            txtDate.Text = game.original_release_date;

            if (game.description != "" || game.description != null)
            {
                txtDesc.Text = game.deck + " \n " + HtmlUtilities.ConvertToText(game.description);
            }
            

            txtName.Text = game.name;

/*
            //Background Downloader

            try
            {
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation imgToDownload;
                StorageFile downFile = await KnownFolders.PicturesLibrary.CreateFileAsync("test.jpg", CreationCollisionOption.ReplaceExisting);

                imgToDownload = downloader.CreateDownload(new Uri(imgLink), downFile);

                

                await StartDownloadAsync(imgToDownload);

                
            }


            catch (Exception)
            {
            }

            */


            ringDownloadImage.IsActive = false;
            ringDownloadImage.Visibility = Windows.UI.Xaml.Visibility.Collapsed; 

        }


        private async Task StartDownloadAsync(DownloadOperation downloadOperation)
        {

            var progress = new Progress<DownloadOperation>(ProgressCallback);

            await downloadOperation.StartAsync();

        }


        private void ProgressCallback(DownloadOperation obj)
        {
            double progress = ((double)obj.Progress.BytesReceived / obj.Progress.TotalBytesToReceive);
            
            
            if (progress >= 1.0)
            {
                ringDownloadImage.IsActive = false;
                ringDownloadImage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;                
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
    }
}
