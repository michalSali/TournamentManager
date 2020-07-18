using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    // since the Match Creation Form is dependant on some of the Tournament attributes,
    // we have to pass it to the constructor
    public class CreateMatchEventModel
    {
        public TournamentDisplayModel Tournament;
        public CreateMatchEventModel(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
        }
    }
}
