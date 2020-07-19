using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class DisplayPlayerEventModel
    {
        public TournamentDisplayModel Tournament;
        public PlayerDisplayModel Team;
        public DisplayPlayerEventModel(TournamentDisplayModel tournament, PlayerDisplayModel team)
        {
            Tournament = tournament;
            Team = team;
        }
    }
}
