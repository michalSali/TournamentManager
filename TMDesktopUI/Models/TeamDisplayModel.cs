﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Models
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

        public bool Equals(TeamDisplayModel team)
        {
            if (team == null)
            {
                return false;
            }

            if (TeamName != team.TeamName)
            {
                return false;
            }

            for (int i = 0; i < Players.Count; ++i)
            {
                if (!Players[i].Equals(team.Players[i]))
                {
                    return false;
                }
            }

            return true;
        }

    }
}
