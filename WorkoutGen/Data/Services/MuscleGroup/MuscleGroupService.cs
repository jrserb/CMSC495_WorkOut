using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.MuscleGroup
{
    public class MuscleGroupService : IMuscleGroupService
    {
        private readonly ApplicationDbContext _context;


        public MuscleGroupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.MuscleGroup> GetMuscleGroup(int id)
        {
            return await _context.MuscleGroup
                        .Where(x => x.Id == id && x.DateDeleted == null)
                        .SingleAsync();
        }


        public async Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups()
        {
            return await _context.MuscleGroup
                        .Where(x => x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroups(int[] ids)
        {
            return await _context.MuscleGroup
                        .Where(x => ids.Contains(x.Id) && x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroupsFromExercise(int id)
        {
            int[] muscleGroupIds = await GetMuscleGroupIdsFromExercise(id);
            return await GetMuscleGroups(muscleGroupIds);
        }


        public async Task<int[]> GetMuscleGroupIdsFromExercise(int id)
        {
            return await _context.ExerciseMuscleGroup
                        .Where(x => x.ExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.MuscleGroupId)
                        .Distinct()
                        .ToArrayAsync();
        }




        //USER METHODS

        public async Task<int[]> GetMuscleGroupIdsFromUserExercise(string userId, int id)
        {
            return await _context.UserExerciseMuscleGroup
                        .Where(x => x.UserId == userId && x.UserExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.MuscleGroupId)
                        .Distinct()
                        .ToArrayAsync();
        }


        public async Task<IEnumerable<Models.MuscleGroup>> GetMuscleGroupsFromUserExercise(string userId, int id)
        {
            int[] muscleGroupIds = await GetMuscleGroupIdsFromUserExercise(userId, id);
            return await GetMuscleGroups(muscleGroupIds);
        }
    }
}
