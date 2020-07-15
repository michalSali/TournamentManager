using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class MatchDisplayModel
    {
        //public int Id { get; set; }
        //public int TournamentId { get; set; }
        public int MatchNumber { get; set; }
        public DateTime Date { get; set; }
        public int Format { get; set; }
        //public int TeamOneId { get; set; }
        //public int TeamTwoId { get; set; }
        public int TeamOneScore { get; set; }
        public int TeamTwoScore { get; set; }

        public List<MapScoreDisplayModel> Maps;
        public TeamDisplayModel TeamOne { get; set; }
        public TeamDisplayModel TeamTwo { get; set; }
        public TournamentDisplayModel Tournament { get; set; }  // CYCLIC DEPENDENCY ???
        
        public MatchDisplayModel(MatchModel model)
        {
            MatchNumber = model.MatchNumber;
            Date = model.Date;
            Format = model.Format;
            TeamOneScore = model.TeamOneScore;
            TeamTwoScore = model.TeamTwoScore;
        }
    }
}
