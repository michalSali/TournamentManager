using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreateTeamViewModel : Screen
    {
        //private TournamentDisplayModel _tournamentDisplayModel;       

        private string _teamName;
        private string _coachName;
        private string _filterText = "";

        private PlayerDisplayModel _playerToAdd;
        public PlayerDisplayModel PlayerToAdd
        {
            get { return _playerToAdd; }
            set
            {
                _playerToAdd = value;
                NotifyOfPropertyChange(() => PlayerToAdd);
                NotifyOfPropertyChange(() => CanAddPlayer);
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
                NotifyOfPropertyChange(() => CanRemovePlayer);
            }
        }

        private BindingList<PlayerDisplayModel> _selectedPlayers = new BindingList<PlayerDisplayModel>();
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
        private IEventAggregator _events;
        public CreateTeamViewModel(IEventAggregator events)
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();
            _events = events;

            AllPlayers = new BindingList<PlayerDisplayModel>(_loader.GetAllPlayers());
            DisplayedPlayers = new BindingList<PlayerDisplayModel>(AllPlayers);

            // testing
            //AllPlayers = new BindingList<PlayerDisplayModel>();
            //DisplayedPlayers = new BindingList<PlayerDisplayModel>();
            //SelectedPlayers = new BindingList<PlayerDisplayModel>();
        }

        /*
        public CreateTeamViewModel(List<TeamDisplayModel> createdTeams) : this()
        {
            _createdTeams = createdTeams;
        }
        */

        public string TeamName
        {
            get { return _teamName; }
            set
            {
                _teamName = value;
                NotifyOfPropertyChange(() => TeamName);
                NotifyOfPropertyChange(() => CanCreateTeam);
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
            filterText = filterText.ToLower();
            foreach (var player in AllPlayers.Except(SelectedPlayers))  // -- mali by byt disjunktne mnoziny ?
            
            //foreach (var player in DisplayedPlayers)
            {
                if (player.FirstName.ToLower().Contains(filterText) ||
                    player.LastName.ToLower().Contains(filterText) ||
                    player.Nickname.ToLower().Contains(filterText))
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
            if (SelectedPlayers == null)
            {
                DisplayedPlayers = new BindingList<PlayerDisplayModel>(AllPlayers);
            } else
            {
                DisplayedPlayers = new BindingList<PlayerDisplayModel>(AllPlayers.Except(SelectedPlayers).ToList());
            }
            FilterText = "";
        }

        
        public bool CanAddPlayer
        {
            get { return (PlayerToAdd != null) && (SelectedPlayers?.Count < 5); } 
        }
               
        public void AddPlayer()
        {
            SelectedPlayers.Add(PlayerToAdd);
            DisplayedPlayers.Remove(PlayerToAdd);
            PlayerToAdd = null;
        }

        // 2nd condition should be useless, since you can choose player only if SelectedPlayers is not empty
        public bool CanRemovePlayer
        {
            get { return (PlayerToRemove != null) && (SelectedPlayers?.Count > 0); }
        }

        public void RemovePlayer()
        {
            DisplayedPlayers.Add(PlayerToRemove);
            SelectedPlayers.Remove(PlayerToRemove);
            //NotifyOfPropertyChange(() => DisplayedPlayers);
            //PlayerToRemove = new PlayerDisplayModel();
            PlayerToRemove = null;
        }

        
        // !!!
        public bool CanCreateTeam
        {
            //get { return !string.IsNullOrWhiteSpace(TeamName) && SelectedPlayers?.Count == 5; }
            //get { return !string.IsNullOrWhiteSpace(TeamName) && SelectedPlayers?.Count > 0; }
            get { return !string.IsNullOrWhiteSpace(TeamName); }
        }
               
        

        public void CreateTeam()
        {
            var selectedPlayers = new List<PlayerDisplayModel>(SelectedPlayers);
            StringBuilder errorMessage = new StringBuilder();

            if (selectedPlayers.Count != 5)
            {
                errorMessage.AppendLine("You have to choose 5 players.");
            }
            /*
            if (_query.ExistsTeam(TeamName, selectedPlayers))
            {
                MessageBox.Show("Team with the specific name and players already exists.");
            }

            {
                TeamDisplayModel newTeam = new TeamDisplayModel();
                newTeam.TeamName = TeamName;
                newTeam.CoachName = string.IsNullOrWhiteSpace(CoachName) ? "Unknown" : CoachName;
                newTeam.Players = selectedPlayers;
                //_saver.SaveTeam(newTeam); // also sets the team.Id

                _events.PublishOnUIThread(new TeamCreatedEventModel(newTeam));
                //_createdTeams.Add(newTeam);
                ClearForm();
            }
            */

            if (errorMessage.Length != 0)
            {
                MessageBox.Show(errorMessage.ToString());
            }

            // testing
            TeamDisplayModel newTeam = new TeamDisplayModel();
            newTeam.TeamName = TeamName;
            newTeam.CoachName = string.IsNullOrWhiteSpace(CoachName) ? "Unknown" : CoachName;
            newTeam.Players = selectedPlayers;
            _saver.SaveTeam(newTeam); // also sets the team.Id

            _events.PublishOnUIThread(new TeamCreatedEventModel(newTeam));
            //_createdTeams.Add(newTeam);
            ClearForm();

        }

        private void ClearForm()
        {
            TeamName = "";
            CoachName = "";
            FilterText = "";
            DisplayedPlayers = new BindingList<PlayerDisplayModel>(DisplayedPlayers.Union(SelectedPlayers).ToList());

            // DOES NOT WORK FOR SOME FUCKING REASON => AFTER CREATING NEW TEAM, AND THEN TRYING TO CREATE
            //    A NEW PLAYER, AddCreatedPlayer RAISES OUT OF RANGE EXCEPTION ?????????????????????
            //DisplayedPlayers = new BindingList<PlayerDisplayModel>(AllPlayers); 

            //SelectedPlayers = new BindingList<PlayerDisplayModel>();
            SelectedPlayers.Clear();
            //DisplayedPlayers = new BindingList<PlayerDisplayModel>(AllPlayers);
            PlayerToAdd = null;
            PlayerToRemove = null;
        }

        public void CreateNewPlayer()
        {
            _events.PublishOnUIThread(new CreatePlayerEvent());
        } 

        public void AddCreatedPlayer(PlayerDisplayModel player)
        {
            DisplayedPlayers.Add(player);
            AllPlayers.Add(player);
            //DisplayedPlayers.Add(player);
        }

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }
    }
}
