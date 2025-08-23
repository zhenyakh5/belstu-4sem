using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;

namespace BookingService
{
    public class EditRoomCommand : ICommand
    {
        private readonly AppDbContext _context;
        private readonly int _roomId;
        private readonly HotelRoom _originalState;
        private readonly HotelRoom _newState;

        public EditRoomCommand(AppDbContext context, HotelRoom originalRoom, HotelRoom newRoom)
        {
            _context = context;
            _roomId = originalRoom.Id;
            _originalState = CloneRoom(originalRoom);
            _newState = CloneRoom(newRoom);
        }

        public async Task ExecuteAsync()
        {
            await ApplyChangesAsync(_newState).ConfigureAwait(false);
        }

        public async Task UndoAsync()
        {
            await ApplyChangesAsync(_originalState).ConfigureAwait(false);
        }

        private async Task ApplyChangesAsync(HotelRoom state)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var room = await _context.HotelRooms
                        .FirstOrDefaultAsync(r => r.Id == _roomId)
                        .ConfigureAwait(false);

                    if (room != null)
                    {
                        _context.Entry(room).CurrentValues.SetValues(state);
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                        _context.NotifyDataChanged();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"Error: {ex.Message}"));
                }
            }
        }

        public void Execute()
        {
            ExecuteAsync().FireAndForget();
        }

        public void Undo()
        {
            UndoAsync().FireAndForget();
        }

        private static HotelRoom CloneRoom(HotelRoom source) => new HotelRoom
        {
            Id = source.Id,
            Name = source.Name,
            ShortDescription = source.ShortDescription,
            FullDescription = source.FullDescription,
            ImagePath = source.ImagePath,
            Category = source.Category,
            Rating = source.Rating,
            Price = source.Price,
            NumberOfBeds = source.NumberOfBeds,
            Amenities = source.Amenities,
            Stars = source.Stars,
            HasBalcony = source.HasBalcony,
            IsNonSmoking = source.IsNonSmoking,
            IsAvailable = source.IsAvailable
        };
    }

    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"Error: {t.Exception?.InnerException?.Message}"));
                }
            });
        }
    }
}