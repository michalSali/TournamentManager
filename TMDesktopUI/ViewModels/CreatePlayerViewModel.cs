using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;
using TMLibrary.DataAccess;
using TMLibrary.Models;

namespace TMDesktopUI.ViewModels
{
    public class CreatePlayerViewModel : Screen
    {
		private string _firstName;
		private string _lastName;
		private string _nickname;
		private string _role;
		private int _age;

		private ModelsQueries _query;
		private ModelsSaver _saver;
		private ModelsLoader _loader;
		public CreatePlayerViewModel()
		{
			_query = new ModelsQueries();
			_saver = new ModelsSaver();
			_loader = new ModelsLoader();
		}

		public string FirstName
		{
			get { return _firstName; }
			set 
			{ 
				_firstName = value;
				NotifyOfPropertyChange(() => FirstName);
			}
		}

		public string LastName
		{
			get { return _lastName; }
			set
			{
				_lastName = value;
				NotifyOfPropertyChange(() => LastName);
			}
		}
		

		public string Nickname
		{
			get { return _nickname; }
			set
			{
				_nickname = value;
				NotifyOfPropertyChange(() => Nickname);
			}
		}
		

		public string Role
		{
			get { return _role; }
			set
			{
				_role = value;
				NotifyOfPropertyChange(() => Role);
			}
		}


		public int Age 
		{
			get { return _age; }
			set
			{
				_age = value;
				NotifyOfPropertyChange(() => Age);
			}
		}

		public bool CanCreatePlayer(string firstName, string lastName, string nickName)
		{
			return (firstName?.Length > 0) && (lastName?.Length > 0) && (nickName?.Length > 0);
		}

		public void CreatePlayer(string firstName, string lastName, string nickname, string role="", int age=0)
		{			
			if (_query.ExistsPlayer(firstName, lastName, nickname))
			{
				MessageBox.Show("Player with the given first name, last name and" +
								"nickname already exists!");
			}
			else
			{
				_saver.SavePlayer(new PlayerDisplayModel(firstName, lastName, nickname, age, role));

				FirstName = "";
				LastName = "";
				Nickname = "";
				Age = 0;
				Role = "";
			}
			
		}		
	}
}
