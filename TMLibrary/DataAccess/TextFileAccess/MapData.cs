using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using static TMLibrary.Internal.DataAccess.TextFileDataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.TextFileAccess
{
    // mapScore and mapPlayerStats sql data access
    public class MapData
    {
        private static readonly string MapScoreFileName = "MapScoreModels.csv";
        private static readonly string MapPlayerStatsFileName = "MapPlayerStatsModels.csv";

        public MapData() { }
        
        public List<MapScoreModel> GetAllMapScores()
        {
            var output = MapScoreFileName.FullFilePath().LoadFile().ConvertToMapScoreModels();
            return output;
        }

        public List<MapScoreModel> GetMapScores(int matchId)
        {
            var output = GetAllMapScores().Where(x => x.MatchId == matchId).ToList();
            return output;
        }        

        public int CreateMapScoreReturnId(MapScoreModel mapScore)
        {
            var mapScores = GetAllMapScores();

            int newId = 1;

            if (mapScores.Count > 0)
            {
                newId = mapScores.OrderByDescending(x => x.Id).First().Id + 1;
            }

            mapScore.Id = newId;
            mapScores.Add(mapScore);

            mapScores.SaveToMapScoreFile(MapScoreFileName);
            return newId;
        }

        public List<MapPlayerStatsModel> GetAllMapPlayerStats()
        {
            var output = MapPlayerStatsFileName.FullFilePath().LoadFile().ConvertToMapPlayerStatsModels();
            return output;
        }

        public List<MapPlayerStatsModel> GetMapPlayerStats(int mapScoreId)
        {
            var output = GetAllMapPlayerStats().Where(x => x.MapScoreId == mapScoreId).ToList();
            return output;
        }

        public void CreateMapPlayerStats(MapPlayerStatsModel mapPlayerStats)
        {
            var stats = GetAllMapPlayerStats();

            int newId = 1;

            if (stats.Count > 0)
            {
                newId = stats.OrderByDescending(x => x.Id).First().Id + 1;
            }

            mapPlayerStats.Id = newId;
            stats.Add(mapPlayerStats);

            stats.SaveToMapPlayerStatsFile(MapPlayerStatsFileName);            
        }
    }
}
