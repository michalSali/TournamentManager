using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class DisplayTournamentStatsViewModel : Screen
    {
        public TournamentDisplayModel Tournament;

        public DisplayTournamentStatsViewModel(TournamentDisplayModel tournament)
        {
            Tournament = tournament;


            //tournament.Matches;
        }


    }
}
