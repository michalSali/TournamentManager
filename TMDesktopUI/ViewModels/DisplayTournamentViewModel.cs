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
        
        private TournamentDisplayModel _tournament;
        public TournamentDisplayModel Tournament
        {
            get { return _tournament; }
            set
            {
                _tournament = value;
                NotifyOfPropertyChange(() => Tournament);
            }
        }
        /*
        private List<MatchDisplayModel> _matches;
        public List<MatchDisplayModel> Matches
        {

        }
        */

        private IEventAggregator _events;

        public DisplayTournamentViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            //Teams = new BindingList<TeamDisplayModel>(tournament.Teams);
                

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

        // SelectedTeam
        // SelectedMatch
    }
}
