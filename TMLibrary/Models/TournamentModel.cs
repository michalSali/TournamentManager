using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class TournamentModel
    {
        public int Id;
        public string TournamentName;
        public DateTime StartDate;
        public DateTime EndDate;
        public int Prizepool;

        public TournamentModel(string tournamentName, DateTime startDate, DateTime endDate, int prizepool)
        {
            TournamentName = tournamentName;
            StartDate = startDate;
            EndDate = endDate;
            Prizepool = prizepool;
        }
    }
}
