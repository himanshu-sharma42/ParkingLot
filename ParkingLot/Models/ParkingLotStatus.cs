/// <summary>
/// Parking Lot Status
/// </summary>
namespace ParkingLot.Models
{
    public class ParkingLotStatus
    {
        /// <summary>
        /// Total Parking Spots
        /// </summary>
        public int TotalParkingSpots { get; set; }

        /// <summary>
        /// Occupied Spots
        /// </summary>
        public int OccupiedSpots { get; set; }

        /// <summary>
        /// Free Spots
        /// </summary>
        public int FreeSpots { get; set; }
    }
}
