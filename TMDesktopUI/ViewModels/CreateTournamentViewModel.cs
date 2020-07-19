using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.Library.Helpers;
using TMDesktopUI.Library.Models;
using Caliburn.Micro;
using System.Windows;
using TMDesktopUI.EventModels;

namespace TMDesktopUI.ViewModels
{
    public class CreateTournamentViewModel : Screen
    {

        private ModelsQueries _query;
        private ModelsSaver _saver;
        private ModelsLoader _loader;
        private IEventAggregator _events;

        private string _tournamentName;        
        private string _filterText;
        private int _prizepool = 0;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;

        private TeamDisplayModel _teamToAdd;
        private TeamDisplayModel _teamToRemove;
        private BindingList<TeamDisplayModel> _selectedTeams;
        private BindingList<TeamDisplayModel> _displayedTeams;
        private int _numberOfSelectedTeams = 0;

        private MatchDisplayModel _selectedMatch;
        private BindingList<MatchDisplayModel> _matches;

        private BindingList<TournamentStandingDisplayModel> _tournamentStandings;


        #region >>> Attribute Getters & Setters <<<
        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }

        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                NotifyOfPropertyChange(() => FilterText);                
            }
        }

        public int Prizepool 
        {
            get { return _prizepool; }
            set
            {
                _prizepool = value;
                NotifyOfPropertyChange(() => Prizepool);
            } 
        }

        public DateTime StartDate 
        {
            get { return _startDate; } 
            set
            {
                _startDate = value;
                NotifyOfPropertyChange(() => StartDate);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                NotifyOfPropertyChange(() => EndDate);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }        

        public TeamDisplayModel TeamToAdd
        {
            get { return _teamToAdd; }
            set
            {
                _teamToAdd = value;
                NotifyOfPropertyChange(() => TeamToAdd);
                NotifyOfPropertyChange(() => CanAddTeam);
            }
        }
        
        public TeamDisplayModel TeamToRemove
        {
            get { return _teamToRemove; }
            set
            {
                _teamToRemove = value;              
                NotifyOfPropertyChange(() => TeamToRemove);
                NotifyOfPropertyChange(() => CanRemoveTeam);
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
             
        public int NumberOfSelectedTeams
        {
            get { return _numberOfSelectedTeams; }
            set
            {
                _numberOfSelectedTeams = value;
                NotifyOfPropertyChange(() => NumberOfSelectedTeams);
            }
        }
                
        public BindingList<TournamentStandingDisplayModel> TournamentStandings
        {
            get { return _tournamentStandings; }
            set
            {
                _tournamentStandings = value;
                NotifyOfPropertyChange(() => TournamentStandings);
            }
        }

        // is not bound to anything, shouldnt need to NotifyOfPropertyChange        
        public BindingList<TeamDisplayModel> AllTeams;
        
        public BindingList<MatchDisplayModel> Matches
        {
            get { return _matches; }
            set
            {
                _matches = value;
                NotifyOfPropertyChange(() => Matches);
            }
        }
        
        public MatchDisplayModel SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                _selectedMatch = value;
                NotifyOfPropertyChange(() => SelectedMatch);
            }
        }
        #endregion
        
        public CreateTournamentViewModel(IEventAggregator events)
        {
            _query = new ModelsQueries();
            _saver = new ModelsSaver();
            _loader = new ModelsLoader();
            _events = events;

            AllTeams = new BindingList<TeamDisplayModel>(_loader.GetAllTeams());
            SelectedTeams = new BindingList<TeamDisplayModel>();
            DisplayedTeams = new BindingList<TeamDisplayModel>();
            DisplayedTeams = new BindingList<TeamDisplayModel>(DisplayedTeams.Union(AllTeams).ToList());

            Matches = new BindingList<MatchDisplayModel>();            
        }
        
        public bool CanApplyFilter(string filterText)
        {
            return (filterText?.Length > 0);
        }
        
        public void ApplyFilter(string filterText)
        {
            List<TeamDisplayModel> filteredTeams = new List<TeamDisplayModel>();

            foreach (var team in DisplayedTeams.Except(SelectedTeams))
            {
                if (team.TeamName.Contains(filterText) || team.CoachName.Contains(filterText))
                {
                    filteredTeams.Add(team);
                    continue;
                }
                foreach (var player in team.Players)
                {
                    if (player.FirstName.Contains(filterText) ||
                        player.LastName.Contains(filterText) ||
                        player.Nickname.Contains(filterText))
                    {
                        filteredTeams.Add(team);
                        break;
                    }
                }
            }            

            DisplayedTeams = new BindingList<TeamDisplayModel>(filteredTeams);
        }

        public bool CanRemoveFilter(string filterText)
        {
            return (filterText?.Length > 0);
        }

        public void RemoveFilter()
        {
            DisplayedTeams = new BindingList<TeamDisplayModel>(AllTeams.Except(SelectedTeams).ToList());
        }

        // there can be at most 16 teams        
        public bool CanAddTeam
        {
            get { return (TeamToAdd != null) && (SelectedTeams?.Count < 16); }
        }

        public void AddTeam()
        {
            SelectedTeams.Add(TeamToAdd);
            DisplayedTeams.Remove(TeamToAdd);
            TeamToAdd = null;
            NumberOfSelectedTeams++;
        }
              
        public bool CanRemoveTeam
        {
            get { return (TeamToRemove != null) && (SelectedTeams?.Count > 0); }
        }

        public void RemoveTeam()
        {
            DisplayedTeams.Add(TeamToRemove);
            SelectedTeams.Remove(TeamToRemove);            
            TeamToRemove = null;
            NumberOfSelectedTeams--;
        }        

        // we set predetermined prize distribution for teams with 4, 8 and 16 players (in %, for first to n-th place)
        private List<double> FourTeamsPrizeDistribution = new List<double> { 55, 30, 15, 10 };
        private List<double> EightTeamsPrizeDistribution = new List<double> { 30, 20, 15, 12, 9, 6, 4, 4 };
        private List<double> SixteenTeamsPrizeDistribution = new List<double> {
            27.2, 17.6, 13.6, 10.4, 8, 6, 4.4, 3.2, 2.4, 1.76, 1.28, 0.96, 0.8, 0.8, 0.8, 0.8 };

        private void InitializeStandings()
        {
            List<double> PrizeDistribution = new List<double>(SelectedTeams.Count) { 0 };
            bool unsupportedTeamSize = false;

            switch (SelectedTeams.Count)
            {
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

            for (int i = 0; i < SelectedTeams.Count; ++i)
            {
                TournamentStandingDisplayModel newModel = new TournamentStandingDisplayModel();
                newModel.Placement = i + 1;
                newModel.PrizeWon = unsupportedTeamSize ? 0 : (int)(PrizeDistribution[i] / 100 * Prizepool);
                TournamentStandings.Add(newModel);
            }            
        }
        
        public bool CanCreateTournament
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(TournamentName) &&
                  StartDate != null && EndDate != null);
            }
                    
        }

        public void CreateTournament()
        {
            StringBuilder errorMessage = new StringBuilder();

            /*
            if (_query.ExistsTournament(TournamentName))
            {
                errorMessage.AppendLine("Tournament with the specific name already exists.");
            }
            */
            
            if (SelectedTeams.Count < 2)  // 2 or 4 teams ?
            {
                errorMessage.AppendLine("You have to choose at least 2 teams.");
            }

            if (Prizepool < 0)
            {
                errorMessage.AppendLine("Prizepool cannot be a negative number.");
            }            

            if (errorMessage.Length == 0)
            {
                TournamentDisplayModel newTournament = new TournamentDisplayModel();
                newTournament.TournamentName = TournamentName;
                newTournament.StartDate = StartDate;
                newTournament.EndDate = EndDate;
                newTournament.Prizepool = Prizepool;
                newTournament.Teams = new List<TeamDisplayModel>(SelectedTeams);
                newTournament.Matches = new List<MatchDisplayModel>(Matches);

                _saver.SaveTournament(newTournament);
                _events.PublishOnUIThread(new TournamentCreatedEventModel(newTournament));
                ClearForm();
            } else
            {
                MessageBox.Show(errorMessage.ToString());
            }
        }
       
        private void ClearForm()
        {
            TournamentName = "";           
            FilterText = "";
            Prizepool = 0;
            
            Matches.Clear();
            DisplayedTeams = new BindingList<TeamDisplayModel>(DisplayedTeams.Union(SelectedTeams).ToList());            
            SelectedTeams.Clear();
            TeamToAdd = null;
            TeamToRemove = null;
        }                        

        public void CreateNewMatch()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (StartDate == null || EndDate == null)
            {
                errorMessage.AppendLine("You have to first set dates of the tournament.");
            }

            if (errorMessage.Length == 0)
            {
                TournamentDisplayModel tournament = new TournamentDisplayModel();
                tournament.StartDate = StartDate;
                tournament.EndDate = EndDate;
                tournament.Teams = new List<TeamDisplayModel>(SelectedTeams);
                tournament.TournamentName = TournamentName;
                tournament.Prizepool = Prizepool;

                _events.PublishOnUIThread(new CreateMatchEventModel(tournament));
            }
                              
        }

        public void AddCreatedMatch(MatchDisplayModel match)
        {
            Matches.Add(match);
        }
        
        public void CreateNewTeam()
        {
            _events.PublishOnUIThread(new CreateTeamEvent());
        }

        public void AddCreatedTeam(TeamDisplayModel team)
        {
            DisplayedTeams.Add(team);
            AllTeams.Add(team);            
        }
        
        public void ReturnToMainScreen()
        {
            _events.PublishOnUIThread(new ReturnToMainScreenEvent());
        }

        // currently not in use
        #region >>> Match Importance Separation <<<
        
        private BindingList<MatchDisplayModel> _groupStageMatches;
        public BindingList<MatchDisplayModel> GroupStageMatches
        {
            get { return _groupStageMatches; }
            set
            {
                _groupStageMatches = value;
                NotifyOfPropertyChange(() => GroupStageMatches);
            }
        }        

        private BindingList<MatchDisplayModel> _quarterfinalMatches;
        public BindingList<MatchDisplayModel> QuarterfinalMatches
        {
            get { return _quarterfinalMatches; }
            set
            {
                _quarterfinalMatches = value;
                NotifyOfPropertyChange(() => QuarterfinalMatches);
            }
        }

        private BindingList<MatchDisplayModel> _semifinalMatches;
        public BindingList<MatchDisplayModel> SemifinalMatches
        {
            get { return _semifinalMatches; }
            set
            {
                _semifinalMatches = value;
                NotifyOfPropertyChange(() => SemifinalMatches);
            }
        }

        private MatchDisplayModel _finalMatch;
        public MatchDisplayModel FinalMatch
        {
            get { return _finalMatch; }
            set
            {
                _finalMatch = value;
                NotifyOfPropertyChange(() => FinalMatch);
            }
        }

        private MatchDisplayModel _consolidationFinalMatch;
        public MatchDisplayModel ConsolidationFinalMatch
        {
            get { return _consolidationFinalMatch; }
            set
            {
                _consolidationFinalMatch = value;
                NotifyOfPropertyChange(() => ConsolidationFinalMatch);
            }
        }


        // FINAL 

        public bool CanAddFinalMatch()
        {
            return SelectedMatch != null && FinalMatch == null;
        }
        public void AddFinalMatch()
        {
            FinalMatch = SelectedMatch;
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveFinalMatch()
        {
            return FinalMatch != null;
        }
        public void RemoveFinalMatch()
        {
            Matches.Add(FinalMatch);
            FinalMatch = null;            
        }
        
        // SEMIFINAL 

        public bool CanAddSemifinalMatch()
        {
            return SelectedMatch != null && SemifinalMatches.Count < 4;
        }
        public void AddSemifinalMatch()
        {
            SemifinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveSemifinalMatch()
        {
            return SelectedMatch != null && SemifinalMatches.Contains(SelectedMatch);
        }
        public void RemoveSemifinalMatch()
        {
            Matches.Add(SelectedMatch);
            SemifinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        // QUARTERFINAL

        public bool CanAddQuarterfinalMatch()
        {
            return SelectedMatch != null && QuarterfinalMatches.Count < 8;
        }
        public void AddQuarterfinalMatch()
        {
            QuarterfinalMatches.Add(SelectedMatch);
            Matches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        public bool CanRemoveQuarterfinalMatch()
        {
            return SelectedMatch != null && QuarterfinalMatches.Contains(SelectedMatch);
        }
        public void RemoveQuarterfinalMatch()
        {
            Matches.Add(SelectedMatch);
            QuarterfinalMatches.Remove(SelectedMatch);
            SelectedMatch = null;
        }

        // xaml
        /*
        <!-- RIGHT COLUMN -->

        <!-- Row 1: -->
        <TextBlock Grid.Row="1" Grid.Column= "5" Margin= "0 0 0 10" >
            Group Stage / Unlabeled Matches:
        </TextBlock>
       
        <ComboBox ItemsSource = "{Binding Matches}" Grid.Row= "8" Grid.Column= "4"
                 SelectedItem= "{Binding SelectedMatch}" >
            < ComboBox.ItemTemplate >
                < DataTemplate >
                    < TextBlock Text= "{Binding MatchInfo}" />
                </ DataTemplate >
            </ ComboBox.ItemTemplate >
        </ ComboBox >

        < ListBox ItemsSource= "{Binding Matches}" Grid.Row= "8" Grid.Column= "4"
                 SelectedItem= "{Binding SelectedMatch}" >
            < ListBox.ItemTemplate >
                < DataTemplate >
                    < TextBlock Text= "{Binding MatchInfo}" />
                </ DataTemplate >
            </ ListBox.ItemTemplate >
        </ ListBox >

        < StackPanel Grid.Row= "3" Grid.Column= "5" Orientation= "Vertical" >
            < Button x:Name= "AddQuarterfinalMatch" Content= "Add to Quarterfinals" />
            < Button x:Name= "AddSemifinalMatch" Content= "Add to Semifinals" />
            < Button x:Name= "AddFinalMatch" Content= "Add to Final" />
        </ StackPanel >


        < !--MATCHES-- >
        < TextBlock Grid.Row= "1" Grid.Column= "6" Margin= "0 0 0 10" >
            Quarterfinal Matches:
        </TextBlock>

        <ListBox ItemsSource = "{Binding QuarterfinalMatches}" Grid.Row= "2" Grid.Column= "5"
                 SelectedItem= "{Binding SelectedMatch}" >
            < ListBox.ItemTemplate >
                < DataTemplate >
                    < TextBlock Text= "{Binding MatchInfo}" />
                </ DataTemplate >
            </ ListBox.ItemTemplate >
        </ ListBox >
        */

        // tournament standings xaml
        /*
        <!-- MIDDLE COLUMN -->
        
        <!-- Row 1: -->
        <TextBlock Grid.Row="1" Grid.Column= "3" Margin= "0 0 0 10" >
            Set Tournament Standings:
        </TextBlock>
        <!-- Row 2: -->
        <DataGrid ItemsSource = "{Binding TournamentStandings}" Grid.Row= "2" Grid.Column= "3"
                  Grid.ColumnSpan= "2" AlternatingRowBackground= "LightGray"
                  CanUserAddRows= "False" CanUserDeleteRows= "False"
                  AutoGenerateColumns= "False" >
            < DataGrid.Columns >
                < DataGridTextColumn Header= "Placement" IsReadOnly= "True"
                                    Binding= "{Binding Path=Placement}" />
                < DataGridTemplateColumn Header= "Team" >
                    < DataGridTemplateColumn.CellTemplate >
                        < DataTemplate >
                            < TextBlock Text= "{Binding Path=Team.TeamName}" />
                        </ DataTemplate >
                    </ DataGridTemplateColumn.CellTemplate >
                    < DataGridTemplateColumn.CellEditingTemplate >
                        < DataTemplate >
                            < ComboBox ItemsSource= "{Binding Path=SelectedTeams}"
                                      SelectedItem= "{Binding Path=Team}" >
                                < ComboBox.ItemTemplate >
                                    < DataTemplate >
                                        < TextBlock Text= "{Binding Path=Team.Name}" />
                                    </ DataTemplate >
                                </ ComboBox.ItemTemplate >
                            </ ComboBox >
                        </ DataTemplate >
                    </ DataGridTemplateColumn.CellEditingTemplate >
                </ DataGridTemplateColumn >
                < DataGridTextColumn Header= "PrizeWon" IsReadOnly= "True"
                                    Binding= "{Binding Path=PrizeWon}" />
            </ DataGrid.Columns >
        </ DataGrid >
        */
        #endregion

    }
}
