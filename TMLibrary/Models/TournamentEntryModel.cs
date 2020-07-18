using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class TournamentEntryModel
    {
        public int Id;
        public int TournamentId;
        public int TeamId;

        public TournamentEntryModel(int tournamentId, int teamId)
        {
            TournamentId = tournamentId;
            TeamId = teamId;
        }

        public TournamentEntryModel() { }
    }
}
