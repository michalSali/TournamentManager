using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreateMapViewModel : Screen
    {
        private IEventAggregator _events;
       
        private string _mapName;        
        private int _teamOneScore;
        private int _teamTwoScore;

        private BindingList<string> _availableMaps;
        private List<string> MapNames = new List<string> { "Mirage", "Dust 2", "Train", "Nuke", "Inferno", "Overpass", "Vertigo", "Cache", "Cobblestone" };

        private TeamDisplayModel _teamOne;
        private TeamDisplayModel _teamTwo;

        public MatchDisplayModel Match { get; set; }
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
                NotifyOfPropertyChange(() => CanCreateMap);
            }
        }
                
        public BindingList<string> AvailableMaps
        {
            get { return _availableMaps; }
            set
            {
                _availableMaps = value;
                NotifyOfPropertyChange(() => AvailableMaps);
            }
        }
        
        public CollectionViewSource TeamOneStatsViewSource { get; set; }
        public CollectionViewSource TeamTwoStatsViewSource { get; set; }
       
        public ObservableCollection<MapPlayerStatsDisplayModel> TeamOneStatsCollection { get; set; }
        public ObservableCollection<MapPlayerStatsDisplayModel> TeamTwoStatsCollection { get; set; }
        

        public CreateMapViewModel(IEventAggregator events)
        {
            _events = events;           
        }
                
        public void InitializeValues(MatchDisplayModel match)
        {
            TeamOne = match.TeamOne;
            TeamTwo = match.TeamTwo;
            Match = match;

            // allows you to choose only map names, that havent been used yet
            AvailableMaps = new BindingList<string>(MapNames.Except(match.Maps.Select(map => map.MapName)).ToList());
            InitializeMapPlayerStats();                       
        }

        private List<MapPlayerStatsDisplayModel> _teamOneStats = new List<MapPlayerStatsDisplayModel>();
        public List<MapPlayerStatsDisplayModel> TeamOneStats
        {
            get { return _teamOneStats; }
            set
            {
                _teamOneStats = value;
                NotifyOfPropertyChange(() => TeamOneStats);
            }
        }

        private List<MapPlayerStatsDisplayModel> _teamTwoStats = new List<MapPlayerStatsDisplayModel>();
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
            TeamOneStatsCollection = new ObservableCollection<MapPlayerStatsDisplayModel>();
            TeamTwoStatsCollection = new ObservableCollection<MapPlayerStatsDisplayModel>();
            
            foreach (var player in TeamOne.Players)
            {
                MapPlayerStatsDisplayModel newModel = new MapPlayerStatsDisplayModel();
                newModel.Player = player;
                TeamOneStatsCollection.Add(newModel);
            }

            foreach (var player in TeamTwo.Players)
            {
                MapPlayerStatsDisplayModel newModel = new MapPlayerStatsDisplayModel();
                newModel.Player = player;
                TeamTwoStatsCollection.Add(newModel);
            }

            TeamOneStatsViewSource = new CollectionViewSource();
            TeamOneStatsViewSource.Source = TeamOneStatsCollection;

            TeamTwoStatsViewSource = new CollectionViewSource();
            TeamTwoStatsViewSource.Source = TeamTwoStatsCollection;

            ClearMapPlayerStats();
        }
       

        private void ClearMapPlayerStats()
        {
            foreach (var stats in TeamOneStatsCollection)
            {
                stats.Kills = 0;
                stats.Assists = 0;
                stats.Deaths = 0;
            }

            foreach (var stats in TeamTwoStatsCollection)
            {
                stats.Kills = 0;
                stats.Assists = 0;
                stats.Deaths = 0;
            }

            TeamOneStatsViewSource.View.Refresh();
            TeamTwoStatsViewSource.View.Refresh();
        }

       
        private void ClearMapForm()
        {
            AvailableMaps.Remove(MapName);
            MapName = null;
            TeamOneScore = 0;
            TeamTwoScore = 0;

            InitializeMapPlayerStats();
            ClearMapPlayerStats();            
        }

        public bool CanCreateMap
        {
            get { return !string.IsNullOrWhiteSpace(MapName); }                
        }
        
        public void CreateMap()
        {
            StringBuilder errorMessage = new StringBuilder();

            // we won't be checking for anything else, as in overtimes, it would be difficult to check for correctness,
            //   it is up to the user
            if (TeamOneScore == TeamTwoScore)
            {
                errorMessage.AppendLine("Ties are not allowed - one of the teams has to win in regular time or overtime.");
            }

            if (errorMessage.Length == 0)
            {
                MapScoreDisplayModel newMap = new MapScoreDisplayModel();
                newMap.MapName = MapName;
                newMap.TeamOneScore = TeamOneScore;
                newMap.TeamTwoScore = TeamTwoScore;
                newMap.Match = Match;

                CreateMapPlayerStats(newMap);

                _events.PublishOnUIThread(new MapCreatedEventModel(newMap));
                ClearMapForm();
            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }            
                     
        }

        public void CreateMapPlayerStats(MapScoreDisplayModel map)
        {
            StringBuilder errorMessage = new StringBuilder();

            foreach (var stats in TeamOneStats)
            {
                if (stats.Kills < 0 || stats.Assists < 0 || stats.Deaths < 0)
                {
                    errorMessage.AppendLine($"{TeamOne.TeamName}'s stats contain a negative number.");
                    break;
                }
            }

            foreach (var stats in TeamTwoStats)
            {
                if (stats.Kills < 0 || stats.Assists < 0 || stats.Deaths < 0)
                {
                    errorMessage.AppendLine($"{TeamTwo.TeamName}'s stats contain a negative number.");
                    break;
                }
            }
           
            if (errorMessage.Length == 0)
            {               
                map.TeamOneStats = new List<MapPlayerStatsDisplayModel>(TeamOneStatsCollection);
                map.TeamTwoStats = new List<MapPlayerStatsDisplayModel>(TeamTwoStatsCollection);                
            }
            else
            {
                MessageBox.Show(errorMessage.ToString());
            }

        }

        public void RandomizeStats()
        {
            Random random = new Random();

            if (string.IsNullOrWhiteSpace(MapName)) 
            {
                MapName = AvailableMaps[random.Next(AvailableMaps.Count)];
            }

            SetUpScores();

            int maxDeaths = TeamOneScore + TeamTwoScore;

            foreach (var stats in TeamOneStatsCollection)
            {
                stats.Kills = random.Next(TeamOneScore/2, Math.Max(maxDeaths, maxDeaths + (2*TeamOneScore - 3*TeamTwoScore)/2));
                stats.Assists = random.Next(0, 2 + TeamOneScore/3);
                stats.Deaths = random.Next(TeamTwoScore/2, Math.Min(maxDeaths, maxDeaths - (TeamOneScore - TeamTwoScore)/2));
            }

            foreach (var stats in TeamTwoStatsCollection)
            {
                stats.Kills = random.Next(TeamTwoScore/2, Math.Max(maxDeaths, maxDeaths + (2*TeamTwoScore - 3*TeamOneScore)/2));
                stats.Assists = random.Next(0, 2 + TeamTwoScore/3);
                stats.Deaths = random.Next(TeamOneScore/2, Math.Min(maxDeaths, maxDeaths - (TeamTwoScore - TeamOneScore)/2));
            }

            NotifyOfPropertyChange(() => TeamOneStats);
            NotifyOfPropertyChange(() => TeamTwoStats);

            TeamOneStatsViewSource.SortDescriptions.Add(new SortDescription("Kills", ListSortDirection.Descending));           
            TeamOneStatsViewSource.View.Refresh();

            TeamTwoStatsViewSource.SortDescriptions.Add(new SortDescription("Kills", ListSortDirection.Descending));
            TeamTwoStatsViewSource.View.Refresh();
        }

        private void SetUpScores()
        {
            Random random = new Random();
            
            // correct format score is either: a) 16 : [0, 14],  or b) [0, 14] : 16
            // we wont be dealing with overtimes - overtimes can be setup if the users create the map themselves
            if (!((TeamOneScore == 16 && TeamTwoScore >= 0 && TeamTwoScore <= 14) ||
                  (TeamTwoScore == 16 && TeamOneScore >= 0 && TeamOneScore <= 14)))               
            {                               
                bool firstTeamWins = (random.Next(2) == 0 ? false : true);
                if (firstTeamWins)
                {
                    TeamOneScore = 16;
                    bool upperHalf = (random.Next(3) == 0 ? false : true);  // 2/3 chance to get score in [7, 14]
                    if (upperHalf)
                    {
                        TeamTwoScore = random.Next(7, 15); // there is a bigger chance for team to get a score in the upper half [7, 14]
                    }
                    else
                    {
                        TeamTwoScore = random.Next(7);  // [0, 6]
                    }
                }
                else
                {
                    TeamTwoScore = 16;
                    bool upperHalf = (random.Next(3) == 0 ? false : true);  // 2/3 chance to get score in [7, 14]
                    if (upperHalf)
                    {
                        TeamOneScore = random.Next(7, 15); // there is a bigger chance for team to get a score in the upper half [7, 14]
                    }
                    else
                    {
                        TeamOneScore = random.Next(7);  // [0, 6]
                    }
                }                
            }
        }

        public void ReturnToMatchCreation()
        {
            _events.PublishOnUIThread(new ReturnToMatchCreationEvent());
        }
        
    }
}
