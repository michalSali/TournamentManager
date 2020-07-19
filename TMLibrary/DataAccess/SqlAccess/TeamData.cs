using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Internal.DataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.SqlAccess
{
    // team and teamMember data access
    public class TeamData
    {
        private SqlDataAccess sql;

        public TeamData()
        {
            sql = new SqlDataAccess();
        }

        public List<TeamModel> GetAllTeams()
        {
            var output = sql.LoadData<TeamModel, dynamic>("dbo.spTeam_GetAll", new { }, "TMData");
            return output;
        }

        public List<TeamModel> GetTeamsByName(string teamName)
        {
            var p = new { TeamName = teamName };
            var output = sql.LoadData<TeamModel, dynamic>("dbo.spTeam_GetByTeamName", p, "TMData");
            return output;
        }

        public TeamModel GetTeam(int teamId)
        {
            var p = new { @Id = teamId };
            var output = sql.LoadData<TeamModel, dynamic>("dbo.spTeam_GetById", p, "TMData");
            return output.First();
        }

        public void CreateTeam(TeamModel team)
        {
            var p = new
            {
                team.TeamName,
                team.CoachName            
            };

            sql.SaveData<dynamic>("dbo.spTeam_Create", p, "TMData");
        }

        public TeamModel CreateTeamReturnModel(TeamModel team)
        {
            var p = new
            {
                team.TeamName,
                team.CoachName
            };
            
            team.Id = sql.SaveDataReturnId<dynamic>("dbo.spTeam_CreateReturnId", p, "TMData");
            return team;
        }

        public int CreateTeamReturnId(TeamModel team)
        {
            var p = new
            {
                team.TeamName,
                team.CoachName
            };

            int id = sql.SaveDataReturnId<dynamic>("dbo.spTeam_CreateReturnId", p, "TMData");
            return id;
        }

        public List<TeamMemberModel> GetAllTeamMembers()
        {
            var output = sql.LoadData<TeamMemberModel, dynamic>("dbo.spTeamMember_GetAll", new { }, "TMData");
            return output;
        }

        public List<TeamMemberModel> GetTeamMembers(int teamId)
        {
            var p = new { TeamId = teamId };
            var output = sql.LoadData<TeamMemberModel, dynamic>("dbo.spTeamMember_GetByTeamId", p, "TMData");
            return output;
        }

        public void CreateTeamMember(TeamMemberModel teamMember)
        {
            var p = new
            {
                teamMember.TeamId,
                teamMember.PlayerId
            };

            sql.SaveData<dynamic>("dbo.spTeamMember_Create", p, "TMData");
        }
    }
}
