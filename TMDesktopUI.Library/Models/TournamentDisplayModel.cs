using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class TournamentDisplayModel
    {
        //public int Id { get; set; }
        public string TournamentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Prizepool { get; set; }
        public List<TeamDisplayModel> Teams { get; set; } = new List<TeamDisplayModel>();
        public List<MatchDisplayModel> Matches { get; set; } = new List<MatchDisplayModel>();

        public TournamentDisplayModel(TournamentModel model)
        {
            TournamentName = model.TournamentName;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            Prizepool = model.Prizepool;
        }

        public TournamentDisplayModel()
        {
        }
    }
}
