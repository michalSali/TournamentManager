using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.EventModels
{
    public class MatchSpecificationsCreatedEventModel
    {
        public List<MatchDisplayModel> Matches;
        public MatchSpecificationsCreatedEventModel(List<MatchDisplayModel> matches)
        {
            Matches = matches;
        }
    }   
}
