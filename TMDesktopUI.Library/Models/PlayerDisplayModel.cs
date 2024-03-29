﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMLibrary.Models;

namespace TMDesktopUI.Library.Models
{
    public class PlayerDisplayModel
    {        
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

        
        // so that we can use PlayerDisplayModel as a key in a dictionary
        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as PlayerDisplayModel);
        }

        public bool Equals(PlayerDisplayModel obj)
        {
            return obj != null && obj.Id == this.Id;
        }
                   
        
    }
}
