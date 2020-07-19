using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;

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
		private IEventAggregator _events;

		public CreatePlayerViewModel(IEventAggregator events)
		{
			_query = new ModelsQueries();
			_saver = new ModelsSaver();			
			_events = events;
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
		
		public void CreatePlayer()
		{			
			StringBuilder errorMessage = new StringBuilder();

			if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Nickname))
			{
				errorMessage.AppendLine("First name, last name and nickname cannot be empty.");
			}

			if (_query.ExistsPlayer(FirstName, LastName, Nickname))
			{
				errorMessage.AppendLine("Player with this first name, last name and nickname already exist.");
			}

			if (errorMessage.Length == 0)
			{
				PlayerDisplayModel newPlayer = new PlayerDisplayModel();
				newPlayer.FirstName = FirstName;
				newPlayer.LastName = LastName;
				newPlayer.Nickname = Nickname;
				newPlayer.Role = string.IsNullOrWhiteSpace(Role) ? "Unknown" : Role;

				_saver.SavePlayer(newPlayer);
				_events.PublishOnUIThread(new PlayerCreatedEventModel(newPlayer));
				ClearForm();
			} else
			{
				MessageBox.Show(errorMessage.ToString());
			}						
		}	

		public void ClearForm()
		{
			FirstName = "";
			LastName = "";
			Nickname = "";
			Age = 0;
			Role = "";
		}
				
		public void ReturnToTeamCreation()
		{
			_events.PublishOnUIThread(new ReturnToTeamCreationEvent());
		}
	}
}
