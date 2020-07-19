using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMLibrary.Internal.DataAccess
{
    public static class TextFileDataAccess
    {        
        private static readonly string DataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TMTextFileDatabase"));
        //private static readonly string DataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\TournamentManager\TMTextFileDatabase\"));

        public static string FullFilePath(this string fileName)
        {
            return $"{DataPath}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        // ===================== LOADING & SAVING MODELS ================================


        // ==================== PLAYER MODEL ========================

        public static List<PlayerModel> ConvertToPlayerModels(this List<string> lines)
        {
            List<PlayerModel> output = new List<PlayerModel>();

            foreach (string line in lines)
            {                
                string[] columns = line.Split(';');

                PlayerModel player = new PlayerModel();
                player.Id = int.Parse(columns[0]);
                player.FirstName = columns[1];
                player.LastName = columns[2];
                player.Nickname = columns[3];
                player.Age = int.Parse(columns[4]);
                player.Role = columns[5];
                
                output.Add(player);
            }

            return output;
        }

        public static void SaveToPlayerFile(this List<PlayerModel> models, string fileName = "PlayerModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.FirstName};{m.LastName};{m.Nickname};{m.Age};{m.Role}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // =================== TEAM MODEL =====================

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines)
        {
            List<TeamModel> output = new List<TeamModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                TeamModel team = new TeamModel();
                team.Id = int.Parse(columns[0]);
                team.TeamName = columns[1];
                team.CoachName = columns[2];                

                output.Add(team);
            }

            return output;
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName = "TeamModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.TeamName};{m.CoachName}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // ===================== TEAM_MEMBER MODEL =================

        public static List<TeamMemberModel> ConvertToTeamMemberModels(this List<string> lines)
        {
            List<TeamMemberModel> output = new List<TeamMemberModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                TeamMemberModel teamMember = new TeamMemberModel();
                teamMember.Id = int.Parse(columns[0]);
                teamMember.TeamId = int.Parse(columns[1]);
                teamMember.PlayerId = int.Parse(columns[2]);

                output.Add(teamMember);
            }

            return output;
        }

        public static void SaveToTeamMemberFile(this List<TeamMemberModel> models, string fileName = "TeamMemberModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.TeamId};{m.PlayerId}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // ======================= TOURNAMENT MODEL =======================

        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines)
        {
            List<TournamentModel> output = new List<TournamentModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                TournamentModel tournament = new TournamentModel();
                tournament.Id = int.Parse(columns[0]);
                tournament.TournamentName = columns[1];
                tournament.StartDate = DateTime.Parse(columns[2]);
                tournament.EndDate = DateTime.Parse(columns[3]);
                tournament.Prizepool = int.Parse(columns[4]);

                output.Add(tournament);
            }

            return output;
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName = "TournamentModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.TournamentName};{m.StartDate.ToString("dd.MM.yyyy")};" +
                          $"{m.EndDate.ToString("dd.MM.yyyy")};{m.Prizepool}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // ===================== TOURNAMENT_ENTRY MODEL ===================

        public static List<TournamentEntryModel> ConvertToTournamentEntryModels(this List<string> lines)
        {
            List<TournamentEntryModel> output = new List<TournamentEntryModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                TournamentEntryModel tournamentEntry = new TournamentEntryModel();
                tournamentEntry.Id = int.Parse(columns[0]);
                tournamentEntry.TournamentId = int.Parse(columns[1]);
                tournamentEntry.TeamId = int.Parse(columns[2]);

                output.Add(tournamentEntry);
            }

            return output;
        }

        public static void SaveToTournamentEntryFile(this List<TournamentEntryModel> models, string fileName = "TournamentEntryModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.TournamentId};{m.TeamId}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // ====================== MATCH MODEL =========================

        public static List<MatchModel> ConvertToMatchModels(this List<string> lines)
        {
            List<MatchModel> output = new List<MatchModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                MatchModel match = new MatchModel();
                match.Id = int.Parse(columns[0]);
                match.TournamentId = int.Parse(columns[1]);
                match.MatchNumber = int.Parse(columns[2]);
                match.Date = DateTime.Parse(columns[3]);
                match.Format = int.Parse(columns[4]);
                match.TeamOneId = int.Parse(columns[5]);
                match.TeamTwoId = int.Parse(columns[6]);
                match.TeamOneScore = int.Parse(columns[7]);
                match.TeamTwoScore = int.Parse(columns[8]);

                output.Add(match);
            }

            return output;
        }

        public static void SaveToMatchFile(this List<MatchModel> models, string fileName = "MatchModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.TournamentId};{m.MatchNumber};{m.Date.ToString("dd.MM.yyyy")};" +
                          $"{m.Format};{m.TeamOneId};{m.TeamTwoId};{m.TeamOneScore};{m.TeamTwoScore}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // ================= MAP_SCORE MODEL ========================

        public static List<MapScoreModel> ConvertToMapScoreModels(this List<string> lines)
        {
            List<MapScoreModel> output = new List<MapScoreModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                MapScoreModel map = new MapScoreModel();
                map.Id = int.Parse(columns[0]);
                map.MatchId = int.Parse(columns[1]);
                map.MapNumber = int.Parse(columns[2]);
                map.MapName = columns[3];
                map.TeamOneScore = int.Parse(columns[4]);
                map.TeamTwoScore = int.Parse(columns[5]);

                output.Add(map);
            }

            return output;
        }

        public static void SaveToMapScoreFile(this List<MapScoreModel> models, string fileName = "MapScoreModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.MatchId};{m.MapNumber};{m.MapName};" +
                          $"{m.TeamOneScore};{m.TeamTwoScore}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }


        // ================== MAP_PLAYER_STATS MODEL ====================

        public static List<MapPlayerStatsModel> ConvertToMapPlayerStatsModels(this List<string> lines)
        {
            List<MapPlayerStatsModel> output = new List<MapPlayerStatsModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(';');

                MapPlayerStatsModel stats = new MapPlayerStatsModel();
                stats.Id = int.Parse(columns[0]);
                stats.MapScoreId = int.Parse(columns[1]);
                stats.PlayerId = int.Parse(columns[2]);
                stats.Kills = int.Parse(columns[3]);
                stats.Assists = int.Parse(columns[4]);
                stats.Deaths = int.Parse(columns[5]);

                output.Add(stats);
            }

            return output;
        }


        public static void SaveToMapPlayerStatsFile(this List<MapPlayerStatsModel> models, string fileName = "MapPlayerStatsModels.csv")
        {
            List<string> lines = new List<string>();

            foreach (var m in models)
            {
                lines.Add($"{m.Id};{m.MapScoreId};{m.PlayerId};{m.Kills};" +
                          $"{m.Assists};{m.Deaths}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /*
        public static List<TournamentStandingModel> ConvertToTournamentStandingModels(this List<string> lines)
        {
            
        }
        */


    }
}
