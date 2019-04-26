using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GamerDesk
{
    public class cGame:INotifyPropertyChanged
    {
        /*
         * Name, Path, Image, 
         * Last Play Time, Total Play Time
         * */
        public static int GamesCount = 0;

        public cGame() { }
        public cGame(string name, string path, string image, string date, string desc)
        {
            ID = GamesCount;
            Name = name;
            Path = path;
            Image = image;
            ReleaseDate = date;
            Description = desc;

            TotalTime = TimeSpan.Zero;
            LastTime = TimeSpan.Zero;
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        

        private string name;

        public string Name
        {
            get { return name; }
            set
            { 
                name = value;
                NotifyPropertyChanged();
            }
        }


        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                NotifyPropertyChanged();
            }
        }


        private string image;

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                NotifyPropertyChanged();
            }
        }

        public string lastTimeS 
        {
            get
            {
                return lastTime.ToString(@"hh\:mm\:ss");
            }
         }

        private TimeSpan lastTime;

        public TimeSpan LastTime
        {
            get
            {
                return lastTime;
            }
            set
            {
                lastTime = value;
                //lastTimeS = lastTime.ToString(@"hh\:mm\:ss");
                NotifyPropertyChanged();
            }
        }
        //ToString(@"hh\:mm\:ss");

        public string totalTimeS 
        { 
            get 
            {
                return totalTime.ToString(@"hh\:mm\:ss");
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
           //     totalTimeS = totalTime.ToString(@"hh\:mm\:ss");
                NotifyPropertyChanged();

            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        private string releaseDate;

        public string ReleaseDate
        {
            get { return releaseDate; }
            set
            {
                releaseDate = value;
                NotifyPropertyChanged();
            }
        }
        
        


        internal void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
