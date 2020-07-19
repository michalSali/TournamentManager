using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDesktopUI.EventModels;
using TMDesktopUI.Library.Models;

namespace TMDesktopUI.ViewModels
{
    public class DisplayMapViewModel : Screen
    {

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

        private MapScoreDisplayModel _map;
        public MapScoreDisplayModel Map
        {
            get { return _map; }
            set
            {
                _map = value;
                NotifyOfPropertyChange(() => Map);
            }
        }


        private IEventAggregator _events;

        public DisplayMapViewModel(IEventAggregator events)
        {
            _events = events;
        }

        public void InitializeValues(TournamentDisplayModel tournament, MapScoreDisplayModel map)
        {
            Tournament = tournament;
            Map = map;
        }

               
        public void ReturnToMatchViewer()
        {
            _events.PublishOnCurrentThread(new ReturnToMatchViewerEvent());
        }
    }

    /*
    <StackPanel Orientation = "Vertical" >
        < TextBlock Text="{Binding Map.MapDescription}" />
        <TextBlock Text = "{Binding Map.ScoreDescription}" />
        < ItemsControl ItemsSource="{Binding Map.TeamOneStats}">
            <ItemsControl.ItemTemplate>
                <DataTemplate>
                    <StackPanel Orientation = "Horizontal" >
                        < TextBlock Text="{Binding Player.FullName}" />
                        <TextBlock Text = "{Binding Kills}" />
                        < TextBlock Text="{Binding Assists}" />
                        <TextBlock Text = "{Binding Deaths}" />
                    </ StackPanel >
                </ DataTemplate >
            </ ItemsControl.ItemTemplate >
        </ ItemsControl >

        < ItemsControl ItemsSource="{Binding Map.TeamTwoStats}">
            <ItemsControl.ItemTemplate>
                <DataTemplate>
                    <StackPanel Orientation = "Horizontal" >
                        < TextBlock Text="{Binding Player.FullName}" />
                        <TextBlock Text = "{Binding Kills}" />
                        < TextBlock Text="{Binding Assists}" />
                        <TextBlock Text = "{Binding Deaths}" />
                    </ StackPanel >
                </ DataTemplate >
            </ ItemsControl.ItemTemplate >
        </ ItemsControl >
    </ StackPanel >
    */
}
