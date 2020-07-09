using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TMLibrary.DataAccess;
using TMLibrary.Models;

namespace TMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }

        public ShellViewModel()
        {
            UserDataAccess db = new UserDataAccess();
            PlayerModel _player = db.GetPlayerById(1);
            FirstName = _player.FirstName;
            LastName = _player.LastName;
            Nickname = _player.Nickname;
        }
    }
}
