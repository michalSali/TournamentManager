using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class PlayerDisplayModel
    {
        // player potrebuje id kvoli zapisu do databazy, aby sme mohli vytvorit TeamMembers
        public int Id;
        public string FirstName;
        public string LastName;
        public string Nickname;
        public int Age;
        public string Role;
        

        public string FullName
        {
            get 
            { 
                return $"{FirstName} \"{Nickname}\" {LastName}"; 
            }            
        }

        public TeamDisplayModel Team;

        public PlayerDisplayModel(PlayerModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            Nickname = model.Nickname;
            Age = model.Age;
            Role = model.Role;
        }

        public PlayerDisplayModel(string firstName, string lastName, string nickname, int age, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            Age = age;
            Role = role;
        }

        public bool Equals(PlayerDisplayModel player)
        {
            if (player == null)
            {
                return false;
            }
            return (FirstName.Equals(player.FirstName)
                 && LastName.Equals(player.LastName)
                 && Nickname.Equals(player.Nickname));
        }
    }
}
