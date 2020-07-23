using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class TeamDisplayModel
    {        
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string CoachName { get; set; }
        public List<PlayerDisplayModel> Players { get; set; } = new List<PlayerDisplayModel>();

        public TeamDisplayModel() { }

        public TeamDisplayModel(TeamModel model)
        {
            TeamName = model.TeamName;
            CoachName = model.CoachName;
        }
       

        // so that we can use PlayerDisplayModel as a key in a dictionary
        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TeamDisplayModel);
        }

        public bool Equals(TeamDisplayModel obj)
        {
            return obj != null && obj.Id == this.Id;
        }

    }
}
