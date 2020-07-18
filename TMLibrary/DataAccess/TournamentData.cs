using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess
{
    // tournament and tournamentEntry data access
    public class TournamentData
    {
        private SqlDataAccess sql;

        public TournamentData()
        {
            sql = new SqlDataAccess();
        }

        // make one-liners at refactoring
        public List<TournamentModel> GetAllTournaments()
        {
            var output = sql.LoadData<TournamentModel, dynamic>("dbo.spTournament_GetAll", new { }, "TMData");
            return output;
            //return sql.LoadData<TournamentModel, dynamic>("dbo.spPlayer_GetAll", new { }, "TMData");
        }

        public bool ExistsTournament(string tournamentName)
        {
            var p = new
            {
                TournamentName = tournamentName
            };
            var output = sql.LoadData<TournamentModel, dynamic>("dbo.spTournament_GetByName", p, "TMData");

            return output.Count > 0;
        }

        public void CreateTournament(TournamentModel tournament)
        {
            var p = new
            {
                tournament.TournamentName,
                tournament.StartDate,
                tournament.EndDate,
                tournament.Prizepool
            };

            sql.SaveData<dynamic>("dbo.spTournament_Create", p, "TMData");
        }

        public TournamentModel CreateTournamentReturnModel(TournamentModel tournament)
        {
            var p = new
            {
                tournament.TournamentName,
                tournament.StartDate,
                tournament.EndDate,
                tournament.Prizepool
            };
            
            tournament.Id = sql.SaveDataReturnId<dynamic>("dbo.spTournament_CreateReturnId", p, "TMData");           
            return tournament;
        }

        public int CreateTournamentReturnId(TournamentModel tournament)
        {
            var p = new
            {
                tournament.TournamentName,
                tournament.StartDate,
                tournament.EndDate,
                tournament.Prizepool
            };

            int id = sql.SaveDataReturnId<dynamic>("dbo.spTournament_CreateReturnId", p, "TMData");
            return id;
        }


        public List<TournamentEntryModel> GetTournamentEntries(int tournamentId)
        {
            var p = new { TournamentId = tournamentId };
            var output = sql.LoadData<TournamentEntryModel, dynamic>("dbo.spTournamentEntry_GetByTournamentId", p, "TMData");
            return output;
        }

        public void CreateTournamentEntry(TournamentEntryModel tournamentEntry)
        {
            var p = new
            {
                tournamentEntry.TournamentId,
                tournamentEntry.TeamId                
            };

            sql.SaveData<dynamic>("dbo.spTournamentEntry_Create", p, "TMData");
        }

    }
}
