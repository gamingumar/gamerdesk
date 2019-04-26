using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace GamerDesk
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// #de3e25 my Gamer Desk Color
    /// </summary>
    sealed partial class App : Application
    {

        public static cGameViewModel gvm = new cGameViewModel();
        public static cGameViewModel modelArchive = new cGameViewModel();
        public static ObservableCollection<ApiSearchResult> ApiGamesList = new ObservableCollection<ApiSearchResult>();

        public static string gdLauncherLink = "http://gamingumar.blogspot.com/p/gamer-desk.html";
        public static string gdHelpLink = "http://gamingumar.blogspot.com/p/gamer-desk.html";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        
        public App()
        {
            
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }



        //gu function to search
        //public static async void SearchInputGame()
        //{
        //    txtData.Text = "";
        //    string text = null;
        //    ApiSearch gamesFound = null;


        //    btnGetData.Content = "Processing...";
        //    using (var client = new HttpClient())
        //    {
        //        try
        //        {
        //            //query to API
        //            text = await client.GetStringAsync(new Uri
        //            ("http://www.giantbomb.com/api/search/?api_key=6d10bfb6e754c5d8183e94476ba65a639565f333&filter=platforms:94&format=json&query="
        //            + txtSearch.QueryText + "&resources=game&limit=100"));

        //            if (text == null) { return; }

        //            //all games list
        //            gamesFound = JsonConvert.DeserializeObject<ApiSearch>(text);

        //            App.ApiGamesList.Clear();
        //            //var game =  gamesFound.results[0];
        //            foreach (var game in gamesFound.results)
        //            {
        //                App.ApiGamesList.Add(game);
        //            }
        //            await new MessageDialog("Number of Games Found: " + App.ApiGamesList.Count.ToString()).ShowAsync();
        //            Frame.Navigate(typeof(pGamesSearch));
        //        }
        //        catch (Exception)
        //        {

        //        }

        //        //txtData.Text = text;  // text;
        //        scroller.ChangeView(txtData.ActualWidth, txtData.ActualHeight, 1);
        //        //scroller.ScrollToVerticalOffset(txtData.ActualHeight);
        //        // await new MessageDialog("done").ShowAsync();







        //        /*
        //        txtData.Text = txtData.Text + " \n Name: \n" + des.name;
        //        txtData.Text = txtData.Text + " \n Image: \n" + des.image.super_url;


        //        BitmapImage img = new BitmapImage();
        //        img.UriSource = new Uri(des.image.super_url);
        //        imgCover.Source = img;


        //        txtData.Text = txtData.Text + " \n Api Detail: \n" + des.api_detail_url;
        //        txtData.Text = txtData.Text + " \n Date Added: \n" + des.date_added;
        //        txtData.Text = txtData.Text + " \n Last Updated: \n" + des.date_last_updated;
        //        txtData.Text = txtData.Text + " \n Deck: \n" + des.deck;
        //        txtData.Text = txtData.Text + " \n Number of User Reviews: \n" + des.number_of_user_reviews;
        //        txtData.Text = txtData.Text + " \n Original Release Date: \n" + des.original_release_date;

        //        //HtmlUtilities.ConvertToText
        //        txtData.Text = txtData.Text + " \n Description: \n" + HtmlUtilities.ConvertToText(des.description);
        //        txtData.Text = txtData.Text + " \n Detail URL: \n" + des.site_detail_url;
        //        txtData.Text = txtData.Text + " \n Release Year: \n" + des.expected_release_year;
        //        txtData.Text = txtData.Text + "\n---------------------------------------------";

        //        */



        //    }
        //    btnGetData.Content = "Done!";

        //}



    }
}
