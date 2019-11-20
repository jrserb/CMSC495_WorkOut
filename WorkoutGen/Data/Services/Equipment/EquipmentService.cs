using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutGen.Models;

namespace WorkoutGen.Data.Services.Equipment
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ApplicationDbContext _context;

        public EquipmentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Models.Equipment>> GetEquipment()
        {
            return await _context.Equipment
                        .OrderBy(x => x.Name)
                        .ToListAsync();
        }

        public async Task<Models.Equipment> GetEquipment(int id)
        {
            return await _context.Equipment
                        .Where(x => x.Id == id)
                        .SingleAsync();
        }

        public async Task<IEnumerable<Models.Equipment>> GetEquipment(int[] ids)
        {
            return await _context.Equipment
                        .Where(x => ids.Contains(x.Id))
                        .ToListAsync();
        }

        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromExercise(int id)
        {

            int[] equipmentIds = await GetEquipmentIdsFromExercise(id);
            return await GetEquipment(equipmentIds);
        }

        public async Task<int[]> GetEquipmentIdsFromExercise(int id)
        {
            return await _context.ExerciseEquipment
                        .Where(x => x.ExerciseId == id)
                        .Select(x => x.EquipmentId)
                        .ToArrayAsync();
        }

        public async Task<int[]> GetEquipmentIdsFromExercises(int[] ids)
        {
            return await _context.ExerciseEquipment
                        .Where(x => ids.Contains(x.ExerciseId))
                        .Select(x => x.EquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<int[]> GetAlternateEquipmentIdsFromEquipment(int id)
        {
            return await _context.ExerciseAlternateEquipment
                        .Where(x => x.ExerciseEquipmentId == id)
                        .Select(x => x.AlternateEquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }

        public async Task<int[]> GetAlternateEquipmentIdsFromEquipment(int[] ids)
        {
            return await _context.ExerciseAlternateEquipment
                        .Where(x => ids.Contains(x.ExerciseEquipmentId))
                        .Select(x => x.AlternateEquipmentId)
                        .Distinct()
                        .ToArrayAsync();
        }
    }
}
