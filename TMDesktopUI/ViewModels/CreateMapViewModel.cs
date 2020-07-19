﻿using Caliburn.Micro;
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
        private Random _random;

        private string _mapName;        
        private int _teamOneScore;
        private int _teamTwoScore;
        public MatchDisplayModel Match { get; set; }

        private PlayerDisplayModel _teamOne;
        private PlayerDisplayModel _teamTwo;

        public PlayerDisplayModel TeamOne
        {
            get { return _teamOne; }
            set
            {
                _teamOne = value;
                NotifyOfPropertyChange(() => TeamOne);
            }
        }

        public PlayerDisplayModel TeamTwo
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

        // ======================================================= CONSTRUCTOR ===============================================

        /*
        public CollectionViewSource TeamOneStatsViewSource { get; set; }
        public CollectionViewSource TeamTwoStatsViewSource { get; set; }

        // Gets or sets the ObservableCollection
        public ObservableCollection<MapPlayerStatsDisplayModel> TeamOneStatsCollection { get; set; }
        public ObservableCollection<MapPlayerStatsDisplayModel> TeamTwoStatsCollection { get; set; }
        */

        public CreateMapViewModel(IEventAggregator events)
        {
            _events = events;
            _random = new Random();
        }

        
        private List<string> MapNames = new List<string> { "Mirage", "Dust 2", "Train", "Nuke", "Inferno", "Overpass", "Vertigo", "Cache", "Cobblestone" };
        public void InitializeValues(MatchDisplayModel match)
        {
            TeamOne = match.TeamOne;
            TeamTwo = match.TeamTwo;
            Match = match;
            // allows you to choose only map names, that havent been used yet
            AvailableMaps = new BindingList<string>(MapNames.Except(match.Maps.Select(map => map.MapName)).ToList());

            InitializeMapPlayerStats();

            /*
            // testing 
            StatsCollection = new ObservableCollection<MapPlayerStatsDisplayModel>();
            foreach (var player in TeamOne.Players)
            {
                MapPlayerStatsDisplayModel stats = new MapPlayerStatsDisplayModel();
                stats.Player = player;
                stats.Kills = _random.Next(10, 30);
                stats.Assists = _random.Next(13);
                stats.Deaths = _random.Next(10, 25);
                StatsCollection.Add(stats);
            }

            ViewSource = new CollectionViewSource();
            ViewSource.Source = StatsCollection;

            ViewSource.SortDescriptions.Add(new SortDescription("Kills", ListSortDirection.Descending));

            // Let the UI control refresh in order for changes to take place.
            ViewSource.View.Refresh();
            */
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
            TeamOneStats = new List<MapPlayerStatsDisplayModel>();
            TeamTwoStats = new List<MapPlayerStatsDisplayModel>();

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
                TeamTwoStats.Add(newModel);
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

            InitializeMapPlayerStats();
            ClearMapPlayerStats();
        }

        public bool CanCreateMap
        {
            get { return !string.IsNullOrWhiteSpace(MapName); }                
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

            // Match has to saved at initialization ??
            newMap.Match = Match;
            
            CreateMapPlayerStats(newMap);            

            _events.PublishOnUIThread(new MapCreatedEventModel(newMap));
            ClearMapForm();
            //ClearMapPlayerStats();
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

            // MAP PLAYER STATS ARE ALREADY CREATED AND POPULATED, THIS WOULD CREATE 10 STATS FOR EACH TEAM, 20 IN TOTAL
            if (errorMessage.Length == 0)
            {

                map.TeamOneStats = new List<MapPlayerStatsDisplayModel>(TeamOneStats);
                map.TeamTwoStats = new List<MapPlayerStatsDisplayModel>(TeamTwoStats);
                //ClearMapPlayerStats();

                /*
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
                */
                // ClearMapPlayerStats();

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

            /*
            int lowerRangeMin;
            int lowerRangeMax;
            int middleRangeMin;
            int middleRangeMax;
            int upperRangeMin;
            int upperRangeMax;
            */

            int maxDeaths = TeamOneScore + TeamTwoScore;

            var randomTeamOneStats = new List<MapPlayerStatsDisplayModel>();
            var randomTeamTwoStats = new List<MapPlayerStatsDisplayModel>();

            /*
            foreach (var stats in TeamOneStats)
            {
                MapPlayerStatsDisplayModel newStats = new MapPlayerStatsDisplayModel();
                newStats.Player = stats.Player;
                newStats.Kills = random.Next(TeamOneScore/2, maxDeaths);
                newStats.Assists = random.Next(0, TeamOneScore);
                newStats.Deaths = random.Next(TeamTwoScore, maxDeaths);
                randomTeamOneStats.Add(newStats);
            }

            foreach (var stats in TeamTwoStats)
            {
                MapPlayerStatsDisplayModel newStats = new MapPlayerStatsDisplayModel();
                newStats.Player = stats.Player;
                newStats.Kills = random.Next(TeamOneScore / 2, maxDeaths);
                newStats.Assists = random.Next(0, TeamOneScore);
                newStats.Deaths = random.Next(TeamTwoScore, maxDeaths);
                randomTeamTwoStats.Add(newStats);
            }
            */

            foreach (var stats in TeamOneStats)
            {
                stats.Kills = random.Next(TeamTwoScore / 2, maxDeaths);
                stats.Assists = random.Next(0, TeamTwoScore);
                stats.Deaths = random.Next(TeamOneScore, maxDeaths);
            }

            foreach (var stats in TeamTwoStats)
            {
                stats.Kills = random.Next(TeamTwoScore / 2, maxDeaths);
                stats.Assists = random.Next(0, TeamTwoScore);
                stats.Deaths = random.Next(TeamOneScore, maxDeaths);
            }

            NotifyOfPropertyChange(() => TeamOneStats);
            NotifyOfPropertyChange(() => TeamTwoStats);

            
            //TeamOneStats = TeamOneStats;
            //TeamTwoStats = TeamTwoStats;

        }

        private void SetUpScores()
        {
            Random random = new Random();

            //if ((TeamOneScore == 0 && TeamTwoScore == 0) || 
            // correct format score is either: a) 16 : [0, 14],  or b) [0, 14] : 16
            // we wont be dealing with overtimes - overtimes can be setup if the users create the map themselves
            if (!((TeamOneScore == 16 && TeamTwoScore >= 0 && TeamTwoScore <= 14) ||
                  (TeamTwoScore == 16 && TeamOneScore >= 0 && TeamOneScore <= 14)))               
            {
                
                /* if we want to choose winning team beforehand
                 * 
                if (isWinningTeamSet)
                {
                    bool firstTeamWins = firstTeam;
                }
                */
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
                // randomize team scores (one team = 16, other team 0-14), no overtimes
            }
        }

        public void ReturnToMatchCreation()
        {
            _events.PublishOnUIThread(new ReturnToMatchCreationEvent());
        }


        // xaml
        /*
        <TextBlock Grid.Row="8" Grid.Column= "4" Margin= "0 0 0 10" >
            < TextBlock.Text >
                < MultiBinding StringFormat= "{}{0} player stats:" >
                    < Binding Path= "TeamOne.TeamName" />
                </ MultiBinding >
            </ TextBlock.Text >
        </ TextBlock >
        */

        /*
        <DataGrid ItemsSource = "{Binding TeamOneStats}" Grid.Row="9" Grid.Column="1" Grid.ColumnSpan="2"
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False" >
            <DataGrid.Columns>
                <DataGridTextColumn Header = "Player's Nickname" IsReadOnly="True"
                                    Binding="{Binding Path=Player.Nickname}"/>
                <DataGridTextColumn Header = "Kills" Binding="{Binding Path=Kills}"/>
                <DataGridTextColumn Header = "Assists" Binding="{Binding Path=Assists}"/>
                <DataGridTextColumn Header = "Deaths" Binding="{Binding Path=Deaths}"/>
            </DataGrid.Columns>
        </DataGrid>

        <DataGrid x:Name="TeamTwoStatsDataGrid" ItemsSource="{Binding TeamTwoStats}" Grid.Row="9" Grid.Column="3" Grid.ColumnSpan="2"
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False" >
            <DataGrid.Columns>
                <DataGridTextColumn Header = "Player's Nickname" IsReadOnly="True"
                                    Binding="{Binding Path=Player.Nickname}"/>
                <DataGridTextColumn Header = "Kills" Binding="{Binding Path=Kills}"/>
                <DataGridTextColumn Header = "Assists" Binding="{Binding Path=Assists}"/>
                <DataGridTextColumn Header = "Deaths" Binding="{Binding Path=Deaths}"/>
            </DataGrid.Columns>
        </DataGrid>



        <DataGrid ItemsSource="{Binding ViewSource.View}" Grid.Row="4" Grid.Column="4" Grid.ColumnSpan="2"
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False" >
            <DataGrid.Columns>
                <DataGridTextColumn Header="Player's Nickname" IsReadOnly="True"
                                    Binding="{Binding Path=Player.Nickname}"/>
                <DataGridTextColumn Header="Kills" Binding="{Binding Path=Kills}"/>
                <DataGridTextColumn Header="Assists" Binding="{Binding Path=Assists}"/>
                <DataGridTextColumn Header="Deaths" Binding="{Binding Path=Deaths}"/>
            </DataGrid.Columns>
        </DataGrid>
        */

    }
}