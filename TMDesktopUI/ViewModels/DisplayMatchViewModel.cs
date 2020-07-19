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
    public class DisplayMatchViewModel : Screen
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

        private MatchDisplayModel _match;
        public MatchDisplayModel Match
        {
            get { return _match; }
            set
            {
                _match = value;
                NotifyOfPropertyChange(() => Match);
            }
        }


        private IEventAggregator _events;

        public DisplayMatchViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament, MatchDisplayModel match)
        {
            Tournament = tournament;
            Match = match;
        }

        public bool CanViewMap
        {
            get { return SelectedMap != null; }
        }

        public void ViewMap()
        {
            _events.PublishOnCurrentThread(new DisplayMapEventModel(Tournament, SelectedMap));
        }




        private PlayerDisplayModel _selectedMap;
        public PlayerDisplayModel SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                _selectedMap = value;
                NotifyOfPropertyChange(() => SelectedMap);
                NotifyOfPropertyChange(() => CanViewMap);
            }
        }

        public void ReturnToTournamentViewer()
        {
            _events.PublishOnCurrentThread(new ReturnToMatchViewerEvent());
        }
    }
}
