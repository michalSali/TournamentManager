﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class TeamCreatedEventModel
    {
        public PlayerDisplayModel Team;
        public TeamCreatedEventModel(PlayerDisplayModel team)
        {
            Team = team;
        }
    }
}