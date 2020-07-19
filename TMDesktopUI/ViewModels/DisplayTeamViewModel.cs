using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class DisplayTeamViewModel : Screen
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

        private PlayerDisplayModel _team;
        public PlayerDisplayModel Team
        {
            get { return _team; }
            set
            {
                _team = value;
                NotifyOfPropertyChange(() => Team);
            }
        }
       

        private IEventAggregator _events;

        public DisplayTeamViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament, PlayerDisplayModel team)
        {
            Tournament = tournament;
            Team = team;           
        }

        public bool CanViewPlayer
        {
            get { return SelectedPlayer != null; }
        }

        public void ViewPlayer()
        {
            _events.PublishOnCurrentThread(new DisplayPlayerEventModel(Tournament, SelectedPlayer));
        }

       
        

        private PlayerDisplayModel _selectedPlayer;
        public PlayerDisplayModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                NotifyOfPropertyChange(() => SelectedPlayer);
                NotifyOfPropertyChange(() => CanViewPlayer);
            }
        }

        public void ReturnToTournamentViewer()
        {
            _events.PublishOnCurrentThread(new ReturnToTournamentViewerEvent());
        }
    }
}
