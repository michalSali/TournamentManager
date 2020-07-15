using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TMLibrary.DataAccess;
using TMLibrary.Models;

namespace TMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {

        private CreateTournamentViewModel _createTournamentVM;
        private CreateTeamViewModel _createTeamVM;

        public void Handle(CreateTeamEvent createTeamEvent)
        {
            _createTeamVM._tournamentDisplayModel = _createTournamentVM.Tournament;
            Activated();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        public ShellViewModel()
        {
            UserDataAccess db = new UserDataAccess();
            int id = 5;
            if (db.ExistsPlayer(id))
            {
                PlayerModel _player = db.GetPlayerById(id);
                FirstName = _player.FirstName;
                LastName = _player.LastName;
                Nickname = _player.Nickname;
            } else
            {
                MessageBox.Show("Player with such id does not exist in the database.");
            }
        }
    }
}
