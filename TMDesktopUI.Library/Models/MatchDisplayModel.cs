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

        // napr. ak ide o informaciu, ktora nejde zachytit inak:
        // napr. niektory zapas je o 3. miesto, pricom toto sa neda nijak zachytit
        public string MatchDescription { get; set; }
        public int MatchImportance { get; set; } // 0 - GroupStage/Other, 1 - Quarterfinal, 2 - Semifinal, 3 - Consolidation Final, 4 - Final
        public string MatchInfo
        {
            get
            {
                return $"{Date.ToString("dd.MM.")} - {TeamOne.TeamName} vs. {TeamTwo.TeamName} ({TeamOneScore} : {TeamTwoScore})";
            }                        
        }

        public string ResultInfo
        {
            get
            {
                return $"{TeamOne.TeamName} - {TeamOneScore} : {TeamTwoScore} - {TeamTwo.TeamName}";
            }
        }

        public string DateFormatted
        {
            get
            {
                return $"{Date.ToString("dd MM yyyy")}";
            }
        }

        public List<MapScoreDisplayModel> Maps { get; set; } = new List<MapScoreDisplayModel>();
        public PlayerDisplayModel TeamOne { get; set; }
        public PlayerDisplayModel TeamTwo { get; set; }
        public TournamentDisplayModel Tournament { get; set; } // CYCLIC DEPENDENCY ???
        
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
