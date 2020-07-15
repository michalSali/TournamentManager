using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDesktopUI.ViewModels
{
    public class MainOptionsViewModel
    {
        public BindableCollection<TournamentModel> Tournaments { get; set; }

		private TournamentModel _selectedTournament;

		public TournamentModel SelectedTournament
		{
			get { return _selectedTournament; }
			set 
			{ 
				_selectedTournament = value;
				NotifyOfPropertyChange(() => SelectedTournament);
			}
		}

		public MainOptionsViewModel()
		{
			UserDataAccess db = new UserDataAccess();
			Tournaments = db.GetAllTournaments();
		}

	}
}
