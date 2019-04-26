using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GamerDesk
{
    public class cGameViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<cGame> gamesCollection;

        public ObservableCollection<cGame> GamesCollection
        {
            get
            {
                if (gamesCollection == null)
                {
                    gamesCollection = new ObservableCollection<cGame>();
                }
                return gamesCollection; 
            }

            set { gamesCollection = value; NotifyPropertyChanged(); } 

        }


        public int GamesCount 
        {
            get
            {
                return gamesCollection.Count;                            
            }
            set
            {
                GamesCount = gamesCollection.Count;
                NotifyPropertyChanged();
            }
        }

        public void AddGame(cGame game)
        {
            gamesCollection.Add(game);
            NotifyPropertyChanged();
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
