using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class DisplayMatchViewModel : Conductor<object>
    {

        private MatchDisplayModel _match;
        public List<MapPlayerStatsDisplayModel> OverallTeamOneStats;
        public List<MapPlayerStatsDisplayModel> OverallTeamTwoStats;

        public DisplayMatchViewModel(MatchDisplayModel match)
        {
            _match = match;
            GetOverallStats();
        }

        public void GetOverallStats()
        {
            // setup new List using list in the first map
            // we set K/D/A to 0, and remember the player instance
            var teamOneStats = _match.Maps[0].TeamOneStats;
            foreach (var playerStats in teamOneStats)
            {
                OverallTeamOneStats.Add(
                    new MapPlayerStatsDisplayModel(0, 0, 0, 0, 0, 0, playerStats.Player));
            }

            var teamTwoStats = _match.Maps[0].TeamTwoStats;
            foreach (var playerStats in teamTwoStats)
            {
                OverallTeamOneStats.Add(
                    new MapPlayerStatsDisplayModel(0, 0, 0, 0, 0, 0, playerStats.Player));
            }

            for (int i = 0; i < teamOneStats.Count; ++i)
            {
                OverallTeamOneStats[i].Kills += teamOneStats[i].Kills;
                OverallTeamOneStats[i].Assists += teamOneStats[i].Assists;
                OverallTeamOneStats[i].Deaths += teamOneStats[i].Deaths;
            }

            for (int i = 0; i < teamTwoStats.Count; ++i)
            {
                OverallTeamTwoStats[i].Kills += teamTwoStats[i].Kills;
                OverallTeamTwoStats[i].Assists += teamTwoStats[i].Assists;
                OverallTeamTwoStats[i].Deaths += teamTwoStats[i].Deaths;
            }
        }

        public MapScoreDisplayModel SelectedMap;
        public void Handler(DisplayMatch)
    }
}
