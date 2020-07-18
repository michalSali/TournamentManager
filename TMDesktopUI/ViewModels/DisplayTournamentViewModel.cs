using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Windows;

namespace TMDesktopUI.ViewModels
{
    public class DisplayTournamentViewModel : Conductor<object>
    {
        public TournamentDisplayModel Tournament;
        public DisplayTournamentViewModel(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            //Teams = new BindingList<TeamDisplayModel>(tournament.Teams);
            //TournamentName = tournament.TournamentName;            

        }

        //public BindingList<TeamDisplayModel> Teams;
        public string _tournamentName;
        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
            }
        }
    }
}
