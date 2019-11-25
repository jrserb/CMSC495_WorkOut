using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGen.Models;

namespace WorkoutGen.Data.Services.UserWorkout
{
    public class UserWorkoutService : IUserWorkoutService
    {
        private readonly ApplicationDbContext _context;

        public UserWorkoutService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.UserWorkout> GetUserWorkout(int id)
        {
            return await _context.UserWorkout
                        .Where(x => x.Id == id)
                        .SingleAsync();
        }

        public async Task<IEnumerable<Models.UserWorkout>> GetUserWorkouts()
        {
            return await _context.UserWorkout
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.UserWorkout>> GetUserWorkouts(int[] ids)
        {
            return await _context.UserWorkout
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }

        public async Task<int[]> GetUserWorkoutsByUserId(string userId)
        {
            return await _context.UserWorkout
                        .Where(x => x.UserId == userId)
                        .Select(x => x.Id)
                        .ToArrayAsync();
        }

        public async Task<int> AddUserWorkout(string userId)
        {
            var UserWorkout = new Models.UserWorkout { UserId = userId };

            await _context.UserWorkout.AddAsync(UserWorkout);
            await _context.SaveChangesAsync();

            return UserWorkout.Id;
        }
    }
}
