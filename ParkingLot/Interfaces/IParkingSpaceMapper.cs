/// <summary>
/// Parking Space Mapper Interface
/// </summary>
namespace ParkingLot.Interfaces
{
    using ParkingLot.Models;

    public interface IParkingSpaceMapper
    {
        ParkingSpaceRequirment GetSmallestParkingSpaceRequired(Vehicle vehicle);
    }
}
