using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
	public class MainScreenViewModel : Conductor<object>
	{
		private BindingList<TournamentDisplayModel> _tournaments;
		private TournamentDisplayModel _selectedTournament;
		private IEventAggregator _events;
		private ModelsLoader _loader;

		public MainScreenViewModel(IEventAggregator events)
		{
			_events = events;
			_loader = new ModelsLoader();

			//Tournaments = new BindingList<TournamentDisplayModel>(_loader.GetAllTournaments());
		}

		public TournamentDisplayModel SelectedTournament
		{
			get { return _selectedTournament; }
			set
			{
				_selectedTournament = value;
				NotifyOfPropertyChange(() => SelectedTournament);
			}
		}

		public BindingList<TournamentDisplayModel> Tournaments
		{
			get { return _tournaments; }
			set
			{
				_tournaments = value;
				NotifyOfPropertyChange(() => Tournaments);
			}
		}

		public void CreateNewTournament()
		{
			_events.PublishOnUIThread(new CreateTournamentEvent());
		}

		public void AddCreatedTournament(TournamentDisplayModel tournament)
		{
			Tournaments.Add(tournament);
		}

		public bool CanViewTournament()
		{
			return SelectedTournament != null;			
		}
		

		public void ViewTournament()
		{						
			_events.PublishOnUIThread(new DisplayTournamentEventModel(SelectedTournament));
		}
		
	}
}
