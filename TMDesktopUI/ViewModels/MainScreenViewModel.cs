using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;
using static TMDesktopUI.Library.Exporters.TournamentExporter;

namespace TMDesktopUI.ViewModels
{
	public class MainScreenViewModel : Screen
	{
		private BindingList<TournamentDisplayModel> _tournaments = new BindingList<TournamentDisplayModel>();
		private TournamentDisplayModel _selectedTournament;
		private IEventAggregator _events;
		private ModelsLoader _loader;

		public MainScreenViewModel(IEventAggregator events)
		{
			_events = events;
			_loader = new ModelsLoader();

			Tournaments = new BindingList<TournamentDisplayModel>(_loader.GetAllTournaments());	
		}

		public TournamentDisplayModel SelectedTournament
		{
			get { return _selectedTournament; }
			set
			{
				_selectedTournament = value;
				NotifyOfPropertyChange(() => SelectedTournament);
				NotifyOfPropertyChange(() => CanViewTournament);
				NotifyOfPropertyChange(() => CanExportTournamentAsync);
				NotifyOfPropertyChange(() => CanViewStats);
			}
		}

		public BindingList<TournamentDisplayModel> Tournaments
		{
			get { return _tournaments; }
			set
			{
				_tournaments = value;
				NotifyOfPropertyChange(() => Tournaments);
				NotifyOfPropertyChange(() => CanViewTournament);
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

		public bool CanViewTournament
		{
			get { return SelectedTournament != null && Tournaments?.Count > 0; }			
		}
		

		public void ViewTournament()
		{						
			_events.PublishOnUIThread(new DisplayTournamentEventModel(SelectedTournament));
		}

		public bool CanExportTournamentAsync
		{
			get { return SelectedTournament != null; }
		}

		public async Task ExportTournamentAsync()
		{
			string fileName = SelectedTournament.TournamentName.GetTournamentFileName();

			await Task.Factory.StartNew(() => SelectedTournament.ExportTournament(fileName));			
			MessageBox.Show($"Tournament has been exported to {fileName.FullFilePath()}.");
		}

		public bool CanViewStats
		{
			get { return SelectedTournament != null; }
		}

		public void ViewStats()
		{
			_events.PublishOnUIThread(new DisplayTournamentStatsEventModel(SelectedTournament));
		}
		
	}
}
