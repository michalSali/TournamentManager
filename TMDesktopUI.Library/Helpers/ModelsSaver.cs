using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;
using TMLibrary.DataAccess.SqlAccess;
//using TMLibrary.DataAccess.TextFileAccess;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Helpers
{
    public class ModelsSaver
    {

        private PlayerData pd;
        private MatchData mtd;
        private MapData mpd;
        private TournamentData tnd;
        private TeamData tmd;

        public ModelsSaver()
        {
            pd = new PlayerData();
            mtd = new MatchData();
            mpd = new MapData();
            tnd = new TournamentData();
            tmd = new TeamData();
        }

        // change to async
        public void SaveTournament(TournamentDisplayModel tournament)
        {
            int tournamentId = tnd.CreateTournamentReturnId(new TournamentModel(
                tournament.TournamentName,
                tournament.StartDate,
                tournament.EndDate,
                tournament.Prizepool));

            // teams and players themselves are already independently saved
            foreach (var team in tournament.Teams)
            {
                tnd.CreateTournamentEntry(new TournamentEntryModel(tournamentId, team.Id));
            }

            //int matchId;
            //int mapScoreId;
            
            foreach (var match in tournament.Matches) 
            {
                int matchId = mtd.CreateMatchReturnId(new MatchModel(
                    tournamentId,
                    match.MatchNumber,
                    match.Date,
                    match.Format,             
                    match.TeamOne.Id,
                    match.TeamTwo.Id,
                    match.TeamOneScore,
                    match.TeamTwoScore));

                foreach (var mapScore in match.Maps) 
                {
                    int mapScoreId = mpd.CreateMapScoreReturnId(new MapScoreModel(
                        matchId,
                        mapScore.MapNumber,
                        mapScore.MapName,
                        mapScore.TeamOneScore,
                        mapScore.TeamTwoScore));

                    foreach (var playerStats in mapScore.TeamOneStats) {
                        mpd.CreateMapPlayerStats(new MapPlayerStatsModel(
                            mapScoreId,
                            playerStats.Player.Id,
                            playerStats.Kills,
                            playerStats.Assists,
                            playerStats.Deaths));                        
                    }

                    foreach (var playerStats in mapScore.TeamTwoStats)
                    {
                        mpd.CreateMapPlayerStats(new MapPlayerStatsModel(
                            mapScoreId,
                            playerStats.Player.Id,
                            playerStats.Kills,
                            playerStats.Assists,
                            playerStats.Deaths));
                    }
                }

            }
            // save tournament and get id
            // 
        }

        // saving players is independent to saving teams (players have to be saved to database first,
        // as we need to know the player's id to create "TeamMember" connection with a team
        public void SaveTeam(TeamDisplayModel team)
        {           
            team.Id = tmd.CreateTeamReturnId(new TeamModel(team.TeamName, team.CoachName));

            foreach (var player in team.Players)
            {
                tmd.CreateTeamMember(new TeamMemberModel(team.Id, player.Id));                
            }               
        }

        public void SavePlayer(PlayerDisplayModel player)
        {            
            player.Id = pd.CreatePlayerReturnId(new PlayerModel(
                player.FirstName,
                player.LastName,
                player.Nickname,
                player.Age,
                player.Role));            
        }

        /*
        public PlayerDisplayModel SavePlayer(PlayerDisplayModel player)
        {
            UserDataAccess da = new UserDataAccess();
            player.Id = da.SavePlayer(new PlayerModel(
                player.FirstName,
                player.LastName,
                player.Nickname,
                player.Age,
                player.Role));

            return player;
        }
        */

    }
}
