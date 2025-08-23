using System.ComponentModel.DataAnnotations;

namespace BookingService
{
    public class HotelRoom
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ImagePath { get; set; }
        public string Category { get; set; }
        public double Rating { get; set; }
        public double Price { get; set; }
        public int NumberOfBeds { get; set; }
        public bool IsAvailable { get; set; }
        public string Amenities { get; set; }
        public int Stars { get; set; }
        public bool HasBalcony { get; set; }
        public bool IsNonSmoking { get; set; }
    }
}