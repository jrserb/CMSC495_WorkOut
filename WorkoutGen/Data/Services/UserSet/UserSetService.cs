using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.UserSet
{
    public class UserSetService : IUserSetService
    {
        private readonly ApplicationDbContext _context;

        public UserSetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.UserSet> GetUserSet(int id)
        {
            return await _context.UserSet
                        .Where(x => x.Id == id)
                        .SingleAsync();
        }

        public async Task<Models.UserSet> GetLastUserSetForExercise(int exerciseId, int[] workoutIds)
        {
            return await _context.UserSet
                        .Where(x => workoutIds.Contains(x.WorkoutId) &&
                        (x.ExerciseId == exerciseId || x.UserExerciseId == exerciseId))
                        .OrderByDescending(x => x.DateAdded.Date)
                        .ThenBy(x => x.Weight)
                        .FirstAsync();
        }

        public async Task<IEnumerable<Models.UserSet>> GetUserSets()
        {
            return await _context.UserSet
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.UserSet>> GetUserSets(int[] ids)
        {
            return await _context.UserSet
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }

        public int AddUserSet(Models.UserSet userSet)
        {
            _context.UserSet.Add(userSet);
            _context.SaveChanges();

            return userSet.Id;
        }
    }
}
