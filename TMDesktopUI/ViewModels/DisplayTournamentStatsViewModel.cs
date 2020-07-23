using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class DisplayTournamentStatsViewModel : Screen
    {
        private TournamentDisplayModel _tournament;
        public TournamentDisplayModel Tournament
        {
            get { return _tournament; }
            set
            {
                _tournament = value;
                NotifyOfPropertyChange(() => Tournament);
            }
        }

        private int _mostPlayedMapCount;
        public int MostPlayedMapCount 
        {
            get { return _mostPlayedMapCount; }
            set
            {
                _mostPlayedMapCount = value;
                NotifyOfPropertyChange(() => MostPlayedMapCount);
            }
        }


        private string _mostPlayedMap;
        public string MostPlayedMap
        {
            get { return _mostPlayedMap; }
            set
            {
                _mostPlayedMap = value;
                NotifyOfPropertyChange(() => MostPlayedMap);
            }
        }

        private PlayerDisplayModel _playerMostKillsTotal;
        public PlayerDisplayModel PlayerMostKillsTotal
        {
            get { return _playerMostKillsTotal; }
            set
            {
                _playerMostKillsTotal = value;
                NotifyOfPropertyChange(() => PlayerMostKillsTotal);
            }
        }

        private PlayerDisplayModel _playerMostKillsPerMap;
        public PlayerDisplayModel PlayerMostKillsPerMap
        {
            get { return _playerMostKillsPerMap; }
            set
            {
                _playerMostKillsPerMap = value;
                NotifyOfPropertyChange(() => PlayerMostKillsPerMap);
            }
        }

        private PlayerDisplayModel _playerLeastKillsPerMap;
        public PlayerDisplayModel PlayerLeastKillsPerMap
        {
            get { return _playerLeastKillsPerMap; }
            set
            {
                _playerLeastKillsPerMap = value;
                NotifyOfPropertyChange(() => PlayerLeastKillsPerMap);
            }
        }

        private int _mostKillsTotal;
        public int MostKillsTotal
        {
            get { return _mostKillsTotal; }
            set
            {
                _mostKillsTotal = value;
                NotifyOfPropertyChange(() => MostKillsTotal);
            }
        }

        private double _mostKillsPerMap;
        public double MostKillsPerMap
        {
            get { return _mostKillsPerMap; }
            set
            {
                _mostKillsPerMap = value;
                NotifyOfPropertyChange(() => MostKillsPerMap);
            }
        }

        private double _leastKillsPerMap;
        public double LeastKillsPerMap
        {
            get { return _leastKillsPerMap; }
            set
            {
                _leastKillsPerMap = value;
                NotifyOfPropertyChange(() => LeastKillsPerMap);
            }
        }


        private List<MapScoreDisplayModel> _allMaps;
        public List<MapScoreDisplayModel> AllMaps
        {
            get { return _allMaps; }
            set
            {
                _allMaps = value;
                NotifyOfPropertyChange(() => AllMaps);
            }
        }


        private List<PlayerDisplayModel> _allPlayers;
        public List<PlayerDisplayModel> AllPlayers
        {
            get { return _allPlayers; }
            set
            {
                _allPlayers = value;
                NotifyOfPropertyChange(() => AllPlayers);
            }
        }

        private List<MapPlayerStatsDisplayModel> _allPlayerStats;
        public List<MapPlayerStatsDisplayModel> AllPlayerStats
        {
            get { return _allPlayerStats; }
            set
            {
                _allPlayerStats = value;
                NotifyOfPropertyChange(() => AllPlayerStats);
            }
        }

        private IEventAggregator _events;
        public DisplayTournamentStatsViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void Initialize(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            AllMaps = new List<MapScoreDisplayModel>();
            AllPlayers = new List<PlayerDisplayModel>();
            AllPlayerStats = new List<MapPlayerStatsDisplayModel>();
            
            foreach (var match in tournament.Matches)
            {
                foreach (var map in match.Maps)
                {
                    AllMaps.Add(map);
                }
            }

            foreach (var team in tournament.Teams)
            {
                foreach (var player in team.Players)
                {
                    AllPlayers.Add(player);
                }
            }

            

            Dictionary<PlayerDisplayModel, List<MapPlayerStatsDisplayModel>> statsByPlayers = new Dictionary<PlayerDisplayModel, List<MapPlayerStatsDisplayModel>>();
            
            foreach (var player in AllPlayers)
            {
                statsByPlayers.Add(player, new List<MapPlayerStatsDisplayModel>());                
            }

            foreach (var map in AllMaps)
            {

                foreach (var stat in map.TeamOneStats)
                {
                    statsByPlayers[stat.Player].Add(stat);                    
                }

                foreach (var stat in map.TeamTwoStats)
                {
                    statsByPlayers[stat.Player].Add(stat);
                }
            }

            Dictionary<string, int> mapCountByMapName = new Dictionary<string, int>();

            foreach (var mapName in AllMaps.Select(x => x.MapName).ToHashSet())
            {
                mapCountByMapName.Add(mapName, 0);
            }

            foreach (var map in AllMaps)
            {
                mapCountByMapName[map.MapName] += 1;
            }

            var mapCounts = mapCountByMapName.Select(x => new Tuple<string, int>(x.Key, x.Value)).ToList();

            var mostPlayedMapAndCount = mapCounts.OrderByDescending(x => x.Item2).First();
            MostPlayedMap = mostPlayedMapAndCount.Item1;
            MostPlayedMapCount = mostPlayedMapAndCount.Item2;

           
            var playersAndMapCount = AllPlayers.Select(x => new Tuple<PlayerDisplayModel, int>(x, statsByPlayers[x].Count)).ToList();

            var playersAndMaps = AllPlayers.Select(x => new Tuple<PlayerDisplayModel, List<MapPlayerStatsDisplayModel>>(x, statsByPlayers[x])).ToList();

            var playersAndKills = playersAndMaps.Select(t => new Tuple<PlayerDisplayModel, int>(t.Item1, t.Item2.Sum(x => x.Kills)));

            var playersAndDeaths = playersAndMaps.Select(t => new Tuple<PlayerDisplayModel, int>(t.Item1, t.Item2.Sum(x => x.Deaths)));

            var playerWithMostKills = playersAndKills.OrderByDescending(t => t.Item2).First();

            PlayerMostKillsTotal = playerWithMostKills.Item1;
            MostKillsTotal = playerWithMostKills.Item2;

            var playerWithBestKillPerMapRatio = playersAndKills
                .OrderByDescending(t => (double)(t.Item2) / statsByPlayers[t.Item1].Count)
                .Select(t => new Tuple<PlayerDisplayModel, double>(t.Item1, (double)(t.Item2) / statsByPlayers[t.Item1].Count))
                .First();

            PlayerMostKillsPerMap = playerWithBestKillPerMapRatio.Item1;
            MostKillsPerMap = playerWithBestKillPerMapRatio.Item2;

            var playerWithWorstKillPerMapRatio = playersAndKills
                .OrderBy(t => (double)(t.Item2) / statsByPlayers[t.Item1].Count)
                .Select(t => new Tuple<PlayerDisplayModel, double>(t.Item1, (double)(t.Item2) / statsByPlayers[t.Item1].Count))
                .First();

            PlayerLeastKillsPerMap = playerWithWorstKillPerMapRatio.Item1;
            LeastKillsPerMap = playerWithWorstKillPerMapRatio.Item2;            
        }

        public void ReturnToMainScreen()
        {
            _events.PublishOnUIThread(new ReturnToMainScreenEvent());
        }

    }
}
