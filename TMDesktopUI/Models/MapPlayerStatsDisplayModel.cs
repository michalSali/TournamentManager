using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Models
{
    public class MapPlayerStatsDisplayModel
    {       
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        
        public PlayerDisplayModel Player { get; set; }

        public MapScoreDisplayModel Map { get; set; }

        public MapPlayerStatsDisplayModel()
        {
        }

        public MapPlayerStatsDisplayModel(MapPlayerStatsModel model)
        {                       
            Kills = model.Kills;
            Assists = model.Assists;
            Deaths = model.Deaths;            
        }
    }
}
