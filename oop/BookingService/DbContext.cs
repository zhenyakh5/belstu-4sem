﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace BookingService
{
    public class AppDbContext : DbContext
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        public event Action DataChanged;

        public DbSet<User> Users { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }

        public AppDbContext() : base("Server=zhenyakh; Database=HotelDatabase; TrustServerCertificate=True; Integrated Security=True;")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();
                _undoStack.Push(command);
                _redoStack.Clear();
                NotifyDataChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NotifyDataChanged()
        {
            DataChanged?.Invoke();
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void Undo()
        {
            if (!CanUndo) return;

            try
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
                DataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отмены: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Redo()
        {
            if (!CanRedo) return;

            try
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
                DataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка повтора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}