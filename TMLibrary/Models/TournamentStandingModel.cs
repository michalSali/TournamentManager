using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class TournamentStandingModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }        
        public int Placement { get; set; }
        public int PrizeWon { get; set; }       
        
        public TournamentStandingModel(int tournamentId, int teamId, int placement, int prizeWon)
        {
            TournamentId = tournamentId;
            TeamId = teamId;
            Placement = placement;
            PrizeWon = prizeWon;
        }

        public TournamentStandingModel() { }
    }
}
