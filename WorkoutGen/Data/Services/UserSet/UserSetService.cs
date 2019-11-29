﻿using Microsoft.EntityFrameworkCore;
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
                        .Where(x => workoutIds.Contains(x.UserWorkoutId) && x.ExerciseId == exerciseId)
                        .OrderByDescending(x => x.DateAdded.Date)
                        .ThenBy(x => x.Weight)
                        .Select( x => new Models.UserSet { Id = x.Id, 
                            ExerciseId = x.ExerciseId,
                            Repetitions = x.Repetitions,
                            Weight = x.Weight, 
                            DateAdded = x.DateAdded  
                        })
                        .FirstOrDefaultAsync();
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

        public async Task<IEnumerable<Models.UserSet>> GetUserSetsFromWorkout(int workoutId)
        {
            return await _context.UserSet
                        .Where(x => x.UserWorkoutId == workoutId)
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