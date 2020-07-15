using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess
{
    // mapScore and mapPlayerStats data access
    public class MapData
    {
        private SqlDataAccess sql;

        public MapData()
        {
            sql = new SqlDataAccess();
        }

        public List<MapScoreModel> GetMapScores(int matchId)
        {
            var p = new { MatchId = matchId };
            var output = sql.LoadData<MapScoreModel, dynamic>("dbo.spMapScore_GetByMatchId", p, "TMData");
            return output;
        }

        public void CreateMapScore(MapScoreModel mapScore)
        {
            var p = new
            {
                mapScore.MatchId,
                mapScore.MapNumber,
                mapScore.MapName,
                mapScore.TeamOneScore,
                mapScore.TeamTwoScore
            };

            sql.SaveData<dynamic>("dbo.spMapScore_Create", p, "TMData");
        }

        public MapScoreModel CreateMapScoreReturnModel(MapScoreModel mapScore)
        {
            var p = new
            {
                mapScore.MatchId,
                mapScore.MapNumber,
                mapScore.MapName,
                mapScore.TeamOneScore,
                mapScore.TeamTwoScore
            };

            mapScore.Id = sql.SaveDataReturnId<dynamic>("dbo.spMapScore_CreateReturnId", p, "TMData");
            return mapScore;
        }

        public int CreateMapScoreReturnId(MapScoreModel mapScore)
        {
            var p = new
            {
                mapScore.MatchId,
                mapScore.MapNumber,
                mapScore.MapName,
                mapScore.TeamOneScore,
                mapScore.TeamTwoScore
            };

            int id = sql.SaveDataReturnId<dynamic>("dbo.spMapScore_CreateReturnId", p, "TMData");
            return id;
        }

        public List<MapPlayerStatsModel> GetMapPlayerStats(int mapScoreId)
        {
            var p = new { MapScoreId = mapScoreId };
            var output = sql.LoadData<MapPlayerStatsModel, dynamic>("dbo.spMapScore_GetByMatchId", p, "TMData");
            return output;
        }

        public void CreateMapPlayerStats(MapPlayerStatsModel mapPlayerStats)
        {
            var p = new
            {
                mapPlayerStats.MapScoreId,
                mapPlayerStats.PlayerId,
                mapPlayerStats.Kills,
                mapPlayerStats.Assists,
                mapPlayerStats.Deaths                
            };

            sql.SaveData<dynamic>("dbo.spMapPlayerStats_Create", p, "TMData");
        }
    }


    }
}
