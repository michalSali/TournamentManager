﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class MatchDisplayModel
    {        
        public int MatchNumber { get; set; }
        public DateTime Date { get; set; }
        public int Format { get; set; }        
        public int TeamOneScore { get; set; }
        public int TeamTwoScore { get; set; }
        
        public string MatchDescription { get; set; }
        public int MatchImportance { get; set; } // 0 - GroupStage/Other, 1 - Quarterfinal, 2 - Semifinal, 3 - Final
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
                return $"{Date.ToString("dd.MM.yyyy")}";
            }
        }

        public List<MapScoreDisplayModel> Maps { get; set; } = new List<MapScoreDisplayModel>();
        public TeamDisplayModel TeamOne { get; set; }
        public TeamDisplayModel TeamTwo { get; set; }
        public TournamentDisplayModel Tournament { get; set; }
        
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
