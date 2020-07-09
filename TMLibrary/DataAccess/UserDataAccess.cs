using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess
{
    public class UserDataAccess
    {
        public PlayerModel GetPlayerById(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id = id };
            var output = sql.LoadData<PlayerModel, dynamic>("dbo.spPlayerLookup", p, "TMData");
            
            return output.First();
        }
    }
}
