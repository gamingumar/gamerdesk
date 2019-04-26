using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GamerDesk
{

    public class ApiSearch
    {
        public string error { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int number_of_page_results { get; set; }
        public int number_of_total_results { get; set; }
        public int status_code { get; set; }
        public ApiSearchResult[] results { get; set; }
        public string version { get; set; }
    }

    public class ApiSearchResult
    {
        public string aliases { get; set; }
        public string api_detail_url { get; set; }
        public string date_added { get; set; }
        public string date_last_updated { get; set; }
        public string deck { get; set; }
        public string description { get; set; }
        public object expected_release_day { get; set; }
        public object expected_release_month { get; set; }
        public object expected_release_quarter { get; set; }
        public object expected_release_year { get; set; }
        public int id { get; set; }
        public Image image { get; set; }
        public string name { get; set; }
        public int number_of_user_reviews { get; set; }
        public Original_Game_Rating[] original_game_rating { get; set; }
        public string original_release_date { get; set; }
        public Platform[] platforms { get; set; }
        public string site_detail_url { get; set; }
        public string resource_type { get; set; }
    }

    public class Image
    {
        public string icon_url { get; set; }
        public string medium_url { get; set; }
        public string screen_url { get; set; }
        public string small_url { get; set; }
        public string super_url { get; set; }
        public string thumb_url { get; set; }
        public string tiny_url { get; set; }
    }

    public class Original_Game_Rating
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Platform
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
        public string abbreviation { get; set; }
    }

    public class guSearch
    {
        public static async Task<bool> SearchInputGame(string SearchKeyword)
        {            
            ApiSearch gamesFound = null;

            string jsonData;
           
            using (var client = new HttpClient())
            {
                try
                {
                    //query to API
                    jsonData = await client.GetStringAsync(new Uri
                    ("http://www.giantbomb.com/api/search/?api_key=6d10bfb6e754c5d8183e94476ba65a639565f333&resources=game&limit=100&format=json&query="
                    + SearchKeyword + ""));

                      

                    if (jsonData == null) { return false; }

                    //all games list
                    gamesFound = JsonConvert.DeserializeObject<ApiSearch>(jsonData);

                    App.ApiGamesList.Clear();
                    //var game =  gamesFound.results[0];

                    //manipulating date of json data
                    DateTime date;
                    foreach (var game in gamesFound.results)
                    {
                        //release date fix
                        if (game.original_release_date != null)
                        {
                            date = DateTime.Parse(game.original_release_date);
                            game.original_release_date = date.ToString("dd MMMM yyyy"); //date.Day + date.Month + date.Year;

                        }
                        else
                        {
                            game.original_release_date = "Not Released Yet ";
                        }


                        App.ApiGamesList.Add(game);
                    }
                    //await new MessageDialog("Number of Games Found: " + App.ApiGamesList.Count.ToString()).ShowAsync();

                }
                catch (HttpRequestException e)
                {
                    
                }



            }
           
           // btnGetData.Content = "Done!";


            return true;
            //function end
        }


    //guSearch class end
    }
}
