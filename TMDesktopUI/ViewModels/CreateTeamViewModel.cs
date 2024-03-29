﻿using System;
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
        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        private IEventAggregator _events;

        private string _teamName;
        private string _coachName;
        private string _filterText = "";

        private PlayerDisplayModel _playerToAdd;
        private PlayerDisplayModel _playerToRemove;

        private BindingList<PlayerDisplayModel> _selectedPlayers;
        private BindingList<PlayerDisplayModel> _displayedPlayers;

        public CreateTeamViewModel(IEventAggregator events)
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();
            _events = events;

            AllPlayers = new BindingList<PlayerDisplayModel>(_loader.GetAllPlayers());
            DisplayedPlayers = new BindingList<PlayerDisplayModel>();
            SelectedPlayers = new BindingList<PlayerDisplayModel>();
            DisplayedPlayers = new BindingList<PlayerDisplayModel>(DisplayedPlayers.Union(AllPlayers).ToList());
        }

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

        public BindingList<PlayerDisplayModel> SelectedPlayers
        {
            get { return _selectedPlayers; }
            set
            {
                _selectedPlayers = value;
                NotifyOfPropertyChange(() => SelectedPlayers);
                NotifyOfPropertyChange(() => CanAddPlayer);
            }
        }
        
        public BindingList<PlayerDisplayModel> DisplayedPlayers
        {
            get { return _displayedPlayers; }
            set
            {
                _displayedPlayers = value;
                NotifyOfPropertyChange(() => DisplayedPlayers);                   
            }
        }
          
        public BindingList<PlayerDisplayModel> AllPlayers;                              

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
            filterText = filterText.ToLower();
            foreach (var player in AllPlayers.Except(SelectedPlayers))  
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
        
        public bool CanRemovePlayer
        {
            get { return (PlayerToRemove != null); }
        }

        public void RemovePlayer()
        {
            DisplayedPlayers.Add(PlayerToRemove);
            SelectedPlayers.Remove(PlayerToRemove);           
            PlayerToRemove = null;
        }
                
       
        public void CreateTeam()
        {
            var selectedPlayers = new List<PlayerDisplayModel>(SelectedPlayers);
            StringBuilder errorMessage = new StringBuilder();

            if (selectedPlayers.Count != 5)
            {
                errorMessage.AppendLine("You have to choose 5 players.");
            }

            if (string.IsNullOrWhiteSpace(TeamName))
            {
                errorMessage.AppendLine("Team name cannot be empty.");
            }

            /*
            if (_query.ExistsTeam(TeamName, selectedPlayers))
            {
                MessageBox.Show("Team with the specific name and players already exists.");
            }
            */

            if ((TeamName + CoachName).Contains(";"))
            {
                errorMessage.AppendLine("You can't use the symbol ';' in any of the fields.");
            }

            if (errorMessage.Length == 0)
            {
                TeamDisplayModel newTeam = new TeamDisplayModel();
                newTeam.TeamName = TeamName;
                newTeam.CoachName = string.IsNullOrWhiteSpace(CoachName) ? "Unknown" : CoachName;
                newTeam.Players = selectedPlayers;

                _saver.SaveTeam(newTeam);
                _events.PublishOnUIThread(new TeamCreatedEventModel(newTeam));               
                ClearForm();
            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }            
        }

        private void ClearForm()
        {
            TeamName = "";
            CoachName = "";
            FilterText = "";
            DisplayedPlayers = new BindingList<PlayerDisplayModel>(DisplayedPlayers.Union(SelectedPlayers).ToList());           
            SelectedPlayers.Clear();           
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
        }

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }
    }
}
