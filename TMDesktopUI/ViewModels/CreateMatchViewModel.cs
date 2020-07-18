﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreateMatchViewModel : Screen
    {
        
        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        private IEventAggregator _events;
        public CreateMatchViewModel(IEventAggregator events)
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();
            _events = events;

            MapNames = new BindingList<string> { "Mirage", "Dust 2", "Train", "Nuke", "Inferno", "Overpass", "Vertigo", "Cache", "Cobblestone" };
        }

        
        public void InitializeValues(TournamentDisplayModel tournament)
        {
            //DisplayedTeams = new BindingList<TeamDisplayModel>(tournament.Teams);
        }

        //private int _matchNumber;               
        private int _format;
        private DateTime _date;

        private int _teamOneScore;
        private int _teamTwoScore;

        private BindingList<MapScoreDisplayModel> _maps;
        private TeamDisplayModel _teamOne;
        private TeamDisplayModel _teamTwo;       

        

        public BindingList<int> Formats = new BindingList<int> { 1, 3, 5, 7 };

        private BindingList<string> _mapNames;
        
        public BindingList<string> MapNames
        {
            get { return _mapNames; }
            set
            {
                _mapNames = value;
                NotifyOfPropertyChange(() => MapNames);
            }
        }
        //public BindingList<string> MapNames = new BindingList<string> { "Mirage", "Dust 2", "Train", "Nuke", "Inferno", "Overpass", "Vertigo", "Cache", "Cobblestone" };
        public List<int> PossibleMatchScores;

       

        public int Format
        {
            get { return _format; }
            set
            {
                _format = value;
                PossibleMatchScores = Enumerable.Range(0, (_format / 2) + 2).ToList();
                NotifyOfPropertyChange(() => Format);
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyOfPropertyChange(() => Date);
            }
        }

        public int TeamOneScore
        {
            get { return _teamOneScore; }
            set
            {
                _teamOneScore = value;
                NotifyOfPropertyChange(() => TeamOneScore);
            }
        }

        public int TeamTwoScore
        {
            get { return _teamTwoScore; }
            set
            {
                _teamTwoScore = value;
                NotifyOfPropertyChange(() => TeamTwoScore);
            }
        }

        public BindingList<MapScoreDisplayModel> Maps
        {
            get { return _maps; }
            set
            {
                _maps = value;
                NotifyOfPropertyChange(() => Maps);
            }
        }

        public TeamDisplayModel TeamOne
        {
            get { return _teamOne; }
            set
            {
                _teamOne = value;
                NotifyOfPropertyChange(() => TeamOne);
            }
        }

        public TeamDisplayModel TeamTwo
        {
            get { return _teamTwo; }
            set
            {
                _teamTwo = value;
                NotifyOfPropertyChange(() => TeamTwo);
            }
        }


        // we will allow to temporarily to choose 2 same teams against each other,
        // but you wont be able to create match like that (so you can switch teams for example)
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
                

      
        public bool CanCreateMatch()
        {
            return (Date != null && TeamOne != null && TeamTwo != null);
        }

        public void CreateMatch()
        {
            int winningScore = (Format / 2) + 1;
            StringBuilder errorMessage = new StringBuilder();

            if (TeamOneScore < 0 || TeamTwoScore < 0)
            {
                errorMessage.AppendLine($"Team match scores have to be in the range 0 to {winningScore}.");
            }

            if (TeamOneScore + TeamTwoScore > Format || (TeamOneScore != winningScore && TeamTwoScore != winningScore))
            {
                errorMessage.AppendLine($"One of the teams has to get the winning score ({winningScore}), \n" +
                                        $"while the other team gets a score in range 0 to {winningScore - 1}).");
            }

            if (TeamOneScore + TeamTwoScore != Maps.Count)
            {
                errorMessage.AppendLine($"The number of maps does not correspond to the match score ({TeamOneScore} : {TeamTwoScore}) - there should be {TeamTwoScore + TeamOneScore} maps.");
            }            

            if (errorMessage.Length == 0)
            {
                MatchDisplayModel newMatch = new MatchDisplayModel();
                newMatch.Date = Date;
                newMatch.Format = Format;
                newMatch.TeamOne = TeamOne;
                newMatch.TeamTwo = TeamTwo;
                //newMatch.Tournament = Tournament;
                newMatch.MatchImportance = 0;
                newMatch.Maps = new List<MapScoreDisplayModel>(Maps);

                _events.PublishOnUIThread(new MatchCreatedEventModel(newMatch));
                ClearMatchForm();
            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }
        }        

        // what to do with dates?
        private void ClearMatchForm()
        {                        
            Maps.Clear();                        
            SelectedMap = null;                   
        }


        public void CreateNewMap()
        {
            if (TeamOne == null || TeamTwo == null)
            {
                MessageBox.Show("You have to choose teams first.");
            } else 
            {
                MatchDisplayModel match = new MatchDisplayModel();
                match.Maps = new List<MapScoreDisplayModel>(Maps);
                match.TeamOne = TeamOne;
                match.TeamTwo = TeamTwo;
                _events.PublishOnUIThread(new CreateMapEventModel(match));
            }
        }

        public void AddCreatedMap(MapScoreDisplayModel map)
        {
            Maps.Add(map);
        }


                          
        

        // 2nd condition should be useless, since you can choose player only if SelectedPlayers is not empty
        public bool CanRemoveMap()
        {
            return (SelectedMap != null) && (Maps.Count > 0);
        }

        public void RemoveMap()
        {
            Maps.Remove(SelectedMap);
            SelectedMap = null;
        }

        //public MapScoreDisplayModel CurrentMap;
        //public MapScoreDisplayModel MapToRemove;

        #region >>> MAP EDITING STUFF <<<
        public MapScoreDisplayModel MapCreationSavedState;
        public MapScoreDisplayModel SelectedMapOriginal;
        public MapScoreDisplayModel SelectedMap;

        public bool CanEditMap()
        {
            return SelectedMap != null;
        }
        public void EditMap()
        {
            // najskor vytvori novu instanciu EditedMatch, ktora si ulozi povodne hodnoty editovanej mapy:
            //     EditedMap = new MapScoreDisplayModel(MapToEdit.MapName, ...);
            // nasledne nastavi vsetky hodnoty: [MapName = MapToEdit.MapName, ...]
            // docasne odstranit MapToEdit z Maps
            // ak sa pouzije "SaveChanges", tak sa normalne vytvori Map pomocou CreateMap ?
            // ak sa pouzije "DiscardChanges", tak sa do Maps vlozi EditedMapOriginal,
            // a nasledne sa nastavia hodnoty spat na MapCreationSavedState
        }

        // sets values of the Map Creation Form according to the given parameter
        // used when editing maps, so that all the changes we've made in the Map Creation Form
        //    are not discarded
        public void SetValues(MapScoreDisplayModel map)
        {
            
        }

        public void SaveChanges()
        {
            //CreateMap();
        }

        public void DiscardChanges()
        {
            
        }
        #endregion

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }

    }
}
