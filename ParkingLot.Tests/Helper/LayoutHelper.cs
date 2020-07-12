/// <summary>
/// Layout Helper
/// </summary>
namespace ParkingLot.Tests.Helper
{
    using ParkingLot.Enums;
    using ParkingLot.Models;
    using System.Collections.Generic;

    public static class LayoutHelper
    {
        public static List<List<List<ParkingSpot>>> Layout = new List<List<List<ParkingSpot>>>()
            {
                new List<List<ParkingSpot>> ()
                {
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10}
                    },
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 1, Row = 4, ParkingSpotTypes = ParkingSpotTypes.Sedan, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 5, ParkingSpotTypes = ParkingSpotTypes.Sedan, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 6, ParkingSpotTypes = ParkingSpotTypes.Sedan, StartPosition = 1, SpotCount = 10}
                    },
                }
            };

        public static List<List<List<ParkingSpot>>> NewLayout = new List<List<List<ParkingSpot>>>()
            {
                new List<List<ParkingSpot>> ()
                {
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 1, Row = 1, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 2, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 3, ParkingSpotTypes = ParkingSpotTypes.Hatchback, StartPosition = 1, SpotCount = 10}
                    },
                    new List<ParkingSpot> ()
                    {
                        new ParkingSpot() { Floor = 1, Row = 4, ParkingSpotTypes = ParkingSpotTypes.MiniTruck, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 5, ParkingSpotTypes = ParkingSpotTypes.MiniTruck, StartPosition = 1, SpotCount = 10},
                        new ParkingSpot() { Floor = 1, Row = 6, ParkingSpotTypes = ParkingSpotTypes.MiniTruck, StartPosition = 1, SpotCount = 10}
                    },
                }
            };
    }
}
