﻿using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System;
using System.Data.Entity;
using BookingService.Controls;

namespace BookingService
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context = new AppDbContext();
        public ObservableCollection<HotelRoom> Rooms { get; set; } = new ObservableCollection<HotelRoom>();
        private List<HotelRoom> _originalRooms = new List<HotelRoom>();

        public static RoutedCommand AdminPanelCommand { get; } = new RoutedCommand();
        public static RoutedCommand AlphabetSortCommand { get; } = new RoutedCommand();
        public static RoutedCommand CostAscSortCommand { get; } = new RoutedCommand();
        public static RoutedCommand CostDescSortCommand { get; } = new RoutedCommand();
        public static RoutedCommand ProfileCommand { get; } = new RoutedCommand();
        public static RoutedCommand RefreshInfo { get; } = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _context.DataChanged += () => Dispatcher.Invoke(LoadRooms);
            LoadRooms();
            ShowAdminPanel();

            CommandBindings.Add(new CommandBinding(AdminPanelCommand, ExecuteAdminPanelCommand));
            CommandBindings.Add(new CommandBinding(AlphabetSortCommand, ExecuteAlphabetSortCommand));
            CommandBindings.Add(new CommandBinding(CostAscSortCommand, ExecuteCostAscSortCommand));
            CommandBindings.Add(new CommandBinding(CostDescSortCommand, ExecuteCostDescSortCommand));
            CommandBindings.Add(new CommandBinding(ProfileCommand, ExecuteProfileCommand));
            CommandBindings.Add(new CommandBinding(RefreshInfo, ExecuteRefreshInfoCommand));
        }

        public void LoadRooms()
        {
            try
            {
                _originalRooms = _context.HotelRooms.AsNoTracking().ToList();
                UpdateRoomsCollection(_originalRooms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void UpdateRoomsCollection(IEnumerable<HotelRoom> rooms)
        {
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }

        private void RefreshData()
        {
            _originalRooms = _context.HotelRooms.AsNoTracking().ToList();

            var filter = SearchBox.Text.ToLower();
            var filteredRooms = string.IsNullOrEmpty(filter)
                ? _originalRooms
                : _originalRooms.Where(r => r.Name.ToLower().Contains(filter));

            UpdateRoomsCollection(filteredRooms);
        }

        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }

        public void ShowAdminPanel()
        {
            AdminPanelButton.Visibility = (SessionManager.CurrentUser?.IsAdmin == true)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ExecuteAdminPanelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            new AdminPanel().Show();
        }

        private void ExecuteAlphabetSortCommand(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateRoomsCollection(_originalRooms.OrderBy(room => room.Name));
        }

        private void ExecuteCostAscSortCommand(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateRoomsCollection(_originalRooms.OrderBy(room => room.Price));
        }

        private void ExecuteCostDescSortCommand(object sender, ExecutedRoutedEventArgs e)
        {
            UpdateRoomsCollection(_originalRooms.OrderByDescending(room => room.Price));
        }

        private void ExecuteProfileCommand(object sender, RoutedEventArgs e)
        {
            new ProfileWindow().Show();
        }

        private void ExecuteRefreshInfoCommand(object sender, ExecutedRoutedEventArgs e)
        {
            RefreshData();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBox.Text.ToLower();

            var filteredRooms = string.IsNullOrEmpty(filter)
                ? _originalRooms
                : _originalRooms.Where(r => r.Name.ToLower().Contains(filter));

            UpdateRoomsCollection(filteredRooms);
        }
    }
}