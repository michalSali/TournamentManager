using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class MapScoreModel
    {
        public int Id;
        public int MatchId;
        public int MapNumber;
        public string MapName;
        public int TeamOneScore;
        public int TeamTwoScore;

        public MapScoreModel(int matchId, int mapNumber, string mapName, int teamOneScore, int teamTwoScore)
        {
            MatchId = matchId;
            MapNumber = mapNumber;
            MapName = mapName;
            TeamOneScore = teamOneScore;
            TeamTwoScore = teamTwoScore;
        }
    }
}
