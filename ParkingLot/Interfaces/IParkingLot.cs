/// <summary>
/// Parking Lot Interface
/// </summary>
namespace ParkingLot.Interfaces
{
    using ParkingLot.Enums;
    using ParkingLot.Models;

    public interface IParkingLot
    {
        /// <summary>
        /// Free Spots
        /// </summary>
        int FreeSpots { get; }

        /// <summary>
        /// Park Vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <param name="parkingSpot">Parking Spot</param>
        /// <returns></returns>
        bool ParkedVehicle(Vehicle vehicle, ParkingSpot parkingSpot);

        /// <summary>
        /// Un-Parked Vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <returns></returns>
        bool UnParkedVehicle(Vehicle vehicle);

        /// <summary>
        /// Get Optimal - Best Fit Parking Spot
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <returns></returns>
        ParkingSpot GetOptimalParkingSpot(Vehicle vehicle);

        /// <summary>
        /// Get Parking Spot Status
        /// </summary>
        /// <param name="spot">Parking Spot</param>
        /// <returns></returns>
        ParkingSpotStatus GetParkingSpotStatus(ParkingSpot spot);
    }
}
