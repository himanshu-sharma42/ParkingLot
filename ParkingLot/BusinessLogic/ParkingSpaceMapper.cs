/// <summary>
/// Parking Space Mapper
/// </summary>
namespace ParkingLot.BusinessLogic
{
    using ParkingLot.Enums;
    using ParkingLot.Interfaces;
    using ParkingLot.Models;
    using System;

    public class ParkingSpaceMapper : IParkingSpaceMapper
    {
        /// <summary>
        /// Get Smallest Parking Space Required
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <returns></returns>
        public ParkingSpaceRequirment GetSmallestParkingSpaceRequired(Vehicle vehicle)
        {
            switch (vehicle.VehicleType)
            {
                case VehicleTypes.HatchbackCar:
                    return new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Hatchback, ParkingSpotsCount = 1 };
                case VehicleTypes.Sedan:
                    return new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.Sedan, ParkingSpotsCount = 1 };
                case VehicleTypes.Truck:
                    return new ParkingSpaceRequirment() { ParkingSpot = ParkingSpotTypes.MiniTruck, ParkingSpotsCount = 5 };
                default:
                    throw new ArgumentException($"VehicleType {vehicle.VehicleType} is invalid.");
            }
        }
    }
}
