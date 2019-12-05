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

        // Returns all active equipment objects
        public async Task<IEnumerable<Models.Equipment>> GetEquipment()
        {
            return await _context.Equipment
                        .Where(x => x.DateDeleted == null)
                        .ToListAsync();
        }

        // Returns single active equipment object that matches the id
        public async Task<Models.Equipment> GetEquipment(int id)
        {
            return await _context.Equipment
                        .Where(x => x.Id == id && x.DateDeleted == null)
                        .SingleAsync();
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

        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserExercise(int id)
        {
            int[] equipmentIds = await GetEquipmentIdsFromUserExercise(id);
            return await GetEquipment(equipmentIds);
        }

        // Returns an array of ids of active equipment objects that are related to an exercise
        public async Task<int[]> GetEquipmentIdsFromExercise(int id)
        {
            return await _context.ExerciseEquipment
                        .Where(x => x.ExerciseId == id && x.DateDeleted == null)
                        .Select(x => x.EquipmentId)
                        .ToArrayAsync();
        }

        public async Task<int[]> GetEquipmentIdsFromUserExercise(int id)
        {
            return await _context.UserExerciseEquipment
                        .Where(x => x.UserExerciseId == id && x.DateDeleted == null)
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

        // Returns active alternate equipment objects related to an equipment id
        public async Task<int[]> GetAlternateEquipmentIdsFromEquipment(int id)
        {
            return await _context.ExerciseAlternateEquipment
                        .Where(x => x.ExerciseEquipmentId == id && x.DateDeleted == null)
                        .Select(x => x.AlternateEquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }

        // Returns active alternate equipment objects related to equipment ids
        public async Task<int[]> GetAlternateEquipmentIdsFromEquipment(int[] ids)
        {
            return await _context.ExerciseAlternateEquipment
                        .Where(x => ids.Contains(x.ExerciseEquipmentId) && x.DateDeleted == null)
                        .Select(x => x.AlternateEquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<IEnumerable<Models.Equipment>> GetAlternateEquipmentFromEquipment(int[] ids)
        {
            int[] alternateEquipmentIds = await GetAlternateEquipmentIdsFromEquipment(ids);
            return await GetEquipment(alternateEquipmentIds);
        }
    }
}
