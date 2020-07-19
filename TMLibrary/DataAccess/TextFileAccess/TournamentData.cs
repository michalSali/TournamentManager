using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TMLibrary.Internal.DataAccess.TextFileDataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.TextFileAccess
{
    public class TournamentData
    {
        private static readonly string TournamentFileName = "TournamentModels.csv";
        private static readonly string TournamentEntryFileName = "TournamentEntryModels.csv";

        public TournamentData() { }        

        // make one-liners at refactoring
        public List<TournamentModel> GetAllTournaments()
        {
            var output = TournamentFileName.FullFilePath().LoadFile().ConvertToTournamentModels();
            return output;
        }

        public bool ExistsTournament(string tournamentName)
        {
            var output = GetAllTournaments().Where(x => x.TournamentName == tournamentName).ToList();            
            return output.Count > 0;
        }
        

        public int CreateTournamentReturnId(TournamentModel tournament)
        {
            var tournaments = GetAllTournaments();

            int newId = 1;

            if (tournaments.Count > 0)
            {
                newId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            tournament.Id = newId;
            tournaments.Add(tournament);

            tournaments.SaveToTournamentFile(TournamentFileName);
            return newId;
        }

        public List<TournamentEntryModel> GetAllTournamentEntries()
        {
            var output = TournamentEntryFileName.FullFilePath().LoadFile().ConvertToTournamentEntryModels();
            return output;
        }
        public List<TournamentEntryModel> GetTournamentEntries(int tournamentId)
        {
            var output = GetAllTournamentEntries().Where(x => x.TournamentId == tournamentId).ToList();
            return output;
        }

        public void CreateTournamentEntry(TournamentEntryModel tournamentEntry)
        {
            var tournamentEntries = GetAllTournamentEntries();

            int newId = 1;

            if (tournamentEntries.Count > 0)
            {
                newId = tournamentEntries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            tournamentEntry.Id = newId;
            tournamentEntries.Add(tournamentEntry);

            tournamentEntries.SaveToTournamentEntryFile(TournamentEntryFileName);            
        }
    }
}
