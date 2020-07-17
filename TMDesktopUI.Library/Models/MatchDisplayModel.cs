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
        public int MatchNumber;
        public DateTime Date;
        public int Format;
        //public int TeamOneId { get; set; }
        //public int TeamTwoId { get; set; }
        public int TeamOneScore;
        public int TeamTwoScore;

        // napr. ak ide o informaciu, ktora nejde zachytit inak:
        // napr. niektory zapas je o 3. miesto, pricom toto sa neda nijak zachytit
        public string MatchDescription;
        public int MatchImportance; // 0 - GroupStage/Other, 1 - Quarterfinal, 2 - Semifinal, 3 - Consolidation Final, 4 - Final
        public string MatchInfo
        {
            get
            {
                return $"{Date.ToString("dd.MM.")} - {TeamOne.TeamName} vs. {TeamTwo.TeamName} ({TeamOneScore} : {TeamTwoScore})";
            }                        
        }       

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

        public MatchDisplayModel()
        {
        }
    }
}
