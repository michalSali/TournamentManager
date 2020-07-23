using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class CreateMatchSpecificationsEventModel
    {
        public TournamentDisplayModel Tournament;
        public CreateMatchSpecificationsEventModel(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
        }
    }    
}
