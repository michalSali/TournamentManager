using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class DisplayMatchEventModel
    {
        public TournamentDisplayModel Tournament;
        public MatchDisplayModel Match;
        public DisplayMatchEventModel(TournamentDisplayModel tournament, MatchDisplayModel match)
        {
            Tournament = tournament;
            Match = match;
        }
    }
}
