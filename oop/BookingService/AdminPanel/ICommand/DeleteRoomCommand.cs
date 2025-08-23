using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace BookingService
{
    public class DeleteRoomCommand : ICommand
    {
        private readonly AppDbContext _context;
        private readonly HotelRoom _room;

        public DeleteRoomCommand(AppDbContext context, HotelRoom room)
        {
            _context = context;
            _room = room;
        }

        public async Task ExecuteAsync()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.HotelRooms.Remove(_room);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    _context.NotifyDataChanged();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"Error: {ex.Message}"));
                }
            }
        }

        public async Task UndoAsync()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.HotelRooms.Add(_room);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    _context.NotifyDataChanged();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"Error: {ex.Message}"));
                }
            }
        }

        public void Execute() => ExecuteAsync().FireAndForget();
        public void Undo() => UndoAsync().FireAndForget();
    }
}