﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class DisplayPlayerEventModel
    {
        public TournamentDisplayModel Tournament;
        public PlayerDisplayModel Player;
        public DisplayPlayerEventModel(TournamentDisplayModel tournament, PlayerDisplayModel player)
        {
            Tournament = tournament;
            Player = player;
        }
    }
}
