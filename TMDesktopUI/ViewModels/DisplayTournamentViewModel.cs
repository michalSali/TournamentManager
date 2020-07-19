using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Windows;
using TMDesktopUI.EventModels;

namespace TMDesktopUI.ViewModels
{
    public class DisplayTournamentViewModel : Screen
    {

        private IEventAggregator _events;

        public DisplayTournamentViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
        }

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
        

        public bool CanViewTeam
        {
            get { return SelectedTeam != null; }
        }

        public void ViewTeam()
        {
            _events.PublishOnCurrentThread(new DisplayTeamEventModel(Tournament, SelectedTeam));
        }

        public bool CanViewMatch
        {
            get { return SelectedMatch != null; }
        }

        public void ViewMatch()
        {
            _events.PublishOnCurrentThread(new DisplayMatchEventModel(Tournament, SelectedMatch));
        }
        

        private TeamDisplayModel _selectedTeam;
        public TeamDisplayModel SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                NotifyOfPropertyChange(() => SelectedTeam);
                NotifyOfPropertyChange(() => CanViewTeam);
            }
        }

        private MatchDisplayModel _selectedMatch;
        public MatchDisplayModel SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                _selectedMatch = value;
                NotifyOfPropertyChange(() => SelectedMatch);
                NotifyOfPropertyChange(() => CanViewMatch);
            }
        }

        public void ReturnToMainScreen()
        {
            _events.PublishOnCurrentThread(new ReturnToMainScreenEvent());
        }
       
    }
}
