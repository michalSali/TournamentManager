using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreateMatchViewModel : Screen
    {

        private TournamentDisplayModel Tournament;
        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        public CreateMatchViewModel()
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();
        }

        public CreateMatchViewModel(TournamentDisplayModel tournament) : this()
        {
            Tournament = tournament;
            //_matchNumber = tournament.Matches.Count + 1;
            DisplayedTeams = new BindingList<TeamDisplayModel>(tournament.Teams);
        }      

        //private int _matchNumber;               
        private int _format;
        private DateTime _date;

        private int _teamOneScore;
        private int _teamTwoScore;

        private BindingList<MapScoreDisplayModel> _maps;
        private TeamDisplayModel _teamOne;
        private TeamDisplayModel _teamTwo;       

        private string _mapName;
        private int _mapNumber;

        private int _teamOneMapScore;
        private int _teamTwoMapScore;

        public List<int> Formats = new List<int> { 1, 3, 5, 7 };
        public List<string> MapNames = new List<string> { "Mirage", "Dust 2", "Train", "Nuke", "Inferno", "Overpass", "Vertigo", "Cache", "Cobblestone" };
        public List<int> PossibleMatchScores;

        public int TeamOneMapScore
        {
            get { return _teamOneMapScore; }
            set
            {
                _teamOneMapScore = value;
                NotifyOfPropertyChange(() => TeamOneMapScore);
            }
        }

        public int TeamTwoMapScore
        {
            get { return _teamTwoMapScore; }
            set
            {
                _teamTwoMapScore = value;
                NotifyOfPropertyChange(() => TeamTwoMapScore);
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

        public int MapNumber
        {
            get { return _mapNumber; }
            set
            {
                _mapNumber = value;
                NotifyOfPropertyChange(() => MapNumber);
            }
        }

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

            if (MapsHaveSameName())
            {
                errorMessage.AppendLine($"There cannot be 2 or more maps with the same name in one match.");
            }

            if (errorMessage.Length == 0)
            {
                MatchDisplayModel newMatch = new MatchDisplayModel();
                newMatch.Date = Date;
                newMatch.Format = Format;
                newMatch.TeamOne = TeamOne;
                newMatch.TeamTwo = TeamTwo;
                newMatch.Tournament = Tournament;
                newMatch.MatchImportance = 0;
                newMatch.Maps = new List<MapScoreDisplayModel>(Maps);                
                
                Tournament.Matches.Add(newMatch);
                ClearMatchForm();
            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }
        }

        private bool MapsHaveSameName()
        {
            var mapNames = Maps.Select(map => map.MapName).ToList();
            return (mapNames.Count != mapNames.ToHashSet().Count);
        }

        // what to do with dates?
        private void ClearMatchForm()
        {                        
            Maps.Clear();                        
            SelectedMap = null;        
            ClearMapForm();
        }

                          
        public List<MapPlayerStatsDisplayModel> TeamOneStats;
        public List<MapPlayerStatsDisplayModel> TeamTwoStats;
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
        private void ClearMapForm()
        {
            MapName = "";

        }

        public bool CanCreateMap()
        {
            return (!string.IsNullOrWhiteSpace(MapName));
        }

        // there cannot be 2 maps with the same name (cant be Map 1: Mirage, and Map 2: Mirage)
        public void CreateMap()
        {
            /*
            if (AlreadyHasMapName(CurrentMap.MapName)) {
                MessageBox.Show("There cannot be multiple same maps in one match");
            }
            */
            MapScoreDisplayModel newMap = new MapScoreDisplayModel();
            newMap.MapName = MapName;
            // newMap.MapNumber = ???
            newMap.TeamOneScore = TeamOneMapScore;
            newMap.TeamTwoScore = TeamTwoMapScore;
            CreateMapPlayerStats(newMap);
            Maps.Add(newMap);
            ClearMapForm();
        }

        private bool AlreadyHasMapName(string mapName)
        {
            foreach (var map in Maps)
            {
                if (mapName.Equals(map.MapName))
                {
                    return true;
                }
            }
            return false; 
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

            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }

        }

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
            CreateMap();
        }

        public void DiscardChanges()
        {
            
        }


    }
}
