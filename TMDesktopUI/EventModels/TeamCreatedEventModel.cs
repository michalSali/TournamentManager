using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class TeamCreatedEventModel
    {
        public TeamDisplayModel Team;
        public TeamCreatedEventModel(TeamDisplayModel team)
        {
            Team = team;
        }
    }
}
