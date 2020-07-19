using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class DisplayMapEventModel
    {
        public TournamentDisplayModel Tournament;
        public MapScoreDisplayModel Map;
        public DisplayMapEventModel(TournamentDisplayModel tournament, MapScoreDisplayModel map)
        {
            Tournament = tournament;
            Map = map;
        }
    }
}
