using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Windows;
using TMDesktopUI.EventModels;

namespace TMDesktopUI.ViewModels
{
    public class DisplayTournamentViewModel : Screen
    {

        private IEventAggregator _events;

        public DisplayTournamentViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
        }

        private TournamentDisplayModel _tournament;
        public TournamentDisplayModel Tournament
        {
            get { return _tournament; }
            set
            {
                _tournament = value;
                NotifyOfPropertyChange(() => Tournament);
            }
        }        
        

        public bool CanViewTeam
        {
            get { return SelectedTeam != null; }
        }

        public void ViewTeam()
        {
            _events.PublishOnCurrentThread(new DisplayTeamEventModel(Tournament, SelectedTeam));
        }

        public bool CanViewMatch
        {
            get { return SelectedMatch != null; }
        }

        public void ViewMatch()
        {
            _events.PublishOnCurrentThread(new DisplayMatchEventModel(Tournament, SelectedMatch));
        }
        

        private TeamDisplayModel _selectedTeam;
        public TeamDisplayModel SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                NotifyOfPropertyChange(() => SelectedTeam);
                NotifyOfPropertyChange(() => CanViewTeam);
            }
        }

        private MatchDisplayModel _selectedMatch;
        public MatchDisplayModel SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                _selectedMatch = value;
                NotifyOfPropertyChange(() => SelectedMatch);
                NotifyOfPropertyChange(() => CanViewMatch);
            }
        }

        public void ReturnToMainScreen()
        {
            _events.PublishOnCurrentThread(new ReturnToMainScreenEvent());
        }

        /*
        <DataGrid ItemsSource="{Binding Tournament.Standings}" Grid.Row="4" Grid.Column="6" 
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False" IsReadOnly="True" >>
            <DataGrid.Columns>
                <DataGridTextColumn Header="Place" Binding="{Binding Path=Placement}" />
                <DataGridTextColumn Header="Team" Binding="{Binding Path=Team.TeamName}" />
                <DataGridTextColumn Header="PrizeWon" Binding="{Binding Path=PrizeWon}" />
            </DataGrid.Columns>
        </DataGrid>

        <ListBox ItemsSource="{Binding Tournament.Matches}" Grid.Row="4" Grid.Column="5" 
                 Grid.RowSpan="6" SelectedItem="{Binding SelectedMatch}">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <TextBlock Text="{Binding MatchInfo}" />
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>


        <ListBox ItemsSource="{Binding Tournament.Standings}" Grid.Row="4" Grid.Column="6" 
                 Grid.RowSpan="6" SelectedItem="{Binding SelectedMatch}">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <TextBlock Text="{Binding Description}" />
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>
        */
    }
}
