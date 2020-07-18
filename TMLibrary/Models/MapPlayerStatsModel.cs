using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class MapPlayerStatsModel
    {
        public int Id;
        public int MapScoreId;
        public int PlayerId;
        public int Kills;
        public int Assists;
        public int Deaths;

        public MapPlayerStatsModel(int mapScoreId, int playerId, int kills, int assists, int deaths)
        {
            MapScoreId = mapScoreId;
            PlayerId = playerId;
            Kills = kills;
            Assists = assists;
            Deaths = deaths;
        }

        public MapPlayerStatsModel() { }
       
    }
}
