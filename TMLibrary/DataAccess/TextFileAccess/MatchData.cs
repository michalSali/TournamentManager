using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using static TMLibrary.Internal.DataAccess.TextFileDataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.TextFileAccess
{
    public class MatchData
    {
        private static readonly string MatchFileName = "MatchModels.csv";

        public MatchData() { }                   
        
        public List<MatchModel> GetAllMatches()
        {
            var output = MatchFileName.FullFilePath().LoadFile().ConvertToMatchModels();
            return output;
        }

        public List<MatchModel> GetTournamentMatches(int tournamentId)
        {
            var output = GetAllMatches().Where(x => x.TournamentId == tournamentId).ToList();
            return output;
        }
        

        public int CreateMatchReturnId(MatchModel match)
        {
            var matches = GetAllMatches();

            int newId = 1;

            if (matches.Count > 0)
            {
                newId = matches.OrderByDescending(x => x.Id).First().Id + 1;
            }

            match.Id = newId;
            matches.Add(match);

            matches.SaveToMatchFile(MatchFileName);
            return newId;
        }
    }
}
