using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class MatchModel
    {
        public int Id;
        public int TournamentId;
        public int MatchNumber;
        public DateTime Date;
        public int Format;
        public int TeamOneId;
        public int TeamTwoId;
        public int TeamOneScore;
        public int TeamTwoScore;

        public MatchModel(int tournamentId, int matchNumber, DateTime date, int format, int teamOneId,
                          int teamTwoId, int teamOneScore, int teamTwoScore)
        {
            TournamentId = tournamentId;
            MatchNumber = matchNumber;
            Date = date;
            Format = format;
            TeamOneId = teamOneId;
            TeamTwoId = teamTwoId;
            TeamOneScore = teamOneScore;
            TeamTwoScore = teamTwoScore;
        }

        public MatchModel() { }
    }
}
