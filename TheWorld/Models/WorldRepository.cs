using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from the database");
            return _context.Trips.ToList();
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name.Equals(tripName, StringComparison.CurrentCultureIgnoreCase));
        }

        public Trip GetUserTripByName(string tripName, string username)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name.Equals(tripName, StringComparison.CurrentCultureIgnoreCase)
                    && t.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
        }

        public void AddStop(string tripName, Stop stop, string username)
        {
            var trip = GetUserTripByName(tripName, username);

            if (trip != null)
            {
                trip.Stops.Add(stop);
                _context.Add(stop);
            }
        }

        public IEnumerable<Trip> GetTripsByUsername(string username)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }
}
