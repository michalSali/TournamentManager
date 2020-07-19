using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class DisplayTeamEventModel
    {
        public TournamentDisplayModel Tournament;
        public TeamDisplayModel Team;
        public DisplayTeamEventModel(TournamentDisplayModel tournament, TeamDisplayModel team)
        {
            Tournament = tournament;
            Team = team;
        }
    }
}
