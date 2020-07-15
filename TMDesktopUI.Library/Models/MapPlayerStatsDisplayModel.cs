using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class MapPlayerStatsDisplayModel
    {
        //public int Id;
        //public int MapScoreId;
        //public int PlayerId;
        public int Kills;
        public int Assists;
        public int Deaths;

        // to display player's fullname
        public PlayerDisplayModel Player;

        public MapScoreDisplayModel Map;

        public MapPlayerStatsDisplayModel(MapPlayerStatsModel model)
        {            
            //MapScoreId = model.MapScoreId;
            //PlayerId = model.PlayerId;
            Kills = model.Kills;
            Assists = model.Assists;
            Deaths = model.Deaths;            
        }
    }
}
