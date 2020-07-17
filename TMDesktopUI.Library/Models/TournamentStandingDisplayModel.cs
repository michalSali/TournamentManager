using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDesktopUI.Library.Models
{
    public class TournamentStandingDisplayModel
    {
        // id - not in display model
        public TournamentDisplayModel Tournament;  // tournamentId
        public TeamDisplayModel Team;  // teamId

        public int Placement;
        public int PrizeWon;
    }
}
