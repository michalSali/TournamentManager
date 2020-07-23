using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{

    public class CreateMatchSpecificationsViewModel : Screen
    {


        private IEventAggregator _events;

        public CreateMatchSpecificationsViewModel(IEventAggregator events)
        {
            _events = events;
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

        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            Matches = new BindingList<MatchDisplayModel>(tournament.Matches);

            QuarterfinalMatches = new BindingList<MatchDisplayModel>();
            SemifinalMatches = new BindingList<MatchDisplayModel>();
            FinalMatches = new BindingList<MatchDisplayModel>();
        }


        private BindingList<MatchDisplayModel> _matches;
        public BindingList<MatchDisplayModel> Matches
        {
            get { return _matches; }
            set
            {
                _matches = value;
                NotifyOfPropertyChange(() => Matches);
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
                NotifyOfPropertyChange(() => CanAddQuarterfinalMatch);
                NotifyOfPropertyChange(() => CanAddSemifinalMatch);
                NotifyOfPropertyChange(() => CanAddFinalMatch);
                NotifyOfPropertyChange(() => CanRemoveQuarterfinalMatch);
                NotifyOfPropertyChange(() => CanRemoveSemifinalMatch);
                NotifyOfPropertyChange(() => CanRemoveFinalMatch);
            }
        }


        private BindingList<MatchDisplayModel> _quarterfinalMatches;
        public BindingList<MatchDisplayModel> QuarterfinalMatches
        {
            get { return _quarterfinalMatches; }
            set
            {
                _quarterfinalMatches = value;
                NotifyOfPropertyChange(() => QuarterfinalMatches);
            }
        }

        private BindingList<MatchDisplayModel> _semifinalMatches;
        public BindingList<MatchDisplayModel> SemifinalMatches
        {
            get { return _semifinalMatches; }
            set
            {
                _semifinalMatches = value;
                NotifyOfPropertyChange(() => SemifinalMatches);
            }
        }

        private BindingList<MatchDisplayModel> _finalMatches;
        public BindingList<MatchDisplayModel> FinalMatches
        {
            get { return _finalMatches; }
            set
            {
                _finalMatches = value;
                NotifyOfPropertyChange(() => FinalMatches);
            }
        }

        /*
        private MatchDisplayModel _consolidationFinalMatch;
        public MatchDisplayModel ConsolidationFinalMatch
        {
            get { return _consolidationFinalMatch; }
            set
            {
                _consolidationFinalMatch = value;
                NotifyOfPropertyChange(() => ConsolidationFinalMatch);
            }
        }
        */


        // FINAL 


        public void SaveSpecification()
        {
            List<MatchDisplayModel> newMatches = new List<MatchDisplayModel>(Matches);

            foreach (var match in QuarterfinalMatches)
            {
                match.MatchImportance = 1;
                newMatches.Add(match);
            }

            foreach (var match in SemifinalMatches)
            {
                match.MatchImportance = 2;
                newMatches.Add(match);
            }

            foreach (var match in FinalMatches)
            {
                match.MatchImportance = 3;
                newMatches.Add(match);
            }

            _events.PublishOnUIThread(new MatchSpecificationsCreatedEventModel(newMatches));
        }

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }

        public bool CanAddFinalMatch
        {
            get { return SelectedMatch != null && Matches.Contains(SelectedMatch) && FinalMatches?.Count == 0; }
        }

        public void AddFinalMatch()
        {
            FinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveFinalMatch
        {
            get { return SelectedMatch != null && FinalMatches.Contains(SelectedMatch); }
        }

        public void RemoveFinalMatch()
        {
            Matches.Add(SelectedMatch);
            FinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        // SEMIFINAL 

        public bool CanAddSemifinalMatch
        {
            get { return SelectedMatch != null && Matches.Contains(SelectedMatch) && SemifinalMatches?.Count < 2; }
        }

        public void AddSemifinalMatch()
        {
            SemifinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveSemifinalMatch
        {
            get { return SelectedMatch != null && SemifinalMatches.Contains(SelectedMatch); }
        }
        public void RemoveSemifinalMatch()
        {
            Matches.Add(SelectedMatch);
            SemifinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        // QUARTERFINAL

        public bool CanAddQuarterfinalMatch
        {
            get { return SelectedMatch != null && Matches.Contains(SelectedMatch) && QuarterfinalMatches.Count < 4; }
        }
        public void AddQuarterfinalMatch()
        {
            QuarterfinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveQuarterfinalMatch
        {
            get { return SelectedMatch != null && QuarterfinalMatches.Contains(SelectedMatch); }
        }
        public void RemoveQuarterfinalMatch()
        {
            Matches.Add(SelectedMatch);
            QuarterfinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

    }
}
