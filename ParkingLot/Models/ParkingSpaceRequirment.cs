/// <summary>
/// Parking Space Requirment Model
/// </summary>
namespace ParkingLot.Models
{
    using ParkingLot.Enums;

    public class ParkingSpaceRequirment
    {
        /// <summary>
        /// Parking Spot
        /// </summary>
        public ParkingSpotTypes ParkingSpot { get; set; }

        /// <summary>
        /// Parking Spots Count
        /// </summary>
        public int ParkingSpotsCount { get; set; }
    }
}
