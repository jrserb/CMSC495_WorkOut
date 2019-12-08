using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutGen.Data.Services.Equipment
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ApplicationDbContext _context;

        public EquipmentService(ApplicationDbContext context)
        {
            _context = context;
        }


        // Returns single active equipment object that matches the id
        public async Task<Models.Equipment> GetEquipment(int id)
        {
            return await _context.Equipment
                        .Where(x => x.Id == id && x.DateDeleted == null)
                        .SingleAsync();
        }


        // Returns all active equipment objects
        public async Task<IEnumerable<Models.Equipment>> GetEquipment()
        {
            return await _context.Equipment
                        .Where(x => x.DateDeleted == null)
                        .ToListAsync();
        }


        // Returns active equipment objects that match the ids
        public async Task<IEnumerable<Models.Equipment>> GetEquipment(int[] ids)
        {
            return await _context.Equipment
                        .Where(x => ids.Contains(x.Id) && x.DateDeleted == null)
                        .ToListAsync();
        }


        // Returns active equipment objects that are related to an exercise
        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromExercise(int id)
        {

            int[] equipmentIds = await GetEquipmentIdsFromExercise(id);
            return await GetEquipment(equipmentIds);
        }


        // Returns alternate equipment based on the exercise equipment
        public async Task<IEnumerable<Models.Equipment>> GetAlternateEquipmentFromExerciseEquipment(int exerciseId)
        {
            int[] exerciseEquipmentIds = await GetExerciseEquipmentIdsFromExercise(exerciseId);
            int[] alternateEquipmentIds = await GetAlternateEquipmentIdsFromExerciseEquipment(exerciseEquipmentIds);
            return await GetEquipment(alternateEquipmentIds);
        }


        // Returns an array of ids of active equipment objects that are related to an exercise
        public async Task<int[]> GetEquipmentIdsFromExercise(int id)
        {
            return await _context.ExerciseEquipment
                        .Where(x => x.ExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.EquipmentId)
                        .ToArrayAsync();
        }
        

        // Returns an array of ids of active equipment objects that are related to exercises
        public async Task<int[]> GetEquipmentIdsFromExercises(int[] ids)
        {
            return await _context.ExerciseEquipment
                        .Where(x => ids.Contains(x.ExerciseId) && x.DateDeleted == null)
                        .Select(x => x.EquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }


        // Returns exercise equipment ids based on a single exercise
        public async Task<int[]> GetExerciseEquipmentIdsFromExercise(int id)
        {
            return await _context.ExerciseEquipment
                        .Where(x => x.ExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArrayAsync();
        }


        // Returns exercise equipment ids based on multiple exercises
        public async Task<int[]> GetExerciseEquipmentIdsFromExercises(int[] ids)
        {
            return await _context.ExerciseEquipment
                        .Where(x => ids.Contains(x.ExerciseId) && x.DateDeleted == null)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArrayAsync();
        }

        // NEW
        public async Task<int[]> GetExerciseEquipmentIdsFromExerciseAndEquipment(int exerciseId, int equipmentId)
        {
            return await _context.ExerciseEquipment
                        .Where(x => x.ExerciseId == exerciseId && x.EquipmentId == equipmentId && x.DateDeleted == null)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArrayAsync();
        }


        // Returns active alternate equipment objects related to an equipment id
        public async Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int id)
        {
            return await _context.ExerciseAlternateEquipment
                        .Where(x => x.ExerciseEquipmentId == id && x.DateDeleted == null)
                        .Select(x => x.AlternateEquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }


        // Returns alternate equipment ids based on multiple exercise equipment
        public async Task<int[]> GetAlternateEquipmentIdsFromExerciseEquipment(int[] ids)
        {
            return await _context.ExerciseAlternateEquipment
                        .Where(x => ids.Contains(x.ExerciseEquipmentId) && x.DateDeleted == null)
                        .Select(x => x.AlternateEquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }

        


        //USER METHODS

        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserExercise(string userId, int id)
        {
            int[] equipmentIds = await GetEquipmentIdsFromUserExercise(userId, id);
            return await GetEquipment(equipmentIds);
        }


        public async Task<int[]> GetEquipmentIdsFromUserExercise(string userId, int id)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserId == userId && x.UserExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.EquipmentId)
                        .ToArrayAsync();
        }


        public async Task<int[]> GetEquipmentIdsFromUserExercises(string userId, int[] ids)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserId == userId && ids.Contains(x.UserExerciseId) && x.DateDeleted == null)
                        .Select(x => x.EquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }


        public async Task<int[]> GetUserExerciseEquipmentIdsFromExercise(int id)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArrayAsync();
        }


        public async Task<int[]> GetUserExerciseEquipmentIdsFromExercises(int[] ids)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => ids.Contains(x.UserExerciseId) && x.DateDeleted == null)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArrayAsync();
        }

        // NEW
        public async Task<int[]> GetUserExerciseEquipmentIdsFromExerciseAndEquipment(int exerciseId, int equipmentId)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserExerciseId == exerciseId && x.EquipmentId == equipmentId && x.DateDeleted == null)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToArrayAsync();
        }
    }
}
