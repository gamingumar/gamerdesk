using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerDesk
{
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

    public class OriginalGameRating
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

    public class Image2
    {
        public string icon_url { get; set; }
        public string medium_url { get; set; }
        public string screen_url { get; set; }
        public string small_url { get; set; }
        public string super_url { get; set; }
        public string thumb_url { get; set; }
        public string tiny_url { get; set; }
        public string tags { get; set; }
    }

    public class Character
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Concept
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Developer
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class FirstAppearanceCharacter
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class FirstAppearanceConcept
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class FirstAppearanceLocation
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class FirstAppearanceObject
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class FirstAppearancePeople
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Franchise
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Genre
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Location
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Object
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Person
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Publisher
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Release
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class SimilarGame
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Theme
    {
        public string api_detail_url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string site_detail_url { get; set; }
    }

    public class Results
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
        public List<OriginalGameRating> original_game_rating { get; set; }
        public string original_release_date { get; set; }
        public List<Platform> platforms { get; set; }
        public string site_detail_url { get; set; }
        public List<Image2> images { get; set; }
        public List<object> videos { get; set; }
        public List<Character> characters { get; set; }
        public List<Concept> concepts { get; set; }
        public List<Developer> developers { get; set; }
        public List<FirstAppearanceCharacter> first_appearance_characters { get; set; }
        public List<FirstAppearanceConcept> first_appearance_concepts { get; set; }
        public List<FirstAppearanceLocation> first_appearance_locations { get; set; }
        public List<FirstAppearanceObject> first_appearance_objects { get; set; }
        public List<FirstAppearancePeople> first_appearance_people { get; set; }
        public List<Franchise> franchises { get; set; }
        public List<Genre> genres { get; set; }
        public object killed_characters { get; set; }
        public List<Location> locations { get; set; }
        public List<Object> objects { get; set; }
        public List<Person> people { get; set; }
        public List<Publisher> publishers { get; set; }
        public List<Release> releases { get; set; }
        public List<SimilarGame> similar_games { get; set; }
        public List<Theme> themes { get; set; }
    }

    public class RootObject
    {
        public string error { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int number_of_page_results { get; set; }
        public int number_of_total_results { get; set; }
        public int status_code { get; set; }
        public Results results { get; set; }
        public string version { get; set; }
    }
}
