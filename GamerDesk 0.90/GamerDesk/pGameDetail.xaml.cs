using GamerDesk.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Html;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class pGameDetail : Page
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


        public pGameDetail()
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

        ApiSearchResult game;
        
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            game = e.NavigationParameter as ApiSearchResult;

            pageTitle.Text = game.name;
            imgCover.Source = new BitmapImage(new Uri(game.image.super_url));


            //txtData.Text += " \n\n[** Game ID: **] " + game.id.ToString() +"\n\n";

            txtData.Text += " \n\n[** Platforms: **] ";
            foreach (Platform platform in game.platforms)
            {
                txtData.Text+= " " + platform.abbreviation;
            }
            txtData.Text += "\n\n";

            //if (game.api_detail_url != null)
            //{
            //    txtData.Text = txtData.Text + " \n\n[** Api Detail: **]\n\n" + game.api_detail_url;
            //}
            //if (game.date_added != null)
            //{
            //    txtData.Text = txtData.Text + " \n\n[** Date Added: **]\n\n" + game.date_added;
            //}
            //if (game.date_last_updated != null)
            //{
            //    txtData.Text = txtData.Text + " \n\n[** Last Updated: **]\n\n" + game.date_last_updated;
            //}
            if (game.deck != null)
            {
                txtData.Text = txtData.Text + " \n\n[** Short Description: **]\n\n" + game.deck;
            }
            if (game.original_release_date != null)
            {
                txtData.Text = txtData.Text + " \n\n[** Original Release Date: **]\n\n" + game.original_release_date;
            }
            
            //if (game.site_detail_url != null)
            //{
            //    txtData.Text = txtData.Text + " \n\n[** Detail URL: **]\n\n" + game.site_detail_url;
            //}
            if (game.expected_release_year != null)
            {
                txtData.Text = txtData.Text + " \n\n[** Release Year: **] " + game.expected_release_year + "\n\n";
            }

            //HtmlUtilities.ConvertToText
            if (game.description != null)
            {
                txtData.Text = txtData.Text + " \n\n[** Description: **]\n\n" + HtmlUtilities.ConvertToText(game.description);
            }
                        

            txtData.Text = txtData.Text + "\n---------------------------------------------";


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

        private async void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            ringImgDownload.IsActive = true;
            btnAddGame.IsEnabled = false;

            string date;
            string imgLink = game.image.super_url;

            string imgName = Path.GetFileName(imgLink);//game.name + Path.GetExtension(imgLink);

            //download picture and save
            await cGuFuncs.DownloadPic(imgLink, imgName);


            if(game.original_release_date != null)
            {
                date = game.original_release_date;
            }
            else if(game.expected_release_year != null)
            {
                date = game.expected_release_year.ToString();
            }
            else
            {
                date = "";
            }



            cGame Game = new cGame(game.name, "", "ms-appdata:///local/" +imgName,
                date, game.deck + " \n " + HtmlUtilities.ConvertToText(game.description));


            App.gvm.AddGame(Game);

            cUserData.AddNewGame();
            await cUserData.GetGames();


            ringImgDownload.IsActive = false;
            btnAddGame.IsEnabled = true;

            //send toast
            cGuFuncs.CreateToast("GameR Desk ", game.name + " Added to Library.");

        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
