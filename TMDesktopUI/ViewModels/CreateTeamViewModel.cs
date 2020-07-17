using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreateTeamViewModel : Conductor<object>
    {
        //private TournamentDisplayModel _tournamentDisplayModel;

        private List<TeamDisplayModel> _createdTeams;


        private string _teamName;
        private string _coachName;
        private string _filterText;

        private PlayerDisplayModel _playerToAdd;
        public PlayerDisplayModel PlayerToAdd
        {
            get { return _playerToAdd; }
            set
            {
                _playerToAdd = value;
                NotifyOfPropertyChange(() => PlayerToAdd);
            }
        }

        private PlayerDisplayModel _playerToRemove;
        public PlayerDisplayModel PlayerToRemove
        {
            get { return _playerToRemove; }
            set
            {
                _playerToRemove = value;
                NotifyOfPropertyChange(() => PlayerToRemove);
            }
        }

        private BindingList<PlayerDisplayModel> _selectedPlayers;
        public BindingList<PlayerDisplayModel> SelectedPlayers
        {
            get { return _selectedPlayers; }
            set
            {
                _selectedPlayers = value;
                NotifyOfPropertyChange(() => SelectedPlayers);
            }
        }

        private BindingList<PlayerDisplayModel> _displayedPlayers;
        public BindingList<PlayerDisplayModel> DisplayedPlayers
        {
            get { return _displayedPlayers; }
            set
            {
                _displayedPlayers = value;
                NotifyOfPropertyChange(() => DisplayedPlayers);
            }
        }

        // is not bound to anything, shouldnt need to NotifyOfPropertyChange        
        public BindingList<PlayerDisplayModel> AllPlayers;       
               

        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        public CreateTeamViewModel()
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();

            AllPlayers = new BindingList<PlayerDisplayModel>(_loader.GetAllPlayers());
            DisplayedPlayers = AllPlayers;
        }

        public CreateTeamViewModel(List<TeamDisplayModel> createdTeams) : this()
        {
            _createdTeams = createdTeams;
        }

        public string TeamName
        {
            get { return _teamName; }
            set
            {
                _teamName = value;
                NotifyOfPropertyChange(() => TeamName);
            }
        }

        public string CoachName
        {
            get { return _coachName; }
            set
            {
                _coachName = value;
                NotifyOfPropertyChange(() => CoachName);
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

        public void ApplyFilter(string filterText)
        {
            List<PlayerDisplayModel> filteredPlayers = new List<PlayerDisplayModel>();
            
            foreach (var player in DisplayedPlayers.Except(SelectedPlayers))
            {
                if (player.FirstName.Contains(filterText) ||
                    player.LastName.Contains(filterText) ||
                    player.Nickname.Contains(filterText))
                {
                    filteredPlayers.Add(player);
                }
            }

            DisplayedPlayers = new BindingList<PlayerDisplayModel>(filteredPlayers);
        }

        public bool CanRemoveFilter(string filterText)
        {
            return (filterText?.Length > 0);
        }

        public void RemoveFilter()
        {            
            DisplayedPlayers = new BindingList<PlayerDisplayModel>(AllPlayers.Except(SelectedPlayers).ToList());
        }

        public bool CanAddPlayer()
        {
            return (PlayerToAdd != null) && (SelectedPlayers.Count < 5); 
        }

        public void AddPlayer()
        {
            SelectedPlayers.Add(PlayerToAdd);
            DisplayedPlayers.Remove(PlayerToAdd);
            PlayerToAdd = null;
        }

        // 2nd condition should be useless, since you can choose player only if SelectedPlayers is not empty
        public bool CanRemovePlayer()
        {
            return (PlayerToRemove != null) && (SelectedPlayers.Count > 0);
        }

        public void RemovePlayer()
        {
            SelectedPlayers.Remove(PlayerToRemove);
            DisplayedPlayers.Add(PlayerToRemove);
            PlayerToRemove = null;
        }

        public bool CanCreateTeam()
        {
            return (!string.IsNullOrWhiteSpace(TeamName) && SelectedPlayers.Count == 5);                
        }

        public void CreateTeam()
        {
            var selectedPlayers = new List<PlayerDisplayModel>(SelectedPlayers);
            if (_query.ExistsTeam(TeamName, selectedPlayers))
            {
                MessageBox.Show("Team with the specific name and players already exists.");
            } else
            {
                TeamDisplayModel newTeam = new TeamDisplayModel();
                newTeam.TeamName = TeamName;
                newTeam.CoachName = CoachName;
                newTeam.Players = selectedPlayers;
                _saver.SaveTeam(newTeam); // also sets the team.Id
                _createdTeams.Add(newTeam);
                ClearForm();
            }
        }

        private void ClearForm()
        {
            TeamName = "";
            CoachName = "";
            FilterText = "";
            SelectedPlayers.Clear();
            DisplayedPlayers = AllPlayers;
            PlayerToAdd = null;
            PlayerToRemove = null;
        }

        public void CreateNewPlayer()
        {
            ...;
        } 
    }
}
