using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TMDesktopUI.EventModels;
using TMLibrary.DataAccess;
using TMLibrary.Models;

namespace TMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<DisplayTournamentEventModel>,
        IHandle<CreateTournamentEvent>, IHandle<ReturnToMainScreenEvent>, IHandle<CreatePlayerEvent>,
        IHandle<PlayerCreatedEventModel>, IHandle<CreateTeamEvent>, IHandle<TeamCreatedEventModel>,
        IHandle<CreateMatchEventModel>, IHandle<MatchCreatedEventModel>, IHandle<ReturnToTeamCreationEvent>,
        IHandle<ReturnToTournamentCreationEvent>, IHandle<CreateMapEventModel>, IHandle<MapCreatedEventModel>,
        IHandle<ReturnToMatchCreationEvent>, IHandle<TournamentCreatedEventModel>, IHandle<DisplayTeamEventModel>,
        IHandle<DisplayPlayerEventModel>, IHandle<ReturnToMatchViewerEvent>, IHandle<ReturnToTournamentViewerEvent>,
        IHandle<ReturnToTeamViewerEvent>, IHandle<DisplayMatchEventModel>, IHandle<DisplayMapEventModel>,
        IHandle<CreateTournamentStandingsEventModel>, IHandle<TournamentStandingsCreatedEventModel>,
        IHandle<CreateMatchSpecificationsEventModel>, IHandle<MatchSpecificationsCreatedEventModel>,
        IHandle<DisplayTournamentStatsEventModel>
    {

        private CreateTournamentViewModel _createTournamentVM;
        private CreateTeamViewModel _createTeamVM;
        private CreateMatchViewModel _createMatchVM;
        private CreateMapViewModel _createMapVM;
        private MainScreenViewModel _mainScreenVM;

        private DisplayTournamentViewModel _displayTournamentVM;
        private DisplayTeamViewModel _displayTeamVM;
        private DisplayMatchViewModel _displayMatchVM;

        private IEventAggregator _events;
       
        public ShellViewModel(IEventAggregator events)
        {
            _events = events;
            _events.Subscribe(this);
            
            _mainScreenVM = IoC.Get<MainScreenViewModel>();
            ActivateItem(_mainScreenVM);            
        }

        public void Handle(DisplayTournamentEventModel message)
        {
            _displayTournamentVM = IoC.Get<DisplayTournamentViewModel>();
            _displayTournamentVM.InitializeValues(message.Tournament);
            ActivateItem(_displayTournamentVM);       
        }

        public void Handle(CreateTournamentEvent message)
        {
            // calling from Main Screen will reset the VM
            _createTournamentVM = IoC.Get<CreateTournamentViewModel>();
            ActivateItem(_createTournamentVM);
        }

        public void Handle(ReturnToMainScreenEvent message)
        {
            ActivateItem(_mainScreenVM);
        }


        public void Handle(CreatePlayerEvent message)
        {
            ActivateItem(IoC.Get<CreatePlayerViewModel>());
        }

        public void Handle(PlayerCreatedEventModel message)
        {
            _createTeamVM.AddCreatedPlayer(message.Player);
        }

        public void Handle(CreateTeamEvent message)
        {
            // calling from Tournament Creation Form will reset the VM
            _createTeamVM = IoC.Get<CreateTeamViewModel>();
            ActivateItem(_createTeamVM);
        }

        public void Handle(TeamCreatedEventModel message)
        {
            _createTournamentVM.AddCreatedTeam(message.Team);
        }

        public void Handle(CreateMatchEventModel message)
        {          
            // calling from Tournament Creation Form will reset the VM
            _createMatchVM = IoC.Get<CreateMatchViewModel>();
            _createMatchVM.InitializeValues(message.Tournament);
            ActivateItem(_createMatchVM);            
        }

        public void Handle(MatchCreatedEventModel message)
        {
            _createTournamentVM.AddCreatedMatch(message.Match);
        }

        public void Handle(ReturnToTeamCreationEvent message)
        {
            ActivateItem(_createTeamVM);
        }

        public void Handle(ReturnToTournamentCreationEvent message)
        {
            ActivateItem(_createTournamentVM);
        }

        public void Handle(CreateMapEventModel message)
        {
            // calling from Tournament Creation Form will reset the VM
            _createMapVM = IoC.Get<CreateMapViewModel>();
            _createMapVM.InitializeValues(message.Match);
            ActivateItem(_createMapVM);
        }

        public void Handle(MapCreatedEventModel message)
        {
            _createMatchVM.AddCreatedMap(message.Map);
        }

        public void Handle(ReturnToMatchCreationEvent message)
        {
            ActivateItem(_createMatchVM);
        }

        public void Handle(TournamentCreatedEventModel message)
        {
            _mainScreenVM.AddCreatedTournament(message.Tournament);
        }

        public void Handle(DisplayTeamEventModel message)
        {
            _displayTeamVM = IoC.Get<DisplayTeamViewModel>();
            _displayTeamVM.InitializeValues(message.Tournament, message.Team);
            ActivateItem(_displayTeamVM);
        }

        public void Handle(DisplayPlayerEventModel message)
        {
            // no need to save the instance, as we wont go back to it
            var displayPlayerVM = IoC.Get<DisplayPlayerViewModel>();
            displayPlayerVM.InitializeValues(message.Tournament, message.Player);
            ActivateItem(displayPlayerVM);
        }

        public void Handle(ReturnToMatchViewerEvent message)
        {
            ActivateItem(_displayMatchVM);
        }

        public void Handle(ReturnToTournamentViewerEvent message)
        {
            ActivateItem(_displayTournamentVM);
        }

        public void Handle(ReturnToTeamViewerEvent message)
        {
            ActivateItem(_displayTeamVM);
        }

        public void Handle(DisplayMatchEventModel message)
        {
            _displayMatchVM = IoC.Get<DisplayMatchViewModel>();
            _displayMatchVM.InitializeValues(message.Tournament, message.Match);
            ActivateItem(_displayMatchVM);
        }

        public void Handle(DisplayMapEventModel message)
        {
            var displayMapVM = IoC.Get<DisplayMapViewModel>();
            displayMapVM.InitializeValues(message.Tournament, message.Map);
            ActivateItem(displayMapVM);
        }

        public void Handle(CreateTournamentStandingsEventModel message)
        {
            var createTournamentStandingsVM = IoC.Get<CreateTournamentStandingsViewModel>();
            createTournamentStandingsVM.InitializeValues(message.Tournament);
            ActivateItem(createTournamentStandingsVM);
        }

        public void Handle(TournamentStandingsCreatedEventModel message)
        {
            _createTournamentVM.SetStandings(message.Standings);
            ActivateItem(_createTournamentVM);
        }

        public void Handle(CreateMatchSpecificationsEventModel message)
        {
            var createMatchSpecificationsVM = IoC.Get<CreateMatchSpecificationsViewModel>();
            createMatchSpecificationsVM.InitializeValues(message.Tournament);
            ActivateItem(createMatchSpecificationsVM);
        }

        public void Handle(MatchSpecificationsCreatedEventModel message)
        {
            _createTournamentVM.AddMatchSpecifications(message.Matches);
            ActivateItem(_createTournamentVM);
        }

        public void Handle(DisplayTournamentStatsEventModel message)
        {
            var displayTournamentStatsVM = IoC.Get<DisplayTournamentStatsViewModel>();
            displayTournamentStatsVM.Initialize(message.Tournament);
            ActivateItem(displayTournamentStatsVM);
        }
    }
}
