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
                        .Where(x => x.Id == id && x.DateDeleted == null)
                        .SingleAsync();
        }       


        public async Task<IEnumerable<Models.UserSet>> GetUserSets()
        {
            return await _context.UserSet
                        .Where(x => x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.UserSet>> GetUserSets(int[] ids)
        {
            return await _context.UserSet
                        .Where(x => ids.Contains(x.Id) && x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.UserSet>> GetUserSetsFromExercise(int id)
        {
            return await _context.UserSet
                        .Where(x => x.ExerciseId == id && x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.UserSet>> GetUserSetsFromUserExercise(int id)
        {
            return await _context.UserSet
                        .Where(x => x.UserExerciseId == id && x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.UserSet>> GetUserSetsFromWorkout(int workoutId)
        {
            return await _context.UserSet
                        .Where(x => x.UserWorkoutId == workoutId && x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<IEnumerable<Models.UserSet>> GetUserSetsFromWorkouts(int[] workoutIds)
        {
            return await _context.UserSet
                        .Where(x => workoutIds.Contains( x.UserWorkoutId ) && x.DateDeleted == null)
                        .ToListAsync();
        }


        public async Task<Models.UserSet> GetLastUserSetForExercise(bool isUserExercise, int exerciseId, int[] workoutIds)
        {
            if (isUserExercise)
            {
                return await _context.UserSet
                            .Where(x => workoutIds.Contains(x.UserWorkoutId) && x.UserExerciseId == exerciseId && x.DateDeleted == null)
                            .OrderByDescending(x => x.DateAdded.Date)
                            .ThenBy(x => x.Weight)
                            .Select(x => new Models.UserSet
                            {
                                Id = x.Id,
                                ExerciseId = x.ExerciseId,
                                UserExerciseId = x.UserExerciseId,
                                Repetitions = x.Repetitions,
                                Weight = x.Weight,
                                DateAdded = x.DateAdded
                            })
                            .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.UserSet
                                .Where(x => workoutIds.Contains(x.UserWorkoutId) && x.ExerciseId == exerciseId && x.DateDeleted == null)
                                .OrderByDescending(x => x.DateAdded.Date)
                                .ThenBy(x => x.Weight)
                                .Select(x => new Models.UserSet
                                {
                                    Id = x.Id,
                                    ExerciseId = x.ExerciseId,
                                    UserExerciseId = x.UserExerciseId,
                                    Repetitions = x.Repetitions,
                                    Weight = x.Weight,
                                    DateAdded = x.DateAdded
                                })
                                .FirstOrDefaultAsync();
            }
        }


        public int AddUserSet(Models.UserSet userSet)
        {
            _context.UserSet.Add(userSet);
            _context.SaveChanges();

            return userSet.Id;
        }


        public async Task DeleteUserSetsFromExercise(int workoutId, int exerciseId)
        {
            var exerciseSets = await _context.UserSet
                                             .Where(x => x.UserWorkoutId == workoutId && x.ExerciseId == exerciseId && x.DateDeleted == null)
                                             .ToListAsync();

            foreach (Models.UserSet set in exerciseSets)
            {
                set.DateDeleted = DateTime.Now;
            }

            _context.UserSet.UpdateRange(exerciseSets);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteUserSetsFromUserExercise(int workoutId, int exerciseId)
        {
            var exerciseSets = await _context.UserSet
                                             .Where(x => x.UserWorkoutId == workoutId && x.UserExerciseId == exerciseId && x.DateDeleted == null)
                                             .ToListAsync();

            foreach (Models.UserSet set in exerciseSets)
            {
                set.DateDeleted = DateTime.Now;
            }

            _context.UserSet.UpdateRange(exerciseSets);
            await _context.SaveChangesAsync();
        }
    }
}
