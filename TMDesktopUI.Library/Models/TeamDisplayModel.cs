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
        // potrebuje id kvoli zapisu do databazy, aby sme mohli vytvorit TournamentEntries
        public int Id;
        public string TeamName;
        public string CoachName;
        public List<PlayerDisplayModel> Players;

        public TeamDisplayModel() { }

        public TeamDisplayModel(TeamModel model)
        {
            TeamName = model.TeamName;
            CoachName = model.CoachName;
        }

    }
}
