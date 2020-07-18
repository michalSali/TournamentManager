using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class CreateMapEventModel
    {
        public MatchDisplayModel Match;
        public CreateMapEventModel(MatchDisplayModel match)
        {
            Match = match;
        }
    }
}
