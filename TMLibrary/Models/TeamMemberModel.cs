using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class TeamMemberModel
    {
        public int Id;
        public int TeamId;
        public int PlayerId;

        public TeamMemberModel(int teamId, int playerId)
        {
            TeamId = teamId;
            PlayerId = playerId;
        }

        public TeamMemberModel() { }
    }
}
