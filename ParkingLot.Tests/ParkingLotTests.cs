/// <summary>
/// Parking Lot Tests
/// </summary>
namespace ParkingLot.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using ParkingLot.BusinessLogic;
    using ParkingLot.Enums;
    using ParkingLot.Models;
    using ParkingLot.Tests.Helper;

    [TestFixture]
    public class ParkingLotTests
    {
        #region "Test Cases"

        [TestCase, Order(1)]
        public void TotalFreeSpotsAfterInitializingTest()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            int freeSpots = parkingLot.FreeSpots;
            freeSpots.Should().Be(60);
        }

        [TestCase, Order(2)]
        public void ParkedVehicleShouldUpdateTheParkedVehicleStatus()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 3, SpotCount = 2 };
            ParkingSpot newVacant = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 5, SpotCount = 6 };
            ParkingSpot newVacant2 = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 2 };
            var vehicle = new Vehicle() { RegistrationNumber = "XYZABC", VehicleType = VehicleTypes.HatchbackCar };
            parkingLot.ParkedVehicle(vehicle, spot).Should().BeTrue();
            parkingLot.GetParkingSpotStatus(newVacant).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(newVacant2).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(spot).Should().Be(ParkingSpotStatus.Occupied);
        }

        [TestCase, Order(3)]
        public void UnParkedVehicleShouldUpdateTheParkedVehicleStatus()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 3, SpotCount = 2 };
            ParkingSpot newVacant = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 5, SpotCount = 6 };
            ParkingSpot newVacant2 = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 2 };
            ParkingSpot newVacant3 = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10 };
            var vehicle = new Vehicle() { RegistrationNumber = "XYZABC", VehicleType = VehicleTypes.HatchbackCar };
            parkingLot.ParkedVehicle(vehicle, spot).Should().BeTrue();
            parkingLot.UnParkedVehicle(vehicle);
            parkingLot.GetParkingSpotStatus(newVacant).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(newVacant2).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(spot).Should().Be(ParkingSpotStatus.Vacant);
            parkingLot.GetParkingSpotStatus(newVacant3).Should().Be(ParkingSpotStatus.Vacant);
        }

        [TestCase, Order(4)]
        public void ParkShouldThrowException()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 2 };
            var vehicle = new Vehicle() { RegistrationNumber = "XYZABC", VehicleType = VehicleTypes.HatchbackCar };
            var actual = parkingLot.ParkedVehicle(vehicle, spot);
            
            if (actual == true)
            {
                Action act = () => parkingLot.ParkedVehicle(vehicle, spot);
                act.Should().Throw<InvalidOperationException>();
            }
        }

        [TestCase]
        public void FreeSpotsAndTotalSpotsMustBeSameAfterInitializingTest()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            int freeSpots = parkingLot.FreeSpots;
            int totalSpots = parkingLot.TotalSpots;
            freeSpots.Should().Be(totalSpots);
        }

        [TestCase]
        public void GetOptimalSpotShouldReturnFirstPosition()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { RegistrationNumber = "STARK12345", VehicleType = VehicleTypes.HatchbackCar };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, Row = 1, SpotCount = 1, StartPosition = 1 };
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase]
        public void GetParkingStatusShouldReturnVacant()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 2 };
            var result = parkingLot.GetParkingSpotStatus(spot);
            result.Should().Be(ParkingSpotStatus.Vacant);
        }

        [TestCase]
        public void GetOptimalSpotShouldReturnFirstPositionForSedan()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { RegistrationNumber = "STARK12345", VehicleType = VehicleTypes.Sedan };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 1, Row = 4, ParkingSpotTypes = ParkingSpotTypes.Sedan, StartPosition = 1, SpotCount = 1 };
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase]
        public void GetOptimalSpotShouldReturnHigherFirstPositionForSedan()
        {

            var parkingLot = new ParkingLotCore(LayoutHelper.NewLayout, new ParkingSpaceMapper());
            var vehicle = new Vehicle() { RegistrationNumber = "STARK12345", VehicleType = VehicleTypes.Sedan };
            var actual = parkingLot.GetOptimalParkingSpot(vehicle);
            var expected = new ParkingSpot() { Floor = 1, Row = 4, ParkingSpotTypes = ParkingSpotTypes.MiniTruck, StartPosition = 1, SpotCount = 1 };
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase]
        public void ParkedVehicleShouldReturnTrue()
        {
            var parkingLot = new ParkingLotCore(LayoutHelper.Layout, new ParkingSpaceMapper());
            ParkingSpot spot = new ParkingSpot { Floor = 1, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 2 };
            var vehicle = new Vehicle() { RegistrationNumber = "XYZABC", VehicleType = VehicleTypes.HatchbackCar };
            var actual = parkingLot.ParkedVehicle(vehicle, spot);
            actual.Should().BeTrue();
        }

        #endregion
    }
}
