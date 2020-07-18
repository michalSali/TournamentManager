using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class MapScoreDisplayModel
    {
        //public int Id { get; set; }
        //public int MatchId { get; set; }
        public int MapNumber { get; set; }
        public string MapName { get; set; }
        public int TeamOneScore { get; set; }
        public int TeamTwoScore { get; set; }

        public string MapDescription
        {
            get
            {
                //return $"Map {MapNumber}: {MapName}";
                return $"Map: {MapName}";
            }
        }

        public string ScoreDescription
        {
            get
            {
                return $"{Match.TeamOne.TeamName} - {TeamOneScore} : "
                        + $"{TeamTwoScore} - {Match.TeamTwo.TeamName}";
            }
        }

        public MatchDisplayModel Match { get; set; }

        //public TeamDisplayModel TeamOne;  // na TeamOne a TeamTwo sa da dostat aj cez Match => Match.TeamOne
        //public TeamDisplayModel TeamTwo;

        public List<MapPlayerStatsDisplayModel> TeamOneStats { get; set; } = new List<MapPlayerStatsDisplayModel>();
        public List<MapPlayerStatsDisplayModel> TeamTwoStats { get; set; } = new List<MapPlayerStatsDisplayModel>();

        public MapScoreDisplayModel(MapScoreModel model)
        {
            MapNumber = model.MapNumber;
            MapName = model.MapName;
            TeamOneScore = model.TeamOneScore;
            TeamTwoScore = model.TeamTwoScore;
        }

        public MapScoreDisplayModel()
        {
        }
    }
}
