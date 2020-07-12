/// <summary>
/// Parking Lot Core
/// </summary>
namespace ParkingLot.BusinessLogic
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using ParkingLot.Enums;
    using ParkingLot.Interfaces;
    using ParkingLot.Models;

    public class ParkingLotCore : IParkingLot
    {
        #region "Properties & Fields"

        private ImmutableSortedSet<ParkingSpot> FreeParkingSpots;

        private ConcurrentDictionary<string, ParkingSpot> ParkedVehicles;

        private readonly IEnumerable<List<List<ParkingSpot>>> ParkingLotLayout;

        private readonly IParkingSpaceMapper ParkingSpaceMapper;

        private int _FreeSpots = 0;

        public int FreeSpots => _FreeSpots;

        private int _TotalSpots = 0;

        public int TotalSpots => _TotalSpots;

        #endregion

        #region "Constructor"

        /// <summary>
        /// parking Lot Layout
        /// </summary>
        /// <param name="parkingLotLayout">Parking Lot Layout</param>
        /// <param name="parkingSpaceMapper">Parking Lot Layout</param>
        public ParkingLotCore(IEnumerable<List<List<ParkingSpot>>> parkingLotLayout, IParkingSpaceMapper parkingSpaceMapper)
        {
            var comparer = Comparer<ParkingSpot>.Create((x, y) => x.Floor == y.Floor ? x.Row == y.Row ? x.StartPosition.CompareTo(y.StartPosition) : x.Row.CompareTo(y.Row) : x.Floor.CompareTo(y.Floor));
            FreeParkingSpots = ImmutableSortedSet.Create<ParkingSpot>(comparer);
            ParkedVehicles = new ConcurrentDictionary<string, ParkingSpot>();
            this.ParkingLotLayout = parkingLotLayout;
            this.ParkingSpaceMapper = parkingSpaceMapper;
            InitializeParkingLot();
        }

        #endregion

        #region "Other Functions"

        /// <summary>
        /// parking Lot Layout
        /// </summary>
        private void InitializeParkingLot()
        {
            foreach (var floor in ParkingLotLayout)
            {
                foreach (var row in floor)
                {
                    foreach (var spot in row)
                    {
                        FreeParkingSpots = FreeParkingSpots.Add(spot);
                        Interlocked.Add(ref _TotalSpots, spot.SpotCount);
                        Interlocked.Add(ref _FreeSpots, spot.SpotCount);
                    }
                }
            }
        }


        /// <summary>
        /// Get Optimal Parking Spot
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <returns></returns>
        public ParkingSpot GetOptimalParkingSpot(Vehicle vehicle)
        {
            ParkingSpaceRequirment requiredSpace = ParkingSpaceMapper.GetSmallestParkingSpaceRequired(vehicle);
            var vacantSpot = FreeParkingSpots.FirstOrDefault(m => m.ParkingSpotTypes >= requiredSpace.ParkingSpot && m.SpotCount >= requiredSpace.ParkingSpotsCount);

            if (vacantSpot != null)
            {
                vacantSpot.SpotCount = Math.Min(vacantSpot.SpotCount, requiredSpace.ParkingSpotsCount);
            }

            return vacantSpot;
        }

        /// <summary>
        /// Parked Vehicle
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <param name="parkingSpot">Parking Spot</param>
        /// <returns></returns>
        public bool ParkedVehicle(Vehicle vehicle, ParkingSpot parkingSpot)
        {
            if (ParkedVehicles.ContainsKey(vehicle.RegistrationNumber))
            {
                throw new InvalidOperationException($"Vehicle with number {vehicle.RegistrationNumber} is already parked.");
            }

            ParkingSpot vacantSpot = FreeParkingSpots.FirstOrDefault(spot => spot.Floor == parkingSpot.Floor && spot.Row == parkingSpot.Row &&
            spot.ParkingSpotTypes == parkingSpot.ParkingSpotTypes && spot.StartPosition <= parkingSpot.StartPosition && spot.SpotCount >= parkingSpot.SpotCount);

            if (vacantSpot == null)
                throw new KeyNotFoundException("The spot could not be found.");

            FreeParkingSpots = FreeParkingSpots.Remove(vacantSpot);
            ParkedVehicles.TryAdd(vehicle.RegistrationNumber, parkingSpot);

            if (parkingSpot.StartPosition > vacantSpot.StartPosition)
            {
                var newSpot = new ParkingSpot()
                {
                    Floor = vacantSpot.Floor,
                    ParkingSpotTypes = vacantSpot.ParkingSpotTypes,
                    Row = vacantSpot.Row,
                    StartPosition = vacantSpot.StartPosition
                };

                newSpot.SpotCount = parkingSpot.StartPosition - vacantSpot.StartPosition;
                FreeParkingSpots = FreeParkingSpots.Add(newSpot);
            }

            if (vacantSpot.SpotCount > parkingSpot.SpotCount)
            {
                var newSpot = new ParkingSpot()
                {
                    Floor = vacantSpot.Floor,
                    ParkingSpotTypes = vacantSpot.ParkingSpotTypes,
                    Row = vacantSpot.Row
                };

                newSpot.StartPosition = parkingSpot.StartPosition + parkingSpot.SpotCount;
                newSpot.SpotCount = vacantSpot.SpotCount - newSpot.StartPosition + 1;
                FreeParkingSpots = FreeParkingSpots.Add(newSpot);
            }

            Interlocked.Add(ref _FreeSpots, parkingSpot.SpotCount * -1);
            return true;
        }

        /// <summary>
        /// ParkedVehicle
        /// </summary>
        /// <param name="vehicle">Vehicle</param>
        /// <returns></returns>
        public bool UnParkedVehicle(Vehicle vehicle)
        {
            ParkedVehicles.TryRemove(vehicle.RegistrationNumber, out ParkingSpot currentSpot);

            if (currentSpot == null)
                throw new ArgumentException($"Vehicle {vehicle.RegistrationNumber} is NOT parked.");

            var leftSpot = FreeParkingSpots.FirstOrDefault(spot => spot.Floor == currentSpot.Floor && spot.Row == currentSpot.Row
             && spot.ParkingSpotTypes == currentSpot.ParkingSpotTypes && spot.StartPosition + spot.SpotCount == currentSpot.StartPosition);

            ParkingSpot newSpotToUpdate = new ParkingSpot()
            {
                Floor = currentSpot.Floor,
                ParkingSpotTypes = currentSpot.ParkingSpotTypes,
                Row = currentSpot.Row,
                StartPosition = currentSpot.StartPosition,
                SpotCount = currentSpot.SpotCount
            };

            if (leftSpot != null)
            {
                newSpotToUpdate.StartPosition = leftSpot.StartPosition;
                newSpotToUpdate.SpotCount = currentSpot.SpotCount + leftSpot.SpotCount;
                FreeParkingSpots = FreeParkingSpots.Remove(leftSpot);
            }

            var rightSpot = FreeParkingSpots.FirstOrDefault(spot => spot.Floor == currentSpot.Floor && spot.Row == currentSpot.Row
             && spot.ParkingSpotTypes == currentSpot.ParkingSpotTypes && spot.StartPosition == currentSpot.StartPosition + currentSpot.SpotCount);

            if (rightSpot != null)
            {
                newSpotToUpdate.SpotCount = newSpotToUpdate.SpotCount + rightSpot.SpotCount;
                FreeParkingSpots = FreeParkingSpots.Remove(rightSpot);
            }

            FreeParkingSpots = FreeParkingSpots.Add(newSpotToUpdate);
            return true;
        }

        /// <summary>
        /// Get Parking Spot Status
        /// </summary>
        /// <param name="parkingSpot">Parking  Spot</param>
        /// <returns>Parking Spot Status</returns>
        public ParkingSpotStatus GetParkingSpotStatus(ParkingSpot parkingSpot)
        {
            var rightSpot = FreeParkingSpots.FirstOrDefault(spot => spot.Floor == parkingSpot.Floor && spot.Row == parkingSpot.Row &&
            spot.ParkingSpotTypes == parkingSpot.ParkingSpotTypes && spot.StartPosition <= parkingSpot.StartPosition &&
            spot.StartPosition + spot.SpotCount >= parkingSpot.SpotCount + parkingSpot.StartPosition);

            if (rightSpot != null)
            {
                return ParkingSpotStatus.Vacant;
            }

            return ParkingSpotStatus.Occupied;
        }

        #endregion
    }
}
