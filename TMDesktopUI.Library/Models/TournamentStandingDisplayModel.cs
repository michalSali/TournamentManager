using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDesktopUI.Library.Models
{
    public class TournamentStandingDisplayModel
    {        
        public TournamentDisplayModel Tournament { get; set; }
        public TeamDisplayModel Team { get; set; }
        public int Placement { get; set; }
        public int PrizeWon { get; set; }
        
        public string Description
        {
            get { return $"#{Placement} - {Team.TeamName} - ${PrizeWon}"; }
        }
    }
}
