using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class TeamModel
    {
        public int Id;
        public string TeamName;
        public string CoachName;

        public TeamModel(string teamName, string coachName)
        {
            TeamName = teamName;
            CoachName = coachName;
        }

        public TeamModel() { }
    }
}
