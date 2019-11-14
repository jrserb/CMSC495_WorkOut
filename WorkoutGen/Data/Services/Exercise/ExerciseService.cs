using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.Exercise
{
    public class ExerciseService : IExerciseService
    {
        private readonly WorkoutGenContext _context;

        public ExerciseService(WorkoutGenContext context)
        {
            _context = context;
        }    

        public async Task<Models.Exercise> GetExercise(int id)
        {
            return await _context.Exercise
                        .Where(x => x.Id == id)
                        .SingleAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercises()
        {
            return await _context.Exercise
                        .OrderBy(x => x.Name)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.Exercise>> GetExercises(int[] ids)
        {
            return await _context.Exercise
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }

        public async Task<int[]> GetExerciseIdsFromMuscleGroups(int[] ids)
        {
            return await _context.ExerciseMuscleGroup
                        .Where(x => ids.Contains(x.MuscleGroupId))
                        .Select(x => x.ExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<int[]> GetExerciseIdsFromEquipment(int[] ids)
        {
            return await _context.ExerciseEquipment
                        .Where(x => ids.Contains(x.EquipmentId))
                        .Select(x => x.ExerciseId)
                        .Distinct()
                        .ToArrayAsync();
        }
    }
}
