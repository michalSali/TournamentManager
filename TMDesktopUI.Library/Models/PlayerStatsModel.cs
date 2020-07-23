using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDesktopUI.Library.Models
{
    public class PlayerStatsModel
    {
        public int Id;
        public List<MapPlayerStatsDisplayModel> Stats;

        public int AllKills
        {
            get 
            {
                int total = 0;
                foreach (var stats in Stats)
                {
                    total += stats.Kills;
                }
                return total;
            }
        }
    }
}
