using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDLauncher
{
    public class cGameViewModel
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
                
            set { gamesCollection = value; }

        }


        public int GamesCount 
        {
            get
            {
                return gamesCollection.Count;
            }
        }

        public void AddGame(cGame game)
        {
            gamesCollection.Add(game);
        }
        
    }
}
