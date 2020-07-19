using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.SqlAccess
{
    // player data access
    public class PlayerData
    {

        private SqlDataAccess sql;

        public PlayerData()
        {
            sql = new SqlDataAccess();
        }

        public List<PlayerModel> GetAllPlayers()
        {
            return sql.LoadData<PlayerModel, dynamic>("dbo.spPlayer_GetAll", new { }, "TMData");
        }

        public PlayerModel GetPlayer(int id)
        {           
            var p = new { Id = id };
            var output = sql.LoadData<PlayerModel, dynamic>("dbo.spPlayer_GetById", p, "TMData");

            // if output is empty, error
            // or output.Single() ?
            return output.First();
        }

        /*
        public bool ExistsPlayer(int id)
        {
            var p = new { Id = id };
            var output = sql.LoadData<PlayerModel, dynamic>("dbo.spPlayer_GetById", p, "TMData");

            return output.Count > 0;
        }
        */

        public bool ExistsPlayer(string firstName, string lastName, string nickName)
        {
            var p = new
            {
                FirstName = firstName,
                LastName = lastName,
                NickName = nickName,
            };
            var output = sql.LoadData<PlayerModel, dynamic>("dbo.spPlayer_GetByAllNames", p, "TMData");

            return output.Count > 0;
        }

        public void CreatePlayer(PlayerModel player)
        {            
            var p = new
            {
                player.FirstName,
                player.LastName,
                player.Nickname,
                player.Age,
                player.Role         
            };

            sql.SaveData<dynamic>("dbo.spPlayer_Create", p, "TMData");
        }

        public PlayerModel CreatePlayerReturnModel(PlayerModel player)
        {            
            var p = new
            {
                player.FirstName,
                player.LastName,
                player.Nickname,
                player.Age,
                player.Role
            };
            
            player.Id = sql.SaveDataReturnId<dynamic>("dbo.spPlayer_CreateReturnId", p, "TMData");  
            return player;
        }

        public int CreatePlayerReturnId(PlayerModel player)
        {
            var p = new
            {
                player.FirstName,
                player.LastName,
                player.Nickname,
                player.Age,
                player.Role
            };
            
            int id = sql.SaveDataReturnId<dynamic>("dbo.spPlayer_CreateReturnId", p, "TMData");            
            return id;
        }

    }
}
