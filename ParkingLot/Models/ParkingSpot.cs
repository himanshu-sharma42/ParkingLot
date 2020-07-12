/// <summary>
/// ParkingSpot Model
/// </summary>
namespace ParkingLot.Models
{
    using ParkingLot.Enums;

    public class ParkingSpot
    {
        /// <summary>
        /// Floor
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// Row Number
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Start Position
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        /// Spot Count
        /// </summary>
        public int SpotCount { get; set; }

        /// <summary>
        /// Parking Spot Types
        /// </summary>
        public ParkingSpotTypes ParkingSpotTypes { get; set; }
    }
}
