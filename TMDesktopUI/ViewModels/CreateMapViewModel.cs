using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreateMapViewModel : Screen
    {
        private IEventAggregator _events;
        public TeamDisplayModel TeamOne;
        public TeamDisplayModel TeamTwo;

        private string _mapName;        
        private int _teamOneScore;
        private int _teamTwoScore;

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

        public string MapName
        {
            get { return _mapName; }
            set
            {
                _mapName = value;
                NotifyOfPropertyChange(() => MapName);
            }
        }
        

        private BindingList<string> _availableMaps;
        public BindingList<string> AvailableMaps
        {
            get { return _availableMaps; }
            set
            {
                _availableMaps = value;
                NotifyOfPropertyChange(() => AvailableMaps);
            }
        }
        
        public CreateMapViewModel(IEventAggregator events)
        {
            _events = events;
        }

        
        private List<string> MapNames = new List<string> { "Mirage", "Dust 2", "Train", "Nuke", "Inferno", "Overpass", "Vertigo", "Cache", "Cobblestone" };
        public void InitializeValues(MatchDisplayModel match)
        {
            TeamOne = match.TeamOne;
            TeamTwo = match.TeamTwo;
            // allows you to choose only map names, that havent been used yet
            AvailableMaps = new BindingList<string>(MapNames.Except(match.Maps.Select(map => map.MapName)).ToList());

            InitializeMapPlayerStats();
        }

        private List<MapPlayerStatsDisplayModel> _teamOneStats;
        public List<MapPlayerStatsDisplayModel> TeamOneStats
        {
            get { return _teamOneStats; }
            set
            {
                _teamOneStats = value;
                NotifyOfPropertyChange(() => TeamOneStats);
            }
        }

        private List<MapPlayerStatsDisplayModel> _teamTwoStats;
        public List<MapPlayerStatsDisplayModel> TeamTwoStats
        {
            get { return _teamTwoStats; }
            set
            {
                _teamTwoStats = value;
                NotifyOfPropertyChange(() => TeamTwoStats);
            }
        }
        private void InitializeMapPlayerStats()
        {
            foreach (var player in TeamOne.Players)
            {
                MapPlayerStatsDisplayModel newModel = new MapPlayerStatsDisplayModel();
                newModel.Player = player;
                TeamOneStats.Add(newModel);
            }

            foreach (var player in TeamTwo.Players)
            {
                MapPlayerStatsDisplayModel newModel = new MapPlayerStatsDisplayModel();
                newModel.Player = player;
                TeamOneStats.Add(newModel);
            }

            ClearMapPlayerStats();
        }

        private void ClearMapPlayerStats()
        {
            foreach (var stats in TeamOneStats)
            {
                stats.Kills = 0;
                stats.Assists = 0;
                stats.Deaths = 0;
            }

            foreach (var stats in TeamTwoStats)
            {
                stats.Kills = 0;
                stats.Assists = 0;
                stats.Deaths = 0;
            }
        }

        private void ClearMapForm()
        {
            AvailableMaps.Remove(MapName);
            MapName = null;
            TeamOneScore = 0;
            TeamTwoScore = 0;
        }

        public bool CanCreateMap()
        {
            return !string.IsNullOrWhiteSpace(MapName);
        }

        // there cannot be 2 maps with the same name (cant be Map 1: Mirage, and Map 2: Mirage)
        public void CreateMap()
        {
            
            /*
            if (IF SCORES ARE INCORRECT) {
                MessageBox.Show("...");
            }
            */
            

            MapScoreDisplayModel newMap = new MapScoreDisplayModel();
            newMap.MapName = MapName;         
            newMap.TeamOneScore = TeamOneScore;
            newMap.TeamTwoScore = TeamTwoScore;
            CreateMapPlayerStats(newMap);            

            _events.PublishOnUIThread(new MapCreatedEventModel(newMap));
            ClearMapForm();
        }

        public void CreateMapPlayerStats(MapScoreDisplayModel map)
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (var stats in TeamOneStats)
            {
                if (stats.Kills < 0 || stats.Assists < 0 || stats.Deaths < 0)
                {
                    errorMessage.AppendLine($"{TeamOne.TeamName}'s stats contains a negative number.");
                    break;
                }
            }

            foreach (var stats in TeamTwoStats)
            {
                if (stats.Kills < 0 || stats.Assists < 0 || stats.Deaths < 0)
                {
                    errorMessage.AppendLine($"{TeamTwo.TeamName}'s stats contains a negative number.");
                    break;
                }
            }

            if (errorMessage.Length == 0)
            {

                foreach (var stats in TeamOneStats)
                {
                    // maybe not neccessary to save the CurrentMap, since it currently does not have "id" anyway
                    MapPlayerStatsDisplayModel newStats = new MapPlayerStatsDisplayModel();
                    //newStats.Map = map; ?????
                    newStats.Player = stats.Player;
                    newStats.Kills = stats.Kills;
                    newStats.Assists = stats.Assists;
                    newStats.Deaths = stats.Deaths;
                    map.TeamOneStats.Add(newStats);
                }

                foreach (var stats in TeamTwoStats)
                {
                    // maybe not neccessary to save the CurrentMap, since it currently does not have "id" anyway
                    MapPlayerStatsDisplayModel newStats = new MapPlayerStatsDisplayModel();
                    //newStats.Map = map;
                    newStats.Player = stats.Player;
                    newStats.Kills = stats.Kills;
                    newStats.Assists = stats.Assists;
                    newStats.Deaths = stats.Deaths;
                    map.TeamTwoStats.Add(newStats);
                }

                ClearMapPlayerStats();

            }
            else
            {
                MessageBox.Show(errorMessage.ToString());
            }

        }

        public void ReturnToMatchCreation()
        {
            _events.PublishOnUIThread(new ReturnToMatchCreationEvent());
        }
    }
}
