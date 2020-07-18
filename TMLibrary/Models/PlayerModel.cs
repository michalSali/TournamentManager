using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLibrary.Models
{
    public class PlayerModel
    {
        public int Id;
        public string FirstName;
        public string LastName;
        public string Nickname;
        public int Age;
        public string Role;

        public PlayerModel(string firstName, string lastName, string nickname, int age, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            Age = age;
            Role = role;
        }

        public PlayerModel() { }
    }
}
