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

        public async Task<Models.UserEquipmentSet> GetUserEquipmentSet(int id)
        {
            return await _context.UserEquipmentSet
                                 .Where(x => x.Id == id && x.DateDeleted == null)
                                 .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Models.UserEquipmentSet>> GetUserEquipmentSets(string userId)
        {
            return await _context.UserEquipmentSet
                                 .Where(x => x.UserId == userId && x.DateDeleted == null)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Models.Equipment>> GetEquipmentFromUserEquipmentSet(int equipmentSetId)
        {
            int[] equipmentIds = await _context.UserEquipmentSetEquipment
                                               .Where(x => x.UserEquipmentSetId == equipmentSetId && x.DateDeleted == null)
                                               .Select(x => x.EquipmentId)
                                               .ToArrayAsync();

            return await GetEquipment(equipmentIds);
        }

        public async Task<int> AddUserEquipmentSet(Models.UserEquipmentSet userEquipmentSet, int[] equipmentIds)
        {
            // First create a new set record
            await _context.UserEquipmentSet.AddAsync(userEquipmentSet);
            await _context.SaveChangesAsync();

            // Then create the equipment records for the set
            var equipmentSetEquipment = new List<Models.UserEquipmentSetEquipment>();
            foreach (int id in equipmentIds)
            {
                equipmentSetEquipment.Add(new Models.UserEquipmentSetEquipment { 
                    UserEquipmentSetId = userEquipmentSet.Id,
                    EquipmentId = id
                });
            }

            await _context.UserEquipmentSetEquipment.AddRangeAsync(equipmentSetEquipment);
            await _context.SaveChangesAsync();

            // Return the id of the newly created set
            return userEquipmentSet.Id;
        }

        public async Task UpdateUserEquipmentSet(Models.UserEquipmentSet userEquipmentSet, int[] equipmentIds)
        {        
            // First remove the old equipment from the set
            var equipmentSetEquipment = await _context.UserEquipmentSetEquipment
                                                      .Where(x => x.UserEquipmentSetId == userEquipmentSet.Id && x.DateDeleted == null)
                                                      .ToListAsync();

            int[] equipmentSetEquipmentIds = equipmentSetEquipment.Select(x => x.EquipmentId).ToArray();

            if (equipmentSetEquipment.Count > 0)
            {
                foreach (Models.UserEquipmentSetEquipment obj in equipmentSetEquipment)
                {
                    // Don't remove an existing equipment if it is still in user selection
                    if (!equipmentIds.Contains(obj.EquipmentId))
                    {
                        obj.DateDeleted = DateTime.Now;
                    }
                }
                _context.UserEquipmentSetEquipment.UpdateRange(equipmentSetEquipment);
                await _context.SaveChangesAsync();
            }

            // Then add the new equipment for the set
            equipmentSetEquipment = new List<Models.UserEquipmentSetEquipment>();
            foreach (int id in equipmentIds)
            {
                // Don't remove an existing equipment if it is still in user selection
                if (!equipmentSetEquipmentIds.Contains(id))
                {
                    equipmentSetEquipment.Add(new Models.UserEquipmentSetEquipment
                    {
                        UserEquipmentSetId = userEquipmentSet.Id,
                        EquipmentId = id
                    });
                }
            }
            if (equipmentSetEquipment.Count > 0)
            {
                await _context.UserEquipmentSetEquipment.AddRangeAsync(equipmentSetEquipment);
                await _context.SaveChangesAsync();
            }

            // Finally update the equipment set record
            _context.UserEquipmentSet.Update(userEquipmentSet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserEquipmentSet(Models.UserEquipmentSet userEquipmentSet, int[] equipmentIds)
        {
            // First remove the equipment for the set
            var equipmentSetEquipment = await _context.UserEquipmentSetEquipment
                                                      .Where(x => x.UserEquipmentSetId == userEquipmentSet.Id && x.DateDeleted == null)
                                                      .ToListAsync();

            foreach (Models.UserEquipmentSetEquipment obj in equipmentSetEquipment)
            {
                obj.DateDeleted = DateTime.Now;
            }
            _context.UserEquipmentSetEquipment.UpdateRange(equipmentSetEquipment);
            await _context.SaveChangesAsync();

            // Then remove the set
            userEquipmentSet.DateDeleted = DateTime.Now;

            _context.UserEquipmentSet.Update(userEquipmentSet);
            await _context.SaveChangesAsync();
        }

        // Returns true or false if an exercise existst
        public async Task<bool> UserEquipmentSetExists(int id)
        {
            return await _context.UserEquipmentSet
                        .Where(x => x.DateDeleted == null)
                        .AnyAsync(e => e.Id == id);
        }
    }
}
