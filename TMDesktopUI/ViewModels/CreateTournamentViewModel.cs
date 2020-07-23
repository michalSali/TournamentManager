using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;
using Caliburn.Micro;
using System.Windows;
using TMDesktopUI.EventModels;

namespace TMDesktopUI.ViewModels
{
    public class CreateTournamentViewModel : Screen
    {

        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        private IEventAggregator _events;

        private string _tournamentName;        
        private string _filterText;
        private int _prizepool = 0;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;

        private TeamDisplayModel _teamToAdd;
        private TeamDisplayModel _teamToRemove;
        private BindingList<TeamDisplayModel> _selectedTeams;
        private BindingList<TeamDisplayModel> _displayedTeams;
        private int _numberOfSelectedTeams = 0;

        private MatchDisplayModel _selectedMatch;
        private BindingList<MatchDisplayModel> _matches;
        
        private List<TournamentStandingDisplayModel> _tournamentStandings;



        #region >>> Attribute Getters & Setters <<<
        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                NotifyOfPropertyChange(() => FilterText);                
            }
        }

        public int Prizepool 
        {
            get { return _prizepool; }
            set
            {
                _prizepool = value;
                NotifyOfPropertyChange(() => Prizepool);
            } 
        }

        public DateTime StartDate 
        {
            get { return _startDate; } 
            set
            {
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }        

        public TeamDisplayModel TeamToAdd
        {
            get { return _teamToAdd; }
            set
            {
                _teamToAdd = value;
                NotifyOfPropertyChange(() => TeamToAdd);
                NotifyOfPropertyChange(() => CanAddTeam);
            }
        }
        
        public TeamDisplayModel TeamToRemove
        {
            get { return _teamToRemove; }
            set
            {
                _teamToRemove = value;              
                NotifyOfPropertyChange(() => TeamToRemove);
                NotifyOfPropertyChange(() => CanRemoveTeam);
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
             
        public int NumberOfSelectedTeams
        {
            get { return _numberOfSelectedTeams; }
            set
            {
                _numberOfSelectedTeams = value;
                NotifyOfPropertyChange(() => NumberOfSelectedTeams);
            }
        }
                
        /*
        public BindingList<TournamentStandingDisplayModel> TournamentStandings
        {
            get { return _tournamentStandings; }
            set
            {
                _tournamentStandings = value;
                NotifyOfPropertyChange(() => TournamentStandings);
            }
        }
        */

        // is not bound to anything, shouldnt need to NotifyOfPropertyChange        
        public BindingList<TeamDisplayModel> AllTeams;
        
        public BindingList<MatchDisplayModel> Matches
        {
            get { return _matches; }
            set
            {
                _matches = value;
                NotifyOfPropertyChange(() => Matches);
            }
        }
        
        public MatchDisplayModel SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                _selectedMatch = value;
                NotifyOfPropertyChange(() => SelectedMatch);
            }
        }
        #endregion
        
        public CreateTournamentViewModel(IEventAggregator events)
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();
            _events = events;

            AllTeams = new BindingList<TeamDisplayModel>(_loader.GetAllTeams());
            SelectedTeams = new BindingList<TeamDisplayModel>();
            DisplayedTeams = new BindingList<TeamDisplayModel>();
            DisplayedTeams = new BindingList<TeamDisplayModel>(DisplayedTeams.Union(AllTeams).ToList());

            Matches = new BindingList<MatchDisplayModel>();            
        }
        
        public bool CanApplyFilter(string filterText)
        {
            return (filterText?.Length > 0);
        }
        
        public void ApplyFilter(string filterText)
        {
            List<TeamDisplayModel> filteredTeams = new List<TeamDisplayModel>();

            foreach (var team in DisplayedTeams.Except(SelectedTeams))
            {
                if (team.TeamName.Contains(filterText) || team.CoachName.Contains(filterText))
                {
                    filteredTeams.Add(team);
                    continue;
                }
                foreach (var player in team.Players)
                {
                    if (player.FirstName.Contains(filterText) ||
                        player.LastName.Contains(filterText) ||
                        player.Nickname.Contains(filterText))
                    {
                        filteredTeams.Add(team);
                        break;
                    }
                }
            }            

            DisplayedTeams = new BindingList<TeamDisplayModel>(filteredTeams);
        }

        public bool CanRemoveFilter(string filterText)
        {
            return (filterText?.Length > 0);
        }

        public void RemoveFilter()
        {
            DisplayedTeams = new BindingList<TeamDisplayModel>(AllTeams.Except(SelectedTeams).ToList());
        }

        // there can be at most 16 teams        
        public bool CanAddTeam
        {
            get { return (TeamToAdd != null) && (SelectedTeams?.Count < 16); }
        }

        public void AddTeam()
        {
            SelectedTeams.Add(TeamToAdd);
            DisplayedTeams.Remove(TeamToAdd);
            TeamToAdd = null;
            NumberOfSelectedTeams++;
        }
              
        public bool CanRemoveTeam
        {
            get { return (TeamToRemove != null) && (SelectedTeams?.Count > 0); }
        }

        public void RemoveTeam()
        {
            DisplayedTeams.Add(TeamToRemove);
            SelectedTeams.Remove(TeamToRemove);            
            TeamToRemove = null;
            NumberOfSelectedTeams--;
        }        
               
        public bool CanCreateTournament
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(TournamentName) &&
                  StartDate != null && EndDate != null);
            }
                    
        }

        public void CreateTournament()
        {
            StringBuilder errorMessage = new StringBuilder();

            /*
            if (_query.ExistsTournament(TournamentName))
            {
                errorMessage.AppendLine("Tournament with the specific name already exists.");
            }
            */
            
            if (SelectedTeams.Count < 2)  // 2 or 4 teams ?
            {
                errorMessage.AppendLine("You have to choose at least 2 teams.");
            }

            if (Prizepool < 0)
            {
                errorMessage.AppendLine("Prizepool cannot be a negative number.");
            }

            if (TournamentName.Contains(";"))
            {
                errorMessage.AppendLine("You can't use the symbol ';' in any of the fields.");
            }

            if (errorMessage.Length == 0)
            {
                TournamentDisplayModel newTournament = new TournamentDisplayModel();
                newTournament.TournamentName = TournamentName;
                newTournament.StartDate = StartDate;
                newTournament.EndDate = EndDate;
                newTournament.Prizepool = Prizepool;
                newTournament.Teams = new List<TeamDisplayModel>(SelectedTeams);
                newTournament.Matches = new List<MatchDisplayModel>(Matches);
                newTournament.Standings = _tournamentStandings;

                _saver.SaveTournament(newTournament);
                _events.PublishOnUIThread(new TournamentCreatedEventModel(newTournament));
                ClearForm();
            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }
        }
       
        private void ClearForm()
        {
            TournamentName = "";           
            FilterText = "";
            Prizepool = 0;
            
            Matches.Clear();
            DisplayedTeams = new BindingList<TeamDisplayModel>(DisplayedTeams.Union(SelectedTeams).ToList());            
            SelectedTeams.Clear();
            TeamToAdd = null;
            TeamToRemove = null;
        }                        

        public void CreateNewMatch()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (StartDate == null || EndDate == null)
            {
                errorMessage.AppendLine("You have to first set dates of the tournament.");
            }

            if (errorMessage.Length == 0)
            {
                TournamentDisplayModel tournament = new TournamentDisplayModel();
                tournament.StartDate = StartDate;
                tournament.EndDate = EndDate;
                tournament.Teams = new List<TeamDisplayModel>(SelectedTeams);
                tournament.TournamentName = TournamentName;
                tournament.Prizepool = Prizepool;

                _events.PublishOnUIThread(new CreateMatchEventModel(tournament));
            }
                              
        }

        public void AddCreatedMatch(MatchDisplayModel match)
        {
            Matches.Add(match);
        }
        
        public void CreateNewTeam()
        {
            _events.PublishOnUIThread(new CreateTeamEvent());
        }

        public void AddCreatedTeam(TeamDisplayModel team)
        {
            DisplayedTeams.Add(team);
            AllTeams.Add(team);            
        }
        
        public void CreateStandings()
        {

            if (SelectedTeams?.Count < 2)
            {
                MessageBox.Show("You have to select at least 2 teams before you can create standings.");
                return;
            }

            TournamentDisplayModel tournament = new TournamentDisplayModel();
            tournament.Prizepool = Prizepool;
            tournament.Teams = new List<TeamDisplayModel>(SelectedTeams);
            _events.PublishOnUIThread(new CreateTournamentStandingsEventModel(tournament));
        }

        public void SetStandings(List<TournamentStandingDisplayModel> standings)
        {
            _tournamentStandings = standings;
        }

        public void CreateMatchSpecifications()
        {
            TournamentDisplayModel tournament = new TournamentDisplayModel();
            tournament.Matches = new List<MatchDisplayModel>(Matches);
            _events.PublishOnUIThread(new CreateMatchSpecificationsEventModel(tournament));
        }

        public void AddMatchSpecifications(List<MatchDisplayModel> matches)
        {
            Matches = new BindingList<MatchDisplayModel>(matches);
        }


        public void ReturnToMainScreen()
        {
            _events.PublishOnUIThread(new ReturnToMainScreenEvent());
        }
    }
}
