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
        public TournamentDisplayModel Tournament { get; set; }  // tournamentId
        public TeamDisplayModel Team { get; set; }  // teamId

        public int Placement { get; set; }
        public int PrizeWon { get; set; }

        public string Description
        {
            get { return $"{Team.TeamName} - #{Placement} - ${PrizeWon}"; }
        }
    }
}
