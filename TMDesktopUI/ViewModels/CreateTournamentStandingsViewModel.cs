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
        private List<double> FourTeamsPrizeDistribution = new List<double> { 55, 30, 10, 5 };
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



        private TournamentStandingDisplayModel _selectedStanding;
        public TournamentStandingDisplayModel SelectedStanding
        {
            get { return _selectedStanding; }
            set
            {
                _selectedStanding = value;
                NotifyOfPropertyChange(() => SelectedStanding);
                NotifyOfPropertyChange(() => CanAssignTeam);
                NotifyOfPropertyChange(() => CanRemoveTeamFromStanding);
            }
        }

        private TeamDisplayModel _selectedTeam;
        public TeamDisplayModel SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                NotifyOfPropertyChange(() => SelectedTeam);
                NotifyOfPropertyChange(() => CanAssignTeam);
            }
        }


        public CreateTournamentStandingsViewModel(IEventAggregator events)
        {           
            _events = events;
            _random = new Random();
            
        }


        public CollectionViewSource StandingsViewSource { get; set; }
        public ObservableCollection<TournamentStandingDisplayModel> StandingsCollection { get; set; }

      
        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            DisplayedTeams = new BindingList<TeamDisplayModel>(tournament.Teams);            

            // add one empty team
            SelectedTeams = new BindingList<TeamDisplayModel>{ new TeamDisplayModel() };

           
            InitializeStandings();
            
        }

        
        private void InitializeStandings()
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
                newModel.PrizeWon = unsupportedTeamSize ? 0 : (int)(PrizeDistribution[i] * Tournament.Prizepool / 100);
                StandingsCollection.Add(newModel);
            }

            StandingsViewSource = new CollectionViewSource();
            StandingsViewSource.Source = StandingsCollection;

            StandingsViewSource.SortDescriptions.Add(new SortDescription("Placement", ListSortDirection.Ascending));
            StandingsViewSource.View.Refresh();
        }
        

        public void AutoFill()
        {            
            var WithoutEmptyPlayer = new BindingList<TeamDisplayModel>(DisplayedTeams.Where(x => !string.IsNullOrEmpty(x.TeamName)).ToList());

            // fills in fields, where a team has not been chosen / "EmptyTeam"
            foreach (var standing in StandingsCollection)
            {
                if (standing.Team == null || string.IsNullOrEmpty(standing.Team?.TeamName))
                    {
                        standing.Team = WithoutEmptyPlayer[_random.Next(WithoutEmptyPlayer.Count)];                       
                        WithoutEmptyPlayer.Remove(standing.Team);
                        DisplayedTeams.Remove(standing.Team);
                    }                
            }

            StandingsViewSource.View.Refresh();
        }

        public void CreateStandings()
        {
            StringBuilder errorMessage = new StringBuilder();

            bool containsEmptyTeam = StandingsCollection
                                        .Where(x => (x.Team == null) || string.IsNullOrEmpty(x.Team.TeamName))
                                        .ToList().Count > 0;

            if (containsEmptyTeam)
            {
                errorMessage.AppendLine("All places need to have assigned a team. You can use the 'Auto-fill' feature " +
                                        "that will fill the empty places.");
            }

            bool containsNegativePrize = StandingsCollection.Where(x => x.PrizeWon < 0).ToList().Count > 0;

            if (containsNegativePrize)
            {
                errorMessage.AppendLine("'PrizeWon' cannot be a negative number.");
            }

            if (errorMessage.Length == 0)
            {
                _events.PublishOnUIThread(new TournamentStandingsCreatedEventModel(new List<TournamentStandingDisplayModel>(StandingsCollection)));                
            }
        }

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }

        public bool CanAssignTeam
        {
            get { return SelectedTeam != null && SelectedStanding != null; }
        }

        public void AssignTeam()
        {
            if (SelectedStanding.Team != null)
            {
                DisplayedTeams.Add(SelectedStanding.Team);
            }                
            
            SelectedStanding.Team = SelectedTeam;
            DisplayedTeams.Remove(SelectedTeam);
           
            StandingsViewSource.View.Refresh();
        }

        public bool CanRemoveTeamFromStanding
        {
            get { return SelectedStanding != null && SelectedStanding?.Team != null; }
        }
        public void RemoveTeamFromStanding()
        {
            DisplayedTeams.Add(SelectedStanding.Team);
            SelectedStanding.Team = null;

            StandingsViewSource.View.Refresh();
        }
    
    }
}
