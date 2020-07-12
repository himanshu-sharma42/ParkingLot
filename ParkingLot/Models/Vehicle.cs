/// <summary>
/// Vehicle Model
/// </summary>
namespace ParkingLot.Models
{
    using ParkingLot.Enums;

    public class Vehicle
    {
        /// <summary>
        /// Vehicle Number
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Vehicle Type
        /// </summary>
        public VehicleTypes VehicleType { get; set; }
    }
}
