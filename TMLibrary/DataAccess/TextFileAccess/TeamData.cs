using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TMLibrary.Internal.DataAccess.TextFileDataAccess;
using TMLibrary.Models;

namespace TMLibrary.DataAccess.TextFileAccess
{
    public class TeamData
    {
        private static readonly string TeamFileName = "TeamModels.csv";
        private static readonly string TeamMemberFileName = "TeamMemberModels.csv";

        public TeamData() { }
           
        public List<TeamModel> GetAllTeams()
        {
            var output = TeamFileName.FullFilePath().LoadFile().ConvertToTeamModels();
            return output;
        }

        public List<TeamModel> GetTeamsByName(string teamName)
        {
            var output = GetAllTeams().Where(team => team.TeamName == teamName).ToList();
            return output;
        }

        public TeamModel GetTeam(int Id)
        {
            var output = GetAllTeams().Where(team => team.Id == Id).Single();
            return output;
        }
        

        public int CreateTeamReturnId(TeamModel team)
        {
            var teams = GetAllTeams();

            int newId = 1;

            if (teams.Count > 0)
            {
                newId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            team.Id = newId;
            teams.Add(team);

            teams.SaveToTeamFile(TeamFileName);
            return newId;
        }

        /*
        public int CreateTeamReturnIdAsync(TeamModel team)
        {
            var teams = GetAllTeams();

            int newId = 1;

            if (teams.Count > 0)
            {
                newId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            team.Id = newId;
            teams.Add(team);

            // fire and forget
            // or wait to check if was successful ?
            Task.Factory.StartNew(() => teams.SaveToTeamFile(TeamFileName));
            return newId;
        }
        */

        public List<TeamMemberModel> GetAllTeamMembers()
        {
            var output = TeamMemberFileName.FullFilePath().LoadFile().ConvertToTeamMemberModels();
            return output;
        }

        public List<TeamMemberModel> GetTeamMembers(int teamId)
        {
            var output = GetAllTeamMembers().Where(x => x.TeamId == teamId).ToList();
            return output;
        }

        public void CreateTeamMember(TeamMemberModel teamMember)
        {
            var teamMembers = GetAllTeamMembers();

            int newId = 1;

            if (teamMembers.Count > 0)
            {
                newId = teamMembers.OrderByDescending(x => x.Id).First().Id + 1;
            }

            teamMember.Id = newId;
            teamMembers.Add(teamMember);

            teamMembers.SaveToTeamMemberFile(TeamMemberFileName);            
        }
    }
}
