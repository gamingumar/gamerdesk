using NotificationsExtensions.ToastContent;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.Networking.Connectivity;

namespace GamerDesk
{
    public class cGuFuncs
    {
        //toast notification function
        public static void CreateToast(string head, string body)
        {
            var toast = ToastContentFactory.CreateToastText02();
            toast.TextHeading.Text = head;
            toast.TextBodyWrap.Text = body;
            
            ToastNotification t = toast.CreateNotification();

            ToastNotificationManager.CreateToastNotifier().Show(t);

        }

        //increment static id counter function
        public static async Task<bool> UpdateCounterFile()
        {
            StorageFolder userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
            StorageFolder dataFolder = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);
            //await new MessageDialog("Folder found" + userFolder.Path.ToString()).ShowAsync();


            StorageFile file = await dataFolder.CreateFileAsync
            ("GamesCount.txt", CreationCollisionOption.OpenIfExists);



            if (file != null)
            {
                await FileIO.WriteTextAsync(file, cGame.GamesCount.ToString());
                return true;
            }
            else
            {
                return false;
            }


        }

        //get Counter file value to set static counter
        public static async Task<bool> GetCounterFileVal()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                StorageFolder userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
                StorageFolder dataFolder = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);
                //await new MessageDialog("Folder found" + userFolder.Path.ToString()).ShowAsync();


                try
                {
                    StorageFile file = await dataFolder.CreateFileAsync
                     ("GamesCount.txt", CreationCollisionOption.OpenIfExists);


                    if (file != null)
                    {
                        cGame.GamesCount = Convert.ToInt32(await FileIO.ReadTextAsync(file));
                        return true;
                    }
                    else
                    {
                        await new MessageDialog("Error Getting Games Data").ShowAsync();
                    }
                }
                catch (Exception)
                { }
            }
            return false;
        }



        //sleep function
        public static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);
        }
        

        //download picture & save
        public async static Task<bool> DownloadPic(string imgLink, string imgName)
        {
            StorageFile GameCover;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                //imgName = imgName + Path.GetExtension(imgLink);

                using (var httpClient = new HttpClient())
                {
                    var data = await httpClient.GetByteArrayAsync(new Uri(imgLink));

                        
                       
                    GameCover = await localFolder.CreateFileAsync(imgName, CreationCollisionOption.ReplaceExisting);

                    var targetStream = await GameCover.OpenAsync(FileAccessMode.ReadWrite);
                    await targetStream.AsStreamForWrite().WriteAsync(data, 0, data.Length);
                    await targetStream.FlushAsync();
                    targetStream.Dispose();

                        
                }

            }
            catch (Exception)
            {

            }
           
               
            
            return true;
        }


        public static bool IsConnectedToInternet()
        {
            ConnectionProfile connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }

    }
}
