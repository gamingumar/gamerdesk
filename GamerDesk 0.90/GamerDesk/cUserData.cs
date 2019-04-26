using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;

/*
 * Gamer Desk V 1.2
 * Message Dialog fixed
 * UI Changes
 * */

namespace GamerDesk
{
    public class cUserData
    {
        public static StorageFolder userFolder { get; set; }
        public static string folderAccessToken { get; set; }
        public static StorageFolder f { get; set; }
        
        //StorageFolder userFolder; //stores user folder
        //string folderAccessToken; //folder access token for future
        //StorageFolder f;
        public static async Task<bool> GetFolder()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken",AccessCacheOptions.FastLocationsOnly);
                f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);
                await new MessageDialog("Gamer Desk Data Folder is Located in " + userFolder.Path.ToString()).ShowAsync();


                /*

                //creating file

                FileOpenPicker picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
                picker.ViewMode = PickerViewMode.List;
                picker.FileTypeFilter.Add(".txt");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".exe");
                await new MessageDialog("Pick a File to save location.").ShowAsync();
                StorageFile filepath = await picker.PickSingleFileAsync();


                await new MessageDialog("File not found").ShowAsync();
                StorageFile file = await f.CreateFileAsync
                ("GameLaunch.txt", CreationCollisionOption.OpenIfExists);


                if (file != null && filepath != null)
                {
                    string json = JsonConvert.SerializeObject(filepath.Path);

                    await FileIO.WriteTextAsync(file, json);
                }
                else
                {
                    await new MessageDialog("Unable to Create file").ShowAsync();
                }

                */
                
            }
            
            else
            {
                PickFolder(); //create user folder to save data
            }

            //ApplicationData.Current.LocalSettings.Values.Remove("AccessToken");

            return true;
            
        }

        //if no user folder found
        public static async void PickFolder()
        {
            
            await new MessageDialog("Select C:/ Drive for all Gamer Desk functionality to work and to easily Backup your Games Data").ShowAsync();
                          
           
            FolderPicker pickFolder = new FolderPicker();
            pickFolder.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            pickFolder.ViewMode = PickerViewMode.List;
            pickFolder.FileTypeFilter.Add(".txt");

            userFolder = await pickFolder.PickSingleFolderAsync();
            if (userFolder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("AccessToken", userFolder);
                //ApplicationData.Current.LocalSettings.Values.Add("AccessToken", folderAccessToken);


                await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);

                await new MessageDialog("All of your Data will now be saved in Gamer Desk Data folder in "+userFolder.Path).ShowAsync();
            
            }

           
        }


        public async static Task<bool> GameRequest(cGame game)
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
                f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);
                //await new MessageDialog("Folder found" + userFolder.Path.ToString()).ShowAsync();


                StorageFile file = await f.CreateFileAsync
                ("GameLaunch.txt", CreationCollisionOption.OpenIfExists);

                /*

                //creating file

                FileOpenPicker picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
                picker.ViewMode = PickerViewMode.List;
                picker.FileTypeFilter.Add(".txt");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".exe");


               // await new MessageDialog("Pick a File to save location.","Pick a File to Launch").ShowAsync();
                MessageDialog msgDialog = new MessageDialog("Pick a File to save location.", "Pick a File to Launch");
                UICommand okBtn = new UICommand("OK");
                //okBtn.Invoked = OkBtnClick;
                msgDialog.Commands.Add(okBtn);
                UICommand cancelBtn = new UICommand("Cancel");
                //cancelBtn.Invoked = CancelBtnClick;
                msgDialog.Commands.Add(cancelBtn);
                await msgDialog.ShowAsync();


                StorageFile filepath = await picker.PickSingleFileAsync();
                */

                if (file != null)// && filepath != null)
                {
                    //string json = JsonConvert.SerializeObject(game.Path);

                    await FileIO.WriteTextAsync(file, game.Path);

                    //await new MessageDialog("Game Launch request made").ShowAsync();
                    return true;
                }
                else
                {
                    //await new MessageDialog("Unable to Launch Game").ShowAsync();
                    return false;
                }

               
            }
            else
            {
                //folder access token not found
                return false;
            }

        }


        public async static void AddNewGame()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
                f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);
                
                StorageFile file = await f.CreateFileAsync
                ("GamesList.txt", CreationCollisionOption.OpenIfExists);

                
                if (file != null)
                {
                    string json = JsonConvert.SerializeObject(App.gvm.GamesCollection, Formatting.Indented);

                    await FileIO.WriteTextAsync(file, json);

                    //await new MessageDialog("Games Library Updated Successfully").ShowAsync();
                }
                else
                {
                    await new MessageDialog("Unable to Create file").ShowAsync();
                }

            }
        }

        public async static void AddToArchive()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
                f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);

                StorageFile file = await f.CreateFileAsync
                ("GamesArchiveList.txt", CreationCollisionOption.OpenIfExists);


                if (file != null)
                {
                    string json = JsonConvert.SerializeObject(App.modelArchive.GamesCollection, Formatting.Indented);

                    await FileIO.WriteTextAsync(file, json);

                    //await new MessageDialog("Games Library Updated Successfully").ShowAsync();
                }
                else
                {
                    await new MessageDialog("Unable to Create file").ShowAsync();
                }

            }
        }


        public async static Task<bool> GetGames()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
                f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);

                try
                {
                    StorageFile file = await f.CreateFileAsync("GamesList.txt", CreationCollisionOption.OpenIfExists);
                    string json = await FileIO.ReadTextAsync(file);
                    var game = JsonConvert.DeserializeObject<List<cGame>>(json);

                    App.gvm.GamesCollection = new ObservableCollection<cGame>(game);
                    
                }
                catch (Exception) 
                { }
            }
            if (App.gvm.GamesCollection.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }



        public async static Task<bool> GetArchiveGames()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("AccessToken"))
            {
                userFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("AccessToken", AccessCacheOptions.FastLocationsOnly);
                f = await userFolder.CreateFolderAsync("Gamer Desk Data", CreationCollisionOption.OpenIfExists);

                try
                {
                    StorageFile file = await f.CreateFileAsync("GamesArchiveList.txt", CreationCollisionOption.OpenIfExists);
                    string json = await FileIO.ReadTextAsync(file);
                    var game = JsonConvert.DeserializeObject<List<cGame>>(json);

                    App.modelArchive.GamesCollection = new ObservableCollection<cGame>(game);

                }
                catch (Exception)
                { }
            }
            if (App.modelArchive.GamesCollection.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public async static Task<bool> delCover(string imgName) 
        {            
            string name = Path.GetFileName(imgName);
            StorageFile file = null;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            try
            {
                //App.Sleep(500);
                file = await folder.GetFileAsync(name);

                if (file != null)
                {
                    //App.Sleep(500);
                    await file.DeleteAsync();
                    

                    return true;
                }

            }
            catch (Exception)
            { }  
            
           
            
            
            return false;
            
        
        }
    }
}
