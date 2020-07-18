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
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }


        public string FullName
        {
            get 
            { 
                return $"{FirstName} \'{Nickname}\' {LastName}"; 
            }            
        }

        public TeamDisplayModel Team;

        public PlayerDisplayModel(PlayerModel model)
        {
            Id = model.Id;
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

        public PlayerDisplayModel()
        {
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
