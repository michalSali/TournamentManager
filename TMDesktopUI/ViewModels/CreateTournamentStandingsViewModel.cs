using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    class CreateTournamentStandingsViewModel : Screen
    {
        private IEventAggregator _events;
        private Random _random;

        // we set predetermined prize distribution for teams with 4, 8 and 16 players (in %, for first to n-th place)
        private List<double> TwoTeamsPrizeDistribution = new List<double> { 65, 35 };
        private List<double> FourTeamsPrizeDistribution = new List<double> { 55, 30, 15, 10 };
        private List<double> EightTeamsPrizeDistribution = new List<double> { 30, 20, 15, 12, 9, 6, 4, 4 };
        private List<double> SixteenTeamsPrizeDistribution = new List<double> {
            27.2, 17.6, 13.6, 10.4, 8, 6, 4.4, 3.2, 2.4, 1.76, 1.28, 0.96, 0.8, 0.8, 0.8, 0.8 };

        private BindingList<TeamDisplayModel> _selectedTeams;
        private BindingList<TeamDisplayModel> _displayedTeams;
        private BindingList<MatchDisplayModel> _matches;
        private BindingList<TournamentStandingDisplayModel> _standings;

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
        public BindingList<TeamDisplayModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set
            {
                _selectedTeams = value;
                NotifyOfPropertyChange(() => SelectedTeams);
            }
        }

        public BindingList<TeamDisplayModel> DisplayedTeams
        {
            get { return _displayedTeams; }
            set
            {
                _displayedTeams = value;
                NotifyOfPropertyChange(() => DisplayedTeams);
            }
        }

        public BindingList<MatchDisplayModel> Matches
        {
            get { return _matches; }
            set
            {
                _matches = value;
                NotifyOfPropertyChange(() => Matches);
            }
        }

        public BindingList<TournamentStandingDisplayModel> Standings
        {
            get { return _standings; }
            set
            {
                _standings = value;
                NotifyOfPropertyChange(() => Standings);
            }
        }
        

        public CreateTournamentStandingsViewModel(IEventAggregator events)
        {
            //_query = new ModelsQueries();
            //_saver = new ModelsSaver();
            //_loader = new ModelsLoader();
            _events = events;
            _random = new Random();
            
        }



        // testing
        public CollectionViewSource ViewSource { get; set; }
        public ObservableCollection<TournamentStandingDisplayModel> StandingsCollection { get; set; }

        public CollectionViewSource TeamsSource { get; set; }
        public ObservableCollection<TeamDisplayModel> TeamsCollection { get; set; }



        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            DisplayedTeams = new BindingList<TeamDisplayModel>(tournament.Teams);

            // add one empty team
            SelectedTeams = new BindingList<TeamDisplayModel>{ new TeamDisplayModel() };
            //Matches = new BindingList<MatchDisplayModel>(tournament.Matches);

            Standings = new BindingList<TournamentStandingDisplayModel>();
            InitializeStandings();


            //testing
            TeamsCollection = new ObservableCollection<TeamDisplayModel>(tournament.Teams);
            TeamsSource = new CollectionViewSource();
            TeamsSource.Source = TeamsCollection;
            TeamsSource.View.Refresh();
            Init();
        }



        // testing
        private void Init()
        {
            StandingsCollection = new ObservableCollection<TournamentStandingDisplayModel>();
            List<double> PrizeDistribution = new List<double>(DisplayedTeams.Count) { 0 };
            bool unsupportedTeamSize = false;

            switch (DisplayedTeams.Count)
            {
                case 2:
                    PrizeDistribution = TwoTeamsPrizeDistribution;
                    break;
                case 4:
                    PrizeDistribution = FourTeamsPrizeDistribution;
                    break;
                case 8:
                    PrizeDistribution = EightTeamsPrizeDistribution;
                    break;
                case 16:
                    PrizeDistribution = SixteenTeamsPrizeDistribution;
                    break;
                default:
                    unsupportedTeamSize = true;
                    break;
            }

            for (int i = 0; i < DisplayedTeams.Count; ++i)
            {
                TournamentStandingDisplayModel newModel = new TournamentStandingDisplayModel();
                newModel.Placement = i + 1;
                newModel.PrizeWon = unsupportedTeamSize ? 0 : (int)(PrizeDistribution[i] / 100 * Tournament.Prizepool);
                StandingsCollection.Add(newModel);
            }

            ViewSource = new CollectionViewSource();
            ViewSource.Source = StandingsCollection;

            ViewSource.SortDescriptions.Add(new SortDescription("Placement", ListSortDirection.Ascending));

            // Let the UI control refresh in order for changes to take place.
            ViewSource.View.Refresh();
        }



        private void InitializeStandings()
        {
            List<double> PrizeDistribution = new List<double>(DisplayedTeams.Count) { 0 };
            bool unsupportedTeamSize = false;

            switch (DisplayedTeams.Count)
            {
                case 2:
                    PrizeDistribution = TwoTeamsPrizeDistribution;
                    break;
                case 4:
                    PrizeDistribution = FourTeamsPrizeDistribution;
                    break;
                case 8:
                    PrizeDistribution = EightTeamsPrizeDistribution;
                    break;
                case 16:
                    PrizeDistribution = SixteenTeamsPrizeDistribution;
                    break;
                default:
                    unsupportedTeamSize = true;
                    break;
            }

            for (int i = 0; i < DisplayedTeams.Count; ++i)
            {
                TournamentStandingDisplayModel newModel = new TournamentStandingDisplayModel();
                newModel.Placement = i + 1;
                newModel.PrizeWon = unsupportedTeamSize ? 0 : (int)(PrizeDistribution[i] / 100 * Tournament.Prizepool);
                Standings.Add(newModel);
            }
        }

        public void AutoFill()
        {
            var RemainingTeams = new BindingList<TeamDisplayModel>(Tournament.Teams.Except(SelectedTeams).ToList());
            
            // fills in fields, where a team has not been chosen / "EmptyTeam"
            foreach (var standing in Standings)
            {
                if (standing.Team == null || string.IsNullOrEmpty(standing.Team.TeamName))
                    {
                        standing.Team = RemainingTeams[_random.Next(RemainingTeams.Count)];
                        RemainingTeams.Remove(standing.Team);
                        SelectedTeams.Add(standing.Team);
                    }                
            }
        }

        public void CreateStandings()
        {
            StringBuilder errorMessage = new StringBuilder();

            bool containsEmptyTeam = Standings
                                        .Where(x => (x.Team == null) || string.IsNullOrEmpty(x.Team.TeamName))
                                        .ToList().Count > 0;

            if (containsEmptyTeam)
            {
                errorMessage.AppendLine("All places need to have assigned a team. You can use the 'Auto-fill' feature " +
                                        "that will fill the empty places.");
            }

            bool containsNegativePrize = Standings.Where(x => x.PrizeWon < 0).ToList().Count > 0;

            if (containsNegativePrize)
            {
                errorMessage.AppendLine("'PrizeWon' cannot be a negative number.");
            }

            if (errorMessage.Length == 0)
            {
                _events.PublishOnUIThread(new TournamentStandingsCreatedEventModel(new List<TournamentStandingDisplayModel>(Standings)));
                //_events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
            }
        }

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }

        /*
        // currently not in use
        #region >>> Match Importance Separation <<<

        private BindingList<MatchDisplayModel> _groupStageMatches;
        public BindingList<MatchDisplayModel> GroupStageMatches
        {
            get { return _groupStageMatches; }
            set
            {
                _groupStageMatches = value;
                NotifyOfPropertyChange(() => GroupStageMatches);
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

        private MatchDisplayModel _finalMatch;
        public MatchDisplayModel FinalMatch
        {
            get { return _finalMatch; }
            set
            {
                _finalMatch = value;
                NotifyOfPropertyChange(() => FinalMatch);
            }
        }

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


        // FINAL 

        public bool CanAddFinalMatch()
        {
            return SelectedMatch != null && FinalMatch == null;
        }
        public void AddFinalMatch()
        {
            FinalMatch = SelectedMatch;
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveFinalMatch()
        {
            return FinalMatch != null;
        }
        public void RemoveFinalMatch()
        {
            Matches.Add(FinalMatch);
            FinalMatch = null;
        }

        // SEMIFINAL 

        public bool CanAddSemifinalMatch()
        {
            return SelectedMatch != null && SemifinalMatches.Count < 4;
        }
        public void AddSemifinalMatch()
        {
            SemifinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveSemifinalMatch()
        {
            return SelectedMatch != null && SemifinalMatches.Contains(SelectedMatch);
        }
        public void RemoveSemifinalMatch()
        {
            Matches.Add(SelectedMatch);
            SemifinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        // QUARTERFINAL

        public bool CanAddQuarterfinalMatch()
        {
            return SelectedMatch != null && QuarterfinalMatches.Count < 8;
        }
        public void AddQuarterfinalMatch()
        {
            QuarterfinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveQuarterfinalMatch()
        {
            return SelectedMatch != null && QuarterfinalMatches.Contains(SelectedMatch);
        }
        public void RemoveQuarterfinalMatch()
        {
            Matches.Add(SelectedMatch);
            QuarterfinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }
        */
    }
}
