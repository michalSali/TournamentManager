﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.Library.Exporters
{
    // exporters have to be located inside of the TMDesktopUI.Library, since it references TMLibrary
    // if we put exporters inside TMLibrary, we could not use DisplayModels (such as the one for the tournament), as using
    //   a reference to TMDesktopUI.Library would create a cyclic dependency
    public static class TournamentExporter
    {
        private static readonly string DataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\ExportedFiles"));        

        public static string FullFilePath(this string fileName)
        {
            return $"{DataPath}\\{fileName}";
        }

        public static string GetTournamentFileName(this string tournamentName)
        {
            string fileName = tournamentName.Replace(" ", "_");
            return $"{fileName}.txt";
        }
       
        public static void ExportTournament(this TournamentDisplayModel tournament, string fileName)
        {
            List<string> lines = new List<string>();

            lines.Add("=========================");
            lines.Add(">> TOURNAMENT OVERVIEW <<");
            lines.Add("=========================");
            lines.Add(string.Empty);

            lines.AddTournamentInfo(tournament);
            lines.AddTeamRelatedInfo(tournament);
                                    
            

        }

        private static void AddTournamentInfo(this List<string> lines, TournamentDisplayModel tournament)
        {
            lines.Add($"Tournament name: {tournament.TournamentName}");
            lines.Add($"Start date: {tournament.StartDateFormatted}");
            lines.Add($"End date: {tournament.EndDateFormatted}");
            lines.Add($"Prizepool: {tournament.Prizepool}");
            lines.Add(string.Empty);
        }

        private static void AddTeamRelatedInfo(this List<string> lines, TournamentDisplayModel tournament)
        {
            lines.Add("Participating teams:");
            foreach (var team in tournament.Teams)
            {
                lines.Add($">> {team.TeamName}");
            }

            lines.Add(string.Empty);
            lines.Add("Tournament final standings:");
            if (tournament.Standings?.Count == 0)
            {
                lines.Add(" -- No standings have been recorded. -- ");
            }
            else
            {
                foreach (var standing in tournament.Standings.OrderBy(x => x.Placement).ToList())
                {
                    lines.Add($"{standing.Description}");
                }
            }

            lines.Add(string.Empty);
        }

        private static void AddMatchRelatedInfo(this List<string> lines, TournamentDisplayModel tournament)
        {
            lines.Add("Tournament matches:");
            if (tournament.Matches?.Count == 0)
            {
                lines.Add(" -- No standings have been recorded. -- ");
            }
            else
            {
                lines.Add("Groupstage / unlabeled matches:");
                var groupStageMatches = tournament.Matches?.Where(x => x.MatchImportance == 0).ToList();
                if (groupStageMatches?.Count == 0)
                {
                    lines.Add(" -- No group stage / unlabeled matches have been recorded. -- ");
                } else
                {
                    foreach (var match in groupStageMatches)
                    {
                        lines.Add(match.MatchInfo);
                    }
                }

                lines.Add("Quarterfinal matches:");
                var quarterfinalMatches = tournament.Matches?.Where(x => x.MatchImportance == 0).ToList();
                if (groupStageMatches?.Count == 0)
                {
                    lines.Add(" -- No group stage / unlabeled matches have been recorded. -- ");
                }
                else
                {
                    foreach (var match in groupStageMatches)
                    {
                        lines.Add(match.MatchInfo);
                    }
                }
            }
        }
    }
}
