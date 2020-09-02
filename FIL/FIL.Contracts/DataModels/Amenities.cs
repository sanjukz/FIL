using FIL.Contracts.Interfaces;

namespace FIL.Contracts.DataModels
{
    public class Amenities : IId<int>
    {
        public int Id { get; set; }
        public string Amenity { get; set; }
    }
}