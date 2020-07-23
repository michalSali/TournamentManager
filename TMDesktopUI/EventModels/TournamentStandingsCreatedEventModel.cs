using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class TournamentStandingsCreatedEventModel
    {
        public List<TournamentStandingDisplayModel> Standings;
        public TournamentStandingsCreatedEventModel(List<TournamentStandingDisplayModel> standings)
        {
            Standings = standings;
        }
    }
}
