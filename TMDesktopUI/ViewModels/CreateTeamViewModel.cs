using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TMDesktopUI.ViewModels
{
    public class CreateTeamViewModel : Conductor<object>
    {
        /*
        public Player SelectedPlayerToAdd;
        public Player SelectedPlayerToRemove;
        public List<Player> SelectedPlayers;

        public List<Player> AvailablePlayers;

        public CreateTeamViewModel()
        {
            AvailablePlayers = database.GetAllPlayers();
        }

        public void AddPlayer_OnClick(...)
        {
            SelectedPlayers.Add(SelectedPlayerToAdd);
            AvailablePlayers.Remove(SelectedPlayerToAdd);
        }

        public void RemovePlayer_OnClick(...)
        {
            SelectedPlayers.Remove(SelectedPlayerToRemove);
            AvailablePlayers.Add(SelectedPlayerToRemove);
        }

        public bool CanAddPlayer(...)
        {
            return SelectedPlayerToAdd != null && SelectedPlayers.Count < 5; // ???
        }

        public bool CanRemovePlayer(...)
        {
            return SelectedPlayerToRemove != null && SelectedPlayers.Size > 0; // ???
        }


        //public bool CanCreateTeam(...)
        //{
        //   return SelectedPlayers.Count = 5 
        //       && !String.IsNullOrEmpty(TeamName.Text)
        //        &&  ...
        //}

        public void CreateTeam_OnClick(...)
        {
            bool teamNameIsValid = IsValidTeamName(TeamName.Text);
            bool 

            if (teamNameIsValid && SelectedPlayers.Count == 5) {
                AddTeamToDatabase();
                return;
            }

            

            if (!teamNameIsValid && SelectedPlayers.Count != 5) {
               ShowMessageBox(Invalid team name and not 5 players)
            } else if (!teamNameIsValid) {
            ShowMessageBox(Invalid team name)
            } else {
             ShowMEssageBox(not 5)
            }

           
        }
        */
    }
}
