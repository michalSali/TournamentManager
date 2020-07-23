using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    class CreateTournamentStandingsViewModel : Screen
    {
        private IEventAggregator _events;
        private Random _random;

        // we set predetermined prize distribution for teams with 4, 8 and 16 players (in %, for first to n-th place)
        private List<double> TwoTeamsPrizeDistribution = new List<double> { 65, 35 };
        private List<double> FourTeamsPrizeDistribution = new List<double> { 55, 30, 10, 5 };
        private List<double> EightTeamsPrizeDistribution = new List<double> { 30, 20, 15, 12, 9, 6, 4, 4 };
        private List<double> SixteenTeamsPrizeDistribution = new List<double> {
            27.2, 17.6, 13.6, 10.4, 8, 6, 4.4, 3.2, 2.4, 1.76, 1.28, 0.96, 0.8, 0.8, 0.8, 0.8 };

        private BindingList<TeamDisplayModel> _selectedTeams;
        private BindingList<TeamDisplayModel> _displayedTeams;
        private BindingList<MatchDisplayModel> _matches;
        private BindingList<TournamentStandingDisplayModel> _standings;

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
        public BindingList<TeamDisplayModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set
            {
                _selectedTeams = value;
                NotifyOfPropertyChange(() => SelectedTeams);
            }
        }

        public BindingList<TeamDisplayModel> DisplayedTeams
        {
            get { return _displayedTeams; }
            set
            {
                _displayedTeams = value;
                NotifyOfPropertyChange(() => DisplayedTeams);
            }
        }

        public BindingList<MatchDisplayModel> Matches
        {
            get { return _matches; }
            set
            {
                _matches = value;
                NotifyOfPropertyChange(() => Matches);
            }
        }

        public BindingList<TournamentStandingDisplayModel> Standings
        {
            get { return _standings; }
            set
            {
                _standings = value;
                NotifyOfPropertyChange(() => Standings);
            }
        }



        private TournamentStandingDisplayModel _selectedStanding;
        public TournamentStandingDisplayModel SelectedStanding
        {
            get { return _selectedStanding; }
            set
            {
                _selectedStanding = value;
                NotifyOfPropertyChange(() => SelectedStanding);
                NotifyOfPropertyChange(() => CanAssignTeam);
                NotifyOfPropertyChange(() => CanRemoveTeamFromStanding);
            }
        }

        private TeamDisplayModel _selectedTeam;
        public TeamDisplayModel SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                NotifyOfPropertyChange(() => SelectedTeam);
                NotifyOfPropertyChange(() => CanAssignTeam);
            }
        }




        public CreateTournamentStandingsViewModel(IEventAggregator events)
        {
            //_query = new ModelsQueries();
            //_saver = new ModelsSaver();
            //_loader = new ModelsLoader();
            _events = events;
            _random = new Random();
            
        }



        // testing
        public CollectionViewSource StandingsViewSource { get; set; }
        public ObservableCollection<TournamentStandingDisplayModel> StandingsCollection { get; set; }

        /*
        public CollectionViewSource TeamsSource { get; set; }
        public ObservableCollection<TeamDisplayModel> TeamsCollection { get; set; }

        public ObservableCollection<TeamDisplayModel> TeamsTwo { get; set; }
        */

        public void InitializeValues(TournamentDisplayModel tournament)
        {
            Tournament = tournament;
            DisplayedTeams = new BindingList<TeamDisplayModel>(tournament.Teams);
            //DisplayedTeams.Add(new TeamDisplayModel());

            // add one empty team
            SelectedTeams = new BindingList<TeamDisplayModel>{ new TeamDisplayModel() };

            //Matches = new BindingList<MatchDisplayModel>(tournament.Matches);

            //Standings = new BindingList<TournamentStandingDisplayModel>();
            //InitializeStandings();


            //testing
            /*
            TeamsTwo = new ObservableCollection<TeamDisplayModel>(tournament.Teams);


            TeamsCollection = new ObservableCollection<TeamDisplayModel>(tournament.Teams);
            TeamsSource = new CollectionViewSource();
            TeamsSource.Source = TeamsCollection;
            TeamsSource.View.Refresh();
            */
            Init();
            
        }



        // testing
        private void Init()
        {
            StandingsCollection = new ObservableCollection<TournamentStandingDisplayModel>();
            // List<double> PrizeDistribution = new List<double>(DisplayedTeams.Count) { 0 };
            // nakolko na zaciatku Init pridavame prazdneho hraca, pocet sa zdvihne napr. zo 4 na 5
            List<double> PrizeDistribution = new List<double>(DisplayedTeams.Count) { 0 };
            bool unsupportedTeamSize = false;

            switch (DisplayedTeams.Count)
            {
                case 2:
                    PrizeDistribution = TwoTeamsPrizeDistribution;
                    break;
                case 4:
                    PrizeDistribution = FourTeamsPrizeDistribution;
                    break;
                case 8:
                    PrizeDistribution = EightTeamsPrizeDistribution;
                    break;
                case 16:
                    PrizeDistribution = SixteenTeamsPrizeDistribution;
                    break;
                default:
                    unsupportedTeamSize = true;
                    break;
            }

            for (int i = 0; i < DisplayedTeams.Count; ++i)
            {
                TournamentStandingDisplayModel newModel = new TournamentStandingDisplayModel();
                newModel.Placement = i + 1;
                newModel.PrizeWon = unsupportedTeamSize ? 0 : (int)(PrizeDistribution[i] / 100 * Tournament.Prizepool);
                StandingsCollection.Add(newModel);
            }

            StandingsViewSource = new CollectionViewSource();
            StandingsViewSource.Source = StandingsCollection;

            StandingsViewSource.SortDescriptions.Add(new SortDescription("Placement", ListSortDirection.Ascending));
            StandingsViewSource.View.Refresh();
        }



        private void InitializeStandings()
        {
            List<double> PrizeDistribution = new List<double>(DisplayedTeams.Count) { 0 };
            bool unsupportedTeamSize = false;

            switch (DisplayedTeams.Count)
            {
                case 2:
                    PrizeDistribution = TwoTeamsPrizeDistribution;
                    break;
                case 4:
                    PrizeDistribution = FourTeamsPrizeDistribution;
                    break;
                case 8:
                    PrizeDistribution = EightTeamsPrizeDistribution;
                    break;
                case 16:
                    PrizeDistribution = SixteenTeamsPrizeDistribution;
                    break;
                default:
                    unsupportedTeamSize = true;
                    break;
            }

            for (int i = 0; i < DisplayedTeams.Count; ++i)
            {
                TournamentStandingDisplayModel newModel = new TournamentStandingDisplayModel();
                newModel.Placement = i + 1;
                newModel.PrizeWon = unsupportedTeamSize ? 0 : (int)(PrizeDistribution[i] / 100 * Tournament.Prizepool);

                // testing
                //newModel.Teams = DisplayedTeams;
                Standings.Add(newModel);
            }
        }

        public void AutoFill()
        {
            //var RemainingTeams = new BindingList<TeamDisplayModel>(Tournament.Teams.Except(SelectedTeams).ToList());

            var WithoutEmptyPlayer = new BindingList<TeamDisplayModel>(DisplayedTeams.Where(x => !string.IsNullOrEmpty(x.TeamName)).ToList());

            // fills in fields, where a team has not been chosen / "EmptyTeam"
            //foreach (var standing in Standings)
            foreach (var standing in StandingsCollection)
            {
                if (standing.Team == null || string.IsNullOrEmpty(standing.Team?.TeamName))
                    {
                        standing.Team = WithoutEmptyPlayer[_random.Next(WithoutEmptyPlayer.Count)];
                        //SelectedTeams.Add(standing.Team);
                        WithoutEmptyPlayer.Remove(standing.Team);
                        DisplayedTeams.Remove(standing.Team);
                    }                
            }

            StandingsViewSource.View.Refresh();
        }

        public void CreateStandings()
        {
            StringBuilder errorMessage = new StringBuilder();

            bool containsEmptyTeam = StandingsCollection
                                        .Where(x => (x.Team == null) || string.IsNullOrEmpty(x.Team.TeamName))
                                        .ToList().Count > 0;

            if (containsEmptyTeam)
            {
                errorMessage.AppendLine("All places need to have assigned a team. You can use the 'Auto-fill' feature " +
                                        "that will fill the empty places.");
            }

            bool containsNegativePrize = StandingsCollection.Where(x => x.PrizeWon < 0).ToList().Count > 0;

            if (containsNegativePrize)
            {
                errorMessage.AppendLine("'PrizeWon' cannot be a negative number.");
            }

            if (errorMessage.Length == 0)
            {
                _events.PublishOnUIThread(new TournamentStandingsCreatedEventModel(new List<TournamentStandingDisplayModel>(StandingsCollection)));
                //_events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
            }
        }

        public void ReturnToTournamentCreation()
        {
            _events.PublishOnUIThread(new ReturnToTournamentCreationEvent());
        }

        public bool CanAssignTeam
        {
            get { return SelectedTeam != null && SelectedStanding != null; }
        }

        public void AssignTeam()
        {
            if (SelectedStanding.Team != null)
            {
                DisplayedTeams.Add(SelectedStanding.Team);
            }                
            
            SelectedStanding.Team = SelectedTeam;
            DisplayedTeams.Remove(SelectedTeam);
           
            StandingsViewSource.View.Refresh();
        }

        public bool CanRemoveTeamFromStanding
        {
            get { return SelectedStanding != null && SelectedStanding?.Team != null; }
        }
        public void RemoveTeamFromStanding()
        {
            DisplayedTeams.Add(SelectedStanding.Team);
            SelectedStanding.Team = null;

            StandingsViewSource.View.Refresh();
        }


        /*
        

        /*
        <!-- Row 3: -->
        <DataGrid ItemsSource = "{Binding Standings}" Grid.Row="3" Grid.Column="1" Grid.ColumnSpan="4"
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False" >
            <DataGrid.Resources>
                <DataTemplate x:Key="DisplayName">
                    <TextBlock Text = "{Binding Team.TeamName}" />
                </ DataTemplate >
                < DataTemplate x:Key="DisplayTeams">
                    <ComboBox SelectedItem = "{Binding Team}" ItemsSource="{Binding TeamTwo}" />
                </DataTemplate>
            </DataGrid.Resources>
            <DataGrid.Columns>
                <DataGridTextColumn Header = "Place" IsReadOnly="True"
                                    Binding="{Binding Placement}"/>

                <DataGridTemplateColumn Header = "Team" >
                    < DataGridTemplateColumn.CellTemplate >
                        < DataTemplate >
                            < TextBlock Text="{Binding Team.TeamName}" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                    <DataGridTemplateColumn.CellEditingTemplate>
                        <DataTemplate>
                            <ComboBox ItemsSource = "{Binding Teams}" SelectedItem="{Binding Team}">
                                <ComboBox.ItemTemplate>
                                    <DataTemplate>
                                        <TextBlock Text = "{Binding TeamName}" />
                                    </ DataTemplate >
                                </ ComboBox.ItemTemplate >
                            </ ComboBox >
                        </ DataTemplate >
                    </ DataGridTemplateColumn.CellEditingTemplate >
                </ DataGridTemplateColumn >

                < DataGridTemplateColumn Header="Attempt" 
                                        CellTemplate="{StaticResource DisplayName}"
                                        CellEditingTemplate="{StaticResource DisplayTeams}">
                </DataGridTemplateColumn>
                
                
                <DataGridTextColumn Header = "PrizeWon" IsReadOnly="True"
                                    Binding="{Binding Path=PrizeWon}"/>
                
            </DataGrid.Columns>
        </DataGrid>










        <DataGrid ItemsSource="{Binding ViewSource.View}" Grid.Row="3" Grid.Column="5" Grid.ColumnSpan="2"
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False" >
            <DataGrid.Columns>
                <DataGridTextColumn Header="Place" IsReadOnly="True"
                                    Binding="{Binding Placement}"/>

                <DataGridTemplateColumn Header="Team">
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <TextBlock Text="{Binding Team.TeamName}" />
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                    <DataGridTemplateColumn.CellEditingTemplate>
                        <DataTemplate>
                            <ComboBox ItemsSource="{Binding Path=TeamsSource.View}" SelectedItem="{Binding Team}">
                                <ComboBox.ItemTemplate>
                                    <DataTemplate>
                                        <TextBlock Text="l{Binding Path=TeamName}" />
                                    </DataTemplate>
                                </ComboBox.ItemTemplate>
                            </ComboBox>
                        </DataTemplate>
                    </DataGridTemplateColumn.CellEditingTemplate>
                </DataGridTemplateColumn>
                

                <DataGridTextColumn Header="PrizeWon" IsReadOnly="True"
                                    Binding="{Binding Path=PrizeWon}"/>

            </DataGrid.Columns>
        </DataGrid>




        <DataGrid ItemsSource="{Binding ViewSource.View}" Grid.Row="3" Grid.Column="1" Grid.ColumnSpan="2"
                  CanUserAddRows="False" CanUserDeleteRows="False"
                  AutoGenerateColumns="False"  SelectedItem="{Binding SelectedStanding}">
            <DataGrid.Columns>
                <DataGridTextColumn Header="Place" IsReadOnly="True"
                                    Binding="{Binding Placement}"/>

                <DataGridTextColumn Header="Team" IsReadOnly="True"
                                    Binding="{Binding Team.TeamName}" />

                <DataGridTextColumn Header="PrizeWon" IsReadOnly="True"
                                    Binding="{Binding PrizeWon}"/>
                
            </DataGrid.Columns>
        </DataGrid>
        */
    }
}
