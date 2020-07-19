using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;
using TMLibrary.DataAccess;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Helpers
{
    public class ModelsLoader
    {
        private PlayerData pd;
        private MatchData mtd;
        private MapData mpd;
        private TournamentData tnd;
        private TeamData tmd;

        public ModelsLoader()
        {
            pd = new PlayerData();
            mtd = new MatchData();
            mpd = new MapData();
            tnd = new TournamentData();
            tmd = new TeamData();
        }

        public List<TournamentDisplayModel> GetAllTournaments()
        {
            List<TournamentDisplayModel> result = new List<TournamentDisplayModel>();
            List<TournamentModel> tournamentModels = tnd.GetAllTournaments();

            foreach (var tournamentModel in tournamentModels)
            {
                TournamentDisplayModel newDisplayModel = new TournamentDisplayModel(tournamentModel);
                newDisplayModel.Teams = GetTournamentTeams(tournamentModel.Id);
                newDisplayModel.Matches = GetTournamentMatches(tournamentModel.Id, newDisplayModel);
                result.Add(newDisplayModel);
            }

            return result;
        }

        public List<MatchDisplayModel> GetTournamentMatches(int tournamentId, TournamentDisplayModel tournament)
        {            
            List<MatchDisplayModel> result = new List<MatchDisplayModel>();
            List<MatchModel> matchModels = mtd.GetTournamentMatches(tournamentId);

            foreach (var matchModel in matchModels)
            {
                MatchDisplayModel newDisplayModel = new MatchDisplayModel(matchModel);
                newDisplayModel.Tournament = tournament;
                newDisplayModel.TeamOne = GetTeam(matchModel.TeamOneId);
                newDisplayModel.TeamTwo = GetTeam(matchModel.TeamTwoId);
                newDisplayModel.Maps = GetMatchMaps(matchModel.Id, newDisplayModel);
                result.Add(newDisplayModel);
            }

            return result;
        }

        public List<MapScoreDisplayModel> GetMatchMaps(int matchId, MatchDisplayModel match)
        {           
            List<MapScoreDisplayModel> result = new List<MapScoreDisplayModel>();
            List<MapScoreModel> mapScoreModels = mpd.GetMapScores(matchId);

            foreach (var mapScoreModel in mapScoreModels)
            {
                MapScoreDisplayModel newDisplayModel = new MapScoreDisplayModel(mapScoreModel);
                newDisplayModel.Match = match;

                List<MapPlayerStatsDisplayModel> mapStats = GetMapPlayerStats(mapScoreModel.Id);
                newDisplayModel.TeamOneStats = mapStats.Where(stats => match.TeamOne.Players.Contains(stats.Player)).ToList();
                newDisplayModel.TeamTwoStats = mapStats.Where(stats => match.TeamTwo.Players.Contains(stats.Player)).ToList();
                //newDisplayModel.TeamOneStats = GetMapsStats(mapScoreModel.Id);
                //newDisplayModel.TeamTwoStats = ...;
                result.Add(newDisplayModel);
            }

            return result;
        }

        public List<MapPlayerStatsDisplayModel> GetMapPlayerStats(int mapScoreId)
        {
            List<MapPlayerStatsModel> mapPlayerStatsModels = mpd.GetMapPlayerStats(mapScoreId);
            List<MapPlayerStatsDisplayModel> result = new List<MapPlayerStatsDisplayModel>();
            foreach (var model in mapPlayerStatsModels)
            {
                MapPlayerStatsDisplayModel newDisplayModel = new MapPlayerStatsDisplayModel(model);
                newDisplayModel.Player = GetPlayer(model.PlayerId);
                result.Add(newDisplayModel);
            }

            return result;
        }

        public List<PlayerDisplayModel> GetTournamentTeams(int tournamentId)
        {
            List<PlayerDisplayModel> result = new List<PlayerDisplayModel>();

            var tournamentEntryModels = tnd.GetTournamentEntries(tournamentId);
            
            foreach (var tournamentEntry in tournamentEntryModels)
            {
                result.Add(GetTeam(tournamentEntry.TeamId));
            }

            return result;
        }

        public PlayerDisplayModel GetTeam(int teamId)
        {
            var teamModel = tmd.GetTeam(teamId);
            List<TeamMemberModel> teamMemberModels = tmd.GetTeamMembers(teamId);
            PlayerDisplayModel result = new PlayerDisplayModel(teamModel);
           
            foreach (var memberModel in teamMemberModels)
            {
                result.Players.Add(GetPlayer(memberModel.PlayerId, result));
            }

            return result;
        }

        public PlayerDisplayModel GetPlayer(int playerId, PlayerDisplayModel team)
        {
            var playerModel = pd.GetPlayer(playerId);
            PlayerDisplayModel newDisplayModel = new PlayerDisplayModel(playerModel);
            newDisplayModel.Team = team;
            return newDisplayModel;           
        }

        // if we dont search for a player within tournament, or a specific match, the player
        // does not have assigned a team (one player can be a part of several teams - roster changes)
        public PlayerDisplayModel GetPlayer(int playerId)  //, TeamDisplayModel team)
        {
            var playerModel = pd.GetPlayer(playerId);
            PlayerDisplayModel newDisplayModel = new PlayerDisplayModel(playerModel);
            newDisplayModel.Id = playerId;
            return newDisplayModel;
            //newDisplayModel.Team = team;
        }
        
        public List<PlayerDisplayModel> GetAllPlayers()
        {
            List<PlayerDisplayModel> result = new List<PlayerDisplayModel>();
            List<PlayerModel> playerModels = pd.GetAllPlayers();
            
            foreach (var playerModel in playerModels)
            {
                result.Add(new PlayerDisplayModel(playerModel));
            }

            return result;
        }

        public List<PlayerDisplayModel> GetAllTeams()
        {
            List<PlayerDisplayModel> result = new List<PlayerDisplayModel>();           
            List<TeamMemberModel> teamMemberModels = tmd.GetAllTeamMembers();
           
            var groupedPlayers = teamMemberModels.GroupBy(
                model => model.TeamId,
                model => model.PlayerId,
                (key, g) => new { TeamId = key, PlayerIds = g.ToList() });

            foreach (var group in groupedPlayers)
            {
                var teamModel = tmd.GetTeam(group.TeamId);
                PlayerDisplayModel newTeam = new PlayerDisplayModel(teamModel);
                newTeam.Id = group.TeamId;
                foreach (var playerId in group.PlayerIds)
                {
                    newTeam.Players.Add(GetPlayer(playerId, newTeam));
                }

                result.Add(newTeam);
            }

            return result;                        
        }

        public List<PlayerDisplayModel> GetTeamsByName(string teamName)
        {
            var teams = tmd.GetTeamsByName(teamName);
            List<PlayerDisplayModel> result = new List<PlayerDisplayModel>();

            foreach (var team in teams)
            {
                result.Add(GetTeam(team.Id));
            }

            return result;
        }
    }
}
