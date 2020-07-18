using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class MapCreatedEventModel
    {
        public MapScoreDisplayModel Map;
        public MapCreatedEventModel(MapScoreDisplayModel map)
        {
            Map = map;
        }
    }
}
