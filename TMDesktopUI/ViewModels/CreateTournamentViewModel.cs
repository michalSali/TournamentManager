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

namespace TMDesktopUI.ViewModels
{
    public class CreateTournamentViewModel : Conductor<object>
    {
        /*
        private TournamentDisplayModel _tournament;
        public TournamentDisplayModel Tournament
        { 
            get
            {
                return _tournament;
            } 
        }

        public CreateTournamentViewModel()
        {
            _tournament = new TournamentDisplayModel();
        }

        public CreateTournament()
        {
           
        }
        */

        private string _tournamentName;        
        private string _filterText;
        private int _prizepool;
        private DateTime _startDate;
        private DateTime _endDate;

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
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }

        private TeamDisplayModel _teamToAdd;
        public TeamDisplayModel TeamToAdd
        {
            get { return _teamToAdd; }
            set
            {
                _teamToAdd = value;
                NotifyOfPropertyChange(() => TeamToAdd);
            }
        }

        private TeamDisplayModel _teamToRemove;
        public TeamDisplayModel TeamToRemove
        {
            get { return _teamToRemove; }
            set
            {
                _teamToRemove = value;              
                NotifyOfPropertyChange(() => TeamToRemove);
            }
        }

        private BindingList<TeamDisplayModel> _selectedTeams;
        public BindingList<TeamDisplayModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set
            {
                _selectedTeams = value;
                NotifyOfPropertyChange(() => SelectedTeams);
            }
        }

        private BindingList<TeamDisplayModel> _displayedTeams;
        public BindingList<TeamDisplayModel> DisplayedTeams
        {
            get { return _displayedTeams; }
            set
            {
                _displayedTeams = value;
                NotifyOfPropertyChange(() => DisplayedTeams);
            }
        }

        // is not bound to anything, shouldnt need to NotifyOfPropertyChange        
        public BindingList<TeamDisplayModel> AllTeams;


        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        public CreateTournamentViewModel()
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();

            AllTeams = new BindingList<TeamDisplayModel>(_loader.GetAllTeams());
            DisplayedTeams = AllTeams;
        }

        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
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

        public bool CanApplyFilter(string filterText)
        {
            return (filterText?.Length > 0);
        }

        // considers team/coach name, all team players' names
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

        // can have 16 teams at most / 24?
        public bool CanAddPlayer()
        {
            return (TeamToAdd != null) && (SelectedTeams.Count < 16);
        }

        public void AddPlayer()
        {
            SelectedTeams.Add(TeamToAdd);
            DisplayedTeams.Remove(TeamToAdd);
            TeamToAdd = null;
        }

        // 2nd condition should be useless, since you can choose player only if SelectedPlayers is not empty
        public bool CanRemovePlayer()
        {
            return (TeamToRemove != null) && (SelectedTeams.Count > 0);
        }

        public void RemovePlayer()
        {
            SelectedTeams.Remove(TeamToRemove);
            DisplayedTeams.Add(TeamToRemove);
            TeamToRemove = null;
        }

        // at least 4 teams should participate - MessageBox
        // Prizepool < 0 - inform user?
        public bool CanCreateTeam()
        {
            return (!string.IsNullOrWhiteSpace(TournamentName) &&
                    StartDate != null && EndDate != null &&
                    Prizepool >= 0);
        }

        public void CreateTeam()
        {            
            if (_query.ExistsTournament(TournamentName))
            {
                MessageBox.Show("Tournament with the specific name already exists.");
            }
            else if (SelectedTeams.Count < 4)
            {
                MessageBox.Show("You have to choose at least 4 teams.");
            } else
            {
                TournamentDisplayModel newTournament = new TournamentDisplayModel();
                newTournament.TournamentName = TournamentName;
                newTournament.StartDate = StartDate;
                newTournament.EndDate = EndDate;
                newTournament.Prizepool = Prizepool;
                newTournament.Teams = new List<TeamDisplayModel>(SelectedTeams);
                newTournament.Matches = ...;
                _saver.SaveTournament(newTournament);
                ClearForm();
            }
        }

        // what to do with dates?
        private void ClearForm()
        {
            TournamentName = "";           
            FilterText = "";
            Prizepool = 0;
            SelectedTeams.Clear();
            Matches.Clear();
            DisplayedTeams = AllTeams;
            TeamToAdd = null;
            TeamToRemove = null;
        }

        public void CreateNewTeam()
        {
            ...;
        }

        public void CreateNewMatch()
        {
            ...;
        }
    }
}
