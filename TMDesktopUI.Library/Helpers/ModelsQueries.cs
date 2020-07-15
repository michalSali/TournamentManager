using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;
using TMLibrary.DataAccess;

namespace TMDesktopUI.Library.Helpers
{
    public class ModelsQueries
    {
        private PlayerData _pd;
        private TeamData _tmd;
        private TournamentData _tnd;

        private ModelsLoader _loader;
        public ModelsQueries()
        {
            _pd = new PlayerData();
            _tmd = new TeamData();
            _tnd = new TournamentData();
        }
        
        public bool ExistsPlayer(string firstName, string lastName, string nickname)
        {
            return _pd.ExistsPlayer(firstName, lastName, nickname);
        }

        // checks if there is already a team with the specific name and
        //   consisting of the same players
        // finds teams that have the same name, and then compares sets of players id's        
        public bool ExistsTeam(string teamName, List<PlayerDisplayModel> players)
        {            
            var teams = _loader.GetTeamsByName(teamName);
            var newTeamIds = players.Select(player => player.Id).ToHashSet();

            foreach (var team in teams)
            {
                var existingTeamIds = team.Players.Select(player => player.Id).ToHashSet();
                if (newTeamIds.SetEquals(existingTeamIds))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ExistsTournament(string tournamentName)
        {
            return _tnd.ExistsTournament(tournamentName);
        }
    }
}
