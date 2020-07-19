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
    public class DisplayPlayerViewModel : Screen
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

        private PlayerDisplayModel _player;
        public PlayerDisplayModel Player
        {
            get { return _player; }
            set
            {
                _player = value;
                NotifyOfPropertyChange(() => Player);
            }
        }


        private IEventAggregator _events;

        public DisplayPlayerViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament, PlayerDisplayModel player)
        {
            Tournament = tournament;
            Player = player;
        }

        public void ReturnToTournamentViewer()
        {
            _events.PublishOnCurrentThread(new ReturnToTeamViewerEvent());
        }
    }
}
