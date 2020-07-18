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
        IHandle<ReturnToMatchCreationEvent>
    {

        private CreateTournamentViewModel _createTournamentVM;
        private CreateTeamViewModel _createTeamVM;
        private CreateMatchViewModel _createMatchVM;
        private CreateMapViewModel _createMapVM;
        private MainScreenViewModel _mainScreenVM;
        private IEventAggregator _events;
        private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SimpleContainer container)
        {
            _events = events;
            _events.Subscribe(this);
            _container = container;

            _mainScreenVM = _container.GetInstance<MainScreenViewModel>();
            ActivateItem(_mainScreenVM);            
        }

        public void Handle(DisplayTournamentEventModel message)
        {
            DisplayTournamentViewModel vm = new DisplayTournamentViewModel(message.Tournament);
            ActivateItem(vm);
            //throw new NotImplementedException();
        }

        public void Handle(CreateTournamentEvent message)
        {
            // calling from Main Screen will reset the VM
            _createTournamentVM = _container.GetInstance<CreateTournamentViewModel>();
            ActivateItem(_createTournamentVM);
        }

        public void Handle(ReturnToMainScreenEvent message)
        {
            ActivateItem(_mainScreenVM);
        }


        public void Handle(CreatePlayerEvent message)
        {
            ActivateItem(_container.GetInstance<CreatePlayerViewModel>());
        }

        public void Handle(PlayerCreatedEventModel message)
        {
            _createTeamVM.AddCreatedPlayer(message.Player);
        }

        public void Handle(CreateTeamEvent message)
        {
            // calling from Tournament Creation Form will reset the VM
            _createTeamVM = _container.GetInstance<CreateTeamViewModel>();
            ActivateItem(_createTeamVM);
        }

        public void Handle(TeamCreatedEventModel message)
        {
            _createTournamentVM.AddCreatedTeam(message.Team);
        }

        public void Handle(CreateMatchEventModel message)
        {          
            // calling from Tournament Creation Form will reset the VM
            _createMatchVM = _container.GetInstance<CreateMatchViewModel>();
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
            _createMapVM = _container.GetInstance<CreateMapViewModel>();
            _createMapVM.InitializeValues(message.Match);
            ActivateItem(_createMapVM);
        }

        public void Handle(MapCreatedEventModel message)
        {
            _createMatchVM.AddCreatedMap(message.Map);
        }

        public void Handle(ReturnToMatchCreationEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
