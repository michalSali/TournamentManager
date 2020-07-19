using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TMLibrary.Internal.DataAccess.TextFileDataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.TextFileAccess
{
    public class PlayerData
    {
        private static readonly string PlayerFileName = "PlayerModels.csv";

        public PlayerData() { }

        public List<PlayerModel> GetAllPlayers()
        {
            var output = PlayerFileName.FullFilePath().LoadFile().ConvertToPlayerModels();
            return output;
        }

        public PlayerModel GetPlayer(int id)
        {            
            var output = GetAllPlayers().Where(player => player.Id == id).Single();
            return output;           
        }

        public bool ExistsPlayer(string firstName, string lastName, string nickName)
        {
            var output = GetAllPlayers().Where(player => (player.FirstName == firstName) && (player.LastName == lastName) && (player.Nickname == nickName)).ToList();
            return output.Count > 0;
        }


        public int CreatePlayerReturnId(PlayerModel player)
        {
            var players = GetAllPlayers();

            int newId = 1;

            if (players.Count > 0)
            {
                newId = players.OrderByDescending(x => x.Id).First().Id + 1;
            }

            player.Id = newId;
            players.Add(player);

            players.SaveToPlayerFile(PlayerFileName);
            return newId;
        }
    }
}
