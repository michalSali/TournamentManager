using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class CreateTournamentStandingsEventModel
    {
        public TournamentDisplayModel Tournament;
        public CreateTournamentStandingsEventModel(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
        }
    }
}
