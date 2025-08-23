namespace BookingService
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
