using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.SqlAccess
{
    // match data access
    public class MatchData
    {
        private SqlDataAccess sql;

        public MatchData()
        {
            sql = new SqlDataAccess();
        }

        public List<MatchModel> GetTournamentMatches(int tournamentId)
        {            
            var p = new { TournamentId = tournamentId };
            var output = sql.LoadData<MatchModel, dynamic>("dbo.spMatch_GetByTournamentId", p, "TMData");           
            return output;
        }

        public void CreateMatch(MatchModel match)
        {
            var p = new
            {
                match.TournamentId,
                match.MatchNumber,
                match.Date,
                match.Format,
                match.TeamOneId,
                match.TeamTwoId,
                match.TeamOneScore,
                match.TeamTwoScore                
            };

            sql.SaveData<dynamic>("dbo.spMatch_Create", p, "TMData");
        }

        public MatchModel CreateMatchReturnModel(MatchModel match)
        {
            var p = new
            {
                match.TournamentId,
                match.MatchNumber,
                match.Date,
                match.Format,
                match.TeamOneId,
                match.TeamTwoId,
                match.TeamOneScore,
                match.TeamTwoScore
            };

            match.Id = sql.SaveDataReturnId<dynamic>("dbo.spMatch_CreateReturnId", p, "TMData");           
            return match;
        }

        public int CreateMatchReturnId(MatchModel match)
        {
            var p = new
            {
                match.TournamentId,
                match.MatchNumber,
                match.Date,
                match.Format,
                match.TeamOneId,
                match.TeamTwoId,
                match.TeamOneScore,
                match.TeamTwoScore
            };

            int id = sql.SaveDataReturnId<dynamic>("dbo.spMatch_CreateReturnId", p, "TMData");
            return id;
        }
    }
}
